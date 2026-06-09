using System;
using UnityEngine;

namespace BBPGlue.API
{
    /// <summary>
    /// Contains all callback groups exposed by BBPGlue.
    /// </summary>
    public sealed class BBPCallbacks
    {
        /// <summary>
        /// Game and level lifecycle callbacks.
        /// </summary>
        public GameCallbacks Game { get; } = new GameCallbacks();

        /// <summary>
        /// Player state, movement, and inventory callbacks.
        /// </summary>
        public PlayerCallbacks Player { get; } = new PlayerCallbacks();

        /// <summary>
        /// NPC and entity state callbacks.
        /// </summary>
        public EntityCallbacks Entity { get; } = new EntityCallbacks();

        /// <summary>
        /// Item, pickup, locker, and notebook callbacks.
        /// </summary>
        public ItemCallbacks Items { get; } = new ItemCallbacks();

        /// <summary>
        /// Environment, room, door, elevator, and random event callbacks.
        /// </summary>
        public WorldCallbacks World { get; } = new WorldCallbacks();

        /// <summary>
        /// HUD, tooltip, announcement, and map callbacks.
        /// </summary>
        public HudCallbacks Hud { get; } = new HudCallbacks();

        private static readonly System.Threading.ThreadLocal<BBPCallbackContext?> _current =
            new System.Threading.ThreadLocal<BBPCallbackContext?>();

        /// <summary>
        /// Gets the currently running callback context for this thread.
        /// Returns null when no cancelable callback is being raised.
        /// </summary>
        public static BBPCallbackContext? Current => _current.Value;

        /// <summary>
        /// Callbacks for general game and level lifecycle events.
        /// </summary>
        public sealed class GameCallbacks
        {
            /// <summary>
            /// Raised when the game start flow begins.
            /// </summary>
            public event Action? OnGameStart;

            /// <summary>
            /// Raised when the game pause state changes.
            /// </summary>
            public event Action? OnGamePauseChanged;

            /// <summary>
            /// Raised when the game requests a level load.
            /// </summary>
            public event Action? OnLevelLoadRequested;

            /// <summary>
            /// Raised when the current level has finished its ready/setup step.
            /// </summary>
            public event Action? OnLevelReady;

            /// <summary>
            /// Raised when the current level is ending.
            /// </summary>
            public event Action? OnLevelExit;

            /// <summary>
            /// Raised when the game seed is set.
            /// </summary>
            public event Action<int>? OnSeedSet;

            /// <summary>
            /// Raised when saved game data is loaded.
            /// </summary>
            public event Action? OnSaveLoaded;

            /// <summary>
            /// Raised when saved game data is written.
            /// </summary>
            public event Action? OnSaveWritten;

            internal void RaiseGameStart() => OnGameStart?.Invoke();
            internal void RaiseGamePauseChanged() => OnGamePauseChanged?.Invoke();
            internal void RaiseLevelLoadRequested() => OnLevelLoadRequested?.Invoke();
            internal void RaiseLevelReady() => OnLevelReady?.Invoke();
            internal void RaiseLevelExit() => OnLevelExit?.Invoke();
            internal void RaiseSeedSet(int seed) => OnSeedSet?.Invoke(seed);
            internal void RaiseSaveLoaded() => OnSaveLoaded?.Invoke();
            internal void RaiseSaveWritten() => OnSaveWritten?.Invoke();
        }

        /// <summary>
        /// Callbacks for player state, movement, rules, and inventory events.
        /// </summary>
        public sealed class PlayerCallbacks
        {
            /// <summary>
            /// Raised when a player is spawned.
            /// </summary>
            public event Action<BBPPlayerRef>? OnPlayerSpawn;

            /// <summary>
            /// Raised when a player is teleported.
            /// </summary>
            public event Action<BBPPlayerRef>? OnPlayerTeleport;

            /// <summary>
            /// Raised when a player breaks a rule.
            /// </summary>
            public event Action<string>? OnPlayerRuleBreak;

            /// <summary>
            /// Raised when the player's guilt is cleared.
            /// </summary>
            public event Action? OnPlayerGuiltClear;

            /// <summary>
            /// Raised when the player's hidden state changes.
            /// </summary>
            public event Action<bool>? OnPlayerHiddenChanged;

            /// <summary>
            /// Raised when the player's stamina value changes.
            /// </summary>
            public event Action<float>? OnPlayerStaminaChanged;

