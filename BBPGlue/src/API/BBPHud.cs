using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BBPGlue.Core;
using BBPGlue.API.HUD;

namespace BBPGlue.API
{
    /// <summary>
    /// Accessor for HUD elements and controls.
    /// </summary>
    public sealed class BBPHud
    {
        /// <summary>
        /// Announcement-related HUD helpers.
        /// </summary>
        public BBPHudAnnouncements Announcements { get; } = new BBPHudAnnouncements();

        /// <summary>
        /// The underlying HUD raw object from the game.
        /// </summary>
        public object? Raw => BBP.Game.GetHud(0);
        /// <summary>
        /// Whether the HUD is present in the current scene.
        /// </summary>
        public bool Exists => Raw != null;

        /// <summary>
        /// Object representing Baldi TV functionality.
        /// </summary>
        public object? BaldiTv => ReflectionUtil.GetProperty<object>(Raw, "BaldiTv");
        /// <summary>
        /// Animator or controller used for points display animations.
        /// </summary>
        public object? PointsAnimator => ReflectionUtil.GetProperty<object>(Raw, "PointsAnimator");

        /// <summary>
        /// Root Canvas for HUD rendering.
        /// </summary>
        public Canvas? Canvas => ReflectionUtil.Call<Canvas>(Raw, "Canvas");
        /// <summary>
        /// Canvas scaler used for HUD layout.
        /// </summary>
        public CanvasScaler? CanvasScaler => ReflectionUtil.GetField<CanvasScaler>(Raw, "canvasScaler");

        /// <summary>
        /// The inventory UI controller object.
        /// </summary>
        public object? Inventory => ReflectionUtil.GetField<object>(Raw, "inventory");
        /// <summary>
        /// Gauge manager for stamina/other gauges.
        /// </summary>
        public object? GaugeManager => ReflectionUtil.GetField<object>(Raw, "gaugeManager");

        /// <summary>
        /// RectTransform for the stamina needle UI element.
        /// </summary>
        public RectTransform? StaminaNeedle => ReflectionUtil.GetField<RectTransform>(Raw, "staminaNeedle");
        /// <summary>
        /// Reticle image used for aiming/interaction.
        /// </summary>
        public Image? Reticle => ReflectionUtil.GetField<Image>(Raw, "reticle");
        /// <summary>
        /// Text field showing the currently selected item's title.
        /// </summary>
        public TMP_Text? ItemTitle => ReflectionUtil.GetField<TMP_Text>(Raw, "itemTitle");
        /// <summary>
        /// Animator controlling general HUD animations.
        /// </summary>
        public Animator? Animator => ReflectionUtil.GetField<Animator>(Raw, "animator");
        /// <summary>
        /// Animator used specifically for notebook UI animations.
        /// </summary>
        public Animator? NotebookAnimator => ReflectionUtil.GetField<Animator>(Raw, "notebookAnimator");

        /// <summary>
        /// Array of item slot sprite images.
        /// </summary>
        public Image[]? ItemSprites => ReflectionUtil.GetField<Image[]>(Raw, "itemSprites");
        /// <summary>
        /// Array of background images for item slots.
        /// </summary>
        public RawImage[]? ItemBackgrounds => ReflectionUtil.GetField<RawImage[]>(Raw, "itemBackgrounds");
        /// <summary>
        /// Sprites that are darkened when appropriate.
        /// </summary>
        public Image[]? SpritesToDarken => ReflectionUtil.GetField<Image[]>(Raw, "spritesToDarken");
        /// <summary>
        /// Text boxes used by the HUD for various messages.
        /// </summary>
        public TMP_Text[]? TextBoxes => ReflectionUtil.GetField<TMP_Text[]>(Raw, "textBox");
        /// <summary>
        /// GameObjects used to display notebook pages.
        /// </summary>
        public GameObject[]? NotebookDisplay => ReflectionUtil.GetField<GameObject[]>(Raw, "notebookDisplay");

        /// <summary>
        /// Sprite used when the reticle is off.
        /// </summary>
        public Sprite? ReticleOffSprite
        {
            get => ReflectionUtil.GetField<Sprite>(Raw, "retOff");
            set => ReflectionUtil.SetField(Raw, "retOff", value);
        }

