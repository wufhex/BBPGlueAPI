using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BBPGlue.API;

namespace BBPGlue.Core
{
    public static class DebugMenu
    {
        public static bool Enabled { get; set; } = true;

        private enum Tab
        {
            Status,
            Player,
            Game,
            Events,
            Environment,
            Entity,
            NPCs,
            Items,
            Spawn,
            Seed,
            HUD,
            Baldi,
            CustomPrefabs
        }

        private static Tab _tab = Tab.Status;

        private static bool _previousCursorVisible;
        private static CursorLockMode _previousCursorLockMode;

        private static bool _visible;
        private static Rect _windowRect = new Rect(20f, 80f, 520f, 620f);
        private static Vector2 _scroll;

        private static string _ruleName = "Running";
        private static string _points = "100";
        private static string _lives = "99";
        private static string _anger = "1";

        private static string _teleportX = "0";
        private static string _teleportY = "5";
        private static string _teleportZ = "0";

        private static string _seed = "12345678";

        private static string _itemSlot = "0";
        private static BBPItemId _selectedDebugItem = BBPItemId.Bsoda;

        private static string _spawnNpcSearch = "Principal";
        private static string _spawnItemSearch = "Bsoda";

        private static string _hudText = "BBPGlue Test";
        private static string _hudTextIndex = "0";
        private static string _staminaValue = "1";

        public static void Update()
        {
            if (!Enabled)
                return;

            if (
                Input.GetKeyDown(KeyCode.F2) &&
                (Input.GetKey(KeyCode.LeftShift) ||
                Input.GetKey(KeyCode.RightShift))
            )
            {
                GameContext.StopGameTime(!_visible);
                SetVisible(!_visible);
            }
        }

        public static void OnGUI()
        {
            if (!_visible || !Enabled)
                return;

            DebugMenuStyle.ApplyRuntimeSkin();

            _windowRect = GUI.Window(
                271828,
                _windowRect,
                DrawWindow,
                "BBPGlue Debug Tools"
            );
        }

        private static void SetVisible(bool visible)
        {
            if (_visible == visible)
                return;

            if (visible)
            {
                _previousCursorVisible = Cursor.visible;
                _previousCursorLockMode = Cursor.lockState;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = _previousCursorVisible;
                Cursor.lockState = _previousCursorLockMode;
            }

            _visible = visible;
        }

        private static void DrawWindow(int id)
        {
            DrawTabs();

            _scroll = DebugMenuStyle.ScrollViewBegin(_scroll);

            switch (_tab)
            {
                case Tab.Status:
                    DrawStatusTab();
                    break;

                case Tab.Player:
                    DrawPlayerTab();
                    break;

                case Tab.Game:
                    DrawGameTab();
                    break;

                case Tab.Events:
                    DrawEventsTab();
                    break;

                case Tab.Environment:
                    DrawEnvironmentTab();
                    break;

                case Tab.Entity:
                    DrawEntityTab();
                    break;

                case Tab.NPCs:
                    DrawNpcsTab();
                    break;

                case Tab.Items:
                    DrawItemsTab();
                    break;

                case Tab.Spawn:
                    DrawSpawnTab();
                    break;

                case Tab.Seed:
                    DrawSeedTab();
                    break;

                case Tab.HUD:
                    DrawHudTab();
                    break;

                case Tab.Baldi:
                    DrawBaldiTab();
                    break;

                case Tab.CustomPrefabs:
                    DrawCustomPrefabsTab();
                    break;
            }

            DebugMenuStyle.ScrollViewEnd();

            GUI.DragWindow(new Rect(0, 0, 10000, 24));
        }

        private static void DrawTabs()
        {
            GUILayout.Space(4f);

            DebugMenuStyle.RowBegin();

            DrawTabButton(Tab.Status, "Status");
            DrawTabButton(Tab.Player, "Player");
            DrawTabButton(Tab.Game, "Game");
            DrawTabButton(Tab.Events, "Events");
            DrawTabButton(Tab.Environment, "Env");

            DebugMenuStyle.RowEnd();

            DebugMenuStyle.RowBegin();

            DrawTabButton(Tab.Entity, "Entity");
            DrawTabButton(Tab.NPCs, "NPCs");
            DrawTabButton(Tab.Items, "Items");
            DrawTabButton(Tab.Spawn, "Spawn");
            DrawTabButton(Tab.Seed, "Seed");

            DebugMenuStyle.RowEnd();

            DebugMenuStyle.RowBegin();

            DrawTabButton(Tab.HUD, "HUD");
            DrawTabButton(Tab.Baldi, "Baldi");
            DrawTabButton(Tab.CustomPrefabs, "Custom Prefabs");

            DebugMenuStyle.RowEnd();

            GUILayout.Space(6f);
        }

        private static void DrawTabButton(Tab tab, string label)
        {
            bool selected = _tab == tab;

            if (GUILayout.Toggle(selected, label, "Button"))
            {
                if (_tab != tab)
                {
                    _tab = tab;
                    _scroll = Vector2.zero;
                }
            }
        }

        private static void Header(string text)
        {
            GUILayout.Space(8f);
            DebugMenuStyle.Label($"<b>{text}</b>");
            GUILayout.Space(4f);
        }

