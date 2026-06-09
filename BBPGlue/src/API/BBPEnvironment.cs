using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Wraps the active environment controller and exposes common level, spawning, lighting, and event controls.
    /// </summary>
    public sealed class BBPEnvironment
    {
        /// <summary>
        /// Gets the raw environment controller instance.
        /// </summary>
        public object? Raw =>
            BBP.Player.EnvironmentController ?? BBP.Game.EnvironmentController;

        /// <summary>
        /// Gets whether an environment controller is available.
        /// </summary>
        public bool Exists => Raw != null;

        /// <summary>
        /// Gets or sets whether the environment is active.
        /// </summary>
        public bool Active
        {
            get => ReflectionUtil.GetProperty<bool>(Raw, "Active");
            set => ReflectionUtil.SetProperty(Raw, "Active", value);
        }

        /// <summary>
        /// Gets all door components currently found in the scene.
        /// </summary>
        public System.Collections.Generic.List<BBPDoor> Doors
        {
            get
            {
                var result = new System.Collections.Generic.List<BBPDoor>();

                foreach (Component component in Object.FindObjectsOfType<Component>())
                {
                    if (component.GetType().Name == "Door")
                        result.Add(new BBPDoor(component));
                }

                return result;
            }
        }

        /// <summary>
        /// Gets all elevator components currently found in the scene.
        /// </summary>
        public System.Collections.Generic.List<BBPElevator> ElevatorsList
        {
            get
            {
                var result = new System.Collections.Generic.List<BBPElevator>();

                foreach (Component component in Object.FindObjectsOfType<Component>())
                {
                    if (component.GetType().Name == "Elevator")
                        result.Add(new BBPElevator(component));
                }

                return result;
            }
        }

        /// <summary>
        /// Gets all room components currently found in the scene.
        /// </summary>
        public System.Collections.Generic.List<BBPRoom> RoomList
        {
            get
            {
                var result = new System.Collections.Generic.List<BBPRoom>();

                foreach (Component component in Object.FindObjectsOfType<Component>())
                {
                    if (component.GetType().Name == "Room")
                        result.Add(new BBPRoom(component));
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the number of random events available in the current level.
        /// </summary>
        public int EventsCount => ReflectionUtil.GetProperty<int>(Raw, "EventsCount");

        /// <summary>
        /// Gets whether random events have started.
        /// </summary>
        public bool EventsStarted => ReflectionUtil.GetProperty<bool>(Raw, "EventsStarted");

        /// <summary>
        /// Gets the currently active random events.
        /// </summary>
        public List<BBPRandomEvent> CurrentEvents
        {
            get
            {
                List<BBPRandomEvent> result = new List<BBPRandomEvent>();

                IList? rawEvents = ReflectionUtil.GetField<IList>(Raw, "currentEvents");

                if (rawEvents == null)
                    return result;

                foreach (object? ev in rawEvents)
                {
                    if (ev != null)
                        result.Add(new BBPRandomEvent(ev));
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the environment height.
        /// </summary>
        public float Height => ReflectionUtil.GetProperty<float>(Raw, "Height");

        /// <summary>
        /// Gets the environment time scale.
        /// </summary>
        public float EnvironmentTimeScale =>
            ReflectionUtil.GetProperty<float>(Raw, "EnvironmentTimeScale");

        /// <summary>
        /// Gets the NPC time scale.
        /// </summary>
        public float NpcTimeScale =>
            ReflectionUtil.GetProperty<float>(Raw, "NpcTimeScale");

        /// <summary>
        /// Gets the player time scale.
        /// </summary>
        public float PlayerTimeScale =>
            ReflectionUtil.GetProperty<float>(Raw, "PlayerTimeScale");

        /// <summary>
        /// Gets the remaining level time.
        /// </summary>
        public int RemainingTime =>
            ReflectionUtil.GetProperty<int>(Raw, "RemainingTime");

        /// <summary>
        /// Gets the elapsed game time.
        /// </summary>
        public float SurpassedGameTime =>
            ReflectionUtil.GetProperty<float>(Raw, "SurpassedGameTime");

        /// <summary>
        /// Gets the elapsed real time.
        /// </summary>
        public float SurpassedRealTime =>
            ReflectionUtil.GetProperty<float>(Raw, "SurpassedRealTime");

        /// <summary>
        /// Gets whether the level timer has run out.
        /// </summary>
        public bool TimeOut =>
            ReflectionUtil.GetProperty<bool>(Raw, "timeOut");

        /// <summary>
        /// Gets whether navigation updates are frozen.
        /// </summary>
        public bool NavigationUpdatesFrozen =>
            ReflectionUtil.GetProperty<bool>(Raw, "NavigationUpdatesFrozen");

        /// <summary>
        /// Gets or sets the total number of notebooks in the level.
        /// </summary>
        public int NotebookTotal
        {
            get => ReflectionUtil.GetField<int>(Raw, "notebookTotal");
            set => ReflectionUtil.SetField(Raw, "notebookTotal", value);
        }

        /// <summary>
        /// Gets or sets the level spawn position.
        /// </summary>
        public Vector3 SpawnPoint
        {
            get => ReflectionUtil.GetField<Vector3>(Raw, "spawnPoint");
            set => ReflectionUtil.SetField(Raw, "spawnPoint", value);
        }

        /// <summary>
        /// Gets or sets the level spawn rotation.
        /// </summary>
        public Quaternion SpawnRotation
        {
            get => ReflectionUtil.GetField<Quaternion>(Raw, "spawnRotation");
            set => ReflectionUtil.SetField(Raw, "spawnRotation", value);
        }

        /// <summary>
        /// Gets the raw map object.
        /// </summary>
        public object? Map => ReflectionUtil.GetField<object>(Raw, "map");

        /// <summary>
        /// Gets the raw elevator manager.
        /// </summary>
        public object? ElevatorManager => ReflectionUtil.GetProperty<object>(Raw, "ElevatorManager");

        /// <summary>
        /// Gets the raw NPC list.
        /// </summary>
        public IList? Npcs => ReflectionUtil.GetProperty<IList>(Raw, "Npcs");

        /// <summary>
        /// Gets the raw player list.
        /// </summary>
        public IList? Players => ReflectionUtil.GetProperty<IList>(Raw, "Players");

        /// <summary>
        /// Gets the raw elevator list.
        /// </summary>
        public IList? Elevators => ReflectionUtil.GetProperty<IList>(Raw, "Elevators");

        /// <summary>
        /// Gets the raw pickup list.
        /// </summary>
        public IList? Items => ReflectionUtil.GetField<IList>(Raw, "items");

        /// <summary>
        /// Gets the raw notebook list.
        /// </summary>
        public IList? Notebooks => ReflectionUtil.GetField<IList>(Raw, "notebooks");

        /// <summary>
        /// Gets the raw activity list.
        /// </summary>
        public IList? Activities => ReflectionUtil.GetField<IList>(Raw, "activities");

        /// <summary>
        /// Gets the raw room list.
        /// </summary>
        public IList? Rooms => ReflectionUtil.GetField<IList>(Raw, "rooms");

        /// <summary>
        /// Gets the number of NPCs in the environment.
        /// </summary>
        public int NpcCount => Npcs?.Count ?? 0;

        /// <summary>
        /// Gets the number of players in the environment.
        /// </summary>
        public int PlayerCount => Players?.Count ?? 0;

        /// <summary>
        /// Gets the number of pickups in the environment.
        /// </summary>
        public int ItemCount => Items?.Count ?? 0;

        /// <summary>
        /// Gets the number of notebooks in the environment.
        /// </summary>
        public int NotebookCount => Notebooks?.Count ?? 0;

        /// <summary>
        /// Gets the number of rooms in the environment.
        /// </summary>
        public int RoomCount => Rooms?.Count ?? 0;

        /// <summary>
        /// Gets Baldi from the current environment.
        /// </summary>
        public BBPBaldi? Baldi => GetBaldi();

        /// <summary>
        /// Refreshes cached environment state.
        /// </summary>
        public void Refresh()
        {
        }

        /// <summary>
        /// Starts environment gameplay.
        /// </summary>
        public void BeginPlay()
        {
            ReflectionUtil.Call(Raw, "BeginPlay");
        }

        /// <summary>
        /// Assigns players to the environment.
        /// </summary>
        public void AssignPlayers()
        {
            ReflectionUtil.Call(Raw, "AssignPlayers");
        }

        /// <summary>
        /// Spawns the level NPCs.
        /// </summary>
        public void SpawnNPCs()
        {
            ReflectionUtil.Call(Raw, "SpawnNPCs");
        }

        /// <summary>
        /// Spawns an NPC using a raw prefab and grid position.
        /// </summary>
        /// <param name="npcPrefab">The raw NPC prefab.</param>
        /// <param name="gridPosition">The target grid position.</param>
        /// <returns>The spawned NPC, or null if spawning fails.</returns>
        public BBPNpc? SpawnNpc(object npcPrefab, object gridPosition)
        {
            object? spawned = ReflectionUtil.Call<object>(
                Raw,
                "SpawnNPC",
                npcPrefab,
                gridPosition
            );

            return spawned != null ? new BBPNpc(spawned) : null;
        }

        /// <summary>
        /// Spawns an NPC at the player's position.
        /// </summary>
        /// <param name="npcPrefab">The raw NPC prefab.</param>
        /// <returns>The spawned NPC, or null if spawning fails.</returns>
        public BBPNpc? SpawnNpcAtPlayer(object npcPrefab)
        {
            return SpawnNpcAtWorld(npcPrefab, BBP.Player.Position);
        }

        /// <summary>
        /// Spawns an NPC at a world position.
        /// </summary>
        /// <param name="npcPrefab">The raw NPC prefab.</param>
        /// <param name="worldPosition">The target world position.</param>
        /// <returns>The spawned NPC, or null if spawning fails.</returns>
        public BBPNpc? SpawnNpcAtWorld(object npcPrefab, Vector3 worldPosition)
        {
            object? gridPosition = ReflectionUtil.CallStatic(
                "IntVector2",
                "GetGridPosition",
                worldPosition
            );

            if (gridPosition == null)
                return null;

            return SpawnNpc(npcPrefab, gridPosition);
        }

        /// <summary>
        /// Finds an NPC prefab by name and spawns it at a world position.
        /// </summary>
        /// <param name="nameContains">The NPC prefab name to look up.</param>
        /// <param name="worldPosition">The target world position.</param>
        /// <returns>The spawned NPC, or null if no matching prefab is found.</returns>
        public BBPNpc? SpawnNpcByName(string nameContains, Vector3 worldPosition)
        {
            object? prefab = BBP.Prefabs.GetNpcPrefabByName(nameContains);

            if (prefab == null)
                return null;

            return SpawnNpcAtWorld(prefab, worldPosition);
        }

        /// <summary>
        /// Finds an NPC prefab by name and spawns it at the player's position.
        /// </summary>
        /// <param name="nameContains">The NPC prefab name to look up.</param>
        /// <returns>The spawned NPC, or null if no matching prefab is found.</returns>
        public BBPNpc? SpawnNpcByName(string nameContains)
        {
            return SpawnNpcByName(nameContains, BBP.Player.Position);
        }

        /// <summary>
        /// Finds an NPC prefab by character name and spawns it at a world position.
        /// </summary>
        /// <param name="characterName">The character name to look up.</param>
        /// <param name="worldPosition">The target world position.</param>
        /// <returns>The spawned NPC, or null if no matching prefab is found.</returns>
        public BBPNpc? SpawnNpcByCharacter(string characterName, Vector3 worldPosition)
        {
            object? prefab = BBP.Prefabs.GetNpcPrefabByCharacter(characterName);

            if (prefab == null)
                return null;

            return SpawnNpcAtWorld(prefab, worldPosition);
        }

        /// <summary>
        /// Finds an NPC prefab by character name and spawns it at the player's position.
        /// </summary>
        /// <param name="characterName">The character name to look up.</param>
        /// <returns>The spawned NPC, or null if no matching prefab is found.</returns>
        public BBPNpc? SpawnNpcByCharacter(string characterName)
        {
            return SpawnNpcByCharacter(characterName, BBP.Player.Position);
        }

        /// <summary>
        /// Spawns a raw pickup in a room.
        /// </summary>
        /// <param name="room">The raw room object.</param>
        /// <param name="itemObject">The raw item object.</param>
        /// <param name="position">The pickup position.</param>
        /// <returns>The raw spawned pickup, or null if spawning fails.</returns>
        public object? SpawnPickupRaw(object room, object itemObject, Vector2 position)
        {
            return ReflectionUtil.Call<object>(
                Raw,
                "CreateItem",
                room,
                itemObject,
                position
            );
        }

        /// <summary>
        /// Spawns a pickup in a room.
        /// </summary>
        /// <param name="room">The raw room object.</param>
        /// <param name="item">The item object to spawn.</param>
        /// <param name="position">The pickup position.</param>
        /// <returns>The spawned pickup, or null if spawning fails.</returns>
        public BBPPickup? SpawnPickup(object room, BBPItemObject item, Vector2 position)
        {
            if (item.Raw == null)
                return null;

            object? pickup = SpawnPickupRaw(room, item.Raw, position);
            return pickup != null ? new BBPPickup(pickup) : null;
        }

        /// <summary>
        /// Spawns a pickup at a world position.
        /// </summary>
        /// <param name="item">The item object to spawn.</param>
        /// <param name="worldPosition">The target world position.</param>
        /// <returns>The spawned pickup, or null if spawning fails.</returns>
        public BBPPickup? SpawnPickupAtWorld(BBPItemObject item, Vector3 worldPosition)
        {
            if (item.Raw == null)
                return null;

            object? cell = ReflectionUtil.Call<object>(
                Raw,
                "CellFromPosition",
                worldPosition
            );

            object? room = ReflectionUtil.GetField<object>(cell, "room");

            if (room == null)
                return null;

            return SpawnPickup(
                room,
                item,
                new Vector2(worldPosition.x, worldPosition.z)
            );
        }

        /// <summary>
        /// Spawns a pickup at the player's position.
        /// </summary>
        /// <param name="item">The item object to spawn.</param>
        /// <returns>The spawned pickup, or null if spawning fails.</returns>
        public BBPPickup? SpawnPickupAtPlayer(BBPItemObject item)
        {
            return SpawnPickupAtWorld(item, BBP.Player.Position);
        }

        /// <summary>
        /// Spawns a pickup by item ID at a world position.
        /// </summary>
        /// <param name="item">The item ID to spawn.</param>
        /// <param name="worldPosition">The target world position.</param>
        /// <returns>The spawned pickup, or null if the item cannot be found.</returns>
        public BBPPickup? SpawnPickup(BBPItemId item, Vector3 worldPosition)
        {
            object? itemObject = BBP.Prefabs.GetItemObject(item);

            if (itemObject == null)
                return null;

            return SpawnPickupAtWorld(new BBPItemObject(itemObject), worldPosition);
        }

        /// <summary>
        /// Spawns a pickup by item ID at the player's position.
        /// </summary>
        /// <param name="item">The item ID to spawn.</param>
        /// <returns>The spawned pickup, or null if the item cannot be found.</returns>
        public BBPPickup? SpawnPickup(BBPItemId item)
        {
            return SpawnPickup(item, BBP.Player.Position);
        }

        /// <summary>
        /// Spawns a pickup by item name at a world position.
        /// </summary>
        /// <param name="name">The item name to look up.</param>
        /// <param name="worldPosition">The target world position.</param>
        /// <returns>The spawned pickup, or null if the item cannot be found.</returns>
        public BBPPickup? SpawnPickupByName(string name, Vector3 worldPosition)
        {
            object? itemObject = BBP.Prefabs.GetItemObjectByName(name);

            if (itemObject == null)
                return null;

            return SpawnPickupAtWorld(new BBPItemObject(itemObject), worldPosition);
        }

        /// <summary>
        /// Spawns a pickup by item name at the player's position.
        /// </summary>
        /// <param name="name">The item name to look up.</param>
        /// <returns>The spawned pickup, or null if the item cannot be found.</returns>
        public BBPPickup? SpawnPickupByName(string name)
        {
            return SpawnPickupByName(name, BBP.Player.Position);
        }

        /// <summary>
        /// Spawns a supported prefab at a world position.
        /// </summary>
        /// <param name="prefab">An NPC prefab, item object, GameObject, or Component prefab.</param>
        /// <param name="worldPosition">The target world position.</param>
        /// <returns>The spawned object or wrapper, or null if the prefab type is unsupported.</returns>
        public object? SpawnPrefab(object prefab, Vector3 worldPosition)
        {
            if (prefab == null || Raw == null)
                return null;

            if (BBPUtil.IsA(prefab, "NPC"))
                return SpawnNpcAtWorld(prefab, worldPosition);

            if (BBPUtil.IsA(prefab, "ItemObject"))
                return SpawnPickupAtWorld(new BBPItemObject(prefab), worldPosition);

            if (prefab is GameObject go)
                return SpawnObject(go, worldPosition);

            if (prefab is Component component)
                return SpawnObject(component, worldPosition);

            return null;
        }

        /// <summary>
        /// Spawns a supported prefab at the player's position.
        /// </summary>
        /// <param name="prefab">An NPC prefab, item object, GameObject, or Component prefab.</param>
        /// <returns>The spawned object or wrapper, or null if the prefab type is unsupported.</returns>
        public object? SpawnPrefabAtPlayer(object prefab)
        {
            return SpawnPrefab(prefab, BBP.Player.Position);
        }

        /// <summary>
        /// Despawns an NPC.
        /// </summary>
        /// <param name="npc">The NPC to despawn.</param>
        /// <returns>True if the despawn call was sent; otherwise, false.</returns>
        public bool DespawnNpc(BBPNpc npc)
        {
            if (npc == null || npc.Raw == null)
                return false;

            ReflectionUtil.Call(npc.Raw, "Despawn");
            return true;
        }

        /// <summary>
        /// Despawns a raw NPC object.
        /// </summary>
        /// <param name="rawNpc">The raw NPC object to despawn.</param>
        /// <returns>True if the despawn call was sent; otherwise, false.</returns>
        public bool DespawnNpc(object rawNpc)
        {
            if (rawNpc == null)
                return false;

            ReflectionUtil.Call(rawNpc, "Despawn");
            return true;
        }

        /// <summary>
        /// Despawns a pickup.
        /// </summary>
        /// <param name="pickup">The raw pickup object to despawn.</param>
        /// <returns>True if the pickup was removed or destroyed; otherwise, false.</returns>
        public bool DespawnPickup(object pickup)
        {
            if (pickup == null)
                return false;

            IList? items = Items;

            if (items != null && items.Contains(pickup))
                items.Remove(pickup);

            if (pickup is Component component)
            {
                Object.Destroy(component.gameObject);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Despawns all NPCs currently known by BBPGlue.
        /// </summary>
        /// <returns>The number of NPCs despawned.</returns>
        public int DespawnAllNpcs()
        {
            int count = 0;

            foreach (BBPNpc npc in BBP.Entities.GetNpcs())
            {
                if (npc.Raw == null)
                    continue;

                ReflectionUtil.Call(npc.Raw, "Despawn");
                count++;
            }

            return count;
        }

        /// <summary>
        /// Despawns all pickups in the environment item list.
        /// </summary>
        /// <returns>The number of pickups despawned.</returns>
        public int DespawnAllPickups()
        {
            IList? items = Items;

            if (items == null)
                return 0;

            object[] copy = new object[items.Count];
            items.CopyTo(copy, 0);

            int count = 0;

            foreach (object pickup in copy)
            {
                if (DespawnPickup(pickup))
                    count++;
            }

            return count;
        }

        /// <summary>
        /// Despawns the closest NPC to the player.
        /// </summary>
        /// <returns>True if an NPC was found and despawned; otherwise, false.</returns>
        public bool DespawnClosestNpc()
        {
            BBPNpc? npc = BBP.Entities.ClosestNpc;

            if (npc == null)
                return false;

            return DespawnNpc(npc);
        }

        /// <summary>
        /// Spawns a GameObject at a position.
        /// </summary>
        /// <param name="prefab">The GameObject prefab to instantiate.</param>
        /// <param name="position">The spawn position.</param>
        /// <returns>The spawned GameObject, or null if the prefab is null.</returns>
        public GameObject? SpawnObject(GameObject prefab, Vector3 position)
        {
            return SpawnObject(prefab, position, Quaternion.identity, null);
        }

        /// <summary>
        /// Spawns a GameObject at a position and rotation.
        /// </summary>
        /// <param name="prefab">The GameObject prefab to instantiate.</param>
        /// <param name="position">The spawn position.</param>
        /// <param name="rotation">The spawn rotation.</param>
        /// <returns>The spawned GameObject, or null if the prefab is null.</returns>
        public GameObject? SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return SpawnObject(prefab, position, rotation, null);
        }

        /// <summary>
        /// Spawns a GameObject at a position, rotation, and optional parent.
        /// </summary>
        /// <param name="prefab">The GameObject prefab to instantiate.</param>
        /// <param name="position">The spawn position.</param>
        /// <param name="rotation">The spawn rotation.</param>
        /// <param name="parent">The optional parent transform.</param>
        /// <returns>The spawned GameObject, or null if the prefab is null.</returns>
        public GameObject? SpawnObject(
            GameObject prefab,
            Vector3 position,
            Quaternion rotation,
            Transform? parent
        )
        {
            if (prefab == null)
                return null;

            GameObject spawned = parent != null
                ? Object.Instantiate(prefab, position, rotation, parent)
                : Object.Instantiate(prefab, position, rotation);

            return spawned;
        }

        /// <summary>
        /// Spawns a Component at a position.
        /// </summary>
        /// <typeparam name="T">The component type.</typeparam>
        /// <param name="prefab">The component prefab to instantiate.</param>
        /// <param name="position">The spawn position.</param>
        /// <returns>The spawned component, or null if the prefab is null.</returns>
        public T? SpawnObject<T>(T prefab, Vector3 position) where T : Component
        {
            return SpawnObject(prefab, position, Quaternion.identity, null);
        }

        /// <summary>
        /// Spawns a Component at a position, rotation, and optional parent.
        /// </summary>
        /// <typeparam name="T">The component type.</typeparam>
        /// <param name="prefab">The component prefab to instantiate.</param>
        /// <param name="position">The spawn position.</param>
        /// <param name="rotation">The spawn rotation.</param>
        /// <param name="parent">The optional parent transform.</param>
        /// <returns>The spawned component, or null if the prefab is null.</returns>
        public T? SpawnObject<T>(
            T prefab,
            Vector3 position,
            Quaternion rotation,
            Transform? parent = null
        ) where T : Component
        {
            if (prefab == null)
                return null;

            T spawned = parent != null
                ? Object.Instantiate(prefab, position, rotation, parent)
                : Object.Instantiate(prefab, position, rotation);

            return spawned;
        }

        /// <summary>
        /// Spawns a raw GameObject or Component prefab.
        /// </summary>
        /// <param name="prefab">The raw prefab to instantiate.</param>
        /// <param name="position">The spawn position.</param>
        /// <param name="rotation">The spawn rotation.</param>
        /// <param name="parent">The optional parent transform.</param>
        /// <returns>The spawned object, or null if the prefab type is unsupported.</returns>
        public object? SpawnRawObject(
            object prefab,
            Vector3 position,
            Quaternion rotation,
            Transform? parent = null
        )
        {
            if (prefab is GameObject go)
            {
                GameObject clone = Object.Instantiate(go, position, rotation, parent);
                return clone;
            }

            if (prefab is Component component)
            {
                Component clone = Object.Instantiate(component, position, rotation, parent);
                return clone;
            }

            return null;
        }

        /// <summary>
        /// Spawns a raw GameObject or Component prefab with no custom rotation.
        /// </summary>
        /// <param name="prefab">The raw prefab to instantiate.</param>
        /// <param name="position">The spawn position.</param>
        /// <returns>The spawned object, or null if the prefab type is unsupported.</returns>
        public object? SpawnRawObject(object prefab, Vector3 position)
        {
            return SpawnRawObject(prefab, position, Quaternion.identity, null);
        }

        /// <summary>
        /// Starts random event timers.
        /// </summary>
        public void StartEventTimers()
        {
            ReflectionUtil.Call(Raw, "StartEventTimers");
        }

        /// <summary>
        /// Stops active random events.
        /// </summary>
        public void StopEvents()
        {
            ReflectionUtil.Call(Raw, "StopEvents");
        }

        /// <summary>
        /// Resets random events.
        /// </summary>
        public void ResetEvents()
        {
            ReflectionUtil.Call(Raw, "ResetEvents");
        }

        /// <summary>
        /// Pauses or unpauses the environment.
        /// </summary>
        /// <param name="paused">True to pause; false to unpause.</param>
        public void PauseEnvironment(bool paused)
        {
            ReflectionUtil.Call(Raw, "PauseEnvironment", paused);
        }

        /// <summary>
        /// Pauses or unpauses random events.
        /// </summary>
        /// <param name="paused">True to pause; false to unpause.</param>
        public void PauseEvents(bool paused)
        {
            ReflectionUtil.Call(Raw, "PauseEvents", paused);
        }

        /// <summary>
        /// Closes the school.
        /// </summary>
        public void CloseSchool()
        {
            ReflectionUtil.Call(Raw, "CloseSchool");
        }

        /// <summary>
        /// Creates a noise at the specified position.
        /// </summary>
        /// <param name="position">The noise position.</param>
        /// <param name="value">The noise value used by the game.</param>
        public void MakeNoise(Vector3 position, int value)
        {
            ReflectionUtil.Call(Raw, "MakeNoise", position, value);
        }

        /// <summary>
        /// Creates a noise at the specified position.
        /// </summary>
        /// <param name="position">The noise position.</param>
        /// <param name="value">The noise value used by the game.</param>
        /// <param name="hasSilentChance">Whether silent chance should be applied.</param>
        public void MakeNoise(Vector3 position, int value, bool hasSilentChance)
        {
            ReflectionUtil.Call(Raw, "MakeNoise", position, value, hasSilentChance);
        }

        /// <summary>
        /// Makes the environment silent for a duration.
        /// </summary>
        /// <param name="time">The duration in seconds.</param>
        public void MakeSilent(float time)
        {
            ReflectionUtil.Call(Raw, "MakeSilent", time);
        }

        /// <summary>
        /// Enables or disables light flickering.
        /// </summary>
        /// <param name="enabled">True to enable flickering; false to disable it.</param>
        public void FlickerLights(bool enabled)
        {
            ReflectionUtil.Call(Raw, "FlickerLights", enabled);
        }

        /// <summary>
        /// Turns all environment lights on or off.
        /// </summary>
        /// <param name="on">True to turn lights on; false to turn them off.</param>
        public void SetAllLights(bool on)
        {
            ReflectionUtil.Call(Raw, "SetAllLights", on);
        }

        /// <summary>
        /// Sets the environment spawn position and rotation.
        /// </summary>
        /// <param name="position">The spawn position.</param>
        /// <param name="rotation">The spawn rotation.</param>
        public void SetSpawn(Vector3 position, Quaternion rotation)
        {
            ReflectionUtil.Call(Raw, "SetSpawn", position, rotation);
        }

        /// <summary>
        /// Sets the level time limit.
        /// </summary>
        /// <param name="time">The time limit in seconds.</param>
        public void SetTimeLimit(float time)
        {
            ReflectionUtil.Call(Raw, "SetTimeLimit", time);
        }

        /// <summary>
        /// Freezes or unfreezes navigation updates.
        /// </summary>
        /// <param name="frozen">True to freeze updates; false to unfreeze them.</param>
        public void FreezeNavigationUpdates(bool frozen)
        {
            ReflectionUtil.Call(Raw, "FreezeNavigationUpdates", frozen);
        }

        /// <summary>
        /// Recalculates environment navigation.
        /// </summary>
        public void RecalculateNavigation()
        {
            ReflectionUtil.Call(Raw, "RecalculateNavigation");
        }

        /// <summary>
        /// Checks whether a position is inside the environment bounds.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns>True if the position is inside the environment bounds; otherwise, false.</returns>
        public bool ContainsCoordinates(Vector3 position)
        {
            return ReflectionUtil.Call<bool>(Raw, "ContainsCoordinates", position);
        }

        /// <summary>
        /// Gets the light level at a position.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns>The light level returned by the game.</returns>
        public float LightLevel(Vector3 position)
        {
            return ReflectionUtil.Call<float>(Raw, "LightLevel", position);
        }

        /// <summary>
        /// Gets the formatted level timer text.
        /// </summary>
        /// <returns>The formatted display time.</returns>
        public string GetDisplayTime()
        {
            return ReflectionUtil.Call<string>(Raw, "GetDisplayTime") ?? "";
        }

        /// <summary>
        /// Temporarily opens breakable windows.
        /// </summary>
        public void TempOpenBreakableWindows()
        {
            ReflectionUtil.Call(Raw, "TempOpenBreakableWindows");
        }

        /// <summary>
        /// Temporarily closes windows.
        /// </summary>
        public void TempCloseWindows()
        {
            ReflectionUtil.Call(Raw, "TempCloseWindows");
        }

        /// <summary>
        /// Temporarily opens bully blockers.
        /// </summary>
        public void TempOpenBully()
        {
            ReflectionUtil.Call(Raw, "TempOpenBully");
        }

        /// <summary>
        /// Temporarily closes bully blockers.
        /// </summary>
        public void TempCloseBully()
        {
            ReflectionUtil.Call(Raw, "TempCloseBully");
        }

        /// <summary>
        /// Temporarily opens locked standard doors.
        /// </summary>
        public void TempOpenLockedStandardDoors()
        {
            ReflectionUtil.Call(Raw, "TempOpenLockedStandardDoors");
        }

        /// <summary>
        /// Temporarily closes locked standard doors.
        /// </summary>
        public void TempCloseLockedStandardDoors()
        {
            ReflectionUtil.Call(Raw, "TempCloseLockedStandardDoors");
        }

        /// <summary>
        /// Gets the closest door to the player.
        /// </summary>
        /// <returns>The closest door, or null if none are found.</returns>
        public BBPDoor? GetClosestDoor()
        {
            return GetClosestDoor(BBP.Player.Position);
        }

        /// <summary>
        /// Gets the closest door to a position.
        /// </summary>
        /// <param name="position">The position to search from.</param>
        /// <returns>The closest door, or null if none are found.</returns>
        public BBPDoor? GetClosestDoor(Vector3 position)
        {
            BBPDoor? closest = null;
            float bestDistance = float.MaxValue;

            foreach (BBPDoor door in Doors)
            {
                float distance = Vector3.Distance(position, door.Position);

                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    closest = door;
                }
            }

            return closest;
        }

        /// <summary>
        /// Gets the closest elevator to the player.
        /// </summary>
        /// <returns>The closest elevator, or null if none are found.</returns>
        public BBPElevator? GetClosestElevator()
        {
            BBPElevator? closest = null;
            float bestDistance = float.MaxValue;

            foreach (BBPElevator elevator in ElevatorsList)
            {
                float distance = Vector3.Distance(BBP.Player.Position, elevator.Position);

                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    closest = elevator;
                }
            }

            return closest;
        }

        /// <summary>
        /// Gets the room containing a position.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns>The containing room, or null if no room contains the position.</returns>
        public BBPRoom? GetRoom(Vector3 position)
        {
            foreach (BBPRoom room in RoomList)
            {
                if (room.ContainsPosition(position))
                    return room;
            }

            return null;
        }

        /// <summary>
        /// Gets the room the player is currently in.
        /// </summary>
        /// <returns>The player room, or null if no room is found.</returns>
        public BBPRoom? GetPlayerRoom()
        {
            return GetRoom(BBP.Player.Position);
        }

        /// <summary>
        /// Gets the closest room to the player.
        /// </summary>
        /// <returns>The closest room, or null if none are found.</returns>
        public BBPRoom? GetClosestRoom()
        {
            return GetClosestRoom(BBP.Player.Position);
        }

        /// <summary>
        /// Gets the closest room to a position.
        /// </summary>
        /// <param name="position">The position to search from.</param>
        /// <returns>The closest room, or null if none are found.</returns>
        public BBPRoom? GetClosestRoom(Vector3 position)
        {
            BBPRoom? closest = null;
            float bestDistance = float.MaxValue;

            foreach (BBPRoom room in RoomList)
            {
                float distance = Vector3.Distance(position, room.Center);

                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    closest = room;
                }
            }

            return closest;
        }

        /// <summary>
        /// Gets Baldi from the environment controller.
        /// </summary>
        /// <returns>Baldi, or null if he is not available.</returns>
        public BBPBaldi? GetBaldi()
        {
            object? baldi = ReflectionUtil.Call<object>(Raw, "GetBaldi");

            return baldi != null
                ? new BBPBaldi(baldi)
                : null;
        }
    }
}
