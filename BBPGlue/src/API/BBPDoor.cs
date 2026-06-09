using UnityEngine;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Represents a door in the environment and exposes its state and control methods.
    /// </summary>
    public sealed class BBPDoor
    {
        /// <summary>
        /// The raw underlying door object wrapped by this API class, or null if none.
        /// </summary>
        public object? Raw { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BBPDoor"/> class wrapping a raw object.
        /// </summary>
        /// <param name="raw">The raw underlying object instance or null.</param>
        public BBPDoor(object? raw)
        {
            Raw = raw;
        }

        /// <summary>
        /// Determines whether the underlying raw object exists.
        /// </summary>
        public bool Exists => Raw != null;

        /// <summary>
        /// Gets the world position of the door.
        /// </summary>
        /// <returns>The position as a <see cref="Vector3"/>.</returns>
        public Vector3 Position =>
            Raw is Component c ? c.transform.position : Vector3.zero;

        /// <summary>
        /// Gets whether the door is currently open according to the underlying property.
        /// </summary>
        /// <returns>True if open; otherwise false.</returns>
        public bool IsOpen =>
            ReflectionUtil.GetProperty<bool>(Raw, "IsOpen");

        /// <summary>
        /// Gets or sets the raw open state flag for the door.
        /// </summary>
        /// <returns>True if open; otherwise false.</returns>
        public bool OpenState
        {
            get => ReflectionUtil.GetField<bool>(Raw, "open");
            set => ReflectionUtil.SetField(Raw, "open", value);
        }

        /// <summary>
        /// Gets or sets whether the door is locked.
        /// </summary>
        /// <returns>True if locked; otherwise false.</returns>
        public bool Locked
        {
            get => ReflectionUtil.GetField<bool>(Raw, "locked");
            set => ReflectionUtil.SetField(Raw, "locked", value);
        }

        /// <summary>
        /// Gets or sets whether the door makes noise when operated.
        /// </summary>
        /// <returns>True if it makes noise; otherwise false.</returns>
        public bool MakesNoise
        {
            get => ReflectionUtil.GetField<bool>(Raw, "makesNoise");
            set => ReflectionUtil.SetField(Raw, "makesNoise", value);
        }

        /// <summary>
        /// Gets or sets the noise value produced by the door when operated.
        /// </summary>
        /// <returns>The noise value as an integer.</returns>
        public int NoiseValue
        {
            get => ReflectionUtil.GetField<int>(Raw, "noiseValue");
            set => ReflectionUtil.SetField(Raw, "noiseValue", value);
        }

        /// <summary>
        /// Gets or sets whether closing the door blocks passage.
        /// </summary>
        /// <returns>True if closing blocks passage; otherwise false.</returns>
        public bool CloseBlocks
        {
            get => ReflectionUtil.GetField<bool>(Raw, "closeBlocks");
            set => ReflectionUtil.SetField(Raw, "closeBlocks", value);
        }

        /// <summary>
        /// Gets or sets whether locking the door blocks passage.
        /// </summary>
        /// <returns>True if locking blocks passage; otherwise false.</returns>
        public bool LockBlocks
        {
            get => ReflectionUtil.GetField<bool>(Raw, "lockBlocks");
            set => ReflectionUtil.SetField(Raw, "lockBlocks", value);
        }

        /// <summary>
        /// Initializes the door by invoking the underlying Initialize method.
        /// </summary>
        public void Initialize() =>
            ReflectionUtil.Call(Raw, "Initialize");

        /// <summary>
        /// Uninitializes the door by invoking the underlying UnInitialize method.
        /// </summary>
        public void UnInitialize() =>
            ReflectionUtil.Call(Raw, "UnInitialize");

        /// <summary>
        /// Opens the door, optionally canceling any open timer and producing noise.
        /// </summary>
        /// <param name="cancelTimer">Whether to cancel the open timer.</param>
        /// <param name="makeNoise">Whether opening should make noise.</param>
        public void Open(bool cancelTimer = true, bool makeNoise = true) =>
            ReflectionUtil.Call(Raw, "Open", cancelTimer, makeNoise);

        /// <summary>
        /// Opens the door silently by not producing noise.
        /// </summary>
        /// <param name="cancelTimer">Whether to cancel the open timer.</param>
        public void OpenSilent(bool cancelTimer = true) =>
            Open(cancelTimer, false);

        /// <summary>
        /// Opens the door for a timed duration.
        /// </summary>
        /// <param name="time">Duration in seconds to keep the door open.</param>
        /// <param name="makeNoise">Whether opening should make noise.</param>
        public void OpenTimed(float time, bool makeNoise = true) =>
            ReflectionUtil.Call(Raw, "OpenTimed", time, makeNoise);

        /// <summary>
        /// Shuts the door immediately.
        /// </summary>
        public void Shut() =>
            ReflectionUtil.Call(Raw, "Shut");

        /// <summary>
        /// Toggles the door's open/closed state.
        /// </summary>
        /// <param name="cancelTimer">Whether to cancel any existing timer.</param>
        /// <param name="makeNoise">Whether the action should make noise.</param>
        public void Toggle(bool cancelTimer = true, bool makeNoise = true) =>
            ReflectionUtil.Call(Raw, "Toggle", cancelTimer, makeNoise);

        /// <summary>
        /// Locks the door.
        /// </summary>
        /// <param name="cancelTimer">Whether to cancel any existing timer when locking.</param>
        public void Lock(bool cancelTimer = true) =>
            ReflectionUtil.Call(Raw, "Lock", cancelTimer);

        /// <summary>
        /// Locks the door for a timed duration.
        /// </summary>
        /// <param name="time">Duration in seconds to lock the door.</param>
        public void LockTimed(float time) =>
            ReflectionUtil.Call(Raw, "LockTimed", time);

        /// <summary>
        /// Unlocks the door.
        /// </summary>
        public void Unlock() =>
            ReflectionUtil.Call(Raw, "Unlock");

        /// <summary>
        /// Sets whether the door blocks passage.
        /// </summary>
        /// <param name="block">True to block; false to unblock.</param>
        public void Block(bool block) =>
            ReflectionUtil.Call(Raw, "Block", block);

        /// <summary>
        /// Gets the cell object on the other side of the door relative to the given room.
        /// </summary>
        /// <param name="room">The room to consider as the reference side.</param>
        /// <returns>The cell object on the other side or null.</returns>
        public object? CellOnOtherSide(object room) =>
            ReflectionUtil.Call<object>(Raw, "CellOnOtherSide", room);

        /// <summary>
        /// Returns a directed integer vector describing the tile coordinates on the other side of the door.
        /// </summary>
        /// <param name="room">The room to consider as the reference side.</param>
        /// <returns>An object representing integer vector coordinates or null.</returns>
        public object? OtherSideDirectedIntVector2(object room) =>
            ReflectionUtil.Call<object>(Raw, "OtherSideDirectedIntVector2", room);
    }
}