        private static void DrawStatusTab()
        {
            DebugMenuStyle.Header("Status");

            DebugMenuStyle.Label($"In Game: {BBP.Game.IsInGame}");
            DebugMenuStyle.Label($"Player Found: {BBP.Player.Exists}");
            DebugMenuStyle.Label($"Player Type: {GameContext.PlayerType?.FullName ?? "NULL"}");
            DebugMenuStyle.Label($"Core Manager: {(GameContext.GetCoreGameManager() != null)}");
            DebugMenuStyle.Label($"Base Manager: {(GameContext.GetBaseGameManager() != null)}");
            DebugMenuStyle.Label($"Environment: {BBP.Environment.Exists}");
            DebugMenuStyle.Label($"Known NPC Prefabs: {BBP.Prefabs.NpcPrefabs.Count}");
            DebugMenuStyle.Label($"Known Item Objects: {BBP.Prefabs.ItemObjects.Count}");

            GUILayout.Space(8f);

            if (DebugMenuStyle.Button("Refresh Prefab Cache"))
                BBP.Prefabs.Refresh();

            if (DebugMenuStyle.Button("Clear BBP Console"))
                BBPConsole.Clear();

            if (BBP.Player.Exists)
            {
                DebugMenuStyle.Header("Player Snapshot");

                DebugMenuStyle.Label($"Position: {BBP.Player.Position}");
                DebugMenuStyle.Label($"Player Number: {BBP.Player.PlayerNumber}");
                DebugMenuStyle.Label($"Invincible: {BBP.Player.Invincible}");
                DebugMenuStyle.Label($"Invisible: {BBP.Player.Invisible}");
                DebugMenuStyle.Label($"Tagged: {BBP.Player.Tagged}");
                DebugMenuStyle.Label($"Reversed: {BBP.Player.Reversed}");
                DebugMenuStyle.Label($"Disobeying: {BBP.Player.Disobeying}");
                DebugMenuStyle.Label($"Rule Break: {BBP.Player.RuleBreakText}");
            }
        }

        private static void DrawPlayerTab()
        {
            DebugMenuStyle.Header("Player Tests");

            if (!BBP.Player.Exists)
            {
                DebugMenuStyle.Label("Player not found.");
                return;
            }

            DebugMenuStyle.Label($"Position: {BBP.Player.Position}");
            DebugMenuStyle.Label($"Player Number: {BBP.Player.PlayerNumber}");

            if (DebugMenuStyle.Button("Toggle Invincible"))
                BBP.Player.Invincible = !BBP.Player.Invincible;

            if (DebugMenuStyle.Button("Toggle Hidden"))
                BBP.Player.SetHidden(!BBP.Player.Invisible);

            if (DebugMenuStyle.Button("Toggle Nametag"))
                BBP.Player.SetNametag(!BBP.Player.Tagged);

            if (DebugMenuStyle.Button("Reverse Controls"))
                BBP.Player.Reverse();

            DebugMenuStyle.RowBegin();
            DebugMenuStyle.Label("Rule", GUILayout.Width(50));
            _ruleName = DebugMenuStyle.TextField(_ruleName);

            if (DebugMenuStyle.Button("RuleBreak", GUILayout.Width(110)))
                BBP.Player.RuleBreak(_ruleName, 5f, 1f);

            DebugMenuStyle.RowEnd();

            if (DebugMenuStyle.Button("Clear Guilt"))
                BBP.Player.ClearGuilt();

            DebugMenuStyle.Header("Teleport");

            DebugMenuStyle.RowBegin();
            _teleportX = DebugMenuStyle.TextField(_teleportX);
            _teleportY = DebugMenuStyle.TextField(_teleportY);
            _teleportZ = DebugMenuStyle.TextField(_teleportZ);
            DebugMenuStyle.RowEnd();

            if (DebugMenuStyle.Button("Teleport"))
            {
                if (
                    float.TryParse(_teleportX, out float x) &&
                    float.TryParse(_teleportY, out float y) &&
                    float.TryParse(_teleportZ, out float z)
                )
                {
                    BBP.Player.Teleport(new Vector3(x, y, z));
                }
            }

            BBPRoom? currentRoom = BBP.Environment.GetRoom(BBP.Player.Position);
            DebugMenuStyle.Label(
                $"Current Room: {(currentRoom != null ? $"{currentRoom.Category}/{currentRoom.Type}" : "NULL")}"
            );

            DebugMenuStyle.Header("Movement");

            BBP.Player.WalkSpeed = GUILayout.HorizontalSlider(
                BBP.Player.WalkSpeed,
                0f,
                100f
            );
            DebugMenuStyle.Label($"Walk Speed: {BBP.Player.WalkSpeed:F2}");

            BBP.Player.RunSpeed = GUILayout.HorizontalSlider(
                BBP.Player.RunSpeed,
                0f,
                100f
            );
            DebugMenuStyle.Label($"Run Speed: {BBP.Player.RunSpeed:F2}");

            BBP.Player.Stamina = GUILayout.HorizontalSlider(
                BBP.Player.Stamina,
                0f,
                BBP.Player.StaminaMax
            );
            DebugMenuStyle.Label($"Stamina: {BBP.Player.Stamina:F2}/{BBP.Player.StaminaMax:F2}");

            BBP.Player.StaminaDrop = GUILayout.HorizontalSlider(
                BBP.Player.StaminaDrop,
                0f,
                10f
            );
            DebugMenuStyle.Label($"Stamina Drop: {BBP.Player.StaminaDrop:F2}");

            BBP.Player.StaminaRise = GUILayout.HorizontalSlider(
                BBP.Player.StaminaRise,
                0f,
                10f
            );
            DebugMenuStyle.Label($"Stamina Rise: {BBP.Player.StaminaRise:F2}");

            BBP.Player.Height = GUILayout.HorizontalSlider(
                BBP.Player.Height,
                0f,
                20f
            );
            DebugMenuStyle.Label($"Height: {BBP.Player.Height:F2}");

            DebugMenuStyle.Label($"Real Velocity: {BBP.Player.RealVelocity:F2}");
            DebugMenuStyle.Label($"Frame Velocity: {BBP.Player.FrameVelocity:F2}");

            if (DebugMenuStyle.Button("Ultra Speed"))
            {
                BBP.Player.WalkSpeed = 100f;
                BBP.Player.RunSpeed = 100f;
            }

            if (DebugMenuStyle.Button("Restore Reasonable Speed"))
            {
                BBP.Player.WalkSpeed = 16f;
                BBP.Player.RunSpeed = 24f;
            }

            if (DebugMenuStyle.Button("Refill Stamina"))
            {
                BBP.Player.Stamina = BBP.Player.StaminaMax;
            }

            DebugMenuStyle.Header("Interaction");

            BBP.Player.BaseReach = GUILayout.HorizontalSlider(
                BBP.Player.BaseReach,
                0f,
                100f
            );

            DebugMenuStyle.Label(
                $"Base Reach: {BBP.Player.BaseReach:F2}"
            );

            DebugMenuStyle.Label(
                $"Effective Reach: {BBP.Player.Reach:F2}"
            );

            if (DebugMenuStyle.Button("Normal Reach"))
            {
                BBP.Player.BaseReach = 10f;
            }

            if (DebugMenuStyle.Button("Pickup Anything"))
            {
                BBP.Player.BaseReach = 10000f;
            }

            DebugMenuStyle.Label(
                $"Sees Clickable: {BBP.Player.SeesClickable}"
            );

            if (BBP.Player.ClickedThisFrame != null)
            {
                DebugMenuStyle.Label(
                    $"Clicked: {BBP.Player.ClickedThisFrame.name}"
                );
            }
        }

