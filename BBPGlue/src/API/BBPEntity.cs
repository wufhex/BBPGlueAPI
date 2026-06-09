using UnityEngine;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Wrapper around game entity objects exposing common state and control methods.
    /// </summary>
    public sealed class BBPEntity
    {
        /// <summary>
        /// The underlying raw game entity object wrapped by this instance.
        /// </summary>
        public object? Raw { get; }

        /// <summary>
        /// Creates a new entity wrapper for the provided raw object.
        /// </summary>
        /// <param name="raw">The raw entity object or null.</param>
        public BBPEntity(object? raw)
        {
            Raw = raw;
        }

        /// <summary>
        /// True when the underlying raw object is not null.
        /// </summary>
        public bool Exists => Raw != null;

        /// <summary>
        /// Unity Component wrapper for the entity, if available.
        /// </summary>
        public Component? Component => Raw as Component;
        /// <summary>
        /// Unity GameObject for the entity, if available.
        /// </summary>
        public GameObject? GameObject => Component?.gameObject;
        /// <summary>
        /// Unity Transform for the entity, if available.
        /// </summary>
        public Transform? Transform => Component?.transform;

        /// <summary>
        /// World position of the entity. Setting will teleport the entity.
        /// </summary>
        public Vector3 Position
        {
            get => Transform != null ? Transform.position : Vector3.zero;
            set => Teleport(value);
        }

        /// <summary>
        /// Current velocity of the entity.
        /// </summary>
        public Vector3 Velocity =>
            ReflectionUtil.GetProperty<Vector3>(Raw, "Velocity");

        /// <summary>
        /// Movement vector used internally by the entity.
        /// </summary>
        public Vector3 InternalMovement =>
            ReflectionUtil.GetProperty<Vector3>(Raw, "InternalMovement");

        /// <summary>
        /// Velocity relative to forced motion applied to the entity.
        /// </summary>
        public Vector3 RelativeToForcedVelocity =>
            ReflectionUtil.GetProperty<Vector3>(Raw, "RelativeToForcedVelocity");

        /// <summary>
        /// Whether the entity is visible.
        /// </summary>
        public bool Visible =>
            ReflectionUtil.GetProperty<bool>(Raw, "Visible");

        /// <summary>
        /// Whether the entity is hidden.
        /// </summary>
        public bool Hidden =>
            ReflectionUtil.GetProperty<bool>(Raw, "Hidden");

        /// <summary>
        /// Whether the entity is frozen.
        /// </summary>
        public bool Frozen =>
            ReflectionUtil.GetProperty<bool>(Raw, "Frozen");

        /// <summary>
        /// Whether the entity is blinded.
        /// </summary>
        public bool Blinded =>
            ReflectionUtil.GetProperty<bool>(Raw, "Blinded");

        /// <summary>
        /// Whether the entity is currently grounded.
        /// </summary>
        public bool Grounded =>
            ReflectionUtil.GetProperty<bool>(Raw, "Grounded");

        /// <summary>
        /// Whether the entity is squished.
        /// </summary>
        public bool Squished =>
            ReflectionUtil.GetProperty<bool>(Raw, "Squished");

        /// <summary>
        /// Whether the entity is flipped horizontally.
        /// </summary>
        public bool Flipped =>
            ReflectionUtil.GetProperty<bool>(Raw, "Flipped");

        /// <summary>
        /// Whether the entity is within valid bounds.
        /// </summary>
        public bool InBounds =>
            ReflectionUtil.GetProperty<bool>(Raw, "InBounds");

        /// <summary>
        /// Whether interactions are disabled for the entity.
        /// </summary>
        public bool InteractionDisabled =>
            ReflectionUtil.GetProperty<bool>(Raw, "InteractionDisabled");

        /// <summary>
        /// Whether the entity is considered fully active by the game logic.
        /// </summary>
        public bool TotallyActive =>
            ReflectionUtil.GetProperty<bool>(Raw, "TotallyActive");

        /// <summary>
        /// Base height of the entity for collision purposes.
        /// </summary>
        public float BaseHeight =>
            ReflectionUtil.GetProperty<float>(Raw, "BaseHeight");

        /// <summary>
        /// Internal height used by physics calculations.
        /// </summary>
        public float InternalHeight =>
            ReflectionUtil.GetProperty<float>(Raw, "InternalHeight");

        /// <summary>
        /// Internal height taking scale factor into account.
        /// </summary>
        public float InternalHeightWithScaleFactor =>
            ReflectionUtil.GetProperty<float>(Raw, "InternalHeightWithScaleFactor");

        /// <summary>
        /// Reference to the environment controller the entity is bound to.
        /// </summary>
        public object? EnvironmentController =>
            ReflectionUtil.GetProperty<object>(Raw, "Ec");

        /// <summary>
        /// Current room object containing this entity.
        /// </summary>
        public object? CurrentRoom =>
            ReflectionUtil.GetProperty<object>(Raw, "CurrentRoom");

        /// <summary>
        /// External activity object if present.
        /// </summary>
        public object? ExternalActivity =>
            ReflectionUtil.GetProperty<object>(Raw, "ExternalActivity");

        /// <summary>
        /// Collider used as the entity's trigger.
        /// </summary>
        public Collider? Trigger =>
            ReflectionUtil.GetProperty<Collider>(Raw, "Trigger");

        /// <summary>
        /// Teleports the entity to the specified world position.
        /// </summary>
        /// <param name="position">Destination position.</param>
        public void Teleport(Vector3 position)
        {
            ReflectionUtil.Call(Raw, "Teleport", position);
        }

        /// <summary>
        /// Moves the entity while performing collision checks.
        /// </summary>
        /// <param name="movement">Movement vector to apply.</param>
        public void MoveWithCollision(Vector3 movement)
        {
            ReflectionUtil.Call(Raw, "MoveWithCollision", movement);
        }

        /// <summary>
        /// Updates the internal movement vector used by the entity.
        /// </summary>
        /// <param name="movement">Movement vector to set.</param>
        public void UpdateInternalMovement(Vector3 movement)
        {
            ReflectionUtil.Call(Raw, "UpdateInternalMovement", movement);
        }

        /// <summary>
        /// Activates or deactivates the entity.
        /// </summary>
        /// <param name="value">True to activate; false to deactivate.</param>
        public void SetActive(bool value)
        {
            ReflectionUtil.Call(Raw, "SetActive", value);
        }

        /// <summary>
        /// Enables or disables the entity's behavior.
        /// </summary>
        public void Enable(bool value)
        {
            ReflectionUtil.Call(Raw, "Enable", value);
        }

        /// <summary>
        /// Sets the entity's frozen state.
        /// </summary>
        /// <param name="value">True to freeze; false to unfreeze.</param>
        public void SetFrozen(bool value)
        {
            ReflectionUtil.Call(Raw, "SetFrozen", value);
        }

        /// <summary>
        /// Sets whether the entity is blinded.
        /// </summary>
        /// <param name="value">True to blind; false to unblind.</param>
        public void SetBlinded(bool value)
        {
            ReflectionUtil.Call(Raw, "SetBlinded", value);
        }

        /// <summary>
        /// Sets the entity's hidden state.
        /// </summary>
        public void SetHidden(bool value)
        {
            ReflectionUtil.Call(Raw, "SetHidden", value);
        }

        /// <summary>
        /// Sets whether the entity is visible.
        /// </summary>
        public void SetVisible(bool value)
        {
            ReflectionUtil.Call(Raw, "SetVisible", value);
        }

        /// <summary>
        /// Sets whether the entity is grounded.
        /// </summary>
        public void SetGrounded(bool value)
        {
            ReflectionUtil.Call(Raw, "SetGrounded", value);
        }

        /// <summary>
        /// Enables or disables interactions for the entity.
        /// </summary>
        /// <param name="value">True to enable; false to disable.</param>
        public void SetInteractionEnabled(bool value)
        {
            ReflectionUtil.Call(Raw, "SetInteractionState", value);
        }

        /// <summary>
        /// Sets whether the entity's collider acts as a trigger.
        /// </summary>
        public void SetTrigger(bool value)
        {
            ReflectionUtil.Call(Raw, "SetTrigger", value);
        }

        /// <summary>
        /// Sets the entity's collision height.
        /// </summary>
        public void SetHeight(float height)
        {
            ReflectionUtil.Call(Raw, "SetHeight", height);
        }

        /// <summary>
        /// Sets vertical scaling factor applied to the entity.
        /// </summary>
        public void SetVerticalScale(float factor)
        {
            ReflectionUtil.Call(Raw, "SetVerticalScale", factor);
        }

        /// <summary>
        /// Sets a base multiplier applied to animations.
        /// </summary>
        public void SetBaseAnimationMultiplier(float value)
        {
            ReflectionUtil.Call(Raw, "SetBaseAnimationMultiplier", value);
        }

        /// <summary>
        /// Rotates the sprite representation of the entity.
        /// </summary>
        public void SetSpriteRotation(float degrees)
        {
            ReflectionUtil.Call(Raw, "SetSpriteRotation", degrees);
        }

        /// <summary>
        /// Changes the target rendering/interaction layer for the entity.
        /// </summary>
        public void SetTargetLayer(int layer)
        {
            ReflectionUtil.Call(Raw, "SetTargetLayer", layer);
        }

        /// <summary>
        /// Sets an additive resistance flag for the entity.
        /// </summary>
        public void SetResistAddend(bool value)
        {
            ReflectionUtil.Call(Raw, "SetResistAddend", value);
        }

        /// <summary>
        /// Flips the entity horizontally.
        /// </summary>
        public void Flip()
        {
            ReflectionUtil.Call(Raw, "Flip");
        }

        /// <summary>
        /// Squishes the entity for the specified duration.
        /// </summary>
        public void Squish(float time)
        {
            ReflectionUtil.Call(Raw, "Squish", time);
        }

        /// <summary>
        /// Reverses a previous squish effect.
        /// </summary>
        public void Unsquish()
        {
            ReflectionUtil.Call(Raw, "Unsquish");
        }

        /// <summary>
        /// Removes all forces acting on the entity.
        /// </summary>
        public void KillAllForces()
        {
            ReflectionUtil.Call(Raw, "KillAllForces");
        }

        /// <summary>
        /// Requests the entity update its internal state.
        /// </summary>
        public void UpdateState()
        {
            ReflectionUtil.Call(Raw, "UpdateState");
        }

        /// <summary>
        /// Refreshes visibility-related state.
        /// </summary>
        public void UpdateVisibility()
        {
            ReflectionUtil.Call(Raw, "UpdateVisibility");
        }

        /// <summary>
        /// Refreshes audio-related state.
        /// </summary>
        public void UpdateAudioState()
        {
            ReflectionUtil.Call(Raw, "UpdateAudioState");
        }

        /// <summary>
        /// Returns whether this entity can collide with another entity.
        /// </summary>
        public bool CanCollideWith(BBPEntity other)
        {
            if (other?.Raw == null) return false;
            return ReflectionUtil.Call<bool>(Raw, "CanCollideWith", other.Raw!);
        }

        /// <summary>
        /// Returns whether a collision with the other entity would be valid.
        /// </summary>
        public bool CollisionValid(BBPEntity other)
        {
            if (other?.Raw == null) return false;
            return ReflectionUtil.Call<bool>(Raw, "CollisionValid", other.Raw!);
        }

        /// <summary>
        /// Adds or removes the other entity from this entity's ignore list.
        /// </summary>
        public void IgnoreEntity(BBPEntity other, bool value)
        {
            if (other?.Raw == null) return;
            ReflectionUtil.Call(Raw, "IgnoreEntity", other.Raw!, value);
        }

        /// <summary>
        /// Returns whether this entity is ignoring the other entity.
        /// </summary>
        public bool IsIgnoring(BBPEntity other)
        {
            if (other?.Raw == null) return false;
            return ReflectionUtil.Call<bool>(Raw, "IsIgnoring", other.Raw!);
        }

        /// <summary>
        /// Returns the GameObject used for audio playback associated with this entity.
        /// </summary>
        public GameObject? AudioGameObject()
        {
            return ReflectionUtil.Call<GameObject>(Raw, "AudioGameObject");
        }
    }
}