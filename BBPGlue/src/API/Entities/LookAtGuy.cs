using UnityEngine;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Wraps the BB+ Look At Guy NPC and exposes freezing,
    /// blinding, fog, respawn, and visual helpers.
    /// </summary>
    public sealed class BBPLookAtGuy : BBPNpc
    {
        /// <summary>
        /// Creates a Look At Guy wrapper around a raw LookAtGuy instance.
        /// </summary>
        /// <param name="raw">The raw LookAtGuy object or component.</param>
        public BBPLookAtGuy(object? raw) : base(raw)
        {
        }

        /// <summary>
        /// Gets Look At Guy's primary audio manager.
        /// </summary>
        public BBPAudioManager Audio =>
            new BBPAudioManager(ReflectionUtil.GetField<object>(Raw, "audMan"));

        /// <summary>
        /// Gets Look At Guy's blindness audio manager.
        /// </summary>
        public BBPAudioManager BlindAudio =>
            new BBPAudioManager(ReflectionUtil.GetField<object>(Raw, "blindAudMan"));

        /// <summary>
        /// Gets Look At Guy's rumble audio manager.
        /// </summary>
        public BBPAudioManager RumbleAudio =>
            new BBPAudioManager(ReflectionUtil.GetField<object>(Raw, "rumbleAudMan"));

        /// <summary>
        /// Gets or sets Look At Guy's visible sprite renderer.
        /// </summary>
        public SpriteRenderer? Sprite
        {
            get => ReflectionUtil.GetField<SpriteRenderer>(Raw, "sprite");
            set => ReflectionUtil.SetField(Raw, "sprite", value);
        }

        /// <summary>
        /// Gets or sets Look At Guy's crumbled sprite.
        /// </summary>
        public Sprite? CrumbledSprite
        {
            get => ReflectionUtil.GetField<Sprite>(Raw, "crumbledSprite");
            set => ReflectionUtil.SetField(Raw, "crumbledSprite", value);
        }

        /// <summary>
        /// Gets or sets the fog object used by Look At Guy.
        /// </summary>
        public object? Fog
        {
            get => ReflectionUtil.GetField<object>(Raw, "fog");
            set => ReflectionUtil.SetField(Raw, "fog", value);
        }

        /// <summary>
        /// Gets or sets Look At Guy's flee speed.
        /// </summary>
        public float FleeSpeed
        {
            get => ReflectionUtil.GetField<float>(Raw, "fleeSpeed");
            set => ReflectionUtil.SetField(Raw, "fleeSpeed", value);
        }

        /// <summary>
        /// Gets or sets the blindness/fog duration.
        /// </summary>
        public float FogTime
        {
            get => ReflectionUtil.GetField<float>(Raw, "fogTime");
            set => ReflectionUtil.SetField(Raw, "fogTime", value);
        }

        /// <summary>
        /// Gets or sets how fast the time scale changes while frozen.
        /// </summary>
        public float TimeScaleChangeSpeed
        {
            get => ReflectionUtil.GetField<float>(Raw, "timeScaleChangeSpeed");
            set => ReflectionUtil.SetField(Raw, "timeScaleChangeSpeed", value);
        }

        /// <summary>
        /// Gets or sets the maximum time the player can look before triggering behavior.
        /// </summary>
        public float MaxLookTime
        {
            get => ReflectionUtil.GetField<float>(Raw, "maxLookTime");
            set => ReflectionUtil.SetField(Raw, "maxLookTime", value);
        }

        /// <summary>
        /// Gets or sets Look At Guy's explosion chance.
        /// </summary>
        public float ExplodeChanceValue
        {
            get => ReflectionUtil.GetField<float>(Raw, "explodeChance");
            set => ReflectionUtil.SetField(Raw, "explodeChance", value);
        }

        /// <summary>
        /// Gets or sets the slow speed applied while freezing NPCs.
        /// </summary>
        public float SlowSpeed
        {
            get => ReflectionUtil.GetField<float>(Raw, "slowSpeed");
            set => ReflectionUtil.SetField(Raw, "slowSpeed", value);
        }

        /// <summary>
        /// Gets whether Look At Guy is currently freezing NPCs.
        /// </summary>
        public bool Freezing =>
            ReflectionUtil.GetField<bool>(Raw, "freezing");

        /// <summary>
        /// Gets whether Look At Guy is currently fleeing.
        /// </summary>
        public bool Fleeing =>
            ReflectionUtil.GetField<bool>(Raw, "fleeing");

        /// <summary>
        /// Activates Look At Guy.
        /// </summary>
        public void Activate() =>
            ReflectionUtil.Call(Raw, "Activate");

        /// <summary>
        /// Triggers Look At Guy's blindness behavior.
        /// </summary>
        public void Blind() =>
            ReflectionUtil.Call(Raw, "Blind");

        /// <summary>
        /// Rolls Look At Guy's explosion chance.
        /// </summary>
        /// <returns>True if the explosion chance succeeds.</returns>
        public bool ExplodeChance() =>
            ReflectionUtil.Call<bool>(Raw, "ExplodeChance");

        /// <summary>
        /// Makes Look At Guy flee from the given player.
        /// </summary>
        /// <param name="player">The player to flee from.</param>
        public void FleePlayer(BBPPlayerRef player)
        {
            if (player.Raw != null)
                ReflectionUtil.Call(Raw, "FleePlayer", player.Raw);
        }

        /// <summary>
        /// Freezes or unfreezes NPCs through Look At Guy's effect.
        /// </summary>
        /// <param name="freeze">True to freeze; false to unfreeze.</param>
        public void FreezeNPCs(bool freeze) =>
            ReflectionUtil.Call(Raw, "FreezeNPCs", freeze);

        /// <summary>
        /// Makes Look At Guy reappear.
        /// </summary>
        public void Reappear() =>
            ReflectionUtil.Call(Raw, "Reappear");

        /// <summary>
        /// Respawns Look At Guy after his blindness/fog behavior.
        /// </summary>
        public void Respawn() =>
            ReflectionUtil.Call(Raw, "Respawn");

        /// <summary>
        /// Sets Look At Guy's target time scale.
        /// </summary>
        /// <param name="target">The target time scale.</param>
        public void SetTargetTimeScale(float target) =>
            ReflectionUtil.Call(Raw, "SetTargetTimeScale", target);

        /// <summary>
        /// Updates Look At Guy's head position based on pressure.
        /// </summary>
        /// <param name="pressure">Look pressure from 0 to 1.</param>
        public void UpdateHeadPosition(float pressure) =>
            ReflectionUtil.Call(Raw, "UpdateHeadPosition", pressure);
    }
}
