using UnityEngine;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Represents a pickup object in the world and provides access to its properties and interactions.
    /// </summary>
    public sealed class BBPPickup
    {
        /// <summary>
        /// The raw underlying object wrapped by this API class, or null if none.
        /// </summary>
        public object? Raw { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BBPPickup"/> class wrapping a raw object.
        /// </summary>
        /// <param name="raw">The raw underlying object instance or null.</param>
        public BBPPickup(object? raw)
        {
            Raw = raw;
        }

        /// <summary>
        /// Determines whether the underlying raw object exists.
        /// </summary>
        public bool Exists => Raw != null;

        /// <summary>
        /// Gets the world position of the pickup.
        /// </summary>
        /// <returns>The position as a <see cref="Vector3"/>.</returns>
        public Vector3 Position =>
            Raw is Component c ? c.transform.position : Vector3.zero;

        /// <summary>
        /// Gets or sets the name of the pickup's GameObject or underlying type name.
        /// </summary>
        /// <returns>The name as a string.</returns>
        public string Name
        {
            get => Raw is Component c ? c.gameObject.name : Raw?.GetType().Name ?? "NULL";
            set
            {
                if (Raw is Component c)
                    c.gameObject.name = value;
            }
        }

        /// <summary>
        /// Gets or sets whether the pickup's GameObject is active in the scene.
        /// </summary>
        /// <returns>True if active; otherwise false.</returns>
        public bool Active
        {
            get => Raw is Component c && c.gameObject.activeSelf;
            set
            {
                if (Raw is Component c)
                    c.gameObject.SetActive(value);
            }
        }

        /// <summary>
        /// Gets or sets the item object contained in the pickup.
        /// </summary>
        /// <returns>A <see cref="BBPItemObject"/> representing the item.</returns>
        public BBPItemObject Item
        {
            get => new BBPItemObject(ReflectionUtil.GetField<object>(Raw, "item"));
            set => ReflectionUtil.SetField(Raw, "item", value.Raw);
        }

        /// <summary>
        /// Gets the sprite renderer used to display the item's sprite.
        /// </summary>
        /// <returns>The <see cref="SpriteRenderer"/> or null.</returns>
        public SpriteRenderer? SpriteRenderer =>
            ReflectionUtil.GetField<SpriteRenderer>(Raw, "itemSprite");

        /// <summary>
        /// Gets or sets the pre-icon object associated with the pickup.
        /// </summary>
        /// <returns>The pre-icon object or null.</returns>
        public object? IconPre
        {
            get => ReflectionUtil.GetField<object>(Raw, "iconPre");
            set => ReflectionUtil.SetField(Raw, "iconPre", value);
        }

        /// <summary>
        /// Gets or sets the icon object associated with the pickup.
        /// </summary>
        /// <returns>The icon object or null.</returns>
        public object? Icon
        {
            get => ReflectionUtil.GetField<object>(Raw, "icon");
            set => ReflectionUtil.SetField(Raw, "icon", value);
        }

        /// <summary>
        /// Gets or sets the sound object associated with the pickup.
        /// </summary>
        /// <returns>A <see cref="BBPSoundObject"/> instance.</returns>
        public BBPSoundObject Sound
        {
            get => new BBPSoundObject(ReflectionUtil.GetField<object>(Raw, "sound"));
            set => ReflectionUtil.SetField(Raw, "sound", value.Raw);
        }

        /// <summary>
        /// Gets or sets the purchase price for the pickup.
        /// </summary>
        /// <returns>The price as an integer.</returns>
        public int Price
        {
            get => ReflectionUtil.GetField<int>(Raw, "price");
            set => ReflectionUtil.SetField(Raw, "price", value);
        }

        /// <summary>
        /// Gets or sets whether the pickup is free.
        /// </summary>
        /// <returns>True if free; otherwise false.</returns>
        public bool Free
        {
            get => ReflectionUtil.GetField<bool>(Raw, "free");
            set => ReflectionUtil.SetField(Raw, "free", value);
        }

        /// <summary>
        /// Gets or sets whether the pickup shows a description when inspected.
        /// </summary>
        /// <returns>True if the description is shown; otherwise false.</returns>
        public bool ShowDescription
        {
            get => ReflectionUtil.GetField<bool>(Raw, "showDescription");
            set => ReflectionUtil.SetField(Raw, "showDescription", value);
        }

        /// <summary>
        /// Gets or sets whether the pickup is repeatable.
        /// </summary>
        /// <returns>True if repeatable; otherwise false.</returns>
        public bool Repeatable
        {
            get => ReflectionUtil.GetField<bool>(Raw, "repeatable");
            set => ReflectionUtil.SetField(Raw, "repeatable", value);
        }

        /// <summary>
        /// Gets or sets whether the pickup survives after being picked up.
        /// </summary>
        /// <returns>True if it survives pickup; otherwise false.</returns>
        public bool SurvivePickup
        {
            get => ReflectionUtil.GetField<bool>(Raw, "survivePickup");
            set => ReflectionUtil.SetField(Raw, "survivePickup", value);
        }

        /// <summary>
        /// Gets or sets whether the pickup still contains an item.
        /// </summary>
        /// <returns>True if still has an item; otherwise false.</returns>
        public bool StillHasItem
        {
            get => ReflectionUtil.GetField<bool>(Raw, "stillHasItem");
            set => ReflectionUtil.SetField(Raw, "stillHasItem", value);
        }

        /// <summary>
        /// Assigns an item object to this pickup.
        /// </summary>
        /// <param name="item">The item object to assign.</param>
        public void AssignItem(BBPItemObject item)
        {
            if (item.Raw != null)
                ReflectionUtil.Call(Raw, "AssignItem", item.Raw);
        }

        /// <summary>
        /// Assigns an item to this pickup using a raw object reference.
        /// </summary>
        /// <param name="item">The raw item object.</param>
        public void AssignItem(object item)
        {
            ReflectionUtil.Call(Raw, "AssignItem", item);
        }

        /// <summary>
        /// Simulates a click on the pickup by the specified player index.
        /// </summary>
        /// <param name="player">The player index invoking the click.</param>
        public void Clicked(int player = 0)
        {
            ReflectionUtil.Call(Raw, "Clicked", player);
        }

        /// <summary>
        /// Collects the pickup on behalf of the specified player index.
        /// </summary>
        /// <param name="player">The player index collecting the pickup.</param>
        public void Collect(int player = 0)
        {
            ReflectionUtil.Call(Raw, "Collect", player);
        }

        /// <summary>
        /// Hides or shows the pickup's GameObject.
        /// </summary>
        /// <param name="hidden">True to hide; false to show.</param>
        public void Hide(bool hidden)
        {
            ReflectionUtil.Call(Raw, "Hide", hidden);
        }

        /// <summary>
        /// Checks whether the pickup is hidden from being clickable.
        /// </summary>
        /// <returns>True if clickable is hidden; otherwise false.</returns>
        public bool ClickableHidden()
        {
            return ReflectionUtil.Call<bool>(Raw, "ClickableHidden");
        }

        /// <summary>
        /// Checks whether the pickup requires normal player height to be clickable.
        /// </summary>
        /// <returns>True if normal height is required; otherwise false.</returns>
        public bool ClickableRequiresNormalHeight()
        {
            return ReflectionUtil.Call<bool>(Raw, "ClickableRequiresNormalHeight");
        }

        /// <summary>
        /// Notifies that the pickup became sighted for clickable interaction by the given player.
        /// </summary>
        /// <param name="player">The player index that sighted the pickup.</param>
        public void ClickableSighted(int player = 0)
        {
            ReflectionUtil.Call(Raw, "ClickableSighted", player);
        }

        /// <summary>
        /// Notifies that the pickup is no longer sighted for clickable interaction by the given player.
        /// </summary>
        /// <param name="player">The player index that lost sight of the pickup.</param>
        public void ClickableUnsighted(int player = 0)
        {
            ReflectionUtil.Call(Raw, "ClickableUnsighted", player);
        }
    }
}