        /// <summary>
        /// Sprite used when the reticle is on.
        /// </summary>
        public Sprite? ReticleOnSprite
        {
            get => ReflectionUtil.GetField<Sprite>(Raw, "retOn");
            set => ReflectionUtil.SetField(Raw, "retOn", value);
        }

        /// <summary>
        /// Color used for darkening UI elements.
        /// </summary>
        public Color DarkColor
        {
            get => ReflectionUtil.GetField<Color>(Raw, "darkColor");
            set => ReflectionUtil.SetField(Raw, "darkColor", value);
        }

        /// <summary>
        /// Speed at which the stamina needle animates.
        /// </summary>
        public float NeedleSpeed
        {
            get => ReflectionUtil.GetField<float>(Raw, "needleSpeed");
            set => ReflectionUtil.SetField(Raw, "needleSpeed", value);
        }

        /// <summary>
        /// Whether the HUD is currently hidden.
        /// </summary>
        public bool Hidden => ReflectionUtil.GetField<bool>(Raw, "hidden");

        /// <summary>
        /// Hides or shows the HUD.
        /// </summary>
        /// <param name="hidden">True to hide; false to show.</param>
        public void Hide(bool hidden) => ReflectionUtil.Call(Raw, "Hide", hidden);
        /// <summary>
        /// Reinitializes HUD internals.
        /// </summary>
        public void ReInit() => ReflectionUtil.Call(Raw, "ReInit");
        /// <summary>
        /// Darkens or undarkens the HUD.
        /// </summary>
        /// <param name="darken">True to darken; false to restore.</param>
        public void Darken(bool darken) => ReflectionUtil.Call(Raw, "Darken", darken);
        /// <summary>
        /// Forces a color update on HUD elements.
        /// </summary>
        public void ForceUpdateColor() => ReflectionUtil.Call(Raw, "ForceUpdateColor");

        /// <summary>
        /// Sets the displayed stamina value.
        /// </summary>
        /// <param name="value">Stamina value to display.</param>
        public void SetStamina(float value) => ReflectionUtil.Call(Raw, "SetStaminaValue", value);
        /// <summary>
        /// Enables or disables the reticle.
        /// </summary>
        /// <param name="active">True to enable; false to disable.</param>
        public void UpdateReticle(bool active) => ReflectionUtil.Call(Raw, "UpdateReticle", active);

        /// <summary>
        /// Sets a tooltip key to display localized text.
        /// </summary>
        /// <param name="key">Tooltip localization key.</param>
        public void SetTooltip(string key) => ReflectionUtil.Call(Raw, "SetTooltip", key);
        /// <summary>
        /// Closes any active tooltip.
        /// </summary>
        public void CloseTooltip() => ReflectionUtil.Call(Raw, "CloseTooltip");

        /// <summary>
        /// Shows or hides the notebook display.
        /// </summary>
        /// <param name="visible">True to show; false to hide.</param>
        public void SetNotebookDisplay(bool visible) =>
            ReflectionUtil.Call(Raw, "SetNotebookDisplay", visible);

        /// <summary>
        /// Updates notebook text at the given index.
        /// </summary>
        /// <param name="index">Notebook line index.</param>
        /// <param name="text">Text to set.</param>
        /// <param name="spin">Whether to apply a spinning animation.</param>
        public void UpdateNotebookText(int index, string text, bool spin = false) =>
            ReflectionUtil.Call(Raw, "UpdateNotebookText", index, text, spin);

        /// <summary>
        /// Updates the inventory UI size.
        /// </summary>
        /// <param name="size">New inventory size.</param>
        public void UpdateInventorySize(int size) =>
            ReflectionUtil.Call(Raw, "UpdateInventorySize", size);

        /// <summary>
        /// Selects an item slot visually with the provided key.
        /// </summary>
        /// <param name="slot">Slot index.</param>
        /// <param name="key">Localization key for the selection.</param>
        public void SetItemSelect(int slot, string key) =>
            ReflectionUtil.Call(Raw, "SetItemSelect", slot, key);

        /// <summary>
        /// Updates the icon for an inventory slot.
        /// </summary>
        /// <param name="slot">Slot index.</param>
        /// <param name="sprite">Icon sprite to set.</param>
        public void UpdateItemIcon(int slot, Sprite sprite) =>
            ReflectionUtil.Call(Raw, "UpdateItemIcon", slot, sprite);

