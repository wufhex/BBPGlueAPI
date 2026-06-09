using UnityEngine;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Wraps the BB+ Dr. Reflex NPC and exposes reflex test,
    /// hammer, animation, audio, and movement helpers.
    /// </summary>
    public sealed class BBPDrReflex : BBPNpc
    {
        /// <summary>
        /// Creates a Dr. Reflex wrapper around a raw DrReflex instance.
        /// </summary>
        /// <param name="raw">The raw DrReflex object or component.</param>
        public BBPDrReflex(object? raw) : base(raw)
        {
        }

        /// <summary>
        /// Gets Dr. Reflex's animator.
        /// </summary>
        public Animator? Animator =>
            ReflectionUtil.GetProperty<Animator>(Raw, "Animator");

        /// <summary>
        /// Gets Dr. Reflex's audio manager.
        /// </summary>
        public BBPAudioManager Audio =>
            new BBPAudioManager(ReflectionUtil.GetProperty<object>(Raw, "AudioManager"));

        /// <summary>
        /// Gets whether Dr. Reflex is currently in his office.
        /// </summary>
        public bool IsInOffice =>
            ReflectionUtil.GetProperty<bool>(Raw, "IsInOffice");

        /// <summary>
        /// Gets Dr. Reflex's last sighted player location.
        /// </summary>
        public Vector3 LastSightedPlayerLocation =>
            ReflectionUtil.GetProperty<Vector3>(Raw, "LastSightedPlayerLocation");

        /// <summary>
        /// Gets Dr. Reflex's room wander cycle duration.
        /// </summary>
        public float RoomWanderCycle =>
            ReflectionUtil.GetProperty<float>(Raw, "RoomWanderCycle");

        /// <summary>
        /// Gets whether Dr. Reflex is currently turning.
        /// </summary>
        public bool Turning =>
            ReflectionUtil.GetProperty<bool>(Raw, "Turning");

        /// <summary>
        /// Gets or sets Dr. Reflex's wander speed.
        /// </summary>
        public float WanderSpeed
        {
            get => ReflectionUtil.GetField<float>(Raw, "wanderSpeed");
            set => ReflectionUtil.SetField(Raw, "wanderSpeed", value);
        }

        /// <summary>
        /// Gets or sets Dr. Reflex's charge speed.
        /// </summary>
        public float ChargeSpeed
        {
            get => ReflectionUtil.GetField<float>(Raw, "chargeSpeed");
            set => ReflectionUtil.SetField(Raw, "chargeSpeed", value);
        }

        /// <summary>
        /// Gets or sets Dr. Reflex's hunt speed.
        /// </summary>
        public float HuntSpeed
        {
            get => ReflectionUtil.GetField<float>(Raw, "huntSpeed");
            set => ReflectionUtil.SetField(Raw, "huntSpeed", value);
        }

        /// <summary>
        /// Gets or sets Dr. Reflex's turn speed.
        /// </summary>
        public float TurnSpeed
        {
            get => ReflectionUtil.GetField<float>(Raw, "turnSpeed");
            set => ReflectionUtil.SetField(Raw, "turnSpeed", value);
        }

        /// <summary>
        /// Gets or sets Dr. Reflex's maximum test distance.
        /// </summary>
        public float MaxTestDistance
        {
            get => ReflectionUtil.GetField<float>(Raw, "maxTestDistance");
            set => ReflectionUtil.SetField(Raw, "maxTestDistance", value);
        }

        /// <summary>
        /// Gets or sets Dr. Reflex's cooldown.
        /// </summary>
        public float Cooldown
        {
            get => ReflectionUtil.GetField<float>(Raw, "cooldown");
            set => ReflectionUtil.SetField(Raw, "cooldown", value);
        }

        /// <summary>
        /// Gets or sets the squish duration applied by Dr. Reflex's hammer.
        /// </summary>
        public float SquishTime
        {
            get => ReflectionUtil.GetField<float>(Raw, "squishTime");
            set => ReflectionUtil.SetField(Raw, "squishTime", value);
        }

        /// <summary>
        /// Gets or sets Dr. Reflex's noise value.
        /// </summary>
        public int NoiseValue
        {
            get => ReflectionUtil.GetField<int>(Raw, "noiseValue");
            set => ReflectionUtil.SetField(Raw, "noiseValue", value);
        }

        /// <summary>
        /// Runs Dr. Reflex's callout chance.
        /// </summary>
        /// <param name="happy">True for happy callouts; false for angry callouts.</param>
        /// <returns>True if a callout was played.</returns>
        public bool CalloutChance(bool happy) =>
            ReflectionUtil.Call<bool>(Raw, "CalloutChance", happy);

        /// <summary>
        /// Ends the current reflex test.
        /// </summary>
        /// <param name="success">True if the player succeeded.</param>
        /// <param name="player">The tested player.</param>
        public void EndTest(bool success, BBPPlayerRef player)
        {
            if (player.Raw != null)
                ReflectionUtil.Call(Raw, "EndTest", success, player.Raw);
        }

        /// <summary>
        /// Faces Dr. Reflex toward an entity.
        /// </summary>
        /// <param name="entity">The entity to face.</param>
        public void FaceEntity(BBPEntity entity)
        {
            if (entity.Raw != null)
                ReflectionUtil.Call(Raw, "FaceEntity", entity.Raw);
        }

        /// <summary>
        /// Gets whether Dr. Reflex is facing his next navigation point.
        /// </summary>
        public bool FacingNextPoint() =>
            ReflectionUtil.Call<bool>(Raw, "FacingNextPoint");

        /// <summary>
        /// Gets whether Dr. Reflex is facing the given player.
        /// </summary>
        /// <param name="player">The player to check.</param>
        public bool FacingPlayer(BBPPlayerRef player)
        {
            if (player.Raw == null)
                return false;

            return ReflectionUtil.Call<bool>(Raw, "FacingPlayer", player.Raw);
        }

        /// <summary>
        /// Triggers Dr. Reflex's hammer acquired animation.
        /// </summary>
        public void GetHammer() =>
            ReflectionUtil.Call(Raw, "GetHammer");

        /// <summary>
        /// Makes Dr. Reflex hammer an entity.
        /// </summary>
        /// <param name="entity">The entity to hammer.</param>
        public void Hammer(BBPEntity entity)
        {
            if (entity.Raw != null)
                ReflectionUtil.Call(Raw, "Hammer", entity.Raw);
        }

        /// <summary>
        /// Runs Dr. Reflex's hammer collision check against a player.
        /// </summary>
        /// <param name="player">The target player.</param>
        public void HammerCheck(BBPPlayerRef player)
        {
            if (player.Raw != null)
                ReflectionUtil.Call(Raw, "HammerCheck", player.Raw);
        }

        /// <summary>
        /// Plays Dr. Reflex's hammer sound.
        /// </summary>
        public void HammerSound() =>
            ReflectionUtil.Call(Raw, "HammerSound");

        /// <summary>
        /// Sends Dr. Reflex back to his office.
        /// </summary>
        public void HeadToOffice() =>
            ReflectionUtil.Call(Raw, "HeadToOffice");

        /// <summary>
        /// Handles a clicked reflex hotspot.
        /// </summary>
        /// <param name="side">Raw BB+ Direction enum value.</param>
        public void HotspotClicked(object side) =>
            ReflectionUtil.Call(Raw, "HotspotClicked", side);

        /// <summary>
        /// Increases Dr. Reflex's audio pitch during thinking.
        /// </summary>
        public void IncreasePitch() =>
            ReflectionUtil.Call(Raw, "IncreasePitch");

        /// <summary>
        /// Pauses and turns Dr. Reflex toward his navigation target.
        /// </summary>
        public void PauseAndTurn() =>
            ReflectionUtil.Call(Raw, "PauseAndTurn");

        /// <summary>
        /// Pauses and turns Dr. Reflex toward a player.
        /// </summary>
        /// <param name="player">The player to face.</param>
        public void PauseAndTurn(BBPPlayerRef player)
        {
            if (player.Raw != null)
                ReflectionUtil.Call(Raw, "PauseAndTurn", player.Raw);
        }

        /// <summary>
        /// Returns true if the player left the reflex test range.
        /// </summary>
        /// <param name="player">The tested player.</param>
        public bool PlayerLeft(BBPPlayerRef player)
        {
            if (player.Raw == null)
                return false;

            return ReflectionUtil.Call<bool>(Raw, "PlayerLeft", player.Raw);
        }

        /// <summary>
        /// Resets Dr. Reflex to his base wandering/test state.
        /// </summary>
        public void Reset() =>
            ReflectionUtil.Call(Raw, "Reset");

        /// <summary>
        /// Resets Dr. Reflex's animator triggers.
        /// </summary>
        public void ResetAnimationTriggers() =>
            ReflectionUtil.Call(Raw, "ResetAnimationTriggers");

        /// <summary>
        /// Resets the current reflex test.
        /// </summary>
        public void ResetTest() =>
            ReflectionUtil.Call(Raw, "ResetTest");
    }
}
