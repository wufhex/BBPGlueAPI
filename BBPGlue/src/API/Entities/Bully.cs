using UnityEngine;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Wraps the BB+ Bully NPC and exposes hiding, spawning,
    /// stealing, pushing, and guilt helpers.
    /// </summary>
    public sealed class BBPBully : BBPNpc
    {
        /// <summary>
        /// Creates a Bully wrapper around a raw Bully instance.
        /// </summary>
        /// <param name="raw">The raw Bully object or component.</param>
        public BBPBully(object? raw) : base(raw)
        {
        }

        /// <summary>
        /// Gets Bully's audio manager.
        /// </summary>
        public BBPAudioManager Audio =>
            new BBPAudioManager(ReflectionUtil.GetField<object>(Raw, "audMan"));

        /// <summary>
        /// Gets whether Bully is currently hidden.
        /// </summary>
        public bool Hidden =>
            ReflectionUtil.GetField<bool>(Raw, "hidden");

        /// <summary>
        /// Gets or sets Bully's minimum respawn delay.
        /// </summary>
        public float MinDelay
        {
            get => ReflectionUtil.GetField<float>(Raw, "minDelay");
            set => ReflectionUtil.SetField(Raw, "minDelay", value);
        }

        /// <summary>
        /// Gets or sets Bully's maximum respawn delay.
        /// </summary>
        public float MaxDelay
        {
            get => ReflectionUtil.GetField<float>(Raw, "maxDelay");
            set => ReflectionUtil.SetField(Raw, "maxDelay", value);
        }

        /// <summary>
        /// Gets or sets the maximum time Bully may stay active.
        /// </summary>
        public float MaxStay
        {
            get => ReflectionUtil.GetField<float>(Raw, "maxStay");
            set => ReflectionUtil.SetField(Raw, "maxStay", value);
        }

        /// <summary>
        /// Gets or sets Bully's player spawn buffer distance.
        /// </summary>
        public float PlayerBuffer
        {
            get => ReflectionUtil.GetField<float>(Raw, "playerBuffer");
            set => ReflectionUtil.SetField(Raw, "playerBuffer", value);
        }

        /// <summary>
        /// Makes Bully express boredom.
        /// </summary>
        public void ExpressBoredom() =>
            ReflectionUtil.Call(Raw, "ExpressBoredom");

        /// <summary>
        /// Hides Bully and schedules his waiting behavior.
        /// </summary>
        public void Hide() =>
            ReflectionUtil.Call(Raw, "Hide");

        /// <summary>
        /// Pushes an entity away from Bully.
        /// </summary>
        /// <param name="entity">The entity to push.</param>
        public void Push(BBPEntity entity)
        {
            if (entity.Raw != null)
                ReflectionUtil.Call(Raw, "Push", entity.Raw);
        }

        /// <summary>
        /// Shows or hides Bully's visual/collision components.
        /// </summary>
        /// <param name="visible">True to show; false to hide.</param>
        public void SetComponents(bool visible) =>
            ReflectionUtil.Call(Raw, "SetComponents", visible);

        /// <summary>
        /// Marks Bully as guilty for bullying.
        /// </summary>
        public void SetGuilty() =>
            ReflectionUtil.Call(Raw, "SetGuilty");

        /// <summary>
        /// Attempts to spawn Bully in a valid hallway tile.
        /// </summary>
        public void Spawn() =>
            ReflectionUtil.Call(Raw, "Spawn");

        /// <summary>
        /// Makes Bully steal an item from the given player.
        /// </summary>
        /// <param name="player">The player to steal from.</param>
        public void StealItem(BBPPlayerRef player)
        {
            if (player.Raw != null)
                ReflectionUtil.Call(Raw, "StealItem", player.Raw);
        }

        /// <summary>
        /// Temporarily opens Bully's blocked area if the game supports it.
        /// </summary>
        public void TempOpen() =>
            ReflectionUtil.Call(Raw, "TempOpen");

        /// <summary>
        /// Temporarily closes Bully's blocked area if the game supports it.
        /// </summary>
        public void TempClose() =>
            ReflectionUtil.Call(Raw, "TempClose");
    }
}