        private static void DrawGameTab()
        {
            DebugMenuStyle.Header("Game Tests");

            DebugMenuStyle.Label($"Paused: {BBP.Game.Paused}");
            DebugMenuStyle.Label($"Lives: {BBP.Game.Lives}");
            DebugMenuStyle.Label($"Attempts: {BBP.Game.Attempts}");
            DebugMenuStyle.Label($"Seed: {BBP.Game.Seed}");
            DebugMenuStyle.Label($"Grade: {BBP.Game.Grade} ({BBP.Game.GradeValue})");
            DebugMenuStyle.Label($"Level: {BBP.Game.CurrentLevel}");
            DebugMenuStyle.Label($"Notebooks: {BBP.Game.FoundNotebooks}/{BBP.Game.NotebookTotal}");

            if (DebugMenuStyle.Button("Toggle Pause"))
                BBP.Game.Paused = !BBP.Game.Paused;

            DebugMenuStyle.RowBegin();
            _points = DebugMenuStyle.TextField(_points);

            if (DebugMenuStyle.Button("Add Points", GUILayout.Width(120)))
            {
                if (int.TryParse(_points, out int points))
                    BBP.Game.GivePoints(points);
            }

            DebugMenuStyle.RowEnd();

            DebugMenuStyle.RowBegin();
            _lives = DebugMenuStyle.TextField(_lives);

            if (DebugMenuStyle.Button("Set Lives", GUILayout.Width(120)))
            {
                if (int.TryParse(_lives, out int lives))
                    BBP.Game.SetLives(lives);
            }

            DebugMenuStyle.RowEnd();

            DebugMenuStyle.RowBegin();
            _anger = DebugMenuStyle.TextField(_anger);

            if (DebugMenuStyle.Button("Anger Baldi", GUILayout.Width(120)))
            {
                if (float.TryParse(_anger, out float anger))
                    BBP.Game.AngerBaldi(anger);
            }

            DebugMenuStyle.RowEnd();

            if (DebugMenuStyle.Button("Collect Notebook"))
                BBP.Game.CollectNotebooks(1);

            if (DebugMenuStyle.Button("Open Map"))
                BBP.Game.OpenMap();

            if (DebugMenuStyle.Button("Close Map"))
                BBP.Game.CloseMap();

            if (DebugMenuStyle.Button("Reset Cameras"))
                BBP.Game.ResetCameras();

            if (DebugMenuStyle.Button("Reset Shaders"))
                BBP.Game.ResetShaders();
        }

