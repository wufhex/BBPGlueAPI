using UnityEngine;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Wraps the BB+ Chalk Face NPC and exposes chalkboard,
    /// timer, room locking, laughter, and activation helpers.
    /// </summary>
    public sealed class BBPChalkFace : BBPNpc
    {
        /// <summary>
        /// Creates a Chalk Face wrapper around a raw ChalkFace instance.
        /// </summary>
        /// <param name="raw">The raw ChalkFace object or component.</param>
        public BBPChalkFace(object? raw) : base(raw)
        {
        }

        /// <summary>
        /// Gets Chalk Face's audio manager.
        /// </summary>
        public BBPAudioManager Audio =>
            new BBPAudioManager(ReflectionUtil.GetField<object>(Raw, "audMan"));

        /// <summary>
        /// Gets or sets the chalk sprite renderer.
        /// </summary>
        public SpriteRenderer? ChalkRenderer
        {
            get => ReflectionUtil.GetField<SpriteRenderer>(Raw, "chalkRenderer");
            set => ReflectionUtil.SetField(Raw, "chalkRenderer", value);
        }

        /// <summary>
        /// Gets or sets the flying sprite renderer.
        /// </summary>
        public SpriteRenderer? FlyingRenderer
        {
            get => ReflectionUtil.GetField<SpriteRenderer>(Raw, "flyingRenderer");
            set => ReflectionUtil.SetField(Raw, "flyingRenderer", value);
        }

        /// <summary>
        /// Gets or sets Chalk Face's set/charge time.
        /// </summary>
        public float SetTime
        {
            get => ReflectionUtil.GetField<float>(Raw, "setTime");
            set => ReflectionUtil.SetField(Raw, "setTime", value);
        }

        /// <summary>
        /// Gets or sets Chalk Face's uncharge rate.
        /// </summary>
        public float UnchargeRate
        {
            get => ReflectionUtil.GetField<float>(Raw, "unchargeRate");
            set => ReflectionUtil.SetField(Raw, "unchargeRate", value);
        }

        /// <summary>
        /// Gets Chalk Face's room lock time.
        /// </summary>
        public float LockTime =>
            ReflectionUtil.GetProperty<float>(Raw, "LockTime");

        /// <summary>
        /// Gets Chalk Face's approach acceleration.
        /// </summary>
        public float ApproachAcceleration =>
            ReflectionUtil.GetProperty<float>(Raw, "Acceleration");

        /// <summary>
        /// Gets or sets Chalk Face's spin speed.
        /// </summary>
        public float SpinSpeed
        {
            get => ReflectionUtil.GetField<float>(Raw, "spinSpeed");
            set => ReflectionUtil.SetField(Raw, "spinSpeed", value);
        }

        /// <summary>
        /// Gets or sets Chalk Face's approach speed.
        /// </summary>
        public float ApproachSpeed
        {
            get => ReflectionUtil.GetField<float>(Raw, "approachSpeed");
            set => ReflectionUtil.SetField(Raw, "approachSpeed", value);
        }

        /// <summary>
        /// Gets or sets Chalk Face's noise value.
        /// </summary>
        public int NoiseValue
        {
            get => ReflectionUtil.GetField<int>(Raw, "noiseVal");
            set => ReflectionUtil.SetField(Raw, "noiseVal", value);
        }

        /// <summary>
        /// Gets or sets Chalk Face's internal charge amount.
        /// </summary>
        public float Charge
        {
            get => ReflectionUtil.GetField<float>(Raw, "charge");
            set => ReflectionUtil.SetField(Raw, "charge", value);
        }

        /// <summary>
        /// Activates Chalk Face in the given room.
        /// </summary>
        /// <param name="room">The room to lock and haunt.</param>
        public void Activate(BBPRoom room)
        {
            if (room.Raw != null)
                ReflectionUtil.Call(Raw, "Activate", room.Raw);
        }

        /// <summary>
        /// Advances Chalk Face's laughter movement around a room.
        /// </summary>
        /// <param name="room">The target room.</param>
        /// <param name="acceleration">Extra acceleration.</param>
        public void AdvanceLaughter(BBPRoom room, float acceleration)
        {
            if (room.Raw != null)
                ReflectionUtil.Call(Raw, "AdvanceLaughter", room.Raw, acceleration);
        }

        /// <summary>
        /// Advances Chalk Face's charge timer.
        /// </summary>
        /// <returns>True if the timer reached the activation threshold.</returns>
        public bool AdvanceTimer() =>
            ReflectionUtil.Call<bool>(Raw, "AdvanceTimer");

        /// <summary>
        /// Cancels Chalk Face's current behavior.
        /// </summary>
        public void Cancel() =>
            ReflectionUtil.Call(Raw, "Cancel");

        /// <summary>
        /// Runs Chalk Face's cancellation cleanup.
        /// </summary>
        public void Cancelled() =>
            ReflectionUtil.Call(Raw, "Cancelled");

        /// <summary>
        /// Decreases Chalk Face's timer by a specific rate.
        /// </summary>
        /// <param name="rate">Amount to decrease.</param>
        public void DecreaseTimer(float rate) =>
            ReflectionUtil.Call(Raw, "DecreaseTimer", rate);

        /// <summary>
        /// Runs Chalk Face's idle update.
        /// </summary>
        public void IdleUpdate() =>
            ReflectionUtil.Call(Raw, "IdleUpdate");

        /// <summary>
        /// Spawns a chalkboard in the given room.
        /// </summary>
        /// <param name="room">The room to spawn a board in.</param>
        public void SpawnBoard(BBPRoom room)
        {
            if (room.Raw != null)
                ReflectionUtil.Call(Raw, "SpawnBoard", room.Raw);
        }

        /// <summary>
        /// Updates Chalk Face's sprite visibility/charge state.
        /// </summary>
        public void UpdateSprite() =>
            ReflectionUtil.Call(Raw, "UpdateSprite");
    }
}
