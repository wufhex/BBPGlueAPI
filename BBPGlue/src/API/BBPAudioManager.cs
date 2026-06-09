using UnityEngine;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Provides a wrapper around the game's audio manager exposing playback and queue controls.
    /// </summary>
    public sealed class BBPAudioManager
    {
        /// <summary>
        /// The raw underlying audio manager object wrapped by this API class, or null if none.
        /// </summary>
        public object? Raw { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BBPAudioManager"/> class wrapping a raw audio manager.
        /// </summary>
        /// <param name="raw">The raw underlying object instance or null.</param>
        public BBPAudioManager(object? raw)
        {
            Raw = raw;
        }

        /// <summary>
        /// Determines whether the underlying raw object exists.
        /// </summary>
        public bool Exists => Raw != null;

        /// <summary>
        /// Gets the underlying AudioSource used by the manager.
        /// </summary>
        /// <returns>The <see cref="AudioSource"/> or null.</returns>
        public AudioSource? AudioSource =>
            ReflectionUtil.GetField<AudioSource>(Raw, "audioDevice");

        /// <summary>
        /// Gets the audio source manager object associated with this manager.
        /// </summary>
        /// <returns>The audio source manager object or null.</returns>
        public object? AudioSourceManager =>
            ReflectionUtil.GetField<object>(Raw, "audioSourceManager");

        /// <summary>
        /// Gets whether any audio is currently playing through this manager.
        /// </summary>
        /// <returns>True if audio is playing; otherwise false.</returns>
        public bool AnyAudioIsPlaying =>
            ReflectionUtil.GetProperty<bool>(Raw, "AnyAudioIsPlaying");

        /// <summary>
        /// Gets whether queued audio is currently playing.
        /// </summary>
        /// <returns>True if queued audio is playing; otherwise false.</returns>
        public bool QueuedAudioIsPlaying =>
            ReflectionUtil.GetProperty<bool>(Raw, "QueuedAudioIsPlaying");

        /// <summary>
        /// Gets whether audio is currently queued up for playback.
        /// </summary>
        /// <returns>True if queued; otherwise false.</returns>
        public bool QueuedUp =>
            ReflectionUtil.GetProperty<bool>(Raw, "QueuedUp");

        /// <summary>
        /// Gets the number of audio files currently queued.
        /// </summary>
        /// <returns>The count of files queued.</returns>
        public int FilesQueued =>
            ReflectionUtil.GetField<int>(Raw, "filesQueued");

        /// <summary>
        /// Gets the identifier of the audio source used by this manager.
        /// </summary>
        /// <returns>The source identifier as an integer.</returns>
        public int SourceId =>
            ReflectionUtil.GetField<int>(Raw, "sourceId");

        /// <summary>
        /// Gets or sets the volume multiplier applied to audio playback.
        /// </summary>
        /// <returns>The volume modifier as a float.</returns>
        public float VolumeModifier
        {
            get => ReflectionUtil.GetField<float>(Raw, "volumeModifier");
            set => ReflectionUtil.SetField(Raw, "volumeModifier", value);
        }

        /// <summary>
        /// Gets or sets the pitch multiplier applied to audio playback.
        /// </summary>
        /// <returns>The pitch modifier as a float.</returns>
        public float PitchModifier
        {
            get => ReflectionUtil.GetField<float>(Raw, "pitchModifier");
            set => ReflectionUtil.SetField(Raw, "pitchModifier", value);
        }

        /// <summary>
        /// Gets or sets whether pitch is applied without time scaling.
        /// </summary>
        /// <returns>True if unscaled pitch is used; otherwise false.</returns>
        public bool UseUnscaledPitch
        {
            get => ReflectionUtil.GetField<bool>(Raw, "useUnscaledPitch");
            set => ReflectionUtil.SetField(Raw, "useUnscaledPitch", value);
        }

        /// <summary>
        /// Gets or sets whether the audio manager should loop playback.
        /// </summary>
        /// <returns>True if looping; otherwise false.</returns>
        public bool Loop
        {
            get => ReflectionUtil.GetField<bool>(Raw, "loop");
            set => ReflectionUtil.Call(Raw, "SetLoop", value);
        }

        /// <summary>
        /// Gets or sets whether loop state should be maintained across queued items.
        /// </summary>
        /// <returns>True if maintain loop is enabled; otherwise false.</returns>
        public bool MaintainLoop
        {
            get => ReflectionUtil.GetField<bool>(Raw, "maintainLoop");
            set => ReflectionUtil.SetField(Raw, "maintainLoop", value);
        }

        /// <summary>
        /// Gets or sets whether this manager ignores the global listener pause state.
        /// </summary>
        /// <returns>True if listener pause is ignored; otherwise false.</returns>
        public bool IgnoreListenerPause
        {
            get => ReflectionUtil.GetField<bool>(Raw, "ignoreListenerPause");
            set => ReflectionUtil.SetField(Raw, "ignoreListenerPause", value);
        }

        /// <summary>
        /// Gets or sets whether audio is positional in world space.
        /// </summary>
        /// <returns>True if positional audio is enabled; otherwise false.</returns>
        public bool Positional
        {
            get => ReflectionUtil.GetField<bool>(Raw, "positional");
            set => ReflectionUtil.SetField(Raw, "positional", value);
        }

        /// <summary>
        /// Gets or sets the position used for caption display related to audio.
        /// </summary>
        /// <returns>The caption position as a <see cref="Vector3"/>.</returns>
        public Vector3 CaptionPosition
        {
            get => ReflectionUtil.GetField<Vector3>(Raw, "captionPosition");
            set => ReflectionUtil.SetField(Raw, "captionPosition", value);
        }

        /// <summary>
        /// Gets or sets the anchor position for captions related to audio.
        /// </summary>
        /// <returns>The caption anchor as a <see cref="Vector2"/>.</returns>
        public Vector2 CaptionAnchor
        {
            get => ReflectionUtil.GetField<Vector2>(Raw, "captionAnchor");
            set => ReflectionUtil.SetField(Raw, "captionAnchor", value);
        }

        /// <summary>
        /// Gets whether the underlying manager is a propagated audio manager type.
        /// </summary>
        /// <returns>True if the underlying type name equals "PropagatedAudioManager"; otherwise false.</returns>
        public bool IsPropagated =>
            Raw != null && Raw.GetType().Name == "PropagatedAudioManager";

        /// <summary>
        /// Gets the current source position used for positional audio.
        /// </summary>
        /// <returns>The source position as a <see cref="Vector3"/>.</returns>
        public Vector3 SourcePosition =>
            ReflectionUtil.GetProperty<Vector3>(Raw, "SourcePosition");

        /// <summary>
        /// Gets the propagation source object if this manager propagates audio.
        /// </summary>
        /// <returns>The propagation source object or null.</returns>
        public object? PropagationSource =>
            ReflectionUtil.GetProperty<object>(Raw, "propagationSource");

        /// <summary>
        /// Queues a sound for playback, optionally playing it immediately.
        /// </summary>
        /// <param name="sound">The sound to queue.</param>
        /// <param name="playImmediately">Whether the sound should play immediately.</param>
        public void Queue(BBPSoundObject sound, bool playImmediately = false)
        {
            if (sound.Raw != null)
                ReflectionUtil.Call(Raw, "QueueAudio", sound.Raw, playImmediately);
        }

        /// <summary>
        /// Plays a single instance of the specified sound immediately.
        /// </summary>
        /// <param name="sound">The sound to play.</param>
        public void PlaySingle(BBPSoundObject sound)
        {
            if (sound.Raw != null)
                ReflectionUtil.Call(Raw, "PlaySingle", sound.Raw);
        }

        /// <summary>
        /// Plays a single instance of the specified sound with a volume scale.
        /// </summary>
        /// <param name="sound">The sound to play.</param>
        /// <param name="volumeScale">Volume scale to apply to the sound.</param>
        public void PlaySingle(BBPSoundObject sound, float volumeScale)
        {
            if (sound.Raw != null)
                ReflectionUtil.Call(Raw, "PlaySingle", sound.Raw, volumeScale);
        }

        /// <summary>
        /// Plays a random element from the provided sound array immediately.
        /// </summary>
        /// <param name="soundArray">An array or collection of sound objects.</param>
        public void PlayRandom(object soundArray)
        {
            ReflectionUtil.Call(Raw, "PlayRandomAudio", soundArray);
        }

        /// <summary>
        /// Queues a random element from the provided sound array for playback.
        /// </summary>
        /// <param name="soundArray">An array or collection of sound objects.</param>
        public void QueueRandom(object soundArray)
        {
            ReflectionUtil.Call(Raw, "QueueRandomAudio", soundArray);
        }

        /// <summary>
        /// Flushes the audio queue, optionally ending the current playback.
        /// </summary>
        /// <param name="endCurrent">If true, end the current playback immediately.</param>
        public void Flush(bool endCurrent = true)
        {
            ReflectionUtil.Call(Raw, "FlushQueue", endCurrent);
        }

        /// <summary>
        /// Fades out audio over the specified duration.
        /// </summary>
        /// <param name="time">Fade out duration in seconds.</param>
        public void FadeOut(float time)
        {
            ReflectionUtil.Call(Raw, "FadeOut", time);
        }

        /// <summary>
        /// Pauses or resumes audio playback.
        /// </summary>
        /// <param name="pause">True to pause; false to resume.</param>
        public void Pause(bool pause)
        {
            ReflectionUtil.Call(Raw, "Pause", pause);
        }

        /// <summary>
        /// Sets the loop state on the underlying audio manager.
        /// </summary>
        /// <param name="value">True to enable looping; false to disable.</param>
        public void SetLoop(bool value)
        {
            ReflectionUtil.Call(Raw, "SetLoop", value);
        }

        /// <summary>
        /// Overrides the audio source position with the specified world position.
        /// </summary>
        /// <param name="position">The position to override the source with.</param>
        public void OverrideSourcePosition(Vector3 position)
        {
            ReflectionUtil.Call(Raw, "OverrideSourcePosition", position);
        }

        /// <summary>
        /// Releases any previously set source position override.
        /// </summary>
        public void ReleaseSourcePositionOverride()
        {
            ReflectionUtil.Call(Raw, "ReleasePositionOverride");
        }

        /// <summary>
        /// Disables volumetric audio processing for this manager.
        /// </summary>
        public void DisableVolumetricAudio()
        {
            ReflectionUtil.Call(Raw, "DisableVolumetricAudio");
        }

        /// <summary>
        /// Sets volumetric audio positions for the manager.
        /// </summary>
        /// <param name="positions">An object representing positions used for volumetric audio.</param>
        public void SetVolumetricAudio(object positions)
        {
            ReflectionUtil.Call(Raw, "SetVolumetricAudio", positions);
        }
    }
}