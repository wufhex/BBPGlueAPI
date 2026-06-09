using UnityEngine;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Represents an elevator in the level, exposing state, doors, and control methods.
    /// </summary>
    public sealed class BBPElevator
    {
        /// <summary>
        /// The raw underlying elevator object wrapped by this API class, or null if none.
        /// </summary>
        public object? Raw { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BBPElevator"/> class wrapping a raw object.
        /// </summary>
        /// <param name="raw">The raw underlying object instance or null.</param>
        public BBPElevator(object? raw)
        {
            Raw = raw;
        }

        /// <summary>
        /// Determines whether the underlying raw object exists.
        /// </summary>
        public bool Exists => Raw != null;

        /// <summary>
        /// Gets the world position of the elevator.
        /// </summary>
        /// <returns>The position as a <see cref="Vector3"/>.</returns>
        public Vector3 Position =>
            Raw is Component c ? c.transform.position : Vector3.zero;

        /// <summary>
        /// Gets the door object attached to the elevator.
        /// </summary>
        /// <returns>A <see cref="BBPDoor"/> instance.</returns>
        public BBPDoor Door =>
            new BBPDoor(ReflectionUtil.GetProperty<object>(Raw, "Door"));

        /// <summary>
        /// Gets the current state object of the elevator state machine.
        /// </summary>
        /// <returns>The current state object or null.</returns>
        public object? CurrentState =>
            ReflectionUtil.GetProperty<object>(Raw, "CurrentState");

        /// <summary>
        /// Gets whether the elevator gate is currently open.
        /// </summary>
        /// <returns>True if the gate is open; otherwise false.</returns>
        public bool GateIsOpen =>
            ReflectionUtil.GetProperty<bool>(Raw, "GateIsOpen");

        /// <summary>
        /// Gets or sets whether this elevator is a spawn point.
        /// </summary>
        /// <returns>True if it is a spawn; otherwise false.</returns>
        public bool IsSpawn
        {
            get => ReflectionUtil.GetProperty<bool>(Raw, "IsSpawn");
            set => ReflectionUtil.SetProperty(Raw, "IsSpawn", value);
        }

        /// <summary>
        /// Gets whether the elevator lobby blocks NPC movement.
        /// </summary>
        /// <returns>True if the lobby blocks NPCs; otherwise false.</returns>
        public bool LobbyBlocksNpcs =>
            ReflectionUtil.GetProperty<bool>(Raw, "LobbyBlocksNpcs");

        /// <summary>
        /// Gets whether the elevator is currently powered.
        /// </summary>
        /// <returns>True if powered; otherwise false.</returns>
        public bool Powered =>
            ReflectionUtil.GetProperty<bool>(Raw, "Powered");

        /// <summary>
        /// Gets the collider group object associated with this elevator.
        /// </summary>
        /// <returns>The collider group object or null.</returns>
        public object? ColliderGroup =>
            ReflectionUtil.GetProperty<object>(Raw, "ColliderGroup");

        /// <summary>
        /// Triggers the elevator's button pressed behavior.
        /// </summary>
        public void ButtonPressed() =>
            ReflectionUtil.Call(Raw, "ButtonPressed");

        /// <summary>
        /// Opens or closes the elevator door.
        /// </summary>
        /// <param name="open">True to open the door; false to close.</param>
        public void OpenDoor(bool open) =>
            ReflectionUtil.Call(Raw, "OpenDoor", open);

        /// <summary>
        /// Opens the elevator gate.
        /// </summary>
        public void OpenGate() =>
            ReflectionUtil.Call(Raw, "OpenGate");

        /// <summary>
        /// Closes the elevator gate.
        /// </summary>
        public void CloseGate() =>
            ReflectionUtil.Call(Raw, "CloseGate");

        /// <summary>
        /// Prepares the elevator for an exit sequence.
        /// </summary>
        public void PrepareForExit() =>
            ReflectionUtil.Call(Raw, "PrepareForExit");

        /// <summary>
        /// Prepares the elevator to close its doors.
        /// </summary>
        public void PrepareToClose() =>
            ReflectionUtil.Call(Raw, "PrepareToClose");

        /// <summary>
        /// Sets the elevator's state in its state machine.
        /// </summary>
        /// <param name="state">The state object to set.</param>
        public void SetState(object state) =>
            ReflectionUtil.Call(Raw, "SetState", state);

        /// <summary>
        /// Performs a state update on the elevator's state machine.
        /// </summary>
        public void StateUpdate() =>
            ReflectionUtil.Call(Raw, "StateUpdate");
    }
}