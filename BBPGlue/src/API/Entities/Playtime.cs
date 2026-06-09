using UnityEngine;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Wraps the BB+ Playtime NPC and exposes jump rope, movement,
    /// animation, and audio helpers.
    /// </summary>
    public sealed class BBPPlaytime : BBPNpc
    {
        /// <summary>
        /// Creates a Playtime wrapper around a raw Playtime instance.
        /// </summary>
        /// <param name="raw">The raw Playtime object or component.</param>
        public BBPPlaytime(object? raw) : base(raw)
        {
        }

        /// <summary>
        /// Gets Playtime's animator.
        /// </summary>
        public Animator? Animator =>
            ReflectionUtil.GetField<Animator>(Raw, "animator");

        /// <summary>
        /// Gets Playtime's audio manager.
        /// </summary>
        public BBPAudioManager Audio =>
            new BBPAudioManager(ReflectionUtil.GetField<object>(Raw, "audMan"));

        /// <summary>
        /// Gets or sets Playtime's normal wandering speed.
        /// </summary>
        public float NormalSpeed
        {
            get => ReflectionUtil.GetField<float>(Raw, "normSpeed");
            set => ReflectionUtil.SetField(Raw, "normSpeed", value);
        }

        /// <summary>
        /// Gets or sets Playtime's running/chasing speed.
        /// </summary>
        public float RunSpeed
        {
            get => ReflectionUtil.GetField<float>(Raw, "runSpeed");
            set => ReflectionUtil.SetField(Raw, "runSpeed", value);
        }

        /// <summary>
        /// Gets Playtime's initial cooldown after jump rope.
        /// </summary>
        public float InitialCooldown =>
            ReflectionUtil.GetProperty<float>(Raw, "InitialCooldown");

        /// <summary>
        /// Gets the current active jumprope object, if any.
        /// </summary>
        public object? CurrentJumprope =>
            ReflectionUtil.GetField<object>(Raw, "currentJumprope");

        /// <summary>
        /// Gets whether Playtime currently has an active jumprope.
        /// </summary>
        public bool HasJumprope => CurrentJumprope != null;

        /// <summary>
        /// Makes Playtime play her sad reaction.
        /// </summary>
        public void BecomeSad() =>
            ReflectionUtil.Call(Raw, "BecomeSad");

        /// <summary>
        /// Runs Playtime's random callout chance.
        /// </summary>
        public void CalloutChance() =>
            ReflectionUtil.Call(Raw, "CalloutChance");

        /// <summary>
        /// Plays a count voice line for a jump count.
        /// </summary>
        /// <param name="jumps">The jump count.</param>
        public void Count(int jumps) =>
            ReflectionUtil.Call(Raw, "Count", jumps);

        /// <summary>
        /// Ends Playtime's cooldown and returns her to wandering.
        /// </summary>
        public void EndCooldown() =>
            ReflectionUtil.Call(Raw, "EndCooldown");

        /// <summary>
        /// Ends the current jump rope session.
        /// </summary>
        /// <param name="won">True if the player won the jump rope minigame.</param>
        public void EndJumprope(bool won) =>
            ReflectionUtil.Call(Raw, "EndJumprope", won);

        /// <summary>
        /// Plays Playtime's jump rope hit reaction.
        /// </summary>
        public void JumpropeHit() =>
            ReflectionUtil.Call(Raw, "JumpropeHit");

        /// <summary>
        /// Makes Playtime pursue a player.
        /// </summary>
        /// <param name="player">The player to pursue.</param>
        public void PersuePlayer(BBPPlayerRef player)
        {
            if (player.Raw != null)
                ReflectionUtil.Call(Raw, "PersuePlayer", player.Raw);
        }

        /// <summary>
        /// Makes Playtime turn the player around and return to wandering.
        /// </summary>
        /// <param name="player">The player to affect.</param>
        public void PlayerTurnAround(BBPPlayerRef player)
        {
            if (player.Raw != null)
                ReflectionUtil.Call(Raw, "PlayerTurnAround", player.Raw);
        }

        /// <summary>
        /// Starts a jump rope session with the given player.
        /// </summary>
        /// <param name="player">The player to force into jump rope.</param>
        public void StartJumprope(BBPPlayerRef player)
        {
            if (player.Raw != null)
                ReflectionUtil.Call(Raw, "StartJumprope", player.Raw);
        }

        /// <summary>
        /// Starts Playtime's player pursuit behavior.
        /// </summary>
        /// <param name="player">The player to pursue.</param>
        public void StartPersuingPlayer(BBPPlayerRef player)
        {
            if (player.Raw != null)
                ReflectionUtil.Call(Raw, "StartPersuingPlayer", player.Raw);
        }
    }
}
