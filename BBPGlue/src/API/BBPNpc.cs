using System.Collections;
using UnityEngine;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Wraps a BB+ NPC instance and exposes common NPC, navigation, entity, and sprite controls.
    /// </summary>
    public class BBPNpc
    {
        /// <summary>
        /// Gets the raw underlying NPC instance.
        /// </summary>
        public object? Raw { get; }

        /// <summary>
        /// Creates a wrapper around a raw NPC instance.
        /// </summary>
        /// <param name="raw">The raw NPC object or component.</param>
        public BBPNpc(object? raw)
        {
            Raw = raw;
        }

        /// <summary>
        /// Gets whether the wrapped NPC instance exists.
        /// </summary>
        public bool Exists => Raw != null;

        /// <summary>
        /// Gets the entity component associated with this NPC.
        /// </summary>
        public BBPEntity Entity =>
            new BBPEntity(ReflectionUtil.GetProperty<object>(Raw, "Entity"));

        /// <summary>
        /// Gets the NPC navigator.
        /// </summary>
        public object? Navigator =>
            ReflectionUtil.GetProperty<object>(Raw, "Navigator");

        /// <summary>
        /// Gets or sets the NPC looker component.
        /// </summary>
        public object? Looker
        {
            get => ReflectionUtil.GetField<object>(Raw, "looker");
            set => ReflectionUtil.SetField(Raw, "looker", value);
        }

        /// <summary>
        /// Gets or sets the environment controller associated with this NPC.
        /// </summary>
        public object? EnvironmentController
        {
            get => ReflectionUtil.GetField<object>(Raw, "ec");
            set => ReflectionUtil.SetField(Raw, "ec", value);
        }

        /// <summary>
        /// Gets the NPC behavior state machine.
        /// </summary>
        public object? BehaviorStateMachine =>
            ReflectionUtil.GetField<object>(Raw, "behaviorStateMachine");

        /// <summary>
        /// Gets the NPC navigation state machine.
        /// </summary>
        public object? NavigationStateMachine =>
            ReflectionUtil.GetField<object>(Raw, "navigationStateMachine");

        /// <summary>
        /// Gets the NPC character identifier as text.
        /// </summary>
        public string Character =>
            ReflectionUtil.GetProperty<object>(Raw, "Character")?.ToString() ?? "Unknown";

        /// <summary>
        /// Gets or sets the raw character value.
        /// </summary>
        public object? CharacterRaw
        {
            get => ReflectionUtil.GetField<object>(Raw, "character");
            set => ReflectionUtil.SetField(Raw, "character", value);
        }

        /// <summary>
        /// Gets or sets the NPC poster object.
        /// </summary>
        public object? Poster
        {
            get => ReflectionUtil.GetProperty<object>(Raw, "Poster");
            set => ReflectionUtil.SetField(Raw, "poster", value);
        }

        /// <summary>
        /// Gets or sets the root GameObject for the NPC sprites.
        /// </summary>
        public GameObject? SpriteBase
        {
            get => ReflectionUtil.GetField<GameObject>(Raw, "spriteBase");
            set => ReflectionUtil.SetField(Raw, "spriteBase", value);
        }

        /// <summary>
        /// Gets or sets the NPC sprite renderers.
        /// </summary>
        public SpriteRenderer[]? SpriteRenderers
        {
            get => ReflectionUtil.GetField<SpriteRenderer[]>(Raw, "spriteRenderer");
            set => ReflectionUtil.SetField(Raw, "spriteRenderer", value);
        }

        /// <summary>
        /// Gets or sets the NPC base trigger colliders.
        /// </summary>
        public Collider[]? BaseTriggers
        {
            get => ReflectionUtil.GetField<Collider[]>(Raw, "baseTrigger");
            set => ReflectionUtil.SetField(Raw, "baseTrigger", value);
        }

        /// <summary>
        /// Gets the rooms this NPC can spawn in.
        /// </summary>
        public IList? SpawnableRooms =>
            ReflectionUtil.GetField<IList>(Raw, "spawnableRooms");

        /// <summary>
        /// Gets or sets the NPC potential room assets.
        /// </summary>
        public object? PotentialRoomAssets
        {
            get => ReflectionUtil.GetField<object>(Raw, "potentialRoomAssets");
            set => ReflectionUtil.SetField(Raw, "potentialRoomAssets", value);
        }

        /// <summary>
        /// Gets or sets whether the NPC ignores the player immediately after spawning.
        /// </summary>
        public bool IgnorePlayerOnSpawn
        {
            get => ReflectionUtil.GetProperty<bool>(Raw, "IgnorePlayerOnSpawn");
            set => ReflectionUtil.SetField(Raw, "ignorePlayerOnSpawn", value);
        }

        /// <summary>
        /// Gets whether the NPC is currently disobeying.
        /// </summary>
        public bool Disobeying =>
            ReflectionUtil.GetProperty<bool>(Raw, "Disobeying");

        /// <summary>
        /// Gets whether the NPC is blinded.
        /// </summary>
        public bool Blinded =>
            ReflectionUtil.GetProperty<bool>(Raw, "Blinded");

        /// <summary>
        /// Gets the rule currently associated with this NPC.
        /// </summary>
        public string BrokenRule =>
            ReflectionUtil.GetProperty<string>(Raw, "BrokenRule") ?? "";

        /// <summary>
        /// Gets whether the NPC currently has a detour.
        /// </summary>
        public bool HasDetour =>
            ReflectionUtil.GetProperty<bool>(Raw, "HasDetour");

        /// <summary>
        /// Gets the NPC's current time scale.
        /// </summary>
        public float TimeScale =>
            ReflectionUtil.GetProperty<float>(Raw, "TimeScale");

        /// <summary>
        /// Gets or sets the NPC position in world space.
        /// </summary>
        public Vector3 Position
        {
            get
            {
                if (Raw is Component component)
                    return component.transform.position;

                return Vector3.zero;
            }
            set
            {
                if (Raw is Component component)
                    component.transform.position = value;
            }
        }

        /// <summary>
        /// Gets or sets the NPC rotation in world space.
        /// </summary>
        public Quaternion Rotation
        {
            get
            {
                if (Raw is Component component)
                    return component.transform.rotation;

                return Quaternion.identity;
            }
            set
            {
                if (Raw is Component component)
                    component.transform.rotation = value;
            }
        }

        /// <summary>
        /// Gets or sets the NPC GameObject name.
        /// </summary>
        public string Name
        {
            get
            {
                if (Raw is Component component)
                    return component.gameObject.name;

                return Raw?.GetType().Name ?? "NULL";
            }
            set
            {
                if (Raw is Component component)
                    component.gameObject.name = value;
            }
        }

        /// <summary>
        /// Gets the NPC's calculated speed from the game object.
        /// </summary>
        public float CalculatedSpeed => ReflectionUtil.GetProperty<float>(Raw, "Speed");

        /// <summary>
        /// Gets or sets the NPC navigator's maximum speed.
        /// </summary>
        public float MaxSpeed
        {
            get => ReflectionUtil.GetField<float>(Navigator, "maxSpeed");
            set => ReflectionUtil.SetField(Navigator, "maxSpeed", value);
        }

        /// <summary>
        /// Gets or sets the NPC navigator's acceleration.
        /// </summary>
        public float Acceleration
        {
            get => ReflectionUtil.GetField<float>(Navigator, "accel");
            set => ReflectionUtil.SetField(Navigator, "accel", value);
        }

        /// <summary>
        /// Gets or sets the NPC navigator radius.
        /// </summary>
        public float Radius
        {
            get => ReflectionUtil.GetProperty<float>(Navigator, "Radius");
            set => ReflectionUtil.SetField(Navigator, "radius", value);
        }

        /// <summary>
        /// Gets whether the NPC navigator has a destination.
        /// </summary>
        public bool HasDestination =>
            ReflectionUtil.GetProperty<bool>(Navigator, "HasDestination");

        /// <summary>
        /// Gets whether the NPC is currently wandering.
        /// </summary>
        public bool Wandering =>
            ReflectionUtil.GetProperty<bool>(Navigator, "Wandering");

        /// <summary>
        /// Gets the NPC navigator's current destination.
        /// </summary>
        public Vector3 CurrentDestination =>
            ReflectionUtil.GetProperty<Vector3>(Navigator, "CurrentDestination");

        /// <summary>
        /// Gets the next point in the NPC navigator's path.
        /// </summary>
        public Vector3 NextPoint =>
            ReflectionUtil.GetProperty<Vector3>(Navigator, "NextPoint");

        /// <summary>
        /// Gets the NPC navigator's current velocity.
        /// </summary>
        public Vector3 Velocity =>
            ReflectionUtil.GetProperty<Vector3>(Navigator, "Velocity");

        /// <summary>
        /// Sets the NPC navigator speed.
        /// </summary>
        /// <param name="speed">The speed to set.</param>
        public void SetSpeed(float speed)
        {
            ReflectionUtil.Call(Navigator, "SetSpeed", speed);
        }

        /// <summary>
        /// Enables or disables room avoidance for NPC pathfinding.
        /// </summary>
        /// <param name="value">True to avoid rooms; otherwise, false.</param>
        public void SetRoomAvoidance(bool value)
        {
            ReflectionUtil.Call(Navigator, "SetRoomAvoidance", value);
        }

        /// <summary>
        /// Clears the NPC's current destination.
        /// </summary>
        public void ClearDestination()
        {
            ReflectionUtil.Call(Navigator, "ClearDestination");
        }

        /// <summary>
        /// Finds a path to the target position.
        /// </summary>
        /// <param name="target">The target position.</param>
        public void FindPath(Vector3 target)
        {
            ReflectionUtil.Call(Navigator, "FindPath", target);
        }

        /// <summary>
        /// Finds a path from a custom start position to a target position.
        /// </summary>
        /// <param name="start">The start position.</param>
        /// <param name="target">The target position.</param>
        public void FindPath(Vector3 start, Vector3 target)
        {
            ReflectionUtil.Call(Navigator, "FindPath", start, target);
        }

        /// <summary>
        /// Finds a path to the target while avoiding a position.
        /// </summary>
        /// <param name="target">The target position.</param>
        /// <param name="avoid">The position to avoid.</param>
        public void FindPathAvoid(Vector3 target, Vector3 avoid)
        {
            ReflectionUtil.Call(Navigator, "FindPathAvoid", target, avoid);
        }

        /// <summary>
        /// Makes the NPC wander to a random destination.
        /// </summary>
        public void WanderRandom()
        {
            ReflectionUtil.Call(Navigator, "WanderRandom");
        }

        /// <summary>
        /// Makes the NPC use its round-wandering behavior.
        /// </summary>
        public void WanderRounds()
        {
            ReflectionUtil.Call(Navigator, "WanderRounds");
        }

        /// <summary>
        /// Skips the NPC navigator's current destination point.
        /// </summary>
        public void SkipCurrentDestinationPoint()
        {
            ReflectionUtil.Call(Navigator, "SkipCurrentDestinationPoint");
        }

        /// <summary>
        /// Appends a destination point to the NPC navigator.
        /// </summary>
        /// <param name="position">The point to append.</param>
        public void ManuallyAppendDestinationPoint(Vector3 position)
        {
            ReflectionUtil.Call(Navigator, "ManuallyAppendDestinationPoints", position);
        }

        /// <summary>
        /// Initializes the NPC.
        /// </summary>
        public void Initialize()
        {
            ReflectionUtil.Call(Raw, "Initialize");
        }

        /// <summary>
        /// Despawns the NPC.
        /// </summary>
        public void Despawn()
        {
            ReflectionUtil.Call(Raw, "Despawn");
        }

        /// <summary>
        /// Clears the NPC's guilt state.
        /// </summary>
        public void ClearGuilt()
        {
            ReflectionUtil.Call(Raw, "ClearGuilt");
        }

        /// <summary>
        /// Sends a target position to the NPC.
        /// </summary>
        /// <param name="target">The target position.</param>
        public void TargetPosition(Vector3 target)
        {
            ReflectionUtil.Call(Raw, "TargetPosition", target);
        }

        /// <summary>
        /// Teleports the NPC to a position.
        /// </summary>
        /// <param name="position">The target position.</param>
        public void Teleport(Vector3 position)
        {
            Entity.Teleport(position);
        }

        /// <summary>
        /// Teleports the NPC to the player position.
        /// </summary>
        public void TeleportToPlayer()
        {
            Teleport(BBP.Player.Position);
        }

        /// <summary>
        /// Makes the NPC hear a sound.
        /// </summary>
        /// <param name="source">The sound source GameObject.</param>
        /// <param name="position">The sound position.</param>
        /// <param name="value">The sound value used by the game.</param>
        public void Hear(GameObject source, Vector3 position, int value)
        {
            ReflectionUtil.Call(Raw, "Hear", source, position, value);
        }

        /// <summary>
        /// Makes the NPC hear a sound without a source GameObject.
        /// </summary>
        /// <param name="position">The sound position.</param>
        /// <param name="value">The sound value used by the game.</param>
        public void Hear(Vector3 position, int value)
        {
            ReflectionUtil.Call(Raw, "Hear", null!, position, value);
        }

        /// <summary>
        /// Calls the NPC sighted handler.
        /// </summary>
        public void Sighted()
        {
            ReflectionUtil.Call(Raw, "Sighted");
        }

        /// <summary>
        /// Calls the NPC unsighted handler.
        /// </summary>
        public void Unsighted()
        {
            ReflectionUtil.Call(Raw, "Unsighted");
        }

        /// <summary>
        /// Notifies the NPC that it made a navigation decision.
        /// </summary>
        public void MadeNavigationDecision()
        {
            ReflectionUtil.Call(Raw, "MadeNavigationDecision");
        }

        /// <summary>
        /// Notifies the NPC that its destination is empty.
        /// </summary>
        public void DestinationEmpty()
        {
            ReflectionUtil.Call(Raw, "DestinationEmpty");
        }

        /// <summary>
        /// Calls the NPC detention handler.
        /// </summary>
        public void SentToDetention()
        {
            ReflectionUtil.Call(Raw, "SentToDetention");
        }

        /// <summary>
        /// Calls the NPC distance check.
        /// </summary>
        /// <param name="value">The value to pass to the game method.</param>
        /// <returns>The value returned by the game method.</returns>
        public float DistanceCheck(float value)
        {
            return ReflectionUtil.Call<float>(Raw, "DistanceCheck", value);
        }

        /// <summary>
        /// Sets the sprite used by all NPC sprite renderers.
        /// </summary>
        /// <param name="sprite">The sprite to use.</param>
        public void SetSprite(Sprite sprite)
        {
            SpriteRenderer[]? renderers = SpriteRenderers;

            if (renderers == null)
                return;

            foreach (SpriteRenderer renderer in renderers)
            {
                if (renderer != null)
                    renderer.sprite = sprite;
            }
        }

        /// <summary>
        /// Sets the color used by all NPC sprite renderers.
        /// </summary>
        /// <param name="color">The color to apply.</param>
        public void SetSpriteColor(Color color)
        {
            SpriteRenderer[]? renderers = SpriteRenderers;

            if (renderers == null)
                return;

            foreach (SpriteRenderer renderer in renderers)
            {
                if (renderer != null)
                    renderer.color = color;
            }
        }

        /// <summary>
        /// Shows or hides the NPC sprite root object.
        /// </summary>
        /// <param name="active">True to show the sprite root; false to hide it.</param>
        public void SetSpriteBaseActive(bool active)
        {
            SpriteBase?.SetActive(active);
        }

        /// <summary>
        /// Freezes or unfreezes the NPC entity.
        /// </summary>
        /// <param name="frozen">True to freeze the NPC; false to unfreeze it.</param>
        public void SetFrozen(bool frozen) => Entity.SetFrozen(frozen);

        /// <summary>
        /// Sets whether the NPC entity is hidden.
        /// </summary>
        /// <param name="hidden">True to hide the NPC; false to show it.</param>
        public void SetHidden(bool hidden) => Entity.SetHidden(hidden);

        /// <summary>
        /// Sets whether the NPC entity is visible.
        /// </summary>
        /// <param name="visible">True to make the NPC visible; false to hide it.</param>
        public void SetVisible(bool visible) => Entity.SetVisible(visible);

        /// <summary>
        /// Sets whether the NPC entity is blinded.
        /// </summary>
        /// <param name="blinded">True to blind the NPC; false to restore sight.</param>
        public void SetBlinded(bool blinded) => Entity.SetBlinded(blinded);

        /// <summary>
        /// Squishes the NPC entity for a duration.
        /// </summary>
        /// <param name="time">The duration in seconds.</param>
        public void Squish(float time) => Entity.Squish(time);

        /// <summary>
        /// Removes the NPC entity's squished state.
        /// </summary>
        public void Unsquish() => Entity.Unsquish();

        /// <summary>
        /// Flips the NPC entity.
        /// </summary>
        public void Flip() => Entity.Flip();

        /// <summary>
        /// Sets the NPC entity's vertical scale.
        /// </summary>
        /// <param name="scale">The vertical scale value.</param>
        public void SetVerticalScale(float scale) => Entity.SetVerticalScale(scale);

        /// <summary>
        /// Clears all forces applied to the NPC entity.
        /// </summary>
        public void KillAllForces() => Entity.KillAllForces();
    }
}
