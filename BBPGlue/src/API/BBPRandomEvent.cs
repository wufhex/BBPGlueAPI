using UnityEngine;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Represents a random event in the environment and exposes its properties and lifecycle methods.
    /// </summary>
    public sealed class BBPRandomEvent
    {
        /// <summary>
        /// The raw underlying object wrapped by this API class, or null if none.
        /// </summary>
        public object? Raw { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BBPRandomEvent"/> class wrapping a raw object.
        /// </summary>
        /// <param name="raw">The raw underlying object instance or null.</param>
        public BBPRandomEvent(object? raw)
        {
            Raw = raw;
        }

        /// <summary>
        /// Determines whether the underlying raw object exists.
        /// </summary>
        public bool Exists => Raw != null;

        /// <summary>
        /// Gets a name for the random event derived from the underlying object.
        /// </summary>
        /// <returns>The name of the event as a string.</returns>
        public string Name =>
            Raw is Component c ? c.gameObject.name : Raw?.GetType().Name ?? "NULL";

        /// <summary>
        /// Gets the type identifier of the random event.
        /// </summary>
        /// <returns>The event type as a string.</returns>
        public string Type =>
            ReflectionUtil.GetProperty<object>(Raw, "Type")?.ToString() ?? "Unknown";

        /// <summary>
        /// Gets whether the random event is currently active.
        /// </summary>
        /// <returns>True if active; otherwise false.</returns>
        public bool Active =>
            ReflectionUtil.GetProperty<bool>(Raw, "Active");

        /// <summary>
        /// Gets the configured event time.
        /// </summary>
        /// <returns>The event time as a float.</returns>
        public float EventTime =>
            ReflectionUtil.GetProperty<float>(Raw, "EventTime");

        /// <summary>
        /// Gets the sound object played at the start of the event.
        /// </summary>
        /// <returns>A <see cref="BBPSoundObject"/> instance.</returns>
        public BBPSoundObject EventIntro =>
            new BBPSoundObject(ReflectionUtil.GetProperty<object>(Raw, "EventIntro"));

        /// <summary>
        /// Gets the optional jingle override sound for the event.
        /// </summary>
        /// <returns>A <see cref="BBPSoundObject"/> instance or an empty wrapper.</returns>
        public BBPSoundObject EventJingleOverride =>
            new BBPSoundObject(ReflectionUtil.GetProperty<object>(Raw, "EventJingleOverride"));

        /// <summary>
        /// Gets the room associated with this event.
        /// </summary>
        /// <returns>A <see cref="BBPRoom"/> instance representing the event's room.</returns>
        public BBPRoom Room =>
            new BBPRoom(ReflectionUtil.GetField<object>(Raw, "room"));

        /// <summary>
        /// Gets or sets the environment controller associated with this event.
        /// </summary>
        /// <returns>The environment controller object or null.</returns>
        public object? EnvironmentController
        {
            get => ReflectionUtil.GetField<object>(Raw, "ec");
            set => ReflectionUtil.SetField(Raw, "ec", value);
        }

        /// <summary>
        /// Gets or sets the remaining time for the event.
        /// </summary>
        /// <returns>The remaining time as a float.</returns>
        public float RemainingTime
        {
            get => ReflectionUtil.GetField<float>(Raw, "remainingTime");
            set => ReflectionUtil.SetField(Raw, "remainingTime", value);
        }

        /// <summary>
        /// Gets or sets the minimum event time for scheduling.
        /// </summary>
        /// <returns>The minimum event time as a float.</returns>
        public float MinEventTime
        {
            get => ReflectionUtil.GetField<float>(Raw, "minEventTime");
            set => ReflectionUtil.SetField(Raw, "minEventTime", value);
        }

        /// <summary>
        /// Gets or sets the maximum event time for scheduling.
        /// </summary>
        /// <returns>The maximum event time as a float.</returns>
        public float MaxEventTime
        {
            get => ReflectionUtil.GetField<float>(Raw, "maxEventTime");
            set => ReflectionUtil.SetField(Raw, "maxEventTime", value);
        }

        /// <summary>
        /// Gets or sets the duration of the event description.
        /// </summary>
        /// <returns>The description time as a float.</returns>
        public float DescriptionTime
        {
            get => ReflectionUtil.GetField<float>(Raw, "descriptionTime");
            set => ReflectionUtil.SetField(Raw, "descriptionTime", value);
        }

        /// <summary>
        /// Gets potential room assets associated with the event.
        /// </summary>
        /// <returns>An object representing potential room assets or null.</returns>
        public object? PotentialRoomAssets =>
            ReflectionUtil.GetProperty<object>(Raw, "PotentialRoomAssets");

        /// <summary>
        /// Initializes the random event with the given environment controller and RNG.
        /// </summary>
        /// <param name="environmentController">The environment controller to associate with the event.</param>
        /// <param name="rng">A random number generator object used for setup.</param>
        public void Initialize(object environmentController, object rng) =>
            ReflectionUtil.Call(Raw, "Initialize", environmentController, rng);

        /// <summary>
        /// Assigns the specified room to this event.
        /// </summary>
        /// <param name="room">The room to assign.</param>
        public void AssignRoom(BBPRoom room)
        {
            if (room.Raw != null)
                ReflectionUtil.Call(Raw, "AssignRoom", room.Raw);
        }

        /// <summary>
        /// Begins the random event by invoking the underlying Begin method.
        /// </summary>
        public void Begin() =>
            ReflectionUtil.Call(Raw, "Begin");

        /// <summary>
        /// Ends the random event by invoking the underlying End method.
        /// </summary>
        public void End() =>
            ReflectionUtil.Call(Raw, "End");

        /// <summary>
        /// Pauses the random event.
        /// </summary>
        public void Pause() =>
            ReflectionUtil.Call(Raw, "Pause");

        /// <summary>
        /// Unpauses the random event.
        /// </summary>
        public void Unpause() =>
            ReflectionUtil.Call(Raw, "Unpause");

        /// <summary>
        /// Performs premade setup for the event by invoking PremadeSetup.
        /// </summary>
        public void PremadeSetup() =>
            ReflectionUtil.Call(Raw, "PremadeSetup");

        /// <summary>
        /// Resets the event's conditions.
        /// </summary>
        public void ResetConditions() =>
            ReflectionUtil.Call(Raw, "ResetConditions");

        /// <summary>
        /// Performs setup after an update cycle using the provided RNG.
        /// </summary>
        /// <param name="rng">Random number generator object used for setup.</param>
        public void AfterUpdateSetup(object rng) =>
            ReflectionUtil.Call(Raw, "AfterUpdateSetup", rng);

        /// <summary>
        /// Sets the event time using the provided RNG by invoking the underlying SetEventTime method.
        /// </summary>
        /// <param name="rng">A random number generator used to set the event time.</param>
        public void SetEventTime(object rng) =>
            ReflectionUtil.Call(Raw, "SetEventTime", rng);
    }
}