            /// <summary>
            /// Raised when the player uses an item.
            /// </summary>
            public event Action<BBPItemObject>? OnPlayerItemUsed;

            /// <summary>
            /// Raised when an item is added to the player's inventory.
            /// </summary>
            public event Action<BBPItemObject>? OnPlayerItemAdded;

            /// <summary>
            /// Raised when an item is removed from the player's inventory.
            /// </summary>
            public event Action<BBPItemObject>? OnPlayerItemRemoved;

            /// <summary>
            /// Raised when the selected inventory slot changes.
            /// </summary>
            public event Action<int>? OnPlayerSlotChanged;

            /// <summary>
            /// Raised when the player is sent to detention.
            /// </summary>
            public event Action<BBPPrincipal, bool>? OnPlayerDetention;

            internal void RaisePlayerSpawn(object raw) => OnPlayerSpawn?.Invoke(new BBPPlayerRef(raw));
            internal void RaisePlayerTeleport(object raw) => OnPlayerTeleport?.Invoke(new BBPPlayerRef(raw));
            internal void RaisePlayerRuleBreak(string rule) => OnPlayerRuleBreak?.Invoke(rule);
            internal void RaisePlayerGuiltClear() => OnPlayerGuiltClear?.Invoke();
            internal void RaisePlayerHiddenChanged(bool hidden) => OnPlayerHiddenChanged?.Invoke(hidden);
            internal void RaisePlayerStaminaChanged(float stamina) => OnPlayerStaminaChanged?.Invoke(stamina);
            internal void RaisePlayerItemUsed(object? item) => OnPlayerItemUsed?.Invoke(new BBPItemObject(item));
            internal void RaisePlayerItemAdded(object? item) => OnPlayerItemAdded?.Invoke(new BBPItemObject(item));
            internal void RaisePlayerItemRemoved(object? item) => OnPlayerItemRemoved?.Invoke(new BBPItemObject(item));
            internal void RaisePlayerSlotChanged(int slot) => OnPlayerSlotChanged?.Invoke(slot);
            internal void RaisePlayerDetention(BBPPrincipal principal, bool validCollision) => OnPlayerDetention?.Invoke(principal, validCollision);
        }

        /// <summary>
        /// Callbacks for NPCs and generic entities.
        /// </summary>
        public sealed class EntityCallbacks
        {
            /// <summary>
            /// Raised when an NPC is spawned.
            /// </summary>
            public event Action<BBPNpc>? OnNpcSpawn;

            /// <summary>
            /// Raised when an NPC is despawned.
            /// </summary>
            public event Action<BBPNpc>? OnNpcDespawn;

            /// <summary>
            /// Raised when an NPC hears a noise.
            /// </summary>
            public event Action<BBPNpc>? OnNpcHearNoise;

            /// <summary>
            /// Raised when an NPC sees the player.
            /// </summary>
            public event Action<BBPNpc>? OnNpcSightPlayer;

            /// <summary>
            /// Raised when an NPC loses sight of the player.
            /// </summary>
            public event Action<BBPNpc>? OnNpcLosePlayer;

            /// <summary>
            /// Raised when an entity is spawned.
            /// </summary>
            public event Action<BBPEntity>? OnEntitySpawn;

            /// <summary>
            /// Raised when an entity is teleported.
            /// </summary>
            public event Action<BBPEntity>? OnEntityTeleport;

            /// <summary>
            /// Raised when an entity's frozen state changes.
            /// </summary>
            public event Action<BBPEntity, bool>? OnEntityFrozenChanged;

            /// <summary>
            /// Raised when an entity's hidden state changes.
            /// </summary>
            public event Action<BBPEntity, bool>? OnEntityHiddenChanged;

            /// <summary>
            /// Raised when an entity becomes squished.
            /// </summary>
            public event Action<BBPEntity>? OnEntitySquished;

            /// <summary>
            /// Raised when an entity is unsquished.
            /// </summary>
            public event Action<BBPEntity>? OnEntityUnsquished;

            /// <summary>
            /// Raised when an NPC updates.
            /// </summary>
            public event Action<BBPNpc>? OnNpcUpdate;

            /// <summary>
            /// Raised when an NPC begins overlapping another entity trigger.
            /// Example: Playtime catches the player and starts jump rope.
            /// </summary>
            public event Action<BBPNpc, object, Collider, bool>? OnNpcTriggerEnter;

