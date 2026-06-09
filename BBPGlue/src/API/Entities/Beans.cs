using UnityEngine;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Wraps the BB+ Beans NPC and exposes gum, sprinting, chewing,
    /// and audio helpers.
    /// </summary>
    public sealed class BBPBeans : BBPNpc
    {
        /// <summary>
        /// Creates a Beans wrapper around a raw Beans instance.
        /// </summary>
        /// <param name="raw">The raw Beans object or component.</param>
        public BBPBeans(object? raw) : base(raw)
        {
        }

        /// <summary>
        /// Gets Beans' animator.
        /// </summary>
        public Animator? Animator =>
            ReflectionUtil.GetField<Animator>(Raw, "animator");

        /// <summary>
        /// Gets Beans' audio manager.
        /// </summary>
        public BBPAudioManager Audio =>
            new BBPAudioManager(ReflectionUtil.GetField<object>(Raw, "audMan"));

        /// <summary>
        /// Gets the active gum object created by Beans.
        /// </summary>
        public object? Gum =>
            ReflectionUtil.GetField<object>(Raw, "gum");

        /// <summary>
        /// Gets whether Beans currently has a gum object.
        /// </summary>
        public bool HasGum => Gum != null;

        /// <summary>
        /// Gets Beans' configured chew time.
        /// </summary>
        public float ChewTime =>
            ReflectionUtil.GetProperty<float>(Raw, "ChewTime");

        /// <summary>
        /// Gets Beans' configured cooldown time.
        /// </summary>
        public float CooldownTime =>
            ReflectionUtil.GetProperty<float>(Raw, "CooldownTime");

        /// <summary>
        /// Gets a random sprint duration from Beans' configured range.
        /// </summary>
        public float SprintTime =>
            ReflectionUtil.GetProperty<float>(Raw, "SprintTime");

        /// <summary>
        /// Gets a random sprint wait time from Beans' configured range.
        /// </summary>
        public float SprintWait =>
            ReflectionUtil.GetProperty<float>(Raw, "SprintWait");

        /// <summary>
        /// Plays Beans' bubble blow sound.
        /// </summary>
        public void Blow() =>
            ReflectionUtil.Call(Raw, "Blow");

        /// <summary>
        /// Plays Beans' chewing animation.
        /// </summary>
        public void Chew() =>
            ReflectionUtil.Call(Raw, "Chew");

        /// <summary>
        /// Prepares Beans to chew and target a player.
        /// </summary>
        /// <param name="player">The target player.</param>
        public void ChewPrep(BBPPlayerRef player)
        {
            if (player.Raw != null)
                ReflectionUtil.Call(Raw, "ChewPrep", player.Raw);
        }

        /// <summary>
        /// Notifies Beans that gum hit something.
        /// </summary>
        /// <param name="gum">The gum object.</param>
        /// <param name="hitSelf">True if Beans hit himself.</param>
        public void GumHit(object gum, bool hitSelf) =>
            ReflectionUtil.Call(Raw, "GumHit", gum, hitSelf);

        /// <summary>
        /// Plays Beans' NPC hit voice reaction.
        /// </summary>
        public void HitNpc() =>
            ReflectionUtil.Call(Raw, "HitNPC");

        /// <summary>
        /// Plays Beans' player hit voice reaction.
        /// </summary>
        public void HitPlayer() =>
            ReflectionUtil.Call(Raw, "HitPlayer");

        /// <summary>
        /// Makes Beans spit gum at the given player.
        /// </summary>
        /// <param name="player">The target player.</param>
        public void Spit(BBPPlayerRef player)
        {
            if (player.Raw != null)
                ReflectionUtil.Call(Raw, "Spit", player.Raw);
        }

        /// <summary>
        /// Starts Beans' sprint behavior.
        /// </summary>
        public void Sprint() =>
            ReflectionUtil.Call(Raw, "Sprint");

        /// <summary>
        /// Stops Beans' sprint behavior.
        /// </summary>
        public void StopSprint() =>
            ReflectionUtil.Call(Raw, "StopSprint");
    }
}
