using UnityEngine;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Reference wrapper for a player instance exposing common transforms and actions.
    /// </summary>
    public sealed class BBPPlayerRef
    {
        /// <summary>
        /// The underlying raw player object.
        /// </summary>
        public object Raw { get; }

        /// <summary>
        /// Initializes a new wrapper around the supplied raw player object.
        /// </summary>
        /// <param name="raw">The raw player object to wrap.</param>
        public BBPPlayerRef(object raw)
        {
            Raw = raw;
        }

        /// <summary>
        /// The Unity component associated with the player, if any.
        /// </summary>
        public Component? Component => Raw as Component;
        /// <summary>
        /// The Unity GameObject representing the player, if available.
        /// </summary>
        public GameObject? GameObject => Component?.gameObject;
        /// <summary>
        /// The Unity Transform of the player, if available.
        /// </summary>
        public Transform? Transform => Component?.transform;

        /// <summary>
        /// The world position of the player, or Vector3.zero when unavailable.
        /// </summary>
        public Vector3 Position =>
            Transform != null ? Transform.position : Vector3.zero;

        /// <summary>
        /// The player index number.
        /// </summary>
        public int PlayerNumber
        {
            get => ReflectionUtil.GetField<int>(Raw, "playerNumber");
            set => ReflectionUtil.SetField(Raw, "playerNumber", value);
        }

        /// <summary>
        /// Text describing the last rule break by the player.
        /// </summary>
        public string RuleBreakText =>
            ReflectionUtil.GetField<string>(Raw, "ruleBreak") ?? "";

        /// <summary>
        /// Whether the player is currently invincible.
        /// </summary>
        public bool Invincible
        {
            get => ReflectionUtil.GetField<bool>(Raw, "invincible");
            set => ReflectionUtil.SetField(Raw, "invincible", value);
        }

        /// <summary>
        /// Whether the player is tagged.
        /// </summary>
        public bool Tagged =>
            ReflectionUtil.GetProperty<bool>(Raw, "Tagged");

        /// <summary>
        /// Whether the player is invisible.
        /// </summary>
        public bool Invisible =>
            ReflectionUtil.GetProperty<bool>(Raw, "Invisible");

        /// <summary>
        /// The entity wrapper associated with this player.
        /// </summary>
        public BBPEntity Entity
        {
            get
            {
                object? plm = ReflectionUtil.GetField<object>(Raw, "plm");
                object? entity = ReflectionUtil.GetProperty<object>(plm, "Entity");
                return new BBPEntity(entity);
            }
        }

        /// <summary>
        /// Teleports the player to the supplied world position.
        /// </summary>
        /// <param name="position">Destination position.</param>
        public void Teleport(Vector3 position)
        {
            ReflectionUtil.Call(Raw, "Teleport", position);
        }

        /// <summary>
        /// Sets the player's hidden state.
        /// </summary>
        /// <param name="hidden">True to hide the player; otherwise false.</param>
        public void SetHidden(bool hidden)
        {
            ReflectionUtil.Call(Raw, "SetHidden", hidden);
        }

        /// <summary>
        /// Enables or disables the player's nametag.
        /// </summary>
        /// <param name="state">True to show the nametag; false to hide it.</param>
        public void SetNametag(bool state)
        {
            ReflectionUtil.Call(Raw, "SetNametag", state);
        }

        /// <summary>
        /// Reverses the player's facing or movement state (game-specific behavior).
        /// </summary>
        public void Reverse()
        {
            ReflectionUtil.Call(Raw, "Reverse");
        }

        /// <summary>
        /// Clears the player's guilt state.
        /// </summary>
        public void ClearGuilt()
        {
            ReflectionUtil.Call(Raw, "ClearGuilt");
        }
    }
}