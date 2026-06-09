using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BBPGlue.Core;

namespace BBPGlue.API.HUD
{
    /// <summary>
    /// Wraps the HUD announcement display, also known as BaldiTV.
    /// </summary>
    public sealed class BBPHudAnnouncements
    {
        private object? Tv => BBP.Hud.BaldiTv;

        /// <summary>
        /// Gets whether the BaldiTV object exists.
        /// </summary>
        public bool Exists => Tv != null;

        /// <summary>
        /// Gets the static overlay object.
        /// </summary>
        public GameObject? StaticObject => ReflectionUtil.GetField<GameObject>(Tv, "staticObject");
        
        /// <summary>
        /// Gets the exclamation mark object.
        /// </summary>
        public GameObject? ExclamationObject => ReflectionUtil.GetField<GameObject>(Tv, "exclamationObject");

        /// <summary>
        /// Gets the BaldiTV animator.
        /// </summary>
        public Animator? Animator => ReflectionUtil.GetField<Animator>(Tv, "baldiTvAnimator");

        /// <summary>
        /// Gets the BaldiTV audio manager.
        /// </summary>
        public object? AudioManager => ReflectionUtil.GetField<object>(Tv, "baldiTvAudioManager");

        /// <summary>
        /// Gets the Baldi image component.
        /// </summary>
        public Image? BaldiImage => ReflectionUtil.GetField<Image>(Tv, "baldiImage");
        
        /// <summary>
        /// Gets the static image component.
        /// </summary>
        public Image? StaticImage => ReflectionUtil.GetField<Image>(Tv, "staticImage");

        /// <summary>
        /// Gets the time text component.
        /// </summary>
        public TMP_Text? TimeText => ReflectionUtil.GetField<TMP_Text>(Tv, "timeTmp");

        /// <summary>
        /// Gets or sets the first static sprite.
        /// </summary>
        public Sprite? StaticSprite1
        {
            get => ReflectionUtil.GetField<Sprite>(Tv, "static1");
            set => ReflectionUtil.SetField(Tv, "static1", value);
        }

        /// <summary>
        /// Gets or sets the second static sprite.
        /// </summary>
        public Sprite? StaticSprite2
        {
            get => ReflectionUtil.GetField<Sprite>(Tv, "static2");
            set => ReflectionUtil.SetField(Tv, "static2", value);
        }

        /// <summary>
        /// Gets or sets the time-limit ticking sound.
        /// </summary>
        public object? TimeLimitTickingSound
        {
            get => ReflectionUtil.GetField<object>(Tv, "timeLimitTicking");
            set => ReflectionUtil.SetField(Tv, "timeLimitTicking", value);
        }

        /// <summary>
        /// Gets or sets whether BaldiTV is allowed to move.
        /// </summary>
        public bool Moves
        {
            get => ReflectionUtil.GetField<bool>(Tv, "moves");
            set => ReflectionUtil.SetField(Tv, "moves", value);
        }

        /// <summary>
        /// Gets whether BaldiTV is currently busy.
        /// </summary>
        public bool Busy => ReflectionUtil.GetField<bool>(Tv, "busy");

        /// <summary>
        /// Reinitializes BaldiTV.
        /// </summary>
        public void ReInit() => ReflectionUtil.Call(Tv, "ReInit");

        /// <summary>
        /// Plays a BaldiTV speech sound.
        /// </summary>
        /// <param name="soundObject">The sound object to play.</param>
        public void Speak(object soundObject) =>
            ReflectionUtil.Call(Tv, "Speak", soundObject);

        /// <summary>
        /// Announces an event through BaldiTV.
        /// </summary>
        /// <param name="soundObject">The event announcement sound.</param>
        public void AnnounceEvent(object soundObject) =>
            ReflectionUtil.Call(Tv, "AnnounceEvent", soundObject);

        /// <summary>
        /// Shows the level time warning.
        /// </summary>
        public void ShowLevelTimeWarning()
        {
            object? environment = BBP.Environment.Raw;
            if (environment == null)
                return;

            ReflectionUtil.Call(
                Tv,
                "ShowLevelTimeWarning",
                environment
            );
        }

        /// <summary>
        /// Sets and shows the Baldi sprite.
        /// </summary>
        /// <param name="sprite">The sprite to display.</param>
        public void SetBaldiSprite(Sprite sprite)
        {
            if (BaldiImage != null)
            {
                BaldiImage.sprite = sprite;
                BaldiImage.enabled = true;
            }
        }

        /// <summary>
        /// Sets the static animation sprites.
        /// </summary>
        /// <param name="sprite1">The first static sprite.</param>
        /// <param name="sprite2">The second static sprite.</param>
        public void SetStaticSprites(Sprite sprite1, Sprite sprite2)
        {
            StaticSprite1 = sprite1;
            StaticSprite2 = sprite2;
        }

        /// <summary>
        /// Shows or hides the static overlay.
        /// </summary>
        /// <param name="visible">True to show the static overlay; false to hide it.</param>
        public void SetStaticVisible(bool visible)
        {
            StaticObject?.SetActive(visible);
        }

        /// <summary>
        /// Shows or hides the exclamation mark.
        /// </summary>
        /// <param name="visible">True to show the exclamation mark; false to hide it.</param>
        public void SetExclamationVisible(bool visible)
        {
            ExclamationObject?.SetActive(visible);
        }

        /// <summary>
        /// Shows or hides the Baldi image.
        /// </summary>
        /// <param name="visible">True to show the image; false to hide it.</param>
        public void SetBaldiVisible(bool visible)
        {
            if (BaldiImage != null)
                BaldiImage.enabled = visible;
        }

        /// <summary>
        /// Shows or hides the time text.
        /// </summary>
        /// <param name="visible">True to show the time text; false to hide it.</param>
        public void SetTimeTextVisible(bool visible)
        {
            if (TimeText != null)
                TimeText.gameObject.SetActive(visible);
        }

        /// <summary>
        /// Sets the time text directly.
        /// </summary>
        /// <param name="text">The text to display.</param>
        public void SetTimeTextRaw(string text)
        {
            if (TimeText != null)
                TimeText.text = text;
        }

        /// <summary>
        /// Plays a BaldiTV animation state.
        /// </summary>
        /// <param name="stateName">The animation state name.</param>
        public void PlayAnimation(string stateName)
        {
            Animator?.Play(stateName, -1, 0f);
        }

        /// <summary>
        /// Sets the BaldiTV Active animator parameter.
        /// </summary>
        /// <param name="active">The value to assign.</param>
        public void SetActiveAnimation(bool active)
        {
            Animator?.SetBool("Active", active);
        }

        /// <summary>
        /// Hides all BaldiTV visual elements controlled by this wrapper.
        /// </summary>
        public void ResetVisuals()
        {
            SetStaticVisible(false);
            SetExclamationVisible(false);
            SetBaldiVisible(false);
            SetTimeTextVisible(false);
        }
    }
}