        private static void DrawEventsTab()
        {
            DebugMenuStyle.Header("Event Tests");

            DebugMenuStyle.Label($"Event Status: {BBP.Events.Status}");
            DebugMenuStyle.Label($"Mapped Events: {BBP.Events.Count}");

            if (DebugMenuStyle.Button("Map Current Level Events"))
                BBP.Events.MapCurrentLevel();

            for (int i = 0; i < BBP.Events.Count; i++)
            {
                DebugMenuStyle.RowBegin();

                DebugMenuStyle.Label($"{i}: {BBP.Events.EventNames[i]}", GUILayout.ExpandWidth(true));

                if (DebugMenuStyle.Button("Trigger", GUILayout.Width(90)))
                    BBP.Events.Trigger(i);

                DebugMenuStyle.RowEnd();
            }

            DebugMenuStyle.Header("Current Events");

            List<BBPRandomEvent> currentEvents = BBP.Environment.CurrentEvents;

            DebugMenuStyle.Label($"Active Events: {currentEvents.Count}");

            foreach (BBPRandomEvent ev in currentEvents)
            {
                DebugMenuStyle.Label(
                    $"{ev.Name} | {ev.Type} | Active={ev.Active} | Remaining={ev.RemainingTime:F1}s"
                );
            }
        }

        private static void DrawEnvironmentTab()
        {
            DebugMenuStyle.Header("Environment Tests");

            DebugMenuStyle.Label($"Exists: {BBP.Environment.Exists}");
            DebugMenuStyle.Label($"Active: {BBP.Environment.Active}");
            DebugMenuStyle.Label($"NPC Time Scale: {BBP.Environment.NpcTimeScale}");
            DebugMenuStyle.Label($"Player Time Scale: {BBP.Environment.PlayerTimeScale}");
            DebugMenuStyle.Label($"Environment Time Scale: {BBP.Environment.EnvironmentTimeScale}");
            DebugMenuStyle.Label($"Remaining Time: {BBP.Environment.GetDisplayTime()}");
            DebugMenuStyle.Label($"NPCs: {BBP.Environment.NpcCount}");
            DebugMenuStyle.Label($"Players: {BBP.Environment.PlayerCount}");
            DebugMenuStyle.Label($"Pickups: {BBP.Environment.ItemCount}");
            DebugMenuStyle.Label($"Rooms: {BBP.Environment.RoomCount}");

            if (DebugMenuStyle.Button("Make Noise At Player"))
                BBP.Environment.MakeNoise(BBP.Player.Position, 100);

            if (DebugMenuStyle.Button("Make Silent 10s"))
                BBP.Environment.MakeSilent(10f);

            if (DebugMenuStyle.Button("Start Flicker Lights"))
                BBP.Environment.FlickerLights(true);

            if (DebugMenuStyle.Button("Stop Flicker Lights"))
                BBP.Environment.FlickerLights(false);

            if (DebugMenuStyle.Button("All Lights Off"))
                BBP.Environment.SetAllLights(false);

            if (DebugMenuStyle.Button("All Lights On"))
                BBP.Environment.SetAllLights(true);

            if (DebugMenuStyle.Button("Close School"))
                BBP.Environment.CloseSchool();

            DebugMenuStyle.Header("Bulk Despawn");

            if (DebugMenuStyle.Button("Despawn All NPCs"))
                BBP.Environment.DespawnAllNpcs();

            if (DebugMenuStyle.Button("Despawn All Pickups"))
                BBP.Environment.DespawnAllPickups();
        }

        private static void DrawEntityTab()
        {
            DebugMenuStyle.Header("Player Entity Tests");

            BBPEntity playerEntity = BBP.Player.Entity;

            DebugMenuStyle.Label($"Entity Exists: {playerEntity.Exists}");
            DebugMenuStyle.Label($"Visible: {playerEntity.Visible}");
            DebugMenuStyle.Label($"Hidden: {playerEntity.Hidden}");
            DebugMenuStyle.Label($"Frozen: {playerEntity.Frozen}");
            DebugMenuStyle.Label($"Blinded: {playerEntity.Blinded}");
            DebugMenuStyle.Label($"Grounded: {playerEntity.Grounded}");
            DebugMenuStyle.Label($"Squished: {playerEntity.Squished}");
            DebugMenuStyle.Label($"Flipped: {playerEntity.Flipped}");
            DebugMenuStyle.Label($"In Bounds: {playerEntity.InBounds}");
            DebugMenuStyle.Label($"Velocity: {playerEntity.Velocity}");

            if (DebugMenuStyle.Button("Toggle Frozen"))
                playerEntity.SetFrozen(!playerEntity.Frozen);

            if (DebugMenuStyle.Button("Toggle Hidden"))
                playerEntity.SetHidden(!playerEntity.Hidden);

            if (DebugMenuStyle.Button("Toggle Visible"))
                playerEntity.SetVisible(!playerEntity.Visible);

            if (DebugMenuStyle.Button("Toggle Blinded"))
                playerEntity.SetBlinded(!playerEntity.Blinded);

            if (DebugMenuStyle.Button("Toggle Interaction"))
                playerEntity.SetInteractionEnabled(playerEntity.InteractionDisabled);

            if (DebugMenuStyle.Button("Squish 5s"))
                playerEntity.Squish(5f);

            if (DebugMenuStyle.Button("Unsquish"))
                playerEntity.Unsquish();

            if (DebugMenuStyle.Button("Flip Entity"))
                playerEntity.Flip();

            if (DebugMenuStyle.Button("Vertical Scale 2x"))
                playerEntity.SetVerticalScale(2f);

            if (DebugMenuStyle.Button("Vertical Scale 1x"))
                playerEntity.SetVerticalScale(1f);

            if (DebugMenuStyle.Button("Kill Forces"))
                playerEntity.KillAllForces();
        }

