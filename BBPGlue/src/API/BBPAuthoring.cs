using System;
using UnityEngine;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Provides runtime authoring helpers for cloning, creating, registering,
    /// and spawning BB+ prefab-like objects.
    /// </summary>
    public sealed class BBPAuthoring
    {
        /// <summary>
        /// Clones any Unity object and registers it as a custom BBPGlue prefab.
        /// </summary>
        public BBPAuthoredPrefab Clone(
            string id,
            object? source,
            Action<object>? configure = null
        )
        {
            object? clone = CloneUnityObject(source);

            if (clone == null)
                return BBPAuthoredPrefab.Null(id);

            configure?.Invoke(clone);
            BBP.Prefabs.RegisterCustomPrefab(id, clone);

            return new BBPAuthoredPrefab(id, clone);
        }

        /// <summary>
        /// Clones a vanilla NPC prefab by BB+ character id and registers it.
        /// </summary>
        public BBPAuthoredPrefab CloneNpc(
            string id,
            string baseCharacter,
            Action<BBPNpc>? configure = null
        )
        {
            object? source = BBP.Prefabs.GetNpcPrefabByCharacter(baseCharacter);

            return Clone(id, source, raw =>
            {
                object wrapped = GetComponentOrSelf(raw, "NPC") ?? raw;
                configure?.Invoke(new BBPNpc(wrapped));
            });
        }

        /// <summary>
        /// Clones a vanilla item object by item id and registers it.
        /// </summary>
        public BBPAuthoredPrefab CloneItem(
            string id,
            BBPItemId baseItem,
            Action<BBPItemObject>? configure = null
        )
        {
            object? source = BBP.Prefabs.GetItemObject(baseItem);

            return Clone(id, source, raw =>
            {
                configure?.Invoke(new BBPItemObject(raw));
            });
        }

        /// <summary>
        /// Clones an existing pickup prefab and registers it.
        /// </summary>
        public BBPAuthoredPrefab ClonePickup(
            string id,
            object? basePickupPrefab,
            Action<BBPPickup>? configure = null
        )
        {
            return Clone(id, basePickupPrefab, raw =>
            {
                configure?.Invoke(new BBPPickup(raw));
            });
        }

        /// <summary>
        /// Clones a provided GameObject prefab, extracts its NPC component if present,
        /// configures it, and registers the whole cloned prefab.
        /// </summary>
        public BBPAuthoredPrefab CreateNpcFromPrefab(
            string id,
            GameObject prefab,
            Action<BBPNpc>? configure = null
        )
        {
            return Clone(id, prefab, raw =>
            {
                object wrapped = GetComponentOrSelf(raw, "NPC") ?? raw;
                configure?.Invoke(new BBPNpc(wrapped));
            });
        }

        /// <summary>
        /// Creates a new ItemObject ScriptableObject, configures it, and registers it.
        /// </summary>
        public BBPAuthoredPrefab CreateItem(
            string id,
            Action<BBPItemObject> configure
        )
        {
            Type? itemObjectType = ReflectionCache.GetType("ItemObject");

            if (itemObjectType == null)
                return BBPAuthoredPrefab.Null(id);

            object raw = ScriptableObject.CreateInstance(itemObjectType);
            configure(new BBPItemObject(raw));

            BBP.Prefabs.RegisterCustomPrefab(id, raw);
            return new BBPAuthoredPrefab(id, raw);
        }

        /// <summary>
        /// Registers an already-created prefab-like object without cloning it.
        /// </summary>
        public BBPAuthoredPrefab Register(
            string id,
            object? prefab
        )
        {
            if (string.IsNullOrWhiteSpace(id) || prefab == null)
                return BBPAuthoredPrefab.Null(id);

            BBP.Prefabs.RegisterCustomPrefab(id, prefab);
            return new BBPAuthoredPrefab(id, prefab);
        }

        /// <summary>
        /// Finds a prefab by custom id or vanilla name and spawns it.
        /// </summary>
        public object? Spawn(string id, Vector3 position)
        {
            object? prefab = BBP.Prefabs.FindPrefabByName(id);

            if (prefab == null)
                return null;

            return BBP.Environment.SpawnPrefab(prefab, position);
        }

        /// <summary>
        /// Finds a prefab by custom id or vanilla name, spawns it, and wraps it as an NPC.
        /// </summary>
        public BBPNpc? SpawnNpc(string id, Vector3 position)
        {
            object? spawned = Spawn(id, position);
            return spawned != null ? new BBPNpc(spawned) : null;
        }

        /// <summary>
        /// Finds a prefab by custom id or vanilla name, spawns it, and wraps it as a pickup.
        /// </summary>
        public BBPPickup? SpawnPickup(string id, Vector3 position)
        {
            object? spawned = Spawn(id, position);
            return spawned != null ? new BBPPickup(spawned) : null;
        }

        /// <summary>
        /// Gets a registered authored prefab by id.
        /// </summary>
        public BBPAuthoredPrefab Get(string id)
        {
            object? prefab = BBP.Prefabs.GetCustomPrefab(id);
            return prefab != null
                ? new BBPAuthoredPrefab(id, prefab)
                : BBPAuthoredPrefab.Null(id);
        }

        private static object? CloneUnityObject(object? source)
        {
            if (source == null)
                return null;

            if (source is GameObject go)
                return UnityEngine.Object.Instantiate(go);

            if (source is Component component)
                return UnityEngine.Object.Instantiate(component);

            if (source is ScriptableObject scriptable)
                return UnityEngine.Object.Instantiate(scriptable);

            if (source is UnityEngine.Object unityObject)
                return UnityEngine.Object.Instantiate(unityObject);

            return null;
        }

        private static object? GetComponentOrSelf(object obj, string typeName)
        {
            Type? type = ReflectionCache.GetType(typeName);

            if (type == null)
                return obj;

            if (obj is Component component)
                return component.GetComponent(type) ?? obj;

            if (obj is GameObject gameObject)
                return gameObject.GetComponent(type) ?? obj;

            return obj;
        }
    }

    /// <summary>
    /// Represents a prefab-like object authored or registered through BBPGlue.
    /// </summary>
    public readonly struct BBPAuthoredPrefab
    {
        /// <summary>
        /// Stable BBPGlue id used to register this prefab.
        /// </summary>
        public readonly string Id;

        /// <summary>
        /// Raw Unity/game object stored by BBPGlue.
        /// </summary>
        public readonly object? Raw;

        /// <summary>
        /// True if this authored prefab contains a valid raw object.
        /// </summary>
        public bool Exists => Raw != null;

        public BBPAuthoredPrefab(string id, object? raw)
        {
            Id = id;
            Raw = raw;
        }

        /// <summary>
        /// Creates an empty authored prefab result.
        /// </summary>
        public static BBPAuthoredPrefab Null(string id)
        {
            return new BBPAuthoredPrefab(id, null);
        }

        /// <summary>
        /// Wraps this prefab as an NPC.
        /// </summary>
        public BBPNpc AsNpc() => new BBPNpc(Raw);

        /// <summary>
        /// Wraps this prefab as a pickup.
        /// </summary>
        public BBPPickup AsPickup() => new BBPPickup(Raw);

        /// <summary>
        /// Wraps this prefab as an item object.
        /// </summary>
        public BBPItemObject AsItemObject() => new BBPItemObject(Raw);

        /// <summary>
        /// Casts the raw object to a specific type when possible.
        /// </summary>
        public T? As<T>() where T : class
        {
            return Raw as T;
        }
    }
}