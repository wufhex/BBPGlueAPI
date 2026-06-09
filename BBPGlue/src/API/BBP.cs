using BepInEx.Logging;

namespace BBPGlue.API
{
    /// <summary>
    /// Central entrypoint for the modding API providing access to common subsystems.
    /// </summary>
    public static class BBP
    {
        internal static ManualLogSource? Logger;

        /// <summary>
        /// Provides access to player-related API surfaces and utilities.
        /// </summary>
        public static BBPPlayer       Player       { get; } = new BBPPlayer();
        /// <summary>
        /// Provides access to game-wide state and controls.
        /// </summary>
        public static BBPGame         Game         { get; } = new BBPGame();
        /// <summary>
        /// Event dispatch surface exposing runtime events for mods to subscribe to.
        /// </summary>
        public static BBPEvents       Events       { get; } = new BBPEvents();
        /// <summary>
        /// Environment and world-related utilities and state.
        /// </summary>
        public static BBPEnvironment  Environment  { get; } = new BBPEnvironment();
        /// <summary>
        /// Collection of runtime entity helpers and lookup functions.
        /// </summary>
        public static BBPEntities     Entities     { get; } = new BBPEntities();
        /// <summary>
        /// Provides access to random seed and deterministic utilities.
        /// </summary>
        public static BBPSeed         Seed         { get; } = new BBPSeed();
        /// <summary>
        /// Item management and lookup utilities.
        /// </summary>
        public static BBPItems        Items        { get; } = new BBPItems();
        /// <summary>
        /// Access to commonly used prefab objects exposed by the game.
        /// </summary>
        public static BBPPrefabs      Prefabs      { get; } = new BBPPrefabs();
        /// <summary>
        /// HUD-related API surface for manipulating on-screen UI.
        /// </summary>
        public static BBPHud          Hud          { get; } = new BBPHud();
        /// <summary>
        /// Centralized callback groups for subscribing to game events.
        /// </summary>
        public static BBPCallbacks    Callbacks    { get; } = new BBPCallbacks();
        /// <summary>
        /// Asset loading and management API (textures, audio, prefabs).
        /// </summary>
        public static BBPAssetLoader  Assets       { get; } = new BBPAssetLoader();
        /// <summary>
        /// Authoring API for cloning and creating new prefabs (used for custom pickups, items, and NPCs).
        /// </summary>
        public static BBPAuthoring    Authoring    { get; } = new BBPAuthoring();

        /// <summary>
        /// Experimental API surfaces that may change between versions.
        /// </summary>
        public static BBPExperimental Experimental { get; } = new BBPExperimental();

        internal static void Update()
        {
            Player.Refresh();
            Game.Refresh();
            Events.Refresh();
            Environment.Refresh();
            Prefabs.Update();
        }
        
        /// <summary>
        /// Writes an informational message to the API console logger.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Log(string message)
        {
            BBPConsole.Log(message);
        }

        /// <summary>
        /// Writes a warning message to the API console logger.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        public static void Warn(string message)
        {
            BBPConsole.Warn(message);
        }

        /// <summary>
        /// Writes an error message to the API console logger.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        public static void Error(string message)
        {
            BBPConsole.Error(message);
        }
    }
}