        private static void DrawNpcsTab()
        {
            DebugMenuStyle.Header("NPC / Player Entity Tests");

            DebugMenuStyle.Label($"NPC Count: {BBP.Entities.Npcs.Count}");
            DebugMenuStyle.Label($"Player Count: {BBP.Entities.Players.Count}");

            BBPNpc? closestNpc = BBP.Entities.ClosestNpc;

            if (closestNpc != null)
            {
                DebugMenuStyle.Label($"Closest NPC: {closestNpc.Character}");
                DebugMenuStyle.Label($"Closest NPC Pos: {closestNpc.Position}");

                if (DebugMenuStyle.Button("Teleport Closest NPC To Player"))
                    closestNpc.TeleportToPlayer();

                if (DebugMenuStyle.Button("Freeze Closest NPC"))
                    closestNpc.Entity.SetFrozen(true);

                if (DebugMenuStyle.Button("Unfreeze Closest NPC"))
                    closestNpc.Entity.SetFrozen(false);

                if (DebugMenuStyle.Button("Hide Closest NPC"))
                    closestNpc.Entity.SetVisible(false);

                if (DebugMenuStyle.Button("Show Closest NPC"))
                    closestNpc.Entity.SetVisible(true);

                if (DebugMenuStyle.Button("Squish Closest NPC"))
                    closestNpc.Entity.Squish(5f);

                if (DebugMenuStyle.Button("Despawn Closest NPC"))
                    closestNpc.Despawn();
            }
            else
            {
                DebugMenuStyle.Label("Closest NPC: NULL");
            }

            DebugMenuStyle.Header("Live NPCs");

            foreach (BBPNpc npc in BBP.Entities.Npcs)
            {
                DebugMenuStyle.RowBegin();

                DebugMenuStyle.Label($"{npc.Character} | {npc.Position}", GUILayout.ExpandWidth(true));

                if (DebugMenuStyle.Button("TP", GUILayout.Width(45)))
                    npc.TeleportToPlayer();

                if (DebugMenuStyle.Button("Target", GUILayout.Width(65)))
                    npc.TargetPosition(BBP.Player.Position);

                if (DebugMenuStyle.Button("Despawn", GUILayout.Width(80)))
                    npc.Despawn();

                DebugMenuStyle.RowEnd();
            }
        }

        private static void DrawItemsTab()
        {
            DebugMenuStyle.Header("Item Tests");

            DebugMenuStyle.Label($"Selected Slot: {BBP.Items.SelectedSlot}");
            DebugMenuStyle.Label($"Max Slot: {BBP.Items.MaxSlot}");
            DebugMenuStyle.Label($"Slot Count: {BBP.Items.SlotCount}");
            DebugMenuStyle.Label($"Total Items: {BBP.Items.TotalItems}");
            DebugMenuStyle.Label($"Inventory Full: {BBP.Items.InventoryFull()}");
            DebugMenuStyle.Label($"Has Any Usable: {BBP.Items.HasAnyUsableItem()}");

            DebugMenuStyle.Label("Item");

            _selectedDebugItem = (BBPItemId)GUILayout.SelectionGrid(
                (int)_selectedDebugItem,
                System.Enum.GetNames(typeof(BBPItemId)),
                3
            );

            DebugMenuStyle.RowBegin();

            DebugMenuStyle.Label("Slot", GUILayout.Width(50));
            _itemSlot = DebugMenuStyle.TextField(_itemSlot, GUILayout.Width(60));

            if (DebugMenuStyle.Button("Set Slot", GUILayout.Width(100)))
            {
                if (int.TryParse(_itemSlot, out int slot))
                    BBP.Items.SetSlot(slot, _selectedDebugItem);
            }

            if (DebugMenuStyle.Button("Clear Slot", GUILayout.Width(100)))
            {
                if (int.TryParse(_itemSlot, out int slot))
                    BBP.Items.RemoveSlot(slot);
            }

            DebugMenuStyle.RowEnd();

            if (DebugMenuStyle.Button("Add Selected Item"))
                BBP.Items.Add(_selectedDebugItem);

            if (DebugMenuStyle.Button("Use Selected Item"))
                BBP.Items.UseSelected();

            if (DebugMenuStyle.Button("Remove Selected Item Type"))
                BBP.Items.Remove(_selectedDebugItem);

            if (DebugMenuStyle.Button("Remove Random Item"))
                BBP.Items.RemoveRandom();

            if (DebugMenuStyle.Button("Clear Inventory"))
                BBP.Items.Clear();

            if (DebugMenuStyle.Button("Disable Item Use"))
                BBP.Items.Disable(true);

            if (DebugMenuStyle.Button("Enable Item Use"))
                BBP.Items.Disable(false);

            if (DebugMenuStyle.Button("Reduce Inventory Size"))
                BBP.Items.ReduceTargetInventorySize();

            if (DebugMenuStyle.Button("Reset Inventory Size"))
                BBP.Items.ResetMaxItem();

            DebugMenuStyle.Header("Slots");

            for (int i = 0; i < BBP.Items.SlotCount; i++)
            {
                DebugMenuStyle.RowBegin();

                DebugMenuStyle.Label($"{i}: {BBP.Items.GetSlotName(i)}", GUILayout.ExpandWidth(true));

                if (DebugMenuStyle.Button("Select", GUILayout.Width(70)))
                    BBP.Items.SelectedSlot = i;

                if (DebugMenuStyle.Button("Lock", GUILayout.Width(55)))
                    BBP.Items.LockSlot(i, true);

                if (DebugMenuStyle.Button("Unlock", GUILayout.Width(65)))
                    BBP.Items.LockSlot(i, false);

                DebugMenuStyle.RowEnd();
            }
        }