            /// <summary>
            /// Raised when an NPC stops overlapping another entity trigger.
            /// Example: Playtime keeps the player attached to the jump rope.
            /// </summary>
            public event Action<BBPNpc, object, Collider, bool>? OnNpcTriggerExit;

            /// <summary>
            /// Raised each frame while an NPC remains overlapping another entity trigger.
            /// Example: Playtime finishes and releases the player.
            /// </summary>
            public event Action<BBPNpc, object, Collider, bool>? OnNpcTriggerStay;

            /// <summary>
            /// Raised when an NPC makes a navigation decision.
            /// </summary>
            public event Action<BBPNpc>? OnNpcNavigationDecision;

            /// <summary>
            /// Raised when an NPC receives a target position.
            /// </summary>
            public event Action<BBPNpc, Vector3>? OnNpcTargetPosition;

            internal void RaiseNpcSpawn(object raw) => OnNpcSpawn?.Invoke(new BBPNpc(raw));
            internal void RaiseNpcDespawn(object raw) => OnNpcDespawn?.Invoke(new BBPNpc(raw));
            internal void RaiseNpcHearNoise(object raw) => OnNpcHearNoise?.Invoke(new BBPNpc(raw));
            internal void RaiseNpcSightPlayer(object raw) => OnNpcSightPlayer?.Invoke(new BBPNpc(raw));
            internal void RaiseNpcLosePlayer(object raw) => OnNpcLosePlayer?.Invoke(new BBPNpc(raw));

            internal void RaiseEntitySpawn(object raw) => OnEntitySpawn?.Invoke(new BBPEntity(raw));
            internal void RaiseEntityTeleport(object raw) => OnEntityTeleport?.Invoke(new BBPEntity(raw));
            internal void RaiseEntityFrozenChanged(object raw, bool frozen) => OnEntityFrozenChanged?.Invoke(new BBPEntity(raw), frozen);
            internal void RaiseEntityHiddenChanged(object raw, bool hidden) => OnEntityHiddenChanged?.Invoke(new BBPEntity(raw), hidden);
            internal void RaiseEntitySquished(object raw) => OnEntitySquished?.Invoke(new BBPEntity(raw));
            internal void RaiseEntityUnsquished(object raw) => OnEntityUnsquished?.Invoke(new BBPEntity(raw));

            internal void RaiseNpcTriggerEnter(object rawNpc, object rawEntity, Collider other, bool validCollision) 
                => OnNpcTriggerEnter?.Invoke(new BBPNpc(rawNpc), rawEntity, other, validCollision);
            internal void RaiseNpcTriggerExit(object rawNpc, object rawEntity, Collider other, bool validCollision)
	            => OnNpcTriggerExit?.Invoke(new BBPNpc(rawNpc), rawEntity, other, validCollision);
            internal void RaiseNpcTriggerStay(object rawNpc, object rawEntity, Collider other, bool validCollision)
	            => OnNpcTriggerStay?.Invoke(new BBPNpc(rawNpc), rawEntity, other, validCollision);

            internal void RaiseNpcNavigationDecision(object raw) => OnNpcNavigationDecision?.Invoke(new BBPNpc(raw));
            internal void RaiseNpcTargetPosition(object raw, Vector3 pos) => OnNpcTargetPosition?.Invoke(new BBPNpc(raw), pos);
            internal void RaiseNpcUpdate(object raw) => OnNpcUpdate?.Invoke(new BBPNpc(raw));
        }

        /// <summary>
        /// Callbacks for item objects, pickups, lockers, and notebooks.
        /// </summary>
        public sealed class ItemCallbacks
        {
            /// <summary>
            /// Raised when a pickup is spawned in the world.
            /// </summary>
            public event Action<BBPPickup>? OnPickupSpawn;

            /// <summary>
            /// Raised when a pickup is collected.
            /// </summary>
            public event Action<BBPPickup>? OnPickupCollect;

            /// <summary>
            /// Raised when a pickup is despawned.
            /// </summary>
            public event Action<BBPPickup>? OnPickupDespawn;

            /// <summary>
            /// Raised when an item object is respawned.
            /// </summary>
            public event Action<BBPItemObject>? OnItemRespawn;

            /// <summary>
            /// Raised when an item stored in a locker changes.
            /// </summary>
            public event Action<int, BBPItemObject>? OnLockerItemChanged;

            /// <summary>
            /// Raised when a notebook is collected.
            /// 
            /// This event might be triggered twice times due to game behavior.
            /// 
            /// </summary>
            public event Action<object?>? OnNotebookCollect;

