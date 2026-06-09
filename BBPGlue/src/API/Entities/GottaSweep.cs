using UnityEngine;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Wraps the BB+ Gotta Sweep NPC and exposes sweeping,
    /// home, and timing helpers.
    /// </summary>
    public sealed class BBPGottaSweep : BBPNpc
    {
        /// <summary>
        /// Creates a Gotta Sweep wrapper around a raw GottaSweep instance.
        /// </summary>
        /// <param name="raw">The raw GottaSweep object or component.</param>
        public BBPGottaSweep(object? raw) : base(raw)
        {
        }

        /// <summary>
        /// Gets Gotta Sweep's audio manager.
        /// </summary>
        public BBPAudioManager Audio =>
            new BBPAudioManager(ReflectionUtil.GetField<object>(Raw, "audMan"));

        /// <summary>
        /// Gets or sets Gotta Sweep's home position.
        /// </summary>
        public Vector3 Home
        {
            get => ReflectionUtil.GetField<Vector3>(Raw, "home");
            set => ReflectionUtil.SetField(Raw, "home", value);
        }

        /// <summary>
        /// Gets whether Gotta Sweep is currently at home.
        /// </summary>
        public bool IsHome =>
            ReflectionUtil.GetProperty<bool>(Raw, "IsHome");

        /// <summary>
        /// Gets a random delay from Gotta Sweep's configured delay range.
        /// </summary>
        public float RandomDelay =>
            ReflectionUtil.GetProperty<float>(Raw, "GetRandomDelay");

        /// <summary>
        /// Gets a random sweep duration from Gotta Sweep's configured active range.
        /// </summary>
        public float RandomSweepTime =>
            ReflectionUtil.GetProperty<float>(Raw, "GetRandomSweepTime");

        /// <summary>
        /// Gets or sets Gotta Sweep's sweep speed.
        /// </summary>
        public float SweepSpeed
        {
            get => ReflectionUtil.GetField<float>(Raw, "speed");
            set => ReflectionUtil.SetField(Raw, "speed", value);
        }

        /// <summary>
        /// Starts Gotta Sweep's sweeping behavior.
        /// </summary>
        public void StartSweeping() =>
            ReflectionUtil.Call(Raw, "StartSweeping");

        /// <summary>
        /// Stops Gotta Sweep's sweeping behavior.
        /// </summary>
        public void StopSweeping() =>
            ReflectionUtil.Call(Raw, "StopSweeping");
    }
}