        private static void DrawSpawnTab()
        {
            DebugMenuStyle.Header("Spawn / Despawn");

            DebugMenuStyle.Label($"Known NPC Prefabs: {BBP.Prefabs.NpcPrefabs.Count}");
            DebugMenuStyle.Label($"Known Item Objects: {BBP.Prefabs.ItemObjects.Count}");
            DebugMenuStyle.Label($"Live NPCs: {BBP.Environment.NpcCount}");
            DebugMenuStyle.Label($"Live Pickups: {BBP.Environment.ItemCount}");

            if (DebugMenuStyle.Button("Refresh Prefab Cache"))
                BBP.Prefabs.Refresh();

            DebugMenuStyle.Header("Spawn NPC");

            DebugMenuStyle.RowBegin();

            _spawnNpcSearch = DebugMenuStyle.TextField(_spawnNpcSearch);

            if (DebugMenuStyle.Button("Name", GUILayout.Width(70)))
                BBP.Environment.SpawnNpcByName(_spawnNpcSearch);

            if (DebugMenuStyle.Button("Character", GUILayout.Width(90)))
                BBP.Environment.SpawnNpcByCharacter(_spawnNpcSearch);

            DebugMenuStyle.RowEnd();

            object? npcPrefab = BBP.Prefabs.GetNpcPrefabByName(_spawnNpcSearch);
            DebugMenuStyle.Label($"Match: {BBP.Prefabs.GetPrefabName(npcPrefab)}");

            if (npcPrefab != null && DebugMenuStyle.Button("Spawn Matched NPC"))
                BBP.Environment.SpawnPrefabAtPlayer(npcPrefab);

            DebugMenuStyle.Header("Spawn Pickup");

            DebugMenuStyle.RowBegin();

            _spawnItemSearch = DebugMenuStyle.TextField(_spawnItemSearch);

            if (DebugMenuStyle.Button("Name", GUILayout.Width(70)))
                BBP.Environment.SpawnPickupByName(_spawnItemSearch);

            DebugMenuStyle.RowEnd();

            if (DebugMenuStyle.Button("Spawn Selected Item Pickup"))
                BBP.Environment.SpawnPickup(_selectedDebugItem);

            object? itemPrefab = BBP.Prefabs.GetItemObjectByName(_spawnItemSearch);
            DebugMenuStyle.Label($"Match: {BBP.Prefabs.GetItemName(itemPrefab)}");

            if (itemPrefab != null && DebugMenuStyle.Button("Spawn Matched Pickup"))
                BBP.Environment.SpawnPrefabAtPlayer(itemPrefab);

            DebugMenuStyle.Header("Despawn");

            if (DebugMenuStyle.Button("Despawn Closest NPC"))
                BBP.Environment.DespawnClosestNpc();

            if (DebugMenuStyle.Button("Despawn All NPCs"))
                BBP.Environment.DespawnAllNpcs();

            if (DebugMenuStyle.Button("Despawn All Pickups"))
                BBP.Environment.DespawnAllPickups();
        }

        private static void DrawSeedTab()
        {
            DebugMenuStyle.Header("Seed Tests");

            DebugMenuStyle.Label($"Current Seed: {BBP.Seed.CurrentSeed}");
            DebugMenuStyle.Label($"Override Enabled: {BBP.Seed.OverrideEnabled}");
            DebugMenuStyle.Label($"Override Seed: {BBP.Seed.OverrideSeed}");

            DebugMenuStyle.RowBegin();

            _seed = DebugMenuStyle.TextField(_seed);

            if (DebugMenuStyle.Button("Set Override", GUILayout.Width(120)))
            {
                if (int.TryParse(_seed, out int seed))
                    BBP.Seed.SetOverride(seed);
            }

            DebugMenuStyle.RowEnd();

            if (DebugMenuStyle.Button("Clear Seed Override"))
                BBP.Seed.ClearOverride();

            DebugMenuStyle.Label("Seed override applies before floor generation.");
        }