        /// <summary>
        /// Activates or deactivates the baldicator UI element.
        /// </summary>
        /// <param name="coming">True when the baldicator is appearing; false when hiding.</param>
        public void ActivateBaldicator(bool coming) =>
            ReflectionUtil.Call(Raw, "ActivateBaldicator", coming);

        /// <summary>
        /// Shows a collected sticker animation using the provided sprite.
        /// </summary>
        /// <param name="stickerSprite">Sprite to display as a sticker.</param>
        public void ShowCollectedSticker(Sprite stickerSprite) =>
            ReflectionUtil.Call(Raw, "ShowCollectedSticker", stickerSprite);

        /// <summary>
        /// Stops any running sticker animation.
        /// </summary>
        public void StopStickerAnimation() =>
            ReflectionUtil.Call(Raw, "StopStickerAnimation");

        /// <summary>
        /// Sets the raw item title text on the HUD.
        /// </summary>
        /// <param name="text">Text to set.</param>
        public void SetItemTitleRaw(string text)
        {
            if (ItemTitle != null)
                ItemTitle.text = text;
        }

        /// <summary>
        /// Sets the text of a HUD text box by index.
        /// </summary>
        /// <param name="index">Index of the text box.</param>
        /// <param name="text">Text to set.</param>
        public void SetTextBoxRaw(int index, string text)
        {
            TMP_Text[]? boxes = TextBoxes;

            if (boxes == null || index < 0 || index >= boxes.Length || boxes[index] == null)
                return;

            boxes[index].text = text;
        }

        /// <summary>
        /// Sets the reticle sprites for off and on states.
        /// </summary>
        /// <param name="offSprite">Sprite used when reticle is off.</param>
        /// <param name="onSprite">Sprite used when reticle is on.</param>
        public void SetReticleSprites(Sprite offSprite, Sprite onSprite)
        {
            ReticleOffSprite = offSprite;
            ReticleOnSprite = onSprite;
            UpdateReticle(false);
        }

        /// <summary>
        /// Replaces the reticle image with the provided sprite.
        /// </summary>
        /// <param name="sprite">Sprite to set as the reticle.</param>
        public void SetReticleImage(Sprite sprite)
        {
            if (Reticle != null)
                Reticle.sprite = sprite;
        }

        /// <summary>
        /// Sets an item slot image directly.
        /// </summary>
        /// <param name="slot">Slot index.</param>
        /// <param name="sprite">Sprite to assign.</param>
        public void SetItemSpriteRaw(int slot, Sprite sprite)
        {
            Image[]? sprites = ItemSprites;

            if (sprites == null || slot < 0 || slot >= sprites.Length || sprites[slot] == null)
                return;

            sprites[slot].sprite = sprite;
        }

        /// <summary>
        /// Sets the background color for an item slot.
        /// </summary>
        /// <param name="slot">Slot index.</param>
        /// <param name="color">Color to apply.</param>
        public void SetItemBackgroundColor(int slot, Color color)
        {
            RawImage[]? backgrounds = ItemBackgrounds;

            if (backgrounds == null || slot < 0 || slot >= backgrounds.Length || backgrounds[slot] == null)
                return;

            backgrounds[slot].color = color;
        }

        /// <summary>
        /// Applies a color to all sprites that can be darkened.
        /// </summary>
        /// <param name="color">Color to set.</param>
        public void SetAllDarkenableSpritesColor(Color color)
        {
            Image[]? sprites = SpritesToDarken;

            if (sprites == null)
                return;

            foreach (Image image in sprites)
            {
                if (image != null)
                    image.color = color;
            }
        }

        /// <summary>
        /// Plays a HUD animator state.
        /// </summary>
        /// <param name="stateName">Animator state name.</param>
        public void PlayHudAnimation(string stateName)
        {
            Animator?.Play(stateName, -1, 0f);
        }

        /// <summary>
        /// Plays a notebook animator state.
        /// </summary>
        /// <param name="stateName">Animator state name.</param>
        public void PlayNotebookAnimation(string stateName)
        {
            NotebookAnimator?.Play(stateName, -1, 0f);
        }
    }
}