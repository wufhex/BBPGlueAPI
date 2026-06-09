using UnityEngine;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Wraps the BB+ Principal NPC and exposes Principal-specific fields,
    /// sounds, detention behavior, and helper methods.
    /// </summary>
    public sealed class BBPPrincipal : BBPNpc
    {
        /// <summary>
        /// Creates a new Principal wrapper around a raw Principal instance.
        /// </summary>
        /// <param name="raw">The raw Principal object or component.</param>
        public BBPPrincipal(object? raw) : base(raw)
        {
        }

        /// <summary>
        /// Gets the audio manager used by Principal.
        /// </summary>
        public BBPAudioManager Audio =>
            new BBPAudioManager(ReflectionUtil.GetField<object>(Raw, "audMan"));

        /// <summary>
        /// Gets or sets Principal's normal sprite.
        /// </summary>
        public Sprite? NormalSprite
        {
            get => ReflectionUtil.GetField<Sprite>(Raw, "normalSprite");
            set => ReflectionUtil.SetField(Raw, "normalSprite", value);
        }

        /// <summary>
        /// Gets or sets Principal's chasing sprite.
        /// </summary>
        public Sprite? ChasingSprite
        {
            get => ReflectionUtil.GetField<Sprite>(Raw, "chasingSprite");
            set => ReflectionUtil.SetField(Raw, "chasingSprite", value);
        }

        /// <summary>
        /// Gets or sets the initial detention duration.
        /// </summary>
        public float DetentionInitialTime
        {
            get => ReflectionUtil.GetField<float>(Raw, "detentionInit");
            set => ReflectionUtil.SetField(Raw, "detentionInit", value);
        }

        /// <summary>
        /// Gets or sets how much detention time increases after each detention.
        /// </summary>
        public float DetentionIncrement
        {
            get => ReflectionUtil.GetField<float>(Raw, "detentionInc");
            set => ReflectionUtil.SetField(Raw, "detentionInc", value);
        }

        /// <summary>
        /// Gets or sets the current detention level.
        /// </summary>
        public int DetentionLevel
        {
            get => ReflectionUtil.GetField<int>(Raw, "detentionLevel");
            set => ReflectionUtil.SetField(Raw, "detentionLevel", value);
        }

        /// <summary>
        /// Gets or sets the noise value made when Principal sends the player to detention.
        /// </summary>
        public int DetentionNoise
        {
            get => ReflectionUtil.GetField<int>(Raw, "detentionNoise");
            set => ReflectionUtil.SetField(Raw, "detentionNoise", value);
        }

        /// <summary>
        /// Gets or sets whether Principal immediately knows where guilty players are.
        /// </summary>
        public bool AllKnowing
        {
            get => ReflectionUtil.GetField<bool>(Raw, "allKnowing");
            set => ReflectionUtil.SetField(Raw, "allKnowing", value);
        }

        /// <summary>
        /// Gets or sets Principal's whistle chance.
        /// </summary>
        public float WhistleChanceValue
        {
            get => ReflectionUtil.GetField<float>(Raw, "whistleChance");
            set => ReflectionUtil.SetField(Raw, "whistleChance", value);
        }

        /// <summary>
        /// Gets or sets Principal's whistle approach speed.
        /// </summary>
        public float WhistleSpeed
        {
            get => ReflectionUtil.GetField<float>(Raw, "whistleSpeed");
            set => ReflectionUtil.SetField(Raw, "whistleSpeed", value);
        }

        /// <summary>
        /// Gets or sets how long Principal pauses after knocking on a faculty door.
        /// </summary>
        public float KnockPauseTime
        {
            get => ReflectionUtil.GetField<float>(Raw, "knockPauseTime");
            set => ReflectionUtil.SetField(Raw, "knockPauseTime", value);
        }

        /// <summary>
        /// Gets or sets Principal's default movement speed.
        /// </summary>
        public float DefaultSpeed
        {
            get => ReflectionUtil.GetField<float>(Raw, "defaultSpeed");
            set => ReflectionUtil.SetField(Raw, "defaultSpeed", value);
        }

        /// <summary>
        /// Gets or sets the player currently targeted by Principal.
        /// </summary>
        public object? TargetedPlayer
        {
            get => ReflectionUtil.GetField<object>(Raw, "targetedPlayer");
            set => ReflectionUtil.SetField(Raw, "targetedPlayer", value);
        }

        /// <summary>
        /// Gets or sets Principal's detention time voice lines.
        /// </summary>
        public object? DetentionTimeSounds
        {
            get => ReflectionUtil.GetField<object>(Raw, "audTimes");
            set => ReflectionUtil.SetField(Raw, "audTimes", value);
        }

        /// <summary>
        /// Gets or sets Principal's detention scold voice lines.
        /// </summary>
        public object? ScoldSounds
        {
            get => ReflectionUtil.GetField<object>(Raw, "audScolds");
            set => ReflectionUtil.SetField(Raw, "audScolds", value);
        }

        /// <summary>
        /// Gets or sets Principal's "no running" sound.
        /// </summary>
        public BBPSoundObject NoRunningSound
        {
            get => new BBPSoundObject(ReflectionUtil.GetField<object>(Raw, "audNoRunning"));
            set => ReflectionUtil.SetField(Raw, "audNoRunning", value.Raw);
        }

        /// <summary>
        /// Gets or sets Principal's "no drinking" sound.
        /// </summary>
        public BBPSoundObject NoDrinkingSound
        {
            get => new BBPSoundObject(ReflectionUtil.GetField<object>(Raw, "audNoDrinking"));
            set => ReflectionUtil.SetField(Raw, "audNoDrinking", value.Raw);
        }

        /// <summary>
        /// Gets or sets Principal's "no eating" sound.
        /// </summary>
        public BBPSoundObject NoEatingSound
        {
            get => new BBPSoundObject(ReflectionUtil.GetField<object>(Raw, "audNoEating"));
            set => ReflectionUtil.SetField(Raw, "audNoEating", value.Raw);
        }

        /// <summary>
        /// Gets or sets Principal's "no faculty" sound.
        /// </summary>
        public BBPSoundObject NoFacultySound
        {
            get => new BBPSoundObject(ReflectionUtil.GetField<object>(Raw, "audNoFaculty"));
            set => ReflectionUtil.SetField(Raw, "audNoFaculty", value.Raw);
        }

        /// <summary>
        /// Gets or sets Principal's "no stabbing" sound.
        /// </summary>
        public BBPSoundObject NoStabbingSound
        {
            get => new BBPSoundObject(ReflectionUtil.GetField<object>(Raw, "audNoStabbing"));
            set => ReflectionUtil.SetField(Raw, "audNoStabbing", value.Raw);
        }

        /// <summary>
        /// Gets or sets Principal's "no bullying" sound.
        /// </summary>
        public BBPSoundObject NoBullyingSound
        {
            get => new BBPSoundObject(ReflectionUtil.GetField<object>(Raw, "audNoBullying"));
            set => ReflectionUtil.SetField(Raw, "audNoBullying", value.Raw);
        }

        /// <summary>
        /// Gets or sets Principal's "no escaping" sound.
        /// </summary>
        public BBPSoundObject NoEscapingSound
        {
            get => new BBPSoundObject(ReflectionUtil.GetField<object>(Raw, "audNoEscaping"));
            set => ReflectionUtil.SetField(Raw, "audNoEscaping", value.Raw);
        }

        /// <summary>
        /// Gets or sets Principal's "no lockers" sound.
        /// </summary>
        public BBPSoundObject NoLockersSound
        {
            get => new BBPSoundObject(ReflectionUtil.GetField<object>(Raw, "audNoLockers"));
            set => ReflectionUtil.SetField(Raw, "audNoLockers", value.Raw);
        }

        /// <summary>
        /// Gets or sets Principal's "no after hours" sound.
        /// </summary>
        public BBPSoundObject NoAfterHoursSound
        {
            get => new BBPSoundObject(ReflectionUtil.GetField<object>(Raw, "audNoAfterHours"));
            set => ReflectionUtil.SetField(Raw, "audNoAfterHours", value.Raw);
        }

        /// <summary>
        /// Gets or sets Principal's detention announcement sound.
        /// </summary>
        public BBPSoundObject DetentionSound
        {
            get => new BBPSoundObject(ReflectionUtil.GetField<object>(Raw, "audDetention"));
            set => ReflectionUtil.SetField(Raw, "audDetention", value.Raw);
        }

        /// <summary>
        /// Gets or sets Principal's whistle sound.
        /// </summary>
        public BBPSoundObject WhistleSound
        {
            get => new BBPSoundObject(ReflectionUtil.GetField<object>(Raw, "audWhistle"));
            set => ReflectionUtil.SetField(Raw, "audWhistle", value.Raw);
        }

        /// <summary>
        /// Gets or sets Principal's "coming" sound after reacting to a whistle target.
        /// </summary>
        public BBPSoundObject ComingSound
        {
            get => new BBPSoundObject(ReflectionUtil.GetField<object>(Raw, "audComing"));
            set => ReflectionUtil.SetField(Raw, "audComing", value.Raw);
        }

        /// <summary>
        /// Makes Principal scold the given rule.
        /// </summary>
        /// <param name="brokenRule">Rule name, such as Running, Faculty, Drinking, Escaping, Lockers, AfterHours, or Bullying.</param>
        public void Scold(string brokenRule) =>
            ReflectionUtil.Call(Raw, "Scold", brokenRule);

        /// <summary>
        /// Sends Principal's targeted player to detention.
        /// </summary>
        /// <param name="validCollision">
        /// True when Principal caught the player through a valid collision;
        /// false when detention is triggered as an escaping penalty.
        /// </param>
        public void SendToDetention(bool validCollision) =>
            ReflectionUtil.Call(Raw, "SendToDetention", validCollision);

        /// <summary>
        /// Resets Principal's sight timer for the given player.
        /// </summary>
        /// <param name="player">Player reference.</param>
        public void LoseTrackOfPlayer(BBPPlayerRef player)
        {
            if (player.Raw != null)
                ReflectionUtil.Call(Raw, "LoseTrackOfPlayer", player.Raw);
        }

        /// <summary>
        /// Makes Principal observe a player and possibly begin chasing if the player is guilty.
        /// </summary>
        /// <param name="player">Player reference.</param>
        public void ObservePlayer(BBPPlayerRef player)
        {
            if (player.Raw != null)
                ReflectionUtil.Call(Raw, "ObservePlayer", player.Raw);
        }

        /// <summary>
        /// Switches Principal to his chasing sprite.
        /// </summary>
        public void SwitchToChaseSprite() =>
            ReflectionUtil.Call(Raw, "SwitchToChaseSprite");

        /// <summary>
        /// Switches Principal to his normal sprite.
        /// </summary>
        public void SwitchToNormalSprite() =>
            ReflectionUtil.Call(Raw, "SwitchToNormalSprite");

        /// <summary>
        /// Runs Principal's random whistle chance behavior.
        /// </summary>
        public void WhistleChance() =>
            ReflectionUtil.Call(Raw, "WhistleChance");

        /// <summary>
        /// Restores Principal's speed after reaching a whistle target.
        /// </summary>
        public void WhistleReached() =>
            ReflectionUtil.Call(Raw, "WhistleReached");

        /// <summary>
        /// Makes Principal react to a whistle target position.
        /// </summary>
        /// <param name="target">Target position.</param>
        public void WhistleReact(Vector3 target) =>
            ReflectionUtil.Call(Raw, "WhistleReact", target);
    }
}