using System.Collections;
using UnityEngine;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Represents scene/level metadata and related assets.
    /// </summary>
    public sealed class BBPScene
    {
        /// <summary>
        /// The raw underlying scene object wrapped by this API class, or null if none.
        /// </summary>
        public object? Raw { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BBPScene"/> class wrapping a raw object.
        /// </summary>
        /// <param name="raw">The raw underlying object instance or null.</param>
        public BBPScene(object? raw)
        {
            Raw = raw;
        }

        /// <summary>
        /// Determines whether the underlying raw object exists.
        /// </summary>
        public bool Exists => Raw != null;

        /// <summary>
        /// Gets or sets the human-readable title of the level.
        /// </summary>
        /// <returns>The level title as a string.</returns>
        public string LevelTitle
        {
            get => ReflectionUtil.GetField<string>(Raw, "levelTitle") ?? "";
            set => ReflectionUtil.SetField(Raw, "levelTitle", value);
        }

        /// <summary>
        /// Gets or sets the localization key for the scene's name.
        /// </summary>
        /// <returns>The name key as a string.</returns>
        public string NameKey
        {
            get => ReflectionUtil.GetField<string>(Raw, "nameKey") ?? "";
            set => ReflectionUtil.SetField(Raw, "nameKey", value);
        }

        /// <summary>
        /// Gets or sets the numeric level index.
        /// </summary>
        /// <returns>The level number as an integer.</returns>
        public int LevelNumber
        {
            get => ReflectionUtil.GetField<int>(Raw, "levelNo");
            set => ReflectionUtil.SetField(Raw, "levelNo", value);
        }

        /// <summary>
        /// Gets or sets the number of additional NPCs to spawn on this level.
        /// </summary>
        /// <returns>The count of additional NPCs as an integer.</returns>
        public int AdditionalNpcs
        {
            get => ReflectionUtil.GetField<int>(Raw, "additionalNPCs");
            set => ReflectionUtil.SetField(Raw, "additionalNPCs", value);
        }

        /// <summary>
        /// Gets or sets the price for the map in this scene.
        /// </summary>
        /// <returns>The map price as an integer.</returns>
        public int MapPrice
        {
            get => ReflectionUtil.GetField<int>(Raw, "mapPrice");
            set => ReflectionUtil.SetField(Raw, "mapPrice", value);
        }

        /// <summary>
        /// Gets or sets the total number of shop items available on this level.
        /// </summary>
        /// <returns>The total shop items as an integer.</returns>
        public int TotalShopItems
        {
            get => ReflectionUtil.GetField<int>(Raw, "totalShopItems");
            set => ReflectionUtil.SetField(Raw, "totalShopItems", value);
        }

        /// <summary>
        /// Gets or sets whether the store uses data from the next level.
        /// </summary>
        /// <returns>True if the store uses next level data; otherwise false.</returns>
        public bool StoreUsesNextLevelData
        {
            get => ReflectionUtil.GetField<bool>(Raw, "storeUsesNextLevelData");
            set => ReflectionUtil.SetField(Raw, "storeUsesNextLevelData", value);
        }

        /// <summary>
        /// Gets or sets whether the scene is skippable.
        /// </summary>
        /// <returns>True if skippable; otherwise false.</returns>
        public bool Skippable
        {
            get => ReflectionUtil.GetField<bool>(Raw, "skippable");
            set => ReflectionUtil.SetField(Raw, "skippable", value);
        }

        /// <summary>
        /// Gets or sets whether this scene uses an in-game map.
        /// </summary>
        /// <returns>True if a map is used; otherwise false.</returns>
        public bool UsesMap
        {
            get => ReflectionUtil.GetField<bool>(Raw, "usesMap");
            set => ReflectionUtil.SetField(Raw, "usesMap", value);
        }

        /// <summary>
        /// Gets or sets the skybox color for the scene.
        /// </summary>
        /// <returns>The skybox color as a <see cref="Color"/>.</returns>
        public Color SkyboxColor
        {
            get => ReflectionUtil.GetField<Color>(Raw, "skyboxColor");
            set => ReflectionUtil.SetField(Raw, "skyboxColor", value);
        }

        /// <summary>
        /// Gets or sets the cubemap used for the scene's skybox.
        /// </summary>
        /// <returns>The <see cref="Cubemap"/> or null.</returns>
        public Cubemap? Skybox
        {
            get => ReflectionUtil.GetField<Cubemap>(Raw, "skybox");
            set => ReflectionUtil.SetField(Raw, "skybox", value);
        }

        /// <summary>
        /// Gets or sets the scene manager object associated with this scene.
        /// </summary>
        /// <returns>The manager object or null.</returns>
        public object? Manager
        {
            get => ReflectionUtil.GetField<object>(Raw, "manager");
            set => ReflectionUtil.SetField(Raw, "manager", value);
        }

        /// <summary>
        /// Gets or sets the level object associated with the scene.
        /// </summary>
        /// <returns>The level object or null.</returns>
        public object? LevelObject
        {
            get => ReflectionUtil.GetField<object>(Raw, "levelObject");
            set => ReflectionUtil.SetField(Raw, "levelObject", value);
        }

        /// <summary>
        /// Gets or sets the level asset associated with the scene.
        /// </summary>
        /// <returns>The level asset object or null.</returns>
        public object? LevelAsset
        {
            get => ReflectionUtil.GetField<object>(Raw, "levelAsset");
            set => ReflectionUtil.SetField(Raw, "levelAsset", value);
        }

        /// <summary>
        /// Gets or sets an extra asset object associated with the scene.
        /// </summary>
        /// <returns>The extra asset object or null.</returns>
        public object? ExtraAsset
        {
            get => ReflectionUtil.GetField<object>(Raw, "extraAsset");
            set => ReflectionUtil.SetField(Raw, "extraAsset", value);
        }

        /// <summary>
        /// Gets or sets the container object that groups level elements.
        /// </summary>
        /// <returns>The level container object or null.</returns>
        public object? LevelContainer
        {
            get => ReflectionUtil.GetField<object>(Raw, "levelContainer");
            set => ReflectionUtil.SetField(Raw, "levelContainer", value);
        }

        /// <summary>
        /// Gets the next level's scene metadata.
        /// </summary>
        /// <returns>A <see cref="BBPScene"/> instance representing the next level.</returns>
        public BBPScene NextLevel =>
            new BBPScene(ReflectionUtil.GetField<object>(Raw, "nextLevel"));

        /// <summary>
        /// Gets the list of previous levels for this scene if available.
        /// </summary>
        /// <returns>An <see cref="IList"/> of previous levels or null.</returns>
        public IList? PreviousLevels =>
            ReflectionUtil.GetField<IList>(Raw, "previousLevels");

        /// <summary>
        /// Gets or sets the Baldi prefab used for this scene.
        /// </summary>
        /// <returns>The Baldi prefab object or null.</returns>
        public object? BaldiPrefab
        {
            get => ReflectionUtil.GetField<object>(Raw, "baldiPrefab");
            set => ReflectionUtil.SetField(Raw, "baldiPrefab", value);
        }

        /// <summary>
        /// Gets the list of potential NPCs that may appear on this scene.
        /// </summary>
        /// <returns>An <see cref="IList"/> of potential NPCs or null.</returns>
        public IList? PotentialNpcs =>
            ReflectionUtil.GetField<IList>(Raw, "potentialNPCs");

        /// <summary>
        /// Gets or sets forced NPCs for this scene.
        /// </summary>
        /// <returns>An object representing forced NPCs or null.</returns>
        public object? ForcedNpcs
        {
            get => ReflectionUtil.GetField<object>(Raw, "forcedNpcs");
            set => ReflectionUtil.SetField(Raw, "forcedNpcs", value);
        }

        /// <summary>
        /// Gets or sets randomized level objects for the scene.
        /// </summary>
        /// <returns>An object representing randomized level objects or null.</returns>
        public object? RandomizedLevelObjects
        {
            get => ReflectionUtil.GetField<object>(Raw, "randomizedLevelObject");
            set => ReflectionUtil.SetField(Raw, "randomizedLevelObject", value);
        }

        /// <summary>
        /// Gets or sets potential stickers available on this scene.
        /// </summary>
        /// <returns>An object representing potential stickers or null.</returns>
        public object? PotentialStickers
        {
            get => ReflectionUtil.GetField<object>(Raw, "potentialStickers");
            set => ReflectionUtil.SetField(Raw, "potentialStickers", value);
        }

        /// <summary>
        /// Gets or sets the collection of shop items available in this scene.
        /// </summary>
        /// <returns>An object representing shop items or null.</returns>
        public object? ShopItems
        {
            get => ReflectionUtil.GetField<object>(Raw, "shopItems");
            set => ReflectionUtil.SetField(Raw, "shopItems", value);
        }
    }
}