        private static void DrawHudTab()
        {
            DebugMenuStyle.Header("HUD Tests");

            DebugMenuStyle.Label($"HUD Exists: {BBP.Hud.Exists}");
            DebugMenuStyle.Label($"HUD Hidden: {BBP.Hud.Hidden}");
            DebugMenuStyle.Label($"Announcements Exists: {BBP.Hud.Announcements.Exists}");
            DebugMenuStyle.Label($"Announcements Busy: {BBP.Hud.Announcements.Busy}");

            if (DebugMenuStyle.Button("Hide HUD"))
                BBP.Hud.Hide(true);

            if (DebugMenuStyle.Button("Show HUD"))
                BBP.Hud.Hide(false);

            if (DebugMenuStyle.Button("Darken HUD"))
                BBP.Hud.Darken(true);

            if (DebugMenuStyle.Button("Undarken HUD"))
                BBP.Hud.Darken(false);

            DebugMenuStyle.Header("Reticle / Stamina");

            if (DebugMenuStyle.Button("Reticle On"))
                BBP.Hud.UpdateReticle(true);

            if (DebugMenuStyle.Button("Reticle Off"))
                BBP.Hud.UpdateReticle(false);

            DebugMenuStyle.RowBegin();

            _staminaValue = DebugMenuStyle.TextField(_staminaValue);

            if (DebugMenuStyle.Button("Set Stamina", GUILayout.Width(120)))
            {
                if (float.TryParse(_staminaValue, out float stamina))
                    BBP.Hud.SetStamina(stamina);
            }

            DebugMenuStyle.RowEnd();

            DebugMenuStyle.Header("Notebook / Tooltip");

            DebugMenuStyle.RowBegin();

            _hudTextIndex = DebugMenuStyle.TextField(_hudTextIndex, GUILayout.Width(50));
            _hudText = DebugMenuStyle.TextField(_hudText);

            DebugMenuStyle.RowEnd();

            if (DebugMenuStyle.Button("Set Notebook Text"))
            {
                if (int.TryParse(_hudTextIndex, out int index))
                    BBP.Hud.UpdateNotebookText(index, _hudText, false);
            }

            if (DebugMenuStyle.Button("Set Notebook Text + Spin"))
            {
                if (int.TryParse(_hudTextIndex, out int index))
                    BBP.Hud.UpdateNotebookText(index, _hudText, true);
            }

            if (DebugMenuStyle.Button("Hide Notebook Display"))
                BBP.Hud.SetNotebookDisplay(false);

            if (DebugMenuStyle.Button("Show Notebook Display"))
                BBP.Hud.SetNotebookDisplay(true);

            if (DebugMenuStyle.Button("Set Tooltip"))
                BBP.Hud.SetTooltip(_hudText);

            if (DebugMenuStyle.Button("Close Tooltip"))
                BBP.Hud.CloseTooltip();

            DebugMenuStyle.Header("Baldicator");

            if (DebugMenuStyle.Button("Baldicator Coming"))
                BBP.Hud.ActivateBaldicator(true);

            if (DebugMenuStyle.Button("Baldicator Thinking"))
                BBP.Hud.ActivateBaldicator(false);

            DebugMenuStyle.Header("BaldiTV / Announcements");

            if (DebugMenuStyle.Button("Reset BaldiTV"))
                BBP.Hud.Announcements.ReInit();

            if (DebugMenuStyle.Button("Show Level Time Warning"))
                BBP.Hud.Announcements.ShowLevelTimeWarning();

            if (DebugMenuStyle.Button("Static Visible"))
                BBP.Hud.Announcements.SetStaticVisible(true);

            if (DebugMenuStyle.Button("Static Hidden"))
                BBP.Hud.Announcements.SetStaticVisible(false);

            if (DebugMenuStyle.Button("Exclamation Visible"))
                BBP.Hud.Announcements.SetExclamationVisible(true);

            if (DebugMenuStyle.Button("Exclamation Hidden"))
                BBP.Hud.Announcements.SetExclamationVisible(false);

            if (DebugMenuStyle.Button("Baldi Face Visible"))
                BBP.Hud.Announcements.SetBaldiVisible(true);

            if (DebugMenuStyle.Button("Baldi Face Hidden"))
                BBP.Hud.Announcements.SetBaldiVisible(false);

            if (DebugMenuStyle.Button("Timer Text Visible"))
                BBP.Hud.Announcements.SetTimeTextVisible(true);

            if (DebugMenuStyle.Button("Timer Text Hidden"))
                BBP.Hud.Announcements.SetTimeTextVisible(false);

            if (DebugMenuStyle.Button("Set Timer Text Raw"))
                BBP.Hud.Announcements.SetTimeTextRaw(_hudText);

            if (DebugMenuStyle.Button("Reset Announcement Visuals"))
                BBP.Hud.Announcements.ResetVisuals();
        }

