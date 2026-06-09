using UnityEngine;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Wraps the BB+ Arts And Crafters NPC and exposes anger,
    /// hiding, attacking, teleporting, and audio helpers.
    /// </summary>
    public sealed class BBPArtsAndCrafters : BBPNpc
    {
        /// <summary>
        /// Creates an Arts And Crafters wrapper around a raw instance.
        /// </summary>
        /// <param name="raw">The raw ArtsAndCrafters object or component.</param>
        public BBPArtsAndCrafters(object? raw) : base(raw)
        {
        }

        /// <summary>
        /// Gets Arts And Crafters' primary audio manager.
        /// </summary>
        public BBPAudioManager Audio =>
            new BBPAudioManager(ReflectionUtil.GetField<object>(Raw, "audMan"));

        /// <summary>
        /// Gets Arts And Crafters' attack audio manager.
        /// </summary>
        public BBPAudioManager AttackAudio =>
            new BBPAudioManager(ReflectionUtil.GetField<object>(Raw, "attackAudMan"));

        /// <summary>
        /// Gets or sets Arts And Crafters' visible sprite renderer.
        /// </summary>
        public SpriteRenderer? VisibleRenderer
        {
            get => ReflectionUtil.GetField<SpriteRenderer>(Raw, "visibleRenderer");
            set => ReflectionUtil.SetField(Raw, "visibleRenderer", value);
        }

        /// <summary>
        /// Gets or sets Arts And Crafters' angry sprite.
        /// </summary>
        public Sprite? AngrySprite
        {
            get => ReflectionUtil.GetField<Sprite>(Raw, "angrySprite");
            set => ReflectionUtil.SetField(Raw, "angrySprite", value);
        }

        /// <summary>
        /// Gets or sets the minimum time the player must look at Arts And Crafters.
        /// </summary>
        public float MinSightTime
        {
            get => ReflectionUtil.GetField<float>(Raw, "minSightTime");
            set => ReflectionUtil.SetField(Raw, "minSightTime", value);
        }

        /// <summary>
        /// Gets or sets the maximum time the player must look at Arts And Crafters.
        /// </summary>
        public float MaxSightTime
        {
            get => ReflectionUtil.GetField<float>(Raw, "maxSightTime");
            set => ReflectionUtil.SetField(Raw, "maxSightTime", value);
        }

        /// <summary>
        /// Gets Arts And Crafters' anger time.
        /// </summary>
        public float AngryTime =>
            ReflectionUtil.GetProperty<float>(Raw, "AngryTime");

        /// <summary>
        /// Gets Arts And Crafters' attack duration.
        /// </summary>
        public float AttackTime =>
            ReflectionUtil.GetProperty<float>(Raw, "AttackTime");

        /// <summary>
        /// Gets Arts And Crafters' attack spin speed.
        /// </summary>
        public float AttackSpinSpeed =>
            ReflectionUtil.GetProperty<float>(Raw, "AttackSpinSpeed");

        /// <summary>
        /// Gets Arts And Crafters' attack spin acceleration.
        /// </summary>
        public float AttackSpinAccel =>
            ReflectionUtil.GetProperty<float>(Raw, "AttackSpinAccel");

        /// <summary>
        /// Gets whether Arts And Crafters is jealous.
        /// </summary>
        public bool Jealous =>
            ReflectionUtil.GetProperty<bool>(Raw, "Jealous");

        /// <summary>
        /// Gets whether Arts And Crafters is angry.
        /// </summary>
        public bool Angry =>
            ReflectionUtil.GetField<bool>(Raw, "angry");

        /// <summary>
        /// Gets whether Arts And Crafters is attacking.
        /// </summary>
        public bool Attacking =>
            ReflectionUtil.GetField<bool>(Raw, "attacking");

        /// <summary>
        /// Gets whether Arts And Crafters is running.
        /// </summary>
        public bool Running =>
            ReflectionUtil.GetField<bool>(Raw, "running");

        /// <summary>
        /// Gets whether Arts And Crafters is hidden.
        /// </summary>
        public bool Hidden =>
            ReflectionUtil.GetField<bool>(Raw, "hidden");

        /// <summary>
        /// Starts Arts And Crafters' attack behavior against a player.
        /// </summary>
        /// <param name="player">The target player.</param>
        public void Attack(BBPPlayerRef player)
        {
            if (player.Raw != null)
                ReflectionUtil.Call(Raw, "Attack", player.Raw);
        }

        /// <summary>
        /// Makes Arts And Crafters disappear permanently.
        /// </summary>
        public void DisappearForever() =>
            ReflectionUtil.Call(Raw, "DisappearForever");

        /// <summary>
        /// Makes Arts And Crafters angry at the given player.
        /// </summary>
        /// <param name="player">The target player.</param>
        public void GetAngry(BBPPlayerRef player)
        {
            if (player.Raw != null)
                ReflectionUtil.Call(Raw, "GetAngry", player.Raw);
        }

        /// <summary>
        /// Hides or reveals Arts And Crafters.
        /// </summary>
        /// <param name="hidden">True to hide; false to show.</param>
        public void Hide(bool hidden) =>
            ReflectionUtil.Call(Raw, "Hide", hidden);

        /// <summary>
        /// Makes Arts And Crafters run away.
        /// </summary>
        public void RunAway() =>
            ReflectionUtil.Call(Raw, "RunAway");

        /// <summary>
        /// Spawns Arts And Crafters at a raw IntVector2 grid position.
        /// </summary>
        /// <param name="position">Raw BB+ IntVector2 position.</param>
        public void SpawnAt(object position) =>
            ReflectionUtil.Call(Raw, "SpawnAt", position);

        /// <summary>
        /// Teleports Arts And Crafters to a raw IntVector2 grid position.
        /// </summary>
        /// <param name="position">Raw BB+ IntVector2 position.</param>
        public void TeleportGrid(object position) =>
            ReflectionUtil.Call(Raw, "Teleport", position);

        /// <summary>
        /// Triggers Arts And Crafters' player teleport behavior.
        /// </summary>
        /// <param name="player">The player to teleport.</param>
        public void TeleportPlayer(BBPPlayerRef player)
        {
            if (player.Raw != null)
                ReflectionUtil.Call(Raw, "TeleportPlayer", player.Raw);
        }

        /// <summary>
        /// Updates Arts And Crafters' echo visuals.
        /// </summary>
        public void UpdateEcho() =>
            ReflectionUtil.Call(Raw, "UpdateEcho");
    }
}
