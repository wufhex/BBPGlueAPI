using UnityEngine;
using UnityEngine.Audio;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Wrapper around a sound object providing access to clip, mixing, and subtitle settings.
    /// </summary>
    public sealed class BBPSoundObject
    {
        /// <summary>
        /// The raw underlying sound object wrapped by this API class, or null if none.
        /// </summary>
        public object? Raw { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BBPSoundObject"/> class wrapping a raw object.
        /// </summary>
        /// <param name="raw">The raw underlying object instance or null.</param>
        public BBPSoundObject(object? raw)
        {
            Raw = raw;
        }

        /// <summary>
        /// Determines whether the underlying raw object exists.
        /// </summary>
        public bool Exists => Raw != null;

        /// <summary>
        /// Gets or sets the primary audio clip for the sound object.
        /// </summary>
        /// <returns>The <see cref="AudioClip"/> or null.</returns>
        public AudioClip? Clip
        {
            get => ReflectionUtil.GetField<AudioClip>(Raw, "soundClip");
            set => ReflectionUtil.SetField(Raw, "soundClip", value);
        }

        /// <summary>
        /// Gets the localized audio clip if available.
        /// </summary>
        /// <returns>The localized <see cref="AudioClip"/> or null.</returns>
        public AudioClip? LocalizedClip =>
            ReflectionUtil.GetProperty<AudioClip>(Raw, "localizedClip");

        /// <summary>
        /// Gets or sets the sound key used for identification or localization lookup.
        /// </summary>
        /// <returns>The sound key as a string.</returns>
        public string Key
        {
            get => ReflectionUtil.GetField<string>(Raw, "soundKey") ?? "";
            set => ReflectionUtil.SetField(Raw, "soundKey", value);
        }

        /// <summary>
        /// Gets or sets the sound type enum/value associated with the sound.
        /// </summary>
        /// <returns>The sound type object or null.</returns>
        public object? SoundType
        {
            get => ReflectionUtil.GetField<object>(Raw, "soundType");
            set => ReflectionUtil.SetField(Raw, "soundType", value);
        }

        /// <summary>
        /// Gets or sets an optional mixer group override for the sound.
        /// </summary>
        /// <returns>The <see cref="AudioMixerGroup"/> or null.</returns>
        public AudioMixerGroup? MixerOverride
        {
            get => ReflectionUtil.GetField<AudioMixerGroup>(Raw, "mixerOverride");
            set => ReflectionUtil.SetField(Raw, "mixerOverride", value);
        }

        /// <summary>
        /// Gets or sets the volume multiplier applied to this sound.
        /// </summary>
        /// <returns>The volume multiplier as a float.</returns>
        public float VolumeMultiplier
        {
            get => ReflectionUtil.GetField<float>(Raw, "volumeMultiplier");
            set => ReflectionUtil.SetField(Raw, "volumeMultiplier", value);
        }

        /// <summary>
        /// Gets or sets the duration for subtitles associated with this sound.
        /// </summary>
        /// <returns>The subtitle duration in seconds as a float.</returns>
        public float SubtitleDuration
        {
            get => ReflectionUtil.GetField<float>(Raw, "subDuration");
            set => ReflectionUtil.SetField(Raw, "subDuration", value);
        }

        /// <summary>
        /// Gets or sets the color used for subtitles or visual indicators for this sound.
        /// </summary>
        /// <returns>The <see cref="Color"/> value.</returns>
        public Color Color
        {
            get => ReflectionUtil.GetField<Color>(Raw, "color");
            set => ReflectionUtil.SetField(Raw, "color", value);
        }

        /// <summary>
        /// Gets or sets whether subtitles should be displayed for this sound.
        /// </summary>
        /// <returns>True if subtitles are enabled; otherwise false.</returns>
        public bool Subtitle
        {
            get => ReflectionUtil.GetField<bool>(Raw, "subtitle");
            set => ReflectionUtil.SetField(Raw, "subtitle", value);
        }

        /// <summary>
        /// Gets or sets whether the sound data is encrypted.
        /// </summary>
        /// <returns>True if encrypted; otherwise false.</returns>
        public bool Encrypted
        {
            get => ReflectionUtil.GetField<bool>(Raw, "encrypted");
            set => ReflectionUtil.SetField(Raw, "encrypted", value);
        }

        /// <summary>
        /// Gets or sets whether settings for this sound are locked from modification.
        /// </summary>
        /// <returns>True if settings are locked; otherwise false.</returns>
        public bool LockSettings
        {
            get => ReflectionUtil.GetField<bool>(Raw, "lockSettings");
            set => ReflectionUtil.SetField(Raw, "lockSettings", value);
        }

        /// <summary>
        /// Gets or sets whether this sound has an associated animation.
        /// </summary>
        /// <returns>True if an animation exists; otherwise false.</returns>
        public bool HasAnimation
        {
            get => ReflectionUtil.GetField<bool>(Raw, "hasAnimation");
            set => ReflectionUtil.SetField(Raw, "hasAnimation", value);
        }

        /// <summary>
        /// Gets or sets whether this sound should be added to memory for later playback.
        /// </summary>
        /// <returns>True if added to memory; otherwise false.</returns>
        public bool AddToMemory
        {
            get => ReflectionUtil.GetField<bool>(Raw, "addToMemory");
            set => ReflectionUtil.SetField(Raw, "addToMemory", value);
        }

        /// <summary>
        /// Gets whether the wrapped object is a looping sound object type.
        /// </summary>
        /// <returns>True if the underlying type name equals "LoopingSoundObject"; otherwise false.</returns>
        public bool IsLoopingSoundObject =>
            Raw != null && Raw.GetType().Name == "LoopingSoundObject";

        /// <summary>
        /// Gets or sets the set of audio clips usable by this sound object.
        /// </summary>
        /// <returns>An array of <see cref="AudioClip"/> or null.</returns>
        public AudioClip[]? Clips
        {
            get => ReflectionUtil.GetField<AudioClip[]>(Raw, "clips");
            set => ReflectionUtil.SetField(Raw, "clips", value);
        }

        /// <summary>
        /// Gets or sets the mixer group used for this sound.
        /// </summary>
        /// <returns>The <see cref="AudioMixerGroup"/> or null.</returns>
        public AudioMixerGroup? Mixer
        {
            get => ReflectionUtil.GetField<AudioMixerGroup>(Raw, "mixer");
            set => ReflectionUtil.SetField(Raw, "mixer", value);
        }
    }
}