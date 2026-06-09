using UnityEngine;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Provides access to overall game state and control functions.
    /// </summary>
    public sealed class BBPGame
    {
        /// <summary>
        /// Whether the application is currently in a playable game session.
        /// </summary>
        public bool IsInGame => GameContext.IsInGame;

        /// <summary>
        /// Core game manager object providing core gameplay operations.
        /// </summary>
        public object? CoreManager => GameContext.GetCoreGameManager();
        /// <summary>
        /// Base manager that exposes higher-level game facilities.
        /// </summary>
        public object? BaseManager => GameContext.GetBaseGameManager();

        /// <summary>
        /// Application/game version string.
        /// </summary>
        public string  Version     => Application.version;

        /// <summary>
        /// Gets or sets whether the game is paused. Setting will open/close the pause screen.
        /// </summary>
        public bool Paused
        {
            get => ReflectionUtil.GetProperty<bool>(CoreManager, "Paused");
            set => Pause(value, true);
        }

        /// <summary>
        /// Gets or sets whether pausing is disabled.
        /// </summary>
        public bool DisablePause
        {
            get => ReflectionUtil.GetField<bool>(CoreManager, "disablePause");
            set => ReflectionUtil.SetField(CoreManager, "disablePause", value);
        }

        /// <summary>
        /// Whether the game is ready to start (level initialization complete).
        /// </summary>
        public bool ReadyToStart
        {
            get => ReflectionUtil.GetField<bool>(CoreManager, "readyToStart");
            set => ReflectionUtil.SetField(CoreManager, "readyToStart", value);
        }

        /// <summary>
        /// Whether the map UI is currently open.
        /// </summary>
        public bool MapOpen => ReflectionUtil.GetProperty<bool>(CoreManager, "MapOpen");
        /// <summary>
        /// Whether a map is available in the current context.
        /// </summary>
        public bool MapAvailable => ReflectionUtil.GetProperty<bool>(CoreManager, "MapAvaialble");

        /// <summary>
        /// Number of player slots supported by the game.
        /// </summary>
        public int TotalPlayers => ReflectionUtil.GetProperty<int>(CoreManager, "TotalPlayers");

        /// <summary>
        /// Lives remaining.
        /// </summary>
        public int Lives => ReflectionUtil.GetProperty<int>(CoreManager, "Lives");
        /// <summary>
        /// Current attempt count.
        /// </summary>
        public int Attempts => ReflectionUtil.GetProperty<int>(CoreManager, "Attempts");

        /// <summary>
        /// Current game seed value.
        /// </summary>
        public int Seed => ReflectionUtil.Call<int>(CoreManager, "Seed");

        /// <summary>
        /// Current grade numeric value.
        /// </summary>
        public int GradeValue
        {
            get => ReflectionUtil.GetProperty<int>(CoreManager, "GradeVal");
            set => ReflectionUtil.SetProperty(CoreManager, "GradeVal", value);
        }

        /// <summary>
        /// Current grade string.
        /// </summary>
        public string Grade => ReflectionUtil.GetProperty<string>(CoreManager, "Grade") ?? "";

        /// <summary>
        /// Currently loaded level index.
        /// </summary>
        public int CurrentLevel
        {
            get => ReflectionUtil.GetProperty<int>(BaseManager, "CurrentLevel");
            set => ReflectionUtil.SetProperty(BaseManager, "CurrentLevel", value);
        }

        /// <summary>
        /// Number of notebooks found this run.
        /// </summary>
        public int FoundNotebooks => ReflectionUtil.GetProperty<int>(BaseManager, "FoundNotebooks");
        /// <summary>
        /// Total notebooks required for the level.
        /// </summary>
        public int NotebookTotal => ReflectionUtil.GetProperty<int>(BaseManager, "NotebookTotal");
        /// <summary>
        /// Whether all notebooks have been found.
        /// </summary>
        public bool AllNotebooksFound => ReflectionUtil.GetProperty<bool>(BaseManager, "AllNotebooksFound");

        /// <summary>
        /// Current anger value added by notebook collection.
        /// </summary>
        public float NotebookAngerValue => ReflectionUtil.GetProperty<float>(BaseManager, "NotebookAngerVal");

        /// <summary>
        /// Environment controller from the base manager.
        /// </summary>
        public object? EnvironmentController => ReflectionUtil.GetProperty<object>(BaseManager, "Ec");

        /// <summary>
        /// Raw scene object currently loaded.
        /// </summary>
        public object? CurrentSceneObject => ReflectionUtil.GetField<object>(GameContext.GetCoreGameManager(), "sceneObject");
        /// <summary>
        /// Strongly-typed scene wrapper for the current scene.
        /// </summary>
        public BBPScene Scene => new BBPScene(CurrentSceneObject);

        /// <summary>
        /// Refreshes the game context.
        /// </summary>
        public void Refresh()
        {
            GameContext.Refresh();
        }

        /// <summary>
        /// Retrieves a player object by index from the core manager.
        /// </summary>
        /// <param name="index">Player index.</param>
        /// <returns>The player object or null.</returns>
        public object? GetPlayer(int index)
        {
            return ReflectionUtil.Call<object>(CoreManager, "GetPlayer", index);
        }

        /// <summary>
        /// Retrieves a camera object by index.
        /// </summary>
        /// <param name="index">Camera index.</param>
        /// <returns>The camera object or null.</returns>
        public object? GetCamera(int index)
        {
            return ReflectionUtil.Call<object>(CoreManager, "GetCamera", index);
        }

        /// <summary>
        /// Retrieves a HUD object by index.
        /// </summary>
        /// <param name="index">HUD index.</param>
        /// <returns>The HUD object or null.</returns>
        public object? GetHud(int index)
        {
            return ReflectionUtil.Call<object>(CoreManager, "GetHud", index);
        }

        /// <summary>
        /// Gets the player's points total.
        /// </summary>
        /// <param name="player">Player index.</param>
        /// <returns>Points total.</returns>
        public int GetPoints(int player = 0)
        {
            return ReflectionUtil.Call<int>(CoreManager, "GetPoints", player);
        }

        /// <summary>
        /// Gets points earned this level for the player.
        /// </summary>
        /// <param name="player">Player index.</param>
        /// <returns>Points this level.</returns>
        public int GetPointsThisLevel(int player = 0)
        {
            return ReflectionUtil.Call<int>(CoreManager, "GetPointsThisLevel", player);
        }

        /// <summary>
        /// Adds points for a player with optional animation.
        /// </summary>
        /// <param name="points">Points to add.</param>
        /// <param name="player">Player index.</param>
        /// <param name="playAnimation">Whether to play points animation.</param>
        public void AddPoints(int points, int player = 0, bool playAnimation = true)
        {
            ReflectionUtil.Call(CoreManager, "AddPoints", points, player, playAnimation);
        }

        /// <summary>
        /// Adds points with full parameterization.
        /// </summary>
        /// <param name="points">Points to add.</param>
        /// <param name="player">Player index.</param>
        /// <param name="playAnimation">Whether to play points animation.</param>
        /// <param name="includeInLevelTotal">Whether to count toward level total.</param>
        /// <param name="multiply">Whether to multiply the amount by a multiplier.
        /// </param>
        public void AddPoints(
            int points,
            int player,
            bool playAnimation,
            bool includeInLevelTotal,
            bool multiply
        )
        {
            ReflectionUtil.Call(
                CoreManager,
                "AddPoints",
                points,
                player,
                playAnimation,
                includeInLevelTotal,
                multiply
            );
        }

        /// <summary>
        /// Convenience overload to give points to player 0.
        /// </summary>
        /// <param name="amount">Amount of points.</param>
        public void GivePoints(int amount)
        {
            AddPoints(amount, 0, true, true, true);
        }

        /// <summary>
        /// Convenience overload to give points to a specific player.
        /// </summary>
        /// <param name="amount">Amount of points.</param>
        /// <param name="player">Player index.</param>
        public void GivePoints(int amount, int player)
        {
            AddPoints(amount, player, true, true, true);
        }

        /// <summary>
        /// Full-parameter give points call.
        /// </summary>
        public void GivePoints(
            int amount,
            int player,
            bool playAnimation,
            bool includeInLevelTotal,
            bool multiply
        )
        {
            AddPoints(amount, player, playAnimation, includeInLevelTotal, multiply);
        }

        /// <summary>
        /// Adds lives to the player.
        /// </summary>
        /// <param name="amount">Number of lives to add.</param>
        public void AddLives(int amount)
        {
            ReflectionUtil.Call(CoreManager, "AddLives", amount);
        }

        /// <summary>
        /// Sets the player's lives and optionally overrides life mode.
        /// </summary>
        /// <param name="lives">Lives to set.</param>
        /// <param name="overrideLifeMode">Whether to override life mode.</param>
        public void SetLives(int lives, bool overrideLifeMode = true)
        {
            ReflectionUtil.Call(CoreManager, "SetLives", lives, overrideLifeMode);
        }

        /// <summary>
        /// Sets the number of attempts.
        /// </summary>
        public void SetAttempts(int attempts)
        {
            ReflectionUtil.Call(CoreManager, "SetAttempts", attempts);
        }

        /// <summary>
        /// Sets the deterministic seed used by the game.
        /// </summary>
        public void SetSeed(int seed)
        {
            ReflectionUtil.Call(CoreManager, "SetSeed", seed);
        }

        /// <summary>
        /// Sets a random seed.
        /// </summary>
        public void SetRandomSeed()
        {
            ReflectionUtil.Call(CoreManager, "SetRandomSeed");
        }

        /// <summary>
        /// Adds a YTP multiplier value.
        /// </summary>
        public void AddYtpMultiplier(float value)
        {
            ReflectionUtil.Call(CoreManager, "AddMultiplier", value);
        }

        /// <summary>
        /// Removes a YTP multiplier value.
        /// </summary>
        public void RemoveYtpMultiplier(float value)
        {
            ReflectionUtil.Call(CoreManager, "RemoveMultiplier", value);
        }

        /// <summary>
        /// Pauses or unpauses the game.
        /// </summary>
        /// <param name="paused">True to pause; false to unpause.</param>
        /// <param name="openScreen">True to open the pause screen when pausing.</param>
        public void Pause(bool paused, bool openScreen = true)
        {
            bool currentlyPaused = ReflectionUtil.GetProperty<bool>(CoreManager, "Paused");

            if (currentlyPaused != paused)
                ReflectionUtil.Call(CoreManager, "Pause", openScreen);
        }

        /// <summary>
        /// Toggles the pause screen on.
        /// </summary>
        public void TogglePauseScreen()
        {
            ReflectionUtil.Call(CoreManager, "Pause", true);
        }

        /// <summary>
        /// Opens the map UI.
        /// </summary>
        public void OpenMap(bool toMap = true)
        {
            ReflectionUtil.Call(CoreManager, "OpenMap", toMap);
        }

        /// <summary>
        /// Closes the map UI.
        /// </summary>
        public void CloseMap()
        {
            ReflectionUtil.Call(CoreManager, "CloseMap");
        }

        /// <summary>
        /// Toggles the map UI.
        /// </summary>
        public void ToggleMap(bool openMap = true)
        {
            ReflectionUtil.Call(CoreManager, "ToggleYctpScreen", openMap);
        }

        /// <summary>
        /// Saves the current game state.
        /// </summary>
        public void Save()
        {
            ReflectionUtil.Call(CoreManager, "Save");
        }

        /// <summary>
        /// Saves the game and quits to desktop/menu.
        /// </summary>
        public void SaveAndQuit()
        {
            ReflectionUtil.Call(CoreManager, "SaveAndQuit");
        }

        /// <summary>
        /// Quits the game immediately.
        /// </summary>
        public void Quit()
        {
            ReflectionUtil.Call(CoreManager, "Quit");
        }

        /// <summary>
        /// Returns to the main menu.
        /// </summary>
        public void ReturnToMenu()
        {
            ReflectionUtil.Call(CoreManager, "ReturnToMenu");
        }

        /// <summary>
        /// Resets all cameras to default states.
        /// </summary>
        public void ResetCameras()
        {
            ReflectionUtil.Call(CoreManager, "ResetCameras");
        }

        /// <summary>
        /// Resets shader-related state.
        /// </summary>
        public void ResetShaders()
        {
            ReflectionUtil.Call(CoreManager, "ResetShaders");
        }

        /// <summary>
        /// Begins normal gameplay from the base manager.
        /// </summary>
        public void BeginPlay()
        {
            ReflectionUtil.Call(BaseManager, "BeginPlay");
        }

        /// <summary>
        /// Starts the spook mode gameplay.
        /// </summary>
        public void BeginSpoopMode()
        {
            ReflectionUtil.Call(BaseManager, "BeginSpoopMode");
        }

        /// <summary>
        /// Programmatically collects notebooks.
        /// </summary>
        public void CollectNotebooks(int count)
        {
            ReflectionUtil.Call(BaseManager, "CollectNotebooks", count);
        }

        /// <summary>
        /// Increases the notebook total by the given count.
        /// </summary>
        public void AddNotebookTotal(int count)
        {
            ReflectionUtil.Call(BaseManager, "AddNotebookTotal", count);
        }

        /// <summary>
        /// Adds anger to Baldi.
        /// </summary>
        public void AngerBaldi(float amount)
        {
            ReflectionUtil.Call(BaseManager, "AngerBaldi", amount);
        }

        /// <summary>
        /// Temporarily pleads with Baldi for the given time and optional sticker reward.
        /// </summary>
        public void PleaseBaldi(float time, bool rewardSticker)
        {
            ReflectionUtil.Call(BaseManager, "PleaseBaldi", time, rewardSticker);
        }

        /// <summary>
        /// Finishes the current level.
        /// </summary>
        public void FinishLevel()
        {
            ReflectionUtil.Call(BaseManager, "FinishLevel");
        }

        /// <summary>
        /// Restarts the current level.
        /// </summary>
        public void RestartLevel()
        {
            ReflectionUtil.Call(BaseManager, "RestartLevel");
        }

        /// <summary>
        /// Loads the next level via the base manager.
        /// </summary>
        public void LoadNextLevel()
        {
            ReflectionUtil.Call(BaseManager, "LoadNextLevel");
        }
    }
}