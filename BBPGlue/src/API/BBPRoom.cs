using System.Collections;
using UnityEngine;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Represents a room within a level providing access to tiles, cells, pickups and other room properties.
    /// </summary>
    public sealed class BBPRoom
    {
        /// <summary>
        /// The raw underlying room object wrapped by this API class, or null if none.
        /// </summary>
        public object? Raw { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BBPRoom"/> class wrapping a raw object.
        /// </summary>
        /// <param name="raw">The raw underlying object instance or null.</param>
        public BBPRoom(object? raw)
        {
            Raw = raw;
        }

        /// <summary>
        /// Determines whether the underlying raw object exists.
        /// </summary>
        public bool Exists => Raw != null;

        /// <summary>
        /// Gets the category identifier for the room.
        /// </summary>
        /// <returns>The category as a string.</returns>
        public string Category => ReflectionUtil.GetField<object>(Raw, "category")?.ToString() ?? "Unknown";
        /// <summary>
        /// Gets the room type identifier.
        /// </summary>
        /// <returns>The type as a string.</returns>
        public string Type => ReflectionUtil.GetField<object>(Raw, "type")?.ToString() ?? "Unknown";
        /// <summary>
        /// Gets the direction label for the room.
        /// </summary>
        /// <returns>The direction as a string.</returns>
        public string Direction => ReflectionUtil.GetField<object>(Raw, "dir")?.ToString() ?? "Unknown";

        /// <summary>
        /// Gets or sets the display color associated with this room.
        /// </summary>
        /// <returns>The <see cref="Color"/> value for the room.</returns>
        public Color Color
        {
            get => ReflectionUtil.GetField<Color>(Raw, "color");
            set => ReflectionUtil.SetField(Raw, "color", value);
        }

        /// <summary>
        /// Gets or sets whether the room is powered.
        /// Setting this will call the underlying SetPower method.
        /// </summary>
        /// <returns>True if powered; otherwise false.</returns>
        public bool Powered
        {
            get => ReflectionUtil.GetProperty<bool>(Raw, "Powered");
            set => ReflectionUtil.Call(Raw, "SetPower", value);
        }

        /// <summary>
        /// Gets or sets whether the room is marked as off-limits.
        /// </summary>
        /// <returns>True if off-limits; otherwise false.</returns>
        public bool OffLimits
        {
            get => ReflectionUtil.GetField<bool>(Raw, "offLimits");
            set => ReflectionUtil.SetField(Raw, "offLimits", value);
        }

        /// <summary>
        /// Gets or sets whether items may spawn in this room.
        /// </summary>
        /// <returns>True if items can spawn; otherwise false.</returns>
        public bool SpawnItems
        {
            get => ReflectionUtil.GetField<bool>(Raw, "spawnItems");
            set => ReflectionUtil.SetField(Raw, "spawnItems", value);
        }

        /// <summary>
        /// Gets the number of tiles contained within the room.
        /// </summary>
        /// <returns>The tile count as an integer.</returns>
        public int TileCount => ReflectionUtil.GetProperty<int>(Raw, "TileCount");
        /// <summary>
        /// Gets the number of available respawn points for items in the room.
        /// </summary>
        /// <returns>The number of available respawn points.</returns>
        public int AvailableItemRespawnPoints => ReflectionUtil.GetProperty<int>(Raw, "AvailableItemRespawnPoints");

        /// <summary>
        /// Gets the collection of cells composing this room, if available.
        /// </summary>
        public IList? Cells => ReflectionUtil.GetField<IList>(Raw, "cells");

        /// <summary>
        /// Gets the collection of doors associated with this room, if available.
        /// </summary>
        public IList? Doors => ReflectionUtil.GetField<IList>(Raw, "doors");

        /// <summary>
        /// Gets the collection of pickups located in this room, if available.
        /// </summary>
        public IList? Pickups => ReflectionUtil.GetField<IList>(Raw, "pickups");

        /// <summary>
        /// Gets the collection of item spawn points in this room, if available.
        /// </summary>
        public IList? ItemSpawnPoints => ReflectionUtil.GetField<IList>(Raw, "itemSpawnPoints");

        /// <summary>
        /// Gets the collection of rooms connected to this room, if available.
        /// </summary>
        public IList? ConnectedRooms => ReflectionUtil.GetField<IList>(Raw, "connectedRooms");

        /// <summary>
        /// Computes the center position of the room. If the underlying object is a component, returns its transform position.
        /// Otherwise computes the average center of its constituent cells.
        /// </summary>
        /// <returns>The center position as a <see cref="Vector3"/>.</returns>
        public Vector3 Center
        {
            get
            {
                if (Raw is Component component)
                    return component.transform.position;

                IList? cells = Cells;
                if (cells == null || cells.Count == 0)
                    return Vector3.zero;

                Vector3 sum = Vector3.zero;
                int count = 0;

                foreach (object? cell in cells)
                {
                    if (cell == null)
                        continue;

                    Vector3 pos = ReflectionUtil.GetProperty<Vector3>(cell, "CenterWorldPosition");
                    sum += pos;
                    count++;
                }

                return count > 0 ? sum / count : Vector3.zero;
            }
        }

        /// <summary>
        /// Determines whether the room contains the specified world position.
        /// </summary>
        /// <param name="position">The world position to test.</param>
        /// <returns>True if the position is within the room; otherwise false.</returns>
        public bool ContainsPosition(Vector3 position)
        {
            return ReflectionUtil.Call<bool>(Raw, "containsPosition", position);
        }

        /// <summary>
        /// Determines whether the room contains the specified tile coordinates.
        /// </summary>
        /// <param name="intVector2">An object representing integer coordinates (e.g., a vector2 of ints).</param>
        /// <returns>True if the coordinates are within the room; otherwise false.</returns>
        public bool ContainsCoordinates(object intVector2)
        {
            return ReflectionUtil.Call<bool>(Raw, "ContainsCoordinates", intVector2);
        }

        /// <summary>
        /// Sets the power state of the room by invoking the underlying SetPower method.
        /// </summary>
        /// <param name="value">True to power the room; false to unpower.</param>
        public void SetPower(bool value)
        {
            ReflectionUtil.Call(Raw, "SetPower", value);
        }

        /// <summary>
        /// Returns a random safe cell suitable for placing an entity without creating garbage objects.
        /// </summary>
        /// <returns>A cell object or null if none found.</returns>
        public object? RandomEntitySafeCell()
        {
            return ReflectionUtil.Call<object>(Raw, "RandomEntitySafeCellNoGarbage");
        }

        /// <summary>
        /// Returns a random safe cell suitable for events without creating garbage objects.
        /// </summary>
        /// <returns>A cell object or null if none found.</returns>
        public object? RandomEventSafeCell()
        {
            return ReflectionUtil.Call<object>(Raw, "RandomEventSafeCellNoGarbage");
        }

        /// <summary>
        /// Returns all cells considered safe for entities without creating garbage objects.
        /// </summary>
        /// <returns>An <see cref="IList"/> of safe cells, or null if unavailable.</returns>
        public IList? AllEntitySafeCells()
        {
            return ReflectionUtil.Call<IList>(Raw, "AllEntitySafeCellsNoGarbage");
        }

        /// <summary>
        /// Returns all cells considered safe for events without creating garbage objects.
        /// </summary>
        /// <returns>An <see cref="IList"/> of safe cells, or null if unavailable.</returns>
        public IList? AllEventSafeCells()
        {
            return ReflectionUtil.Call<IList>(Raw, "AllEventSafeCellsNoGarbage");
        }

        /// <summary>
        /// Returns all tiles in the room, with options to include off-limits or hard coverage tiles.
        /// </summary>
        /// <param name="includeOffLimits">Whether to include off-limits tiles.</param>
        /// <param name="includeHardCoverage">Whether to include hard coverage tiles.</param>
        /// <returns>An <see cref="IList"/> of tiles, or null if unavailable.</returns>
        public IList? AllTiles(bool includeOffLimits = false, bool includeHardCoverage = false)
        {
            return ReflectionUtil.Call<IList>(
                Raw,
                "AllTilesNoGarbage",
                includeOffLimits,
                includeHardCoverage
            );
        }

        /// <summary>
        /// Retrieves the tile object at the specified index.
        /// </summary>
        /// <param name="index">The index of the tile.</param>
        /// <returns>The tile object or null.</returns>
        public object? TileAtIndex(int index)
        {
            return ReflectionUtil.Call<object>(Raw, "TileAtIndex", index);
        }

        /// <summary>
        /// Updates any cached position information for the room by invoking the underlying UpdatePosition method.
        /// </summary>
        public void UpdatePosition()
        {
            ReflectionUtil.Call(Raw, "UpdatePosition");
        }
    }
}