using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Provides discovery, lookup, registration, and spawning helpers for BB+ prefabs
    /// and runtime-authored prefab-like objects.
    /// </summary>
    public sealed class BBPPrefabs
    {
        private readonly List<object> _npcPrefabs = new();
        private readonly List<object> _itemObjects = new();

        private readonly Dictionary<string, object> _customPrefabs =
            new Dictionary<string, object>();

        private bool _cacheBuilt;

        /// <summary>
        /// Gets the discovered vanilla NPC prefabs.
        /// </summary>
        public IReadOnlyList<object> NpcPrefabs
        {
            get
            {
                EnsureCache();
                return _npcPrefabs;
            }
        }

        /// <summary>
        /// Gets the discovered vanilla item object templates.
        /// </summary>
        public IReadOnlyList<object> ItemObjects
        {
            get
            {
                EnsureCache();
                return _itemObjects;
            }
        }

        /// <summary>
        /// Gets runtime-registered prefabs and prefab-like authored objects.
        /// </summary>
        public IReadOnlyDictionary<string, object> CustomPrefabs => _customPrefabs;

        internal void Update()
        {
            if (!_cacheBuilt)
                Refresh();
        }

        /// <summary>
        /// Rebuilds the vanilla prefab cache from the current game state.
        /// Custom prefabs registered through <see cref="RegisterCustomPrefab"/>
        /// are not cleared.
        /// </summary>
        public void Refresh()
        {
            _npcPrefabs.Clear();
            _itemObjects.Clear();

            CollectItems();
            CollectNpcPrefabs();

            _cacheBuilt = true;
        }

        private void EnsureCache()
        {
            if (!_cacheBuilt)
                Refresh();
        }

        // =========================
        // Custom authored prefabs
        // =========================

        /// <summary>
        /// Registers a runtime-authored prefab or prefab-like object under a stable BBPGlue id.
        /// </summary>
        /// <remarks>
        /// This is the general registration path used by authored NPCs, pickups,
        /// item objects, GameObjects, Components, ScriptableObjects, or other Unity objects.
        /// It does not modify BB+ resources; it only stores the object for BBPGlue lookup.
        /// </remarks>
        /// <param name="id">Unique id used to retrieve the object later.</param>
        /// <param name="prefab">Prefab, template, component, or scriptable object to register.</param>
        /// <returns>True if the object was registered; otherwise false.</returns>
        public bool RegisterCustomPrefab(string id, object prefab)
        {
            if (string.IsNullOrWhiteSpace(id) || prefab == null)
                return false;

            _customPrefabs[id] = prefab;
            return true;
        }

        /// <summary>
        /// Gets a runtime-authored prefab or prefab-like object by id.
        /// </summary>
        /// <param name="id">The id passed to <see cref="RegisterCustomPrefab"/>.</param>
        /// <returns>The registered object, or null if no object exists for the id.</returns>
        public object? GetCustomPrefab(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            return _customPrefabs.TryGetValue(id, out object prefab)
                ? prefab
                : null;
        }

        /// <summary>
        /// Removes a runtime-authored prefab registration.
        /// </summary>
        /// <param name="id">The id of the custom prefab to remove.</param>
        /// <returns>True if an entry was removed; otherwise false.</returns>
        public bool RemoveCustomPrefab(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return false;

            return _customPrefabs.Remove(id);
        }

        /// <summary>
        /// Clears all runtime-authored prefab registrations.
        /// Vanilla discovered prefab caches are not affected.
        /// </summary>
        public void ClearCustomPrefabs()
        {
            _customPrefabs.Clear();
        }

        // =========================
        // Items
        // =========================

        private void CollectItems()
        {
            object? pfm = ReflectionUtil.GetSingletonInstance("PlayerFileManager");
            IList? itemObjects = ReflectionUtil.GetField<IList>(pfm, "itemObjects");

            if (itemObjects == null)
                return;

            foreach (object? item in itemObjects)
            {
                if (item != null)
                    AddUnique(_itemObjects, item);
            }
        }

        /// <summary>
        /// Finds a vanilla item object template by BBPGlue item id.
        /// </summary>
        /// <param name="id">The vanilla item id to locate.</param>
        /// <returns>The matching vanilla item object template, or null if not found.</returns>
        public object? GetItemObject(BBPItemId id)
        {
            EnsureCache();

            object? wantedType = ReflectionUtil.GetEnumValue("Items", id.ToString());
            if (wantedType == null)
                return null;

            foreach (object item in _itemObjects)
            {
                object? itemType = ReflectionUtil.GetField<object>(item, "itemType");

                if (itemType != null && itemType.Equals(wantedType))
                    return item;
            }

            return null;
        }

        /// <summary>
        /// Finds a vanilla item object by display/localization name or item type name.
        /// </summary>
        /// <param name="name">The item name, localization key, or type name to search for.</param>
        /// <returns>The matching vanilla item object, or null if not found.</returns>
        public object? GetItemObjectByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            EnsureCache();

            foreach (object item in _itemObjects)
            {
                if (string.Equals(
                    GetItemName(item),
                    name,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the best available name for a vanilla item object.
        /// </summary>
        /// <param name="item">The item object to inspect.</param>
        /// <returns>Name key, item type name, prefab name, or "NULL".</returns>
        public string GetItemName(object? item)
        {
            if (item == null)
                return "NULL";

            string? nameKey = ReflectionUtil.GetField<string>(item, "nameKey");
            if (!string.IsNullOrEmpty(nameKey))
                return nameKey!;

            object? itemType = ReflectionUtil.GetField<object>(item, "itemType");
            if (itemType != null)
                return itemType.ToString();

            return GetPrefabName(item);
        }

        /// <summary>
        /// Finds a vanilla item object by its internal item type name.
        /// </summary>
        /// <param name="typeName">The item type name to search for.</param>
        /// <returns>The matching vanilla item object, or null if not found.</returns>
        public object? GetItemObjectByTypeName(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
                return null;

            EnsureCache();

            foreach (object item in _itemObjects)
            {
                object? itemType = ReflectionUtil.GetField<object>(item, "itemType");

                if (
                    itemType != null &&
                    string.Equals(
                        itemType.ToString(),
                        typeName,
                        StringComparison.OrdinalIgnoreCase
                    )
                )
                {
                    return item;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a vanilla item object by index from the discovered item cache.
        /// </summary>
        /// <param name="index">Zero-based index into <see cref="ItemObjects"/>.</param>
        /// <returns>The item object, or null if the index is invalid.</returns>
        public object? GetItemObjectByIndex(int index)
        {
            EnsureCache();

            if (index < 0 || index >= _itemObjects.Count)
                return null;

            return _itemObjects[index];
        }

        // =========================
        // NPCs
        // =========================

        private void CollectNpcPrefabs()
        {
            int before = _npcPrefabs.Count;

            CollectNpcsFromLevelList();

            if (_npcPrefabs.Count != before)
                return;

            CollectNpcsFromCurrentScene();

            if (_npcPrefabs.Count != before)
                return;

            CollectNpcsFromEnvironmentSpawnList();
        }

        private void CollectNpcsFromLevelList()
        {
            foreach (Component loader in FindComponents("GameLoader"))
            {
                object? list = ReflectionUtil.GetField<object>(loader, "list");
                Array? scenes = ReflectionUtil.GetField<Array>(list, "scenes");

                if (scenes == null)
                    continue;

                foreach (object? scene in scenes)
                {
                    if (scene != null)
                        CollectSceneObject(scene);
                }
            }
        }

        private void CollectNpcsFromCurrentScene()
        {
            object? scene = ReflectionUtil.GetField<object>(
                BBP.Game.CoreManager,
                "sceneObject"
            );

            if (scene != null)
                CollectSceneObject(scene);
        }

        private void CollectNpcsFromEnvironmentSpawnList()
        {
            IList? npcsToSpawn = ReflectionUtil.GetField<IList>(
                BBP.Environment.Raw,
                "npcsToSpawn"
            );

            AddNpcList(npcsToSpawn);
        }

        private void CollectSceneObject(object scene)
        {
            AddNpcArray(ReflectionUtil.GetField<Array>(scene, "forcedNpcs"));

            IList? potentialNpcs =
                ReflectionUtil.GetField<IList>(scene, "potentialNPCs");

            if (potentialNpcs != null)
            {
                foreach (object? weightedNpc in potentialNpcs)
                {
                    if (weightedNpc == null)
                        continue;

                    object? selection = ReflectionUtil.GetField<object>(
                        weightedNpc,
                        "selection"
                    );

                    if (selection != null)
                        AddUniqueNpc(selection);
                }
            }

            Array? previousLevels =
                ReflectionUtil.GetField<Array>(scene, "previousLevels");

            if (previousLevels == null)
                return;

            foreach (object? previous in previousLevels)
            {
                if (previous != null)
                    CollectSceneObject(previous);
            }
        }

        private void AddNpcArray(Array? npcs)
        {
            if (npcs == null)
                return;

            foreach (object? npc in npcs)
            {
                if (npc != null)
                    AddUniqueNpc(npc);
            }
        }

        private void AddNpcList(IList? npcs)
        {
            if (npcs == null)
                return;

            foreach (object? npc in npcs)
            {
                if (npc != null)
                    AddUniqueNpc(npc);
            }
        }

        private void AddUniqueNpc(object npc)
        {
            if (!BBPUtil.IsA(npc, "NPC"))
                return;

            AddUnique(_npcPrefabs, npc);
        }

        /// <summary>
        /// Finds a vanilla NPC prefab by Unity object name.
        /// </summary>
        /// <param name="name">The prefab object name to search for.</param>
        /// <returns>The matching vanilla NPC prefab, or null if not found.</returns>
        public object? GetNpcPrefabByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            EnsureCache();

            foreach (object npc in _npcPrefabs)
            {
                if (string.Equals(
                    GetPrefabName(npc),
                    name,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return npc;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds a vanilla NPC prefab by its BB+ character identifier.
        /// </summary>
        /// <param name="character">The character identifier to search for.</param>
        /// <returns>The matching vanilla NPC prefab, or null if not found.</returns>
        public object? GetNpcPrefabByCharacter(string character)
        {
            if (string.IsNullOrWhiteSpace(character))
                return null;

            EnsureCache();

            foreach (object npc in _npcPrefabs)
            {
                if (string.Equals(
                    GetNpcCharacter(npc),
                    character,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return npc;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds a vanilla NPC prefab by component/runtime type name.
        /// </summary>
        /// <param name="typeName">The runtime type name to search for.</param>
        /// <returns>The matching vanilla NPC prefab, or null if not found.</returns>
        public object? GetNpcPrefabByTypeName(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
                return null;

            EnsureCache();

            foreach (object npc in _npcPrefabs)
            {
                if (string.Equals(
                    npc.GetType().Name,
                    typeName,
                    StringComparison.OrdinalIgnoreCase
                ))
                {
                    return npc;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a vanilla NPC prefab by index from the discovered NPC cache.
        /// </summary>
        /// <param name="index">Zero-based index into <see cref="NpcPrefabs"/>.</param>
        /// <returns>The NPC prefab, or null if the index is invalid.</returns>
        public object? GetNpcPrefabByIndex(int index)
        {
            EnsureCache();

            if (index < 0 || index >= _npcPrefabs.Count)
                return null;

            return _npcPrefabs[index];
        }

        /// <summary>
        /// Gets the BB+ character identifier from an NPC prefab.
        /// </summary>
        /// <param name="npc">The NPC prefab or component to inspect.</param>
        /// <returns>The character id, "Unknown", or "NULL".</returns>
        public string GetNpcCharacter(object? npc)
        {
            if (npc == null)
                return "NULL";

            object? character = ReflectionUtil.GetProperty<object>(npc, "Character");
            return character?.ToString() ?? "Unknown";
        }

        // =========================
        // Universal lookup/spawning
        // =========================

        /// <summary>
        /// Finds a prefab or prefab-like object by id/name.
        /// </summary>
        /// <remarks>
        /// Search order:
        /// custom authored prefabs, vanilla NPC prefabs, then vanilla item objects.
        /// </remarks>
        /// <param name="name">Custom id, NPC prefab name, or item name.</param>
        /// <returns>The matching object, or null if not found.</returns>
        public object? FindPrefabByName(string name)
        {
            object? custom = GetCustomPrefab(name);
            if (custom != null)
                return custom;

            object? npc = GetNpcPrefabByName(name);
            if (npc != null)
                return npc;

            return GetItemObjectByName(name);
        }

        /// <summary>
        /// Gets a best-effort readable name for a prefab or prefab-like object.
        /// </summary>
        /// <param name="prefab">The object to inspect.</param>
        /// <returns>Unity object name, GameObject name, type name, or "NULL".</returns>
        public string GetPrefabName(object? prefab)
        {
            if (prefab == null)
                return "NULL";

            if (prefab is Component component)
                return component.name;

            if (prefab is GameObject gameObject)
                return gameObject.name;

            if (prefab is UnityEngine.Object unityObject)
                return unityObject.name;

            return prefab.GetType().Name;
        }

        /// <summary>
        /// Spawns a prefab at the player's current position.
        /// </summary>
        /// <param name="prefab">The prefab or component to spawn.</param>
        /// <returns>The spawned object returned by the environment bridge, or null.</returns>
        public object? SpawnAtPlayer(object prefab)
        {
            if (prefab == null)
                return null;

            return BBP.Environment.SpawnPrefabAtPlayer(prefab);
        }

        /// <summary>
        /// Spawns a prefab at a world position.
        /// </summary>
        /// <param name="prefab">The prefab or component to spawn.</param>
        /// <param name="position">World position where the prefab should be spawned.</param>
        /// <returns>The spawned object returned by the environment bridge, or null.</returns>
        public object? SpawnAtWorld(object prefab, Vector3 position)
        {
            if (prefab == null)
                return null;

            return BBP.Environment.SpawnPrefab(prefab, position);
        }

        /// <summary>
        /// Finds a prefab by id/name and spawns it at a world position.
        /// </summary>
        /// <param name="name">Custom id, NPC prefab name, or item name.</param>
        /// <param name="position">World position where the prefab should be spawned.</param>
        /// <returns>The spawned object, or null if lookup/spawn failed.</returns>
        public object? SpawnByName(string name, Vector3 position)
        {
            object? prefab = FindPrefabByName(name);

            if (prefab == null)
                return null;

            return SpawnAtWorld(prefab, position);
        }

        private static void AddUnique(List<object> list, object value)
        {
            if (!list.Contains(value))
                list.Add(value);
        }

        private static IEnumerable<Component> FindComponents(string className)
        {
            Type? type = ReflectionCache.GetType(className);

            if (type == null)
                yield break;

            UnityEngine.Object[] found = UnityEngine.Object.FindObjectsOfType(type);

            foreach (UnityEngine.Object obj in found)
            {
                if (obj is Component component)
                    yield return component;
            }
        }
    }
}