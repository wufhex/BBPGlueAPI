<div align="center">
	<img src="assets/baldi_icon.ico" width="200" height="200">
	<h1>Baldi's Basics Plus Glue API</h1>
	<p>
		<b>A high-level modding framework for Baldi's Basics Plus.</b>
	</p>
	<br>
</div>

# BBPGlue

BBPGlue is a modding framework built on top of **BepInEx** and **Harmony** that exposes Baldi's Basics Plus through a clean, strongly-typed API.

Instead of spending time reverse-engineering game internals, writing Harmony patches, and manually accessing private fields through reflection, modders can interact with the game through a unified API:

```csharp
BBP.Player
BBP.Environment
BBP.Callbacks
BBP.Authoring
BBP.Assets
```

BBPGlue is designed to be the foundation for gameplay mods, custom items, custom NPCs, game mode modifications, utility tools, and larger projects.

> **Disclaimer**
>
> BBPGlue is a modding framework and not a crack, launcher, bypass, redistribution package, or replacement for Baldi's Basics Plus.
>
> The project does not contain game assets and requires a legitimate copy of Baldi's Basics Plus to function.
>
> All game content remains property of Basically Games and their respective owners.

# Why BBPGlue?

Traditional BepInEx modding often requires:

- Reverse engineering game internals
- Writing Harmony patches
- Using reflection
- Maintaining version-specific hacks
- Repeating boilerplate code

With BBPGlue:

```csharp
BBP.Player.Stamina = 100f;

BBP.Environment.MakeNoise(
    BBP.Player.Position,
    127
);

BBP.Callbacks.Items.OnItemUse += item =>
{
    BBPConsole.Log(item.NameKey);
};
```

The framework handles most of the plumbing so you can focus on creating mods.

# Features

## Unified Game Access

```csharp
BBP.Game
BBP.Player
BBP.Environment
BBP.Entities
BBP.Hud
BBP.Events
BBP.Prefabs
BBP.Authoring
BBP.Assets
BBP.Callbacks
BBP.Experimental
```

## Strongly Typed Wrappers

- BBPPlayer
- BBPEntity
- BBPNpc
- BBPDoor
- BBPElevator
- BBPRoom
- BBPRandomEvent
- BBPPickup
- BBPItemObject
- BBPSoundObject
- BBPAudioManager

## Callback System

No Harmony patching required for common events.

Examples:

```csharp
BBP.Callbacks.Items.OnItemUse
BBP.Callbacks.Items.OnPickupCollect

BBP.Callbacks.World.OnDoorOpen
BBP.Callbacks.World.OnRoomEnter

BBP.Callbacks.Npcs.OnNpcSpawn
BBP.Callbacks.Npcs.OnNpcCatchPlayer

BBP.Callbacks.Game.OnLevelReady
```

## Runtime Authoring

Create custom content without modifying game assets.

```csharp
BBP.Authoring.CloneNpc(
    "my_mod:old_principal",
    "Principal",
    npc =>
    {
        npc.MaxSpeed = 10f;
    }
);
```

## Asset Loading

```csharp
Sprite sprite =
    BBP.Assets.LoadSprite("icon.png");

AudioClip clip =
    await BBP.Assets.LoadAudioClipAsync("sound.wav");
```

## Debugging Tools

- Console (`SHIFT + F1`)
- Debug Menu (`SHIFT + F2`)

# Installation

## Requirements

- Baldi's Basics Plus
- BepInEx 5.x
- Harmony

## Install BepInEx

Download:

https://github.com/BepInEx/BepInEx/releases

Extract into the Baldi's Basics Plus installation folder.

## Install BBPGlue

Copy:

```text
BBPGlue.dll
```

into:

```text
BepInEx/plugins/
```

Launch the game.

Press:

```text
SHIFT + F1
```

If the console appears, BBPGlue is loaded successfully.

# Your First Mod

Create a normal BepInEx plugin.

