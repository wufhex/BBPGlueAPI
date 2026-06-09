using UnityEngine;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Wraps the BB+ Cumulo NPC and exposes wind,
    /// hallway selection, and blowing helpers.
    /// </summary>
    public sealed class BBPCumulo : BBPNpc
    {
        /// <summary>
        /// Creates a Cumulo wrapper around a raw Cumulo instance.
        /// </summary>
        /// <param name="raw">The raw Cumulo object or component.</param>
        public BBPCumulo(object? raw) : base(raw)
        {
        }

        /// <summary>
        /// Gets Cumulo's audio manager.
        /// </summary>
        public BBPAudioManager Audio =>
            new BBPAudioManager(ReflectionUtil.GetField<object>(Raw, "audMan"));

        /// <summary>
        /// Gets Cumulo's raw wind manager object.
        /// </summary>
        public object? WindManager =>
            ReflectionUtil.GetField<object>(Raw, "windManager");

        /// <summary>
        /// Gets or sets the parent transform for Cumulo's wind graphics.
        /// </summary>
        public Transform? WindGraphicsParent
        {
            get => ReflectionUtil.GetField<Transform>(Raw, "windGraphicsParent");
            set => ReflectionUtil.SetField(Raw, "windGraphicsParent", value);
        }

        /// <summary>
        /// Gets or sets Cumulo's wind graphics renderers.
        /// </summary>
        public MeshRenderer[]? WindGraphics
        {
            get => ReflectionUtil.GetField<MeshRenderer[]>(Raw, "windGraphics");
            set => ReflectionUtil.SetField(Raw, "windGraphics", value);
        }

        /// <summary>
        /// Gets a random blow time from Cumulo's configured range.
        /// </summary>
        public float RandomBlowTime =>
            ReflectionUtil.GetProperty<float>(Raw, "RandomBlowTime");

        /// <summary>
        /// Gets or sets Cumulo's minimum stay time.
        /// </summary>
        public float MinStay
        {
            get => ReflectionUtil.GetField<float>(Raw, "minStay");
            set => ReflectionUtil.SetField(Raw, "minStay", value);
        }

        /// <summary>
        /// Gets or sets Cumulo's maximum stay time.
        /// </summary>
        public float MaxStay
        {
            get => ReflectionUtil.GetField<float>(Raw, "maxStay");
            set => ReflectionUtil.SetField(Raw, "maxStay", value);
        }

        /// <summary>
        /// Gets or sets Cumulo's wind speed.
        /// </summary>
        public float WindSpeed
        {
            get => ReflectionUtil.GetField<float>(Raw, "windSpeed");
            set => ReflectionUtil.SetField(Raw, "windSpeed", value);
        }

        /// <summary>
        /// Gets or sets the minimum hallway length Cumulo can use.
        /// </summary>
        public int MinHallLength
        {
            get => ReflectionUtil.GetField<int>(Raw, "minHallLength");
            set => ReflectionUtil.SetField(Raw, "minHallLength", value);
        }

        /// <summary>
        /// Starts Cumulo's blowing behavior.
        /// </summary>
        public void Blow() =>
            ReflectionUtil.Call(Raw, "Blow");

        /// <summary>
        /// Finds a valid hallway destination for Cumulo.
        /// </summary>
        public void FindDestination() =>
            ReflectionUtil.Call(Raw, "FindDestination");

        /// <summary>
        /// Gets whether Cumulo is on his destination cell.
        /// </summary>
        public bool OnDestinationCell() =>
            ReflectionUtil.Call<bool>(Raw, "OnDestinationCell");

        /// <summary>
        /// Stops Cumulo's blowing behavior.
        /// </summary>
        public void StopBlowing() =>
            ReflectionUtil.Call(Raw, "StopBlowing");

        /// <summary>
        /// Targets Cumulo's current destination cell.
        /// </summary>
        public void TargetDestinationCell() =>
            ReflectionUtil.Call(Raw, "TargetDestinationCell");

        /// <summary>
        /// Updates Cumulo's wind volume, graphics, collider, and direction.
        /// </summary>
        public void UpdateWind() =>
            ReflectionUtil.Call(Raw, "UpdateWind");
    }
}
