from __future__ import annotations

import argparse
import os
import platform
import shutil
import subprocess
import sys
from pathlib import Path

ROOT_DIR = Path(__file__).resolve().parent
PROJECT_DIR = ROOT_DIR / "BBPGlue"
PUBLISH_DIR = PROJECT_DIR / "publish"
LIB_DIR = PROJECT_DIR / "lib"

PLUGIN_NAME = "BBPGlue"
PLUGIN_DLL = f"{PLUGIN_NAME}.dll"

DLLS_FROM_BEPINEX_CORE = [
    "BepInEx.dll",
    "0Harmony.dll",
]

DLLS_FROM_MANAGED = [
    # === Reverse purpose only! ===
    "Assembly-CSharp.dll",
    # =============================

    "UnityEngine.dll",
    "UnityEngine.CoreModule.dll",
    "UnityEngine.IMGUIModule.dll",
    "UnityEngine.InputLegacyModule.dll",
    "UnityEngine.TextRenderingModule.dll",
    "UnityEngine.PhysicsModule.dll",
    "UnityEngine.UIModule.dll",
    "UnityEngine.AnimationModule.dll",
    "Unity.TextMeshPro.dll",
    "UnityEngine.UI.dll",
    "UnityEngine.AudioModule.dll",
    "UnityEngine.ImageConversionModule.dll",
    "UnityEngine.UnityWebRequestModule.dll",
    "UnityEngine.UnityWebRequestAudioModule.dll",
]

def die(message: str) -> None:
    print(f"error: {message}", file=sys.stderr)
    raise SystemExit(1)

def run_command(args: list[str | Path], cwd: Path | None = None) -> None:
    printable = " ".join(str(a) for a in args)
    print(f"+ {printable}")

    subprocess.run(
        [str(a) for a in args],
        cwd=str(cwd) if cwd else None,
        check=True,
    )

def get_game_path(args: argparse.Namespace) -> Path:
    raw = args.game or os.environ.get("BBP_GAME")

    if not raw:
        die("BBP_GAME is not set. Set BBP_GAME in your environment variables script or pass --game.")

    game = Path(raw).expanduser().resolve()

    if not game.exists():
        die(f"game directory does not exist: {game}")

    if not game.is_dir():
        die(f"BBP_GAME is not a directory: {game}")

    return game

def find_unity_data_dir(game: Path) -> Path:
    matches = sorted(
        path for path in game.iterdir()
        if path.is_dir() and path.name.endswith("_Data")
    )

    if not matches:
        die(f"could not find Unity *_Data directory in: {game}")

    return matches[0]

def copy_file(src: Path, dst: Path) -> None:
    if not src.is_file():
        die(f"missing file: {src}")

    dst.parent.mkdir(parents=True, exist_ok=True)
    shutil.copy2(src, dst)

    try:
        dst.chmod(0o644)
    except OSError:
        pass

    print(f"Copied {src.name}")

def list_dir(path: Path, title: str) -> None:
    print(title)

    if not path.exists():
        print(f"  missing: {path}")
        return

    for item in sorted(path.iterdir()):
        size = item.stat().st_size if item.is_file() else 0
        kind = "dir " if item.is_dir() else "file"
        print(f"  {kind:4} {size:>10}  {item.name}")

def setup_deps(args: argparse.Namespace) -> None:
    if not PROJECT_DIR.exists():
        die(f"project directory not found: {PROJECT_DIR}")

    run_command(["dotnet", "restore"], cwd=PROJECT_DIR)
    print("NuGet restore complete.")

def build(args: argparse.Namespace) -> None:
    csproj = PROJECT_DIR / f"{PLUGIN_NAME}.csproj"

    if not csproj.is_file():
        die(f"project file not found: {csproj}")

    run_command(
        [
            "dotnet",
            "publish",
            csproj,
            "-c",
            args.configuration,
            "-o",
            PUBLISH_DIR,
        ]
    )

    list_dir(PUBLISH_DIR, "Build output:")

def copy_dlls(args: argparse.Namespace) -> None:
    game = get_game_path(args)
    data_dir = find_unity_data_dir(game)

    bepinex_core = game / "BepInEx" / "core"
    managed = data_dir / "Managed"

    LIB_DIR.mkdir(parents=True, exist_ok=True)

    for name in DLLS_FROM_BEPINEX_CORE:
        copy_file(bepinex_core / name, LIB_DIR / name)

    for name in DLLS_FROM_MANAGED:
        copy_file(managed / name, LIB_DIR / name)

    print(f"References copied to: {LIB_DIR}")

def install_plugin(args: argparse.Namespace) -> None:
    game = get_game_path(args)

    src = PUBLISH_DIR / PLUGIN_DLL
    dst = game / "BepInEx" / "plugins" / PLUGIN_NAME / PLUGIN_DLL

    if not src.is_file():
        die(f"published plugin not found: {src}")

    copy_file(src, dst)
    list_dir(dst.parent, "Installed files:")

def choose_launcher(game: Path, args: argparse.Namespace) -> list[str | Path]:
    explicit = args.launcher or os.environ.get("GAME_LAUNCHER")

    if explicit:
        launcher = Path(explicit).expanduser()
        if not launcher.is_absolute():
            launcher = game / launcher
    else:
        if platform.system() == "Windows":
            launcher = game / "run_bepinex.bat"
        else:
            launcher = game / "run_bepinex.sh"

    if not launcher.exists():
        die(f"launcher does not exist: {launcher}")

    system = platform.system()

    if launcher.suffix == ".sh":
        bash = shutil.which("bash")
        if bash:
            return [bash, launcher]
        return [launcher]

    if launcher.suffix.lower() in {".bat", ".cmd"}:
        if system != "Windows":
            die(f"cannot run Windows batch file on {system}: {launcher}")
        return ["cmd", "/c", launcher]

    if system == "Darwin" and launcher.suffix == ".app":
        return ["open", launcher]

    return [launcher]

def run_game(args: argparse.Namespace) -> None:
    game = get_game_path(args)
    command = choose_launcher(game, args)
    run_command(command, cwd=game)

def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(
        description="Build, install, run, and copy refs for BBPGlue."
    )

    parser.add_argument(
        "--game",
        help="Path to the game directory. Usually set through BBP_GAME env var.",
    )

    parser.add_argument(
        "--launcher",
        help="Optional launcher path. Can also be set through GAME_LAUNCHER.",
    )

    parser.add_argument(
        "-c",
        "--configuration",
        default="Release",
        help="dotnet build configuration. Default: Release.",
    )

    parser.add_argument("--setup-deps", "--setup_deps", action="store_true")
    parser.add_argument("--copy-dlls", "--copy_dlls", action="store_true")
    parser.add_argument("--build", action="store_true")
    parser.add_argument("--install", action="store_true")
    parser.add_argument("--run", action="store_true", dest="run_game")

    return parser.parse_args()

def main() -> None:
    args = parse_args()

    if not any(
        [
            args.setup_deps,
            args.copy_dlls,
            args.build,
            args.install,
            args.run_game,
        ]
    ):
        die("nothing to do. Use --build, --install, --copy-dlls, --setup-deps, or --run.")

    built = False

    if args.setup_deps:
        setup_deps(args)

    if args.copy_dlls:
        copy_dlls(args)

    if args.build:
        build(args)
        built = True

    if args.install:
        if not built:
            build(args)
        install_plugin(args)

    if args.run_game:
        run_game(args)

if __name__ == "__main__":
    main()