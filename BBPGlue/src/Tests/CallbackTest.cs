using BBPGlue.API;

namespace BBPGlue.Core
{
    public static class CallbackTest
    {
        private static bool _registered;

        public static void Register()
        {
            if (_registered)
                return;

            _registered = true;

            RegisterGame();
            RegisterPlayer();
            RegisterEntity();
            RegisterItems();
            RegisterWorld();
            RegisterHud();

            BBPConsole.Log("[CallbackTest] Registered callback debug listeners.");
        }

        private static void RegisterGame()
        {
            BBP.Callbacks.Game.OnGameStart += () => Log("Game Start");
            BBP.Callbacks.Game.OnGamePauseChanged += () => Log("Game Pause Changed");
            BBP.Callbacks.Game.OnLevelLoadRequested += () => Log("Level Load Requested");
            BBP.Callbacks.Game.OnLevelReady += () => Log("Level Ready");
            BBP.Callbacks.Game.OnLevelExit += () => Log("Level Exit");
            BBP.Callbacks.Game.OnSeedSet += seed => Log($"Seed Set: {seed}");
            BBP.Callbacks.Game.OnSaveLoaded += () => Log("Save Loaded");
            BBP.Callbacks.Game.OnSaveWritten += () => Log("Save Written");
        }

        private static void RegisterPlayer()
        {
            BBP.Callbacks.Player.OnPlayerSpawn += p => Log($"Player Spawn: {p.Position}");
            BBP.Callbacks.Player.OnPlayerTeleport += p => Log($"Player Teleport: {p.Position}");
            BBP.Callbacks.Player.OnPlayerRuleBreak += rule => Log($"Rule Break: {rule}");
            BBP.Callbacks.Player.OnPlayerGuiltClear += () => Log("Player Guilt Clear");
            BBP.Callbacks.Player.OnPlayerHiddenChanged += hidden => Log($"Player Hidden: {hidden}");
            BBP.Callbacks.Player.OnPlayerStaminaChanged += stamina => Log($"Stamina: {stamina:F2}");
            BBP.Callbacks.Player.OnPlayerItemUsed += item => Log($"Item Used: {item.NameKey}/{item.ItemTypeName}");
            BBP.Callbacks.Player.OnPlayerItemAdded += item => Log($"Item Added: {item.NameKey}/{item.ItemTypeName}");
            BBP.Callbacks.Player.OnPlayerItemRemoved += item => Log($"Item Removed: {item.NameKey}/{item.ItemTypeName}");
            BBP.Callbacks.Player.OnPlayerSlotChanged += slot => Log($"Slot Changed: {slot}");
            BBP.Callbacks.Player.OnPlayerDetention += (principal, valid) => Log($"Player sent to detention. Valid: {valid}");
        }

        private static void RegisterEntity()
        {
            BBP.Callbacks.Entity.OnNpcSpawn += npc => Log($"NPC Spawn: {npc.Character}");
            BBP.Callbacks.Entity.OnNpcDespawn += npc => Log($"NPC Despawn: {npc.Character}");
            BBP.Callbacks.Entity.OnNpcHearNoise += npc => Log($"NPC Hear Noise: {npc.Character}");
            BBP.Callbacks.Entity.OnNpcSightPlayer += npc => Log($"NPC Sight Player: {npc.Character}");
            BBP.Callbacks.Entity.OnNpcLosePlayer += npc => Log($"NPC Lose Player: {npc.Character}");

            BBP.Callbacks.Entity.OnNpcTriggerEnter += (npc, entity, collider, valid) =>
                Log($"NPC Trigger Enter: {npc.Character} Valid={valid}");

            BBP.Callbacks.Entity.OnNpcTriggerStay += (npc, entity, collider, valid) =>
                Log($"NPC Trigger Stay: {npc.Character} Valid={valid}");

            BBP.Callbacks.Entity.OnNpcTriggerExit += (npc, entity, collider, valid) =>
                Log($"NPC Trigger Exit: {npc.Character} Valid={valid}");

            BBP.Callbacks.Entity.OnEntitySpawn += e => Log("Entity Spawn");
            BBP.Callbacks.Entity.OnEntityTeleport += e => Log($"Entity Teleport: {e.Position}");
            BBP.Callbacks.Entity.OnEntityFrozenChanged += (e, frozen) => Log($"Entity Frozen: {frozen}");
            BBP.Callbacks.Entity.OnEntityHiddenChanged += (e, hidden) => Log($"Entity Hidden: {hidden}");
            BBP.Callbacks.Entity.OnEntitySquished += e => Log("Entity Squished");
            BBP.Callbacks.Entity.OnEntityUnsquished += e => Log("Entity Unsquished");
        }