```csharp
using BepInEx;
using BBPGlue.API;

[BepInPlugin(
    "com.example.mymod",
    "My Mod",
    "1.0.0"
)]
[BepInDependency("com.wufhex.BBPGlue")]
public sealed class Plugin : BaseUnityPlugin
{
    private void Awake()
    {
        BBPConsole.Log("Hello BBPGlue!");
    }
}
```
# Listening To Events

Most mods begin by subscribing to callbacks.

```csharp
private void Awake()
{
    BBP.Callbacks.Items.OnItemUse += item =>
    {
        BBPConsole.Log(
            $"Used: {item.NameKey}"
        );
    };
}
```

# Accessing Game Systems

```csharp
BBP.Player.Stamina = 100f;

BBP.Environment.MakeNoise(
    BBP.Player.Position,
    127
);
```

```csharp
BBPRoom? room =
    BBP.Environment.GetPlayerRoom();
```

```csharp
BBPDoor? door =
    BBP.Environment.GetClosestDoor();
```

# Runtime Authoring

BBPGlue can clone and register content at runtime.

```csharp
BBP.Callbacks.Game.OnLevelReady += () =>
{
    BBP.Prefabs.Refresh();

    BBP.Authoring.CloneNpc(
        "my_mod:old_principal",
        "Principal",
        npc =>
        {
            npc.Name = "Old Principal";
            npc.MaxSpeed = 10f;
        }
    );
};
```

Spawn later:

```csharp
BBP.Authoring.Spawn(
    "my_mod:old_principal",
    BBP.Player.Position
);
```

# Custom Behavior

Callbacks can alter or cancel built-in behavior.

```csharp
BBP.Callbacks.Items.OnItemUse += item =>
{
    if (item.NameKey != "Monster BSoda")
        return;

    BBPCallbacks.Cancel();

    BBP.Items.RemoveSlot(
        BBP.Items.SelectedSlot
    );

    BBP.Player.RunSpeed *= 4f;
};
```

# Example Project

See:

```text
BBPGlue/src/Tests/CustomPrefabsTest.cs
```

The example demonstrates:

- Custom NPC creation
- Custom item creation
- Runtime sprite replacement
- Callback-driven behavior
- Cancelable item usage
- Timed effects

# Architecture

## API

Public modding surface.

Examples:

- BBPPlayer
- BBPNpc
- BBPEnvironment
- BBPEvents

## Core

Internal systems.

Examples:

- ReflectionUtil
- ReflectionCache
- Harmony patches
- Runtime helpers

## Wrappers

Typed wrappers around BB+ classes.

Examples:

- Door
- Room
- Pickup
- ItemObject
- RandomEvent

# Compatibility

BBPGlue attempts to remain compatible with future BB+ updates whenever possible.

If the game version differs from the version BBPGlue was built against, a warning may appear in the console.

This warning is informational and does not necessarily indicate a problem.

Alpha Limitations:
- API may change
- Some callbacks may be incomplete or untested
- Compatibility only tested on BB+ 0.14.2
- Advanced custom NPC behavior is experimental and might be untested

# FAQ
###### (no one asked but someone might)

### Does BBPGlue modify game files?

No.

### Does BBPGlue redistribute BB+ assets?

No.

### Can I create custom NPCs?

Yes, through runtime authoring.

### Can I create custom items?

Yes.

### Do I Need Harmony or Custom Patches?

Usually, no.

BBPGlue already uses Harmony internally and exposes most common game functionality through wrappers, callbacks, and helper APIs. For many mods, you can create custom content, listen for game events, and modify gameplay behavior without writing a single Harmony patch.

If the functionality you need is not currently exposed by BBPGlue, you have several options:

1. Open an Issue and request the feature.
2. Submit a Pull Request.
3. Use Harmony directly in your mod.
4. Use utilities from BBPGlue.Core to simplify reflection and runtime interaction.

BBPGlue is designed to reduce Harmony and reflection boilerplate, not prevent advanced users from accessing the game's internals when necessary.

# License

BBPGlue is licensed under the MIT License. Attribution is appreciated but not required.