            /// <summary>
            /// Raised when the player successfully uses an item.
            /// </summary>
            public event Action<BBPItemObject>? OnItemUse;

            /// <summary>
            /// Raised when a pickup is clicked.
            /// </summary>
            public event Action<BBPPickup, int>? OnPickupClicked;

            internal void RaisePickupSpawn(object? pickup) => OnPickupSpawn?.Invoke(new BBPPickup(pickup));
            internal void RaisePickupCollect(object? pickup) => OnPickupCollect?.Invoke(new BBPPickup(pickup));
            internal void RaisePickupDespawn(object? pickup) => OnPickupDespawn?.Invoke(new BBPPickup(pickup));
            internal void RaiseItemRespawn(object? item) => OnItemRespawn?.Invoke(new BBPItemObject(item));
            internal void RaiseLockerItemChanged(int slot, object? item) => OnLockerItemChanged?.Invoke(slot, new BBPItemObject(item));
            internal void RaiseNotebookCollect(object? notebook) => OnNotebookCollect?.Invoke(notebook);
            internal void RaiseItemUse(object? item) => OnItemUse?.Invoke(new BBPItemObject(item));
            internal void RaisePickupClicked(object? pickup, int player) => OnPickupClicked?.Invoke(new BBPPickup(pickup), player);
        }

        /// <summary>
        /// Callbacks for environment, rooms, random events, doors, windows, and elevators.
        /// </summary>
        public sealed class WorldCallbacks
        {
            /// <summary>
            /// Raised when a random event is queued.
            /// </summary>
            public event Action<BBPRandomEvent>? OnRandomEventQueued;

            /// <summary>
            /// Raised when a random event announcement sound is queued or played.
            /// </summary>
            public event Action<BBPSoundObject>? OnRandomEventAnnounced;

            /// <summary>
            /// Raised when a random event begins.
            /// </summary>
            public event Action<BBPRandomEvent>? OnRandomEventBegin;

            /// <summary>
            /// Raised when a random event ends.
            /// </summary>
            public event Action<BBPRandomEvent>? OnRandomEventEnd;

            /// <summary>
            /// Raised when noise is made in the environment.
            /// </summary>
            public event Action<UnityEngine.Vector3, int>? OnNoiseMade;

            /// <summary>
            /// Raised when the school is closed by the game.
            /// </summary>
            public event Action? OnSchoolClosed;

            /// <summary>
            /// Raised when all lights are changed on or off.
            /// </summary>
            public event Action<bool>? OnLightsChanged;

            /// <summary>
            /// Raised when light flickering is enabled or disabled.
            /// </summary>
            public event Action<bool>? OnLightsFlickerChanged;

            /// <summary>
            /// Raised when the level time limit is set.
            /// </summary>
            public event Action<float>? OnTimeLimitSet;

            /// <summary>
            /// Raised when the level timer warning is shown.
            /// </summary>
            public event Action? OnTimerWarning;

            /// <summary>
            /// Raised when the player enters a room.
            /// </summary>
            public event Action<BBPRoom>? OnRoomEnter;

            /// <summary>
            /// Raised when the player exits a room.
            /// </summary>
            public event Action<BBPRoom>? OnRoomExit;

            /// <summary>
            /// Raised when a door opens.
            /// </summary>
            public event Action<BBPDoor>? OnDoorOpen;

            /// <summary>
            /// Raised when a door closes.
            /// </summary>
            public event Action<BBPDoor>? OnDoorClose;

            /// <summary>
            /// Raised when a window breaks.
            /// </summary>
            public event Action<BBPDoor>? OnWindowBreak;

            /// <summary>
            /// Raised when the player enters an elevator.
            /// </summary>
            public event Action<BBPElevator>? OnElevatorEnter;

            /// <summary>
            /// Raised when the player exits an elevator.
            /// 
            /// This event might be triggered multiple times due to game behavior.
            /// 
            /// </summary>
            public event Action<BBPElevator>? OnElevatorExit;

            internal void RaiseRandomEventQueued(object? ev) => OnRandomEventQueued?.Invoke(new BBPRandomEvent(ev));
            internal void RaiseRandomEventAnnounced(object? sound) => OnRandomEventAnnounced?.Invoke(new BBPSoundObject(sound));
            internal void RaiseRandomEventBegin(object? ev) => OnRandomEventBegin?.Invoke(new BBPRandomEvent(ev));
            internal void RaiseRandomEventEnd(object? ev) => OnRandomEventEnd?.Invoke(new BBPRandomEvent(ev));

