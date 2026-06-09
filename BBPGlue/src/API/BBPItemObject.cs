using UnityEngine;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Wrapper for an item object providing access to metadata, sprites, and sounds.
    /// </summary>
    public sealed class BBPItemObject
    {
        /// <summary>
        /// The raw underlying object wrapped by this API class, or null if none.
        /// </summary>
        public object? Raw { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BBPItemObject"/> class wrapping a raw object.
        /// </summary>
        /// <param name="raw">The raw underlying object instance or null.</param>
        public BBPItemObject(object? raw)
        {
            Raw = raw;
        }

        /// <summary>
        /// Determines whether the underlying raw object exists.
        /// </summary>
        public bool Exists => Raw != null;

        /// <summary>
        /// Gets or sets the raw item reference contained by this wrapper.
        /// </summary>
        /// <returns>The raw item object or null.</returns>
        public object? Item
        {
            get => ReflectionUtil.GetField<object>(Raw, "item");
            set => ReflectionUtil.SetField(Raw, "item", value);
        }

        /// <summary>
        /// Gets or sets the item type object associated with this item.
        /// </summary>
        /// <returns>The item type object or null.</returns>
        public object? ItemType
        {
            get => ReflectionUtil.GetField<object>(Raw, "itemType");
            set => ReflectionUtil.SetField(Raw, "itemType", value);
        }

        /// <summary>
        /// Gets the name of the item type as a string, or "None" if not available.
        /// </summary>
        /// <returns>The item type name.</returns>
        public string ItemTypeName => ItemType?.ToString() ?? "None";


        /// <summary>
        /// Gets the name of the item type as a string, or "None" if not available.
        /// </summary>
        /// <returns>The item type name.</returns>
        public string Type => ItemTypeName;

        /// <summary>
        /// Gets or sets the small sprite used to represent the item.
        /// </summary>
        /// <returns>The small <see cref="Sprite"/> or null.</returns>
        public Sprite? SmallSprite
        {
            get => ReflectionUtil.GetField<Sprite>(Raw, "itemSpriteSmall");
            set => ReflectionUtil.SetField(Raw, "itemSpriteSmall", value);
        }

        /// <summary>
        /// Gets or sets the large sprite used to represent the item.
        /// </summary>
        /// <returns>The large <see cref="Sprite"/> or null.</returns>
        public Sprite? LargeSprite
        {
            get => ReflectionUtil.GetField<Sprite>(Raw, "itemSpriteLarge");
            set => ReflectionUtil.SetField(Raw, "itemSpriteLarge", value);
        }

        /// <summary>
        /// Gets the sound object that overrides the default pickup sound, if any.
        /// </summary>
        /// <returns>A <see cref="BBPSoundObject"/> instance.</returns>
        public BBPSoundObject PickupSoundOverride
        {
            get => new BBPSoundObject(ReflectionUtil.GetField<object>(Raw, "audPickupOverride"));
        }

        /// <summary>
        /// Gets or sets the localization key for the item's name.
        /// </summary>
        /// <returns>The name key as a string.</returns>
        public string NameKey
        {
            get => ReflectionUtil.GetField<string>(Raw, "nameKey") ?? "";
            set => ReflectionUtil.SetField(Raw, "nameKey", value);
        }

        /// <summary>
        /// Gets or sets the item's name.
        /// </summary>
        /// <returns>The name as a string.</returns>
        public string Name => NameKey;

        /// <summary>
        /// Gets or sets the localization key for the item's description.
        /// </summary>
        /// <returns>The description key as a string.</returns>
        public string DescriptionKey
        {
            get => ReflectionUtil.GetField<string>(Raw, "descKey") ?? "";
            set => ReflectionUtil.SetField(Raw, "descKey", value);
        }

        /// <summary>
        /// Gets or sets the item's description.
        /// </summary>
        /// <returns>The description as a string.</returns>
        public string Description => DescriptionKey;

        /// <summary>
        /// Gets or sets the in-game value of the item.
        /// </summary>
        /// <returns>The value as an integer.</returns>
        public int Value
        {
            get => ReflectionUtil.GetField<int>(Raw, "value");
            set => ReflectionUtil.SetField(Raw, "value", value);
        }

        /// <summary>
        /// Gets or sets the purchase price of the item.
        /// </summary>
        /// <returns>The price as an integer.</returns>
        public int Price
        {
            get => ReflectionUtil.GetField<int>(Raw, "price");
            set => ReflectionUtil.SetField(Raw, "price", value);
        }

        /// <summary>
        /// Gets or sets whether the item is added to the player's inventory when picked up.
        /// </summary>
        /// <returns>True if added to inventory; otherwise false.</returns>
        public bool AddToInventory
        {
            get => ReflectionUtil.GetField<bool>(Raw, "addToInventory");
            set => ReflectionUtil.SetField(Raw, "addToInventory", value);
        }

        /// <summary>
        /// Gets or sets whether the item's override behavior is disabled.
        /// </summary>
        /// <returns>True if override is disabled; otherwise false.</returns>
        public bool OverrideDisabled
        {
            get => ReflectionUtil.GetField<bool>(Raw, "overrideDisabled");
            set => ReflectionUtil.SetField(Raw, "overrideDisabled", value);
        }
    }
}