        private static void RegisterItems()
        {
            BBP.Callbacks.Items.OnPickupSpawn += p => Log($"Pickup Spawn: {p.Name}");
            BBP.Callbacks.Items.OnPickupCollect += p => Log($"Pickup Collect: {p.Name} | {p.Item.ItemTypeName}");
            BBP.Callbacks.Items.OnPickupDespawn += p => Log($"Pickup Despawn: {p.Name}");
            BBP.Callbacks.Items.OnItemRespawn += item => Log($"Item Respawn: {item.NameKey}/{item.ItemTypeName}");
            BBP.Callbacks.Items.OnLockerItemChanged += (slot, item) => Log($"Locker Item Changed: {slot} -> {item.NameKey}/{item.ItemTypeName}");
            BBP.Callbacks.Items.OnNotebookCollect += notebook => Log($"Notebook Collect: {notebook?.GetType().Name ?? "NULL"}");
            BBP.Callbacks.Items.OnItemUse += item => {
                Log("=== ITEM USED ===");
                Log($"NameKey: {item.NameKey}");
                Log($"DescriptionKey: {item.DescriptionKey}");
                Log($"Type: {item.ItemTypeName}");
                Log($"Value: {item.Value}");
                Log($"Price: {item.Price}");
                Log($"AddToInventory: {item.AddToInventory}");
                Log($"Raw Type: {item.Raw?.GetType().FullName ?? "NULL"}");
            };
            BBP.Callbacks.Items.OnPickupClicked += (pickup, player) =>
            {
                BBPConsole.Log($"Pickup clicked: {pickup.Item.NameKey}");
                // Testing uncollectable item
                // if (pickup.Item.NameKey == "Custom BSoda")
                //     BBPCallbacks.Cancel();
            };
        }

        private static void RegisterWorld()
        {
            BBP.Callbacks.World.OnRandomEventQueued += ev => Log($"Random Event Queued: {ev.Name}/{ev.Type}");
            BBP.Callbacks.World.OnRandomEventAnnounced += sound => Log($"Random Event Announced: {sound.Key}");
            BBP.Callbacks.World.OnRandomEventBegin += ev => Log($"Random Event Begin: {ev.Name}/{ev.Type}");
            BBP.Callbacks.World.OnRandomEventEnd += ev => Log($"Random Event End: {ev.Name}/{ev.Type}");

            BBP.Callbacks.World.OnNoiseMade += (pos, value) => Log($"Noise: {pos} | {value}");
            BBP.Callbacks.World.OnSchoolClosed += () => Log("School Closed");
            BBP.Callbacks.World.OnLightsChanged += on => Log($"Lights Changed: {on}");
            BBP.Callbacks.World.OnLightsFlickerChanged += on => Log($"Lights Flicker: {on}");
            BBP.Callbacks.World.OnTimeLimitSet += time => Log($"Time Limit: {time}");
            BBP.Callbacks.World.OnTimerWarning += () => Log("Timer Warning");

            BBP.Callbacks.World.OnRoomEnter += room => Log($"Room Enter: {room.Category}/{room.Type}");
            BBP.Callbacks.World.OnRoomExit += room => Log($"Room Exit: {room.Category}/{room.Type}");
            BBP.Callbacks.World.OnDoorOpen += door => Log($"Door Open: {door.Position}");
            BBP.Callbacks.World.OnDoorClose += door => Log($"Door Close: {door.Position}");
            BBP.Callbacks.World.OnWindowBreak += window => Log($"Window Break: {window.Position}");
            BBP.Callbacks.World.OnElevatorEnter += elevator => Log($"Elevator Enter: {elevator.Position}");
            BBP.Callbacks.World.OnElevatorExit += elevator => Log($"Elevator Exit: {elevator.Position}");
        }

        private static void RegisterHud()
        {
            BBP.Callbacks.Hud.OnHudHideChanged += hidden => Log($"HUD Hidden: {hidden}");
            BBP.Callbacks.Hud.OnTooltipShown += key => Log($"Tooltip: {key}");
            BBP.Callbacks.Hud.OnAnnouncementQueued += sound => Log($"Announcement Queued: {sound.Key}");
            BBP.Callbacks.Hud.OnBaldiTvSpeak += sound => Log($"BaldiTV Speak: {sound.Key}");
            BBP.Callbacks.Hud.OnMapOpen += toMap => Log($"Map Open: {toMap}");
            BBP.Callbacks.Hud.OnMapClose += () => Log("Map Close");
        }

        private static void Log(string message)
        {
            BBPConsole.Log("[Callback] " + message);
        }
    }
}