            internal void RaiseNoiseMade(UnityEngine.Vector3 pos, int value) => OnNoiseMade?.Invoke(pos, value);
            internal void RaiseSchoolClosed() => OnSchoolClosed?.Invoke();
            internal void RaiseLightsChanged(bool on) => OnLightsChanged?.Invoke(on);
            internal void RaiseLightsFlickerChanged(bool on) => OnLightsFlickerChanged?.Invoke(on);
            internal void RaiseTimeLimitSet(float time) => OnTimeLimitSet?.Invoke(time);
            internal void RaiseTimerWarning() => OnTimerWarning?.Invoke();
            internal void RaiseRoomEnter(object? room) => OnRoomEnter?.Invoke(new BBPRoom(room));
            internal void RaiseRoomExit(object? room) => OnRoomExit?.Invoke(new BBPRoom(room));
            internal void RaiseDoorOpen(object? door) => OnDoorOpen?.Invoke(new BBPDoor(door));
            internal void RaiseDoorClose(object? door) => OnDoorClose?.Invoke(new BBPDoor(door));
            internal void RaiseWindowBreak(object? window) => OnWindowBreak?.Invoke(new BBPDoor(window));
            internal void RaiseElevatorEnter(object? elevator) => OnElevatorEnter?.Invoke(new BBPElevator(elevator));
            internal void RaiseElevatorExit(object? elevator) => OnElevatorExit?.Invoke(new BBPElevator(elevator));
        }

        /// <summary>
        /// Callbacks for HUD, tooltip, announcement, and map UI events.
        /// </summary>
        public sealed class HudCallbacks
        {
            /// <summary>
            /// Raised when the HUD hidden state changes.
            /// </summary>
            public event Action<bool>? OnHudHideChanged;

            /// <summary>
            /// Raised when a tooltip is shown.
            /// </summary>
            public event Action<string>? OnTooltipShown;

            /// <summary>
            /// Raised when an announcement sound is queued.
            /// </summary>
            public event Action<BBPSoundObject>? OnAnnouncementQueued;

            /// <summary>
            /// Raised when BaldiTV plays speech.
            /// </summary>
            public event Action<BBPSoundObject>? OnBaldiTvSpeak;

            /// <summary>
            /// Raised when the map is opened.
            /// The bool value matches the game's OpenMap(toMap) argument.
            /// </summary>
            public event Action<bool>? OnMapOpen;

            /// <summary>
            /// Raised when the map is closed.
            /// </summary>
            public event Action? OnMapClose;

            internal void RaiseHudHideChanged(bool hidden) => OnHudHideChanged?.Invoke(hidden);
            internal void RaiseTooltipShown(string key) => OnTooltipShown?.Invoke(key);
            internal void RaiseAnnouncementQueued(object? sound) => OnAnnouncementQueued?.Invoke(new BBPSoundObject(sound));
            internal void RaiseBaldiTvSpeak(object? sound) => OnBaldiTvSpeak?.Invoke(new BBPSoundObject(sound));
            internal void RaiseMapOpen(bool toMap) => OnMapOpen?.Invoke(toMap);
            internal void RaiseMapClose() => OnMapClose?.Invoke();
        }

        /// <summary>
        /// Runs a callback in a cancelable context.
        /// </summary>
        /// <param name="action">The callback invocation to run.</param>
        /// <returns>True if <see cref="Cancel"/> was called during the callback; otherwise, false.</returns>
        internal static bool RunCancelable(Action action)
        {
            BBPCallbackContext ctx = new BBPCallbackContext();
            _current.Value = ctx;

            try
            {
                action();
                return ctx.Cancel;
            }
            finally
            {
                _current.Value = null;
            }
        }

        /// <summary>
        /// Requests cancellation of the original game method for the current cancelable callback.
        /// Has no effect when called outside a cancelable callback.
        /// </summary>
        public static void Cancel()
        {
            if (_current.Value != null)
                _current.Value.Cancel = true;
        }
    }

    /// <summary>
    /// Context data for a cancelable callback invocation.
    /// </summary>
    public sealed class BBPCallbackContext
    {
        /// <summary>
        /// Gets or sets whether the original game method should be skipped.
        /// </summary>
        public bool Cancel { get; set; }
    }
}