        private static void DrawBaldiTab()
        {
            DebugMenuStyle.Header("Baldi");

            BBPBaldi? baldi = BBP.Environment.Baldi;

            if (baldi == null || !baldi.Exists)
            {
                DebugMenuStyle.Label("Baldi not found.");
                return;
            }

            DebugMenuStyle.Label($"Name: {baldi.Name}");
            DebugMenuStyle.Label($"Position: {baldi.Position}");
            DebugMenuStyle.Label($"Character: {baldi.Character}");
            DebugMenuStyle.Label($"Anger: {baldi.Anger:F2}");
            DebugMenuStyle.Label($"Extra Anger: {baldi.ExtraAnger:F2}");
            DebugMenuStyle.Label($"Base Anger: {baldi.BaseAnger:F2}");
            DebugMenuStyle.Label($"Base Speed: {baldi.BaseSpeed:F2}");
            DebugMenuStyle.Label($"Calculated Speed: {baldi.CalculatedSpeed:F2}");
            DebugMenuStyle.Label($"Navigator Speed: {baldi.Speed:F2}");
            DebugMenuStyle.Label($"Delay: {baldi.Delay:F2}");
            DebugMenuStyle.Label($"Movement Portion: {baldi.MovementPortion:F2}");
            DebugMenuStyle.Label($"Slap Distance: {baldi.SlapDistance:F2}");
            DebugMenuStyle.Label($"Next Slap Distance: {baldi.NextSlapDistance:F2}");
            DebugMenuStyle.Label($"Total Distance: {baldi.TotalDistance:F2}");

            DebugMenuStyle.Header("Anger / Speed");

            baldi.Anger = GUILayout.HorizontalSlider(baldi.Anger, 0.1f, 20f);
            DebugMenuStyle.Label($"Set Anger: {baldi.Anger:F2}");

            baldi.ExtraAnger = GUILayout.HorizontalSlider(baldi.ExtraAnger, 0f, 50f);
            DebugMenuStyle.Label($"Set Extra Anger: {baldi.ExtraAnger:F2}");

            baldi.BaseSpeed = GUILayout.HorizontalSlider(baldi.BaseSpeed, 0f, 100f);
            DebugMenuStyle.Label($"Set Base Speed: {baldi.BaseSpeed:F2}");

            baldi.SpeedMultiplier = GUILayout.HorizontalSlider(baldi.SpeedMultiplier, 0f, 10f);
            DebugMenuStyle.Label($"Speed Multiplier: {baldi.SpeedMultiplier:F2}");

            if (DebugMenuStyle.Button("Anger +1"))
                baldi.GetAngry(1f);

            if (DebugMenuStyle.Button("Extra Anger +5"))
                baldi.AddExtraAnger(5f);

            if (DebugMenuStyle.Button("Reset Anger"))
            {
                baldi.SetAnger(0.1f);
                baldi.ExtraAnger = 0f;
            }

            DebugMenuStyle.Header("Actions");

            if (DebugMenuStyle.Button("Teleport Baldi To Player"))
                baldi.TeleportToPlayer();

            if (DebugMenuStyle.Button("Target Player"))
                baldi.TargetPosition(BBP.Player.Position);

            if (DebugMenuStyle.Button("Distract"))
                baldi.Distract();

            if (DebugMenuStyle.Button("Clear Sound Locations"))
                baldi.ClearSoundLocations();

            if (DebugMenuStyle.Button("Update Sound Target"))
                baldi.UpdateSoundTarget();

            if (DebugMenuStyle.Button("Hear Noise At Player"))
                baldi.Hear(null, BBP.Player.Position, 127, true);

            DebugMenuStyle.Header("Slap / Ruler");

            if (DebugMenuStyle.Button("Slap"))
                baldi.Slap();

            if (DebugMenuStyle.Button("Slap Normal"))
                baldi.SlapNormal();

            if (DebugMenuStyle.Button("Slap Broken"))
                baldi.SlapBroken();

            if (DebugMenuStyle.Button("Slap Break"))
                baldi.SlapBreak();

            if (DebugMenuStyle.Button("Break Ruler"))
                baldi.BreakRuler();

            if (DebugMenuStyle.Button("Restore Ruler"))
                baldi.RestoreRuler();

            if (DebugMenuStyle.Button("Reset Slap Distance"))
                baldi.ResetSlapDistance();

            if (DebugMenuStyle.Button("End Slap"))
                baldi.EndSlap();

            DebugMenuStyle.Header("Special States");

            if (DebugMenuStyle.Button("Take Apple"))
                baldi.TakeApple();

            if (DebugMenuStyle.Button("Resume Apple"))
                baldi.ResumeApple();

            if (DebugMenuStyle.Button("Praise 5s"))
                baldi.Praise(5f, false);

            if (DebugMenuStyle.Button("Praise 5s + Reward Sticker"))
                baldi.Praise(5f, true);

            if (DebugMenuStyle.Button("Praise Animation"))
                baldi.PraiseAnimation();

            if (DebugMenuStyle.Button("Reset Sprite"))
                baldi.ResetSprite();

            if (DebugMenuStyle.Button("Tutorial Catch"))
                baldi.TutorialCatch();
        }

        private static void DrawCustomPrefabsTab()
        {
            DebugMenuStyle.Header("Custom Prefabs");

            DebugMenuStyle.Label($"Registered: {BBP.Prefabs.CustomPrefabs.Count}");

            if (BBP.Prefabs.CustomPrefabs.Count == 0)
            {
                DebugMenuStyle.Label("No custom prefabs registered.");
                return;
            }

            foreach (KeyValuePair<string, object> entry in BBP.Prefabs.CustomPrefabs)
            {
                string id = entry.Key;
                object prefab = entry.Value;

                DebugMenuStyle.RowBegin();

                DebugMenuStyle.Label(
                    $"{id} -> {BBP.Prefabs.GetPrefabName(prefab)}",
                    GUILayout.ExpandWidth(true)
                );

                if (DebugMenuStyle.Button("Spawn", GUILayout.Width(80)))
                {
                    object? spawned = BBP.Authoring.Spawn(
                        id,
                        BBP.Player.Position
                    );

                    BBPConsole.Log(
                        spawned != null
                            ? $"Spawned custom prefab: {id}"
                            : $"Failed to spawn custom prefab: {id}"
                    );
                }

                DebugMenuStyle.RowEnd();
            }
        }
    }
}