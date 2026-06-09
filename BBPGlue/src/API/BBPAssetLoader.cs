using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Utility for loading and registering assets like sprites, audio clips, and sound objects.
    /// </summary>
    public sealed class BBPAssetLoader
    {
        private readonly Dictionary<string, Sprite>         _sprites    = new Dictionary<string, Sprite>();
        private readonly Dictionary<string, AudioClip>      _audioClips = new Dictionary<string, AudioClip>();
        private readonly Dictionary<string, BBPSoundObject> _sounds     = new Dictionary<string, BBPSoundObject>();

        /// <summary>
        /// Gets a read-only view of registered sprites.
        /// </summary>
        public IReadOnlyDictionary<string, Sprite> Sprites => _sprites;
        /// <summary>
        /// Gets a read-only view of registered audio clips.
        /// </summary>
        public IReadOnlyDictionary<string, AudioClip> AudioClips => _audioClips;
        /// <summary>
        /// Gets a read-only view of registered sound objects.
        /// </summary>
        public IReadOnlyDictionary<string, BBPSoundObject> Sounds => _sounds;

        /// <summary>
        /// Registers a sprite with the given identifier.
        /// </summary>
        /// <param name="id">The identifier for the sprite.</param>
        /// <param name="sprite">The sprite to register.</param>
        public void RegisterSprite(string id, Sprite sprite)
        {
            if (!string.IsNullOrWhiteSpace(id) && sprite != null)
                _sprites[id] = sprite;
        }

        /// <summary>
        /// Retrieves a registered sprite by identifier.
        /// </summary>
        /// <param name="id">The identifier of the sprite.</param>
        /// <returns>The <see cref="Sprite"/> if found; otherwise null.</returns>
        public Sprite? GetSprite(string id)
        {
            return _sprites.TryGetValue(id, out Sprite sprite) ? sprite : null;
        }

        /// <summary>
        /// Loads a sprite from disk and registers it under the given identifier.
        /// </summary>
        /// <param name="id">The identifier to register the loaded sprite under.</param>
        /// <param name="path">The filesystem path to the image file.</param>
        /// <param name="pixelsPerUnit">Pixels per unit for the created sprite.</param>
        /// <returns>The loaded <see cref="Sprite"/> if successful; otherwise null.</returns>
        public Sprite? LoadSprite(string id, string path, float pixelsPerUnit = 100f)
        {
            Sprite? sprite = LoadSprite(path, pixelsPerUnit);

            if (sprite != null)
                RegisterSprite(id, sprite);

            return sprite;
        }

        /// <summary>
        /// Loads a sprite from a file path.
        /// </summary>
        /// <param name="path">The filesystem path to the image file.</param>
        /// <param name="pixelsPerUnit">Pixels per unit used when creating the sprite.</param>
        /// <returns>The created <see cref="Sprite"/> if loading succeeded; otherwise null.</returns>
        public Sprite? LoadSprite(string path, float pixelsPerUnit = 100f)
        {
            if (!File.Exists(path))
                return null;

            byte[] data = File.ReadAllBytes(path);

            Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            texture.name = Path.GetFileNameWithoutExtension(path);

            if (!texture.LoadImage(data))
                return null;

            texture.filterMode = FilterMode.Point;

            return Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f),
                pixelsPerUnit
            );
        }

        /// <summary>
        /// Registers an audio clip with the given identifier.
        /// </summary>
        /// <param name="id">The identifier for the audio clip.</param>
        /// <param name="clip">The audio clip to register.</param>
        public void RegisterAudioClip(string id, AudioClip clip)
        {
            if (!string.IsNullOrWhiteSpace(id) && clip != null)
                _audioClips[id] = clip;
        }

        /// <summary>
        /// Retrieves a registered audio clip by identifier.
        /// </summary>
        /// <param name="id">The identifier of the audio clip.</param>
        /// <returns>The <see cref="AudioClip"/> if found; otherwise null.</returns>
        public AudioClip? GetAudioClip(string id)
        {
            return _audioClips.TryGetValue(id, out AudioClip clip) ? clip : null;
        }

        /// <summary>
        /// Asynchronously loads an audio clip from the given path and registers it under the provided id.
        /// </summary>
        /// <param name="id">The identifier to register the loaded clip under.</param>
        /// <param name="path">The filesystem path to the audio file.</param>
        /// <param name="audioType">The audio type to use when loading.</param>
        /// <param name="callback">Callback invoked with the loaded <see cref="AudioClip"/>, or null on failure.</param>
        public IEnumerator LoadAudioClip(
            string id,
            string path,
            AudioType audioType,
            Action<AudioClip?> callback
        )
        {
            yield return LoadAudioClip(path, audioType, clip =>
            {
                if (clip != null)
                    RegisterAudioClip(id, clip);

                callback(clip);
            });
        }

        /// <summary>
        /// Asynchronously loads an audio clip from a file path.
        /// </summary>
        /// <param name="path">The filesystem path to the audio file.</param>
        /// <param name="audioType">The audio type to use when loading.</param>
        /// <param name="callback">Callback invoked with the loaded <see cref="AudioClip"/>, or null on failure.</param>
        public IEnumerator LoadAudioClip(
            string path,
            AudioType audioType,
            Action<AudioClip?> callback
        )
        {
            if (!File.Exists(path))
            {
                callback(null);
                yield break;
            }

            string uri = "file://" + Path.GetFullPath(path);

            using UnityWebRequest request =
                UnityWebRequestMultimedia.GetAudioClip(uri, audioType);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                callback(null);
                yield break;
            }

            AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
            clip.name = Path.GetFileNameWithoutExtension(path);

            callback(clip);
        }

        /// <summary>
        /// Registers a sound object with the given identifier.
        /// </summary>
        /// <param name="id">The identifier for the sound.</param>
        /// <param name="sound">The sound object to register.</param>
        public void RegisterSound(string id, BBPSoundObject sound)
        {
            if (!string.IsNullOrWhiteSpace(id) && sound.Exists)
                _sounds[id] = sound;
        }

        /// <summary>
        /// Retrieves a registered sound object by identifier.
        /// </summary>
        /// <param name="id">The identifier of the sound.</param>
        /// <returns>The <see cref="BBPSoundObject"/> if found; otherwise null.</returns>
        public BBPSoundObject? GetSound(string id)
        {
            return _sounds.TryGetValue(id, out BBPSoundObject sound) ? sound : null;
        }

        /// <summary>
        /// Creates a sound object from an AudioClip and registers it under the given identifier.
        /// </summary>
        /// <param name="id">The identifier to register the created sound under.</param>
        /// <param name="clip">The audio clip to use for the sound.</param>
        /// <param name="key">An optional key identifying the sound.</param>
        /// <param name="soundType">The sound type as string (e.g., "Effect").</param>
        /// <param name="volume">Volume multiplier for the sound.</param>
        /// <param name="subtitle">Whether the sound should show subtitles.</param>
        /// <returns>The created <see cref="BBPSoundObject"/> if successful; otherwise null.</returns>
        public BBPSoundObject? CreateSound(
            string id,
            AudioClip clip,
            string key = "",
            string soundType = "Effect",
            float volume = 1f,
            bool subtitle = false
        )
        {
            object? raw = CreateSoundObjectRaw(
                clip,
                key,
                soundType,
                volume,
                subtitle
            );

            if (raw == null)
                return null;

            BBPSoundObject sound = new BBPSoundObject(raw);
            RegisterSound(id, sound);
            return sound;
        }

        /// <summary>
        /// Creates a raw sound object from the given AudioClip and parameters.
        /// </summary>
        /// <param name="clip">The audio clip to use.</param>
        /// <param name="key">An optional key identifying the sound.</param>
        /// <param name="soundType">The sound type as a string.</param>
        /// <param name="volume">Volume multiplier for the sound.</param>
        /// <param name="subtitle">Whether the sound should display subtitles.</param>
        /// <returns>The raw sound object if creation succeeded; otherwise null.</returns>
        public object? CreateSoundObject(
            AudioClip clip,
            string key = "",
            string soundType = "Effect",
            float volume = 1f,
            bool subtitle = false
        )
        {
            return CreateSoundObjectRaw(
                clip,
                key,
                soundType,
                volume,
                subtitle
            );
        }

        private object? CreateSoundObjectRaw(
            AudioClip clip,
            string key,
            string soundType,
            float volume,
            bool subtitle
        )
        {
            Type? soundObjectType = Type.GetType("SoundObject, Assembly-CSharp");

            if (soundObjectType == null)
                return null;

            object soundObject = ScriptableObject.CreateInstance(soundObjectType);

            ReflectionUtil.SetField(soundObject, "soundClip", clip);
            ReflectionUtil.SetField(soundObject, "soundKey", key);
            ReflectionUtil.SetField(soundObject, "volumeMultiplier", volume);
            ReflectionUtil.SetField(soundObject, "subtitle", subtitle);

            object? enumValue = ReflectionUtil.GetEnumValue("SoundType", soundType);

            if (enumValue != null)
                ReflectionUtil.SetField(soundObject, "soundType", enumValue);

            return soundObject;
        }

        /// <summary>
        /// Removes a registered sprite by identifier.
        /// </summary>
        /// <param name="id">The identifier of the sprite to remove.</param>
        /// <returns>True if the sprite was removed; otherwise false.</returns>
        public bool RemoveSprite(string id) => _sprites.Remove(id);
        
        /// <summary>
        /// Removes a registered audio clip by identifier.
        /// </summary>
        /// <param name="id">The identifier of the audio clip to remove.</param>
        /// <returns>True if the audio clip was removed; otherwise false.</returns>
        public bool RemoveAudioClip(string id) => _audioClips.Remove(id);
        
        /// <summary>
        /// Removes a registered sound by identifier.
        /// </summary>
        /// <param name="id">The identifier of the sound to remove.</param>
        /// <returns>True if the sound was removed; otherwise false.</returns>
        public bool RemoveSound(string id) => _sounds.Remove(id);

        /// <summary>
        /// Clears all registered assets from the loader.
        /// </summary>
        public void Clear()
        {
            _sprites.Clear();
            _audioClips.Clear();
            _sounds.Clear();
        }
    }
}