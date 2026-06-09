using UnityEngine;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Wraps the BB+ No Late Teacher NPC and exposes classroom,
    /// dragging, attacking, timing, and audio helpers.
    /// </summary>
    public sealed class BBPNoLateTeacher : BBPNpc
    {
        /// <summary>
        /// Creates a No Late Teacher wrapper around a raw NoLateTeacher instance.
        /// </summary>
        /// <param name="raw">The raw NoLateTeacher object or component.</param>
        public BBPNoLateTeacher(object? raw) : base(raw)
        {
        }

        /// <summary>
        /// Gets No Late Teacher's main audio manager.
        /// </summary>
        public BBPAudioManager Audio =>
            new BBPAudioManager(ReflectionUtil.GetField<object>(Raw, "audMan"));

        /// <summary>
        /// Gets No Late Teacher's stomp audio manager.
        /// </summary>
        public BBPAudioManager StompAudio =>
            new BBPAudioManager(ReflectionUtil.GetField<object>(Raw, "stompAudMan"));

        /// <summary>
        /// Gets No Late Teacher's animator.
        /// </summary>
        public Animator? Animator =>
            ReflectionUtil.GetField<Animator>(Raw, "animator");

        /// <summary>
        /// Gets No Late Teacher's popup animator.
        /// </summary>
        public Animator? PopupAnimator =>
            ReflectionUtil.GetField<Animator>(Raw, "popupAnimator");

        /// <summary>
        /// Gets or sets No Late Teacher's sprite renderer.
        /// </summary>
        public SpriteRenderer? Sprite
        {
            get => ReflectionUtil.GetField<SpriteRenderer>(Raw, "sprite");
            set => ReflectionUtil.SetField(Raw, "sprite", value);
        }

        /// <summary>
        /// Gets or sets No Late Teacher's normal sprite.
        /// </summary>
        public Sprite? NormalSprite
        {
            get => ReflectionUtil.GetField<Sprite>(Raw, "normalSprite");
            set => ReflectionUtil.SetField(Raw, "normalSprite", value);
        }

        /// <summary>
        /// Gets or sets No Late Teacher's angry sprite.
        /// </summary>
        public Sprite? AngrySprite
        {
            get => ReflectionUtil.GetField<Sprite>(Raw, "angrySprite");
            set => ReflectionUtil.SetField(Raw, "angrySprite", value);
        }

        /// <summary>
        /// Gets No Late Teacher's cooldown duration.
        /// </summary>
        public float Cooldown =>
            ReflectionUtil.GetProperty<float>(Raw, "Cooldown");

        /// <summary>
        /// Gets whether No Late Teacher is currently speaking.
        /// </summary>
        public bool IsSpeaking =>
            ReflectionUtil.GetProperty<bool>(Raw, "IsSpeaking");

        /// <summary>
        /// Gets the target classroom as a room wrapper.
        /// </summary>
        public BBPRoom TargetClassRoom =>
            new BBPRoom(ReflectionUtil.GetProperty<object>(Raw, "TargetClassRoom"));

        /// <summary>
        /// Gets or sets No Late Teacher's walk speed.
        /// </summary>
        public float WalkSpeed
        {
            get => ReflectionUtil.GetField<float>(Raw, "walkSpeed");
            set => ReflectionUtil.SetField(Raw, "walkSpeed", value);
        }

        /// <summary>
        /// Gets or sets No Late Teacher's run speed.
        /// </summary>
        public float RunSpeed
        {
            get => ReflectionUtil.GetField<float>(Raw, "runSpeed");
            set => ReflectionUtil.SetField(Raw, "runSpeed", value);
        }

        /// <summary>
        /// Gets or sets No Late Teacher's angry speed.
        /// </summary>
        public float AngrySpeedValue
        {
            get => ReflectionUtil.GetField<float>(Raw, "angrySpeed");
            set => ReflectionUtil.SetField(Raw, "angrySpeed", value);
        }

        /// <summary>
        /// Gets or sets the class time limit.
        /// </summary>
        public float ClassTime
        {
            get => ReflectionUtil.GetField<float>(Raw, "classTime");
            set => ReflectionUtil.SetField(Raw, "classTime", value);
        }

        /// <summary>
        /// Gets or sets No Late Teacher's drag speed.
        /// </summary>
        public float DragSpeed
        {
            get => ReflectionUtil.GetField<float>(Raw, "dragSpeed");
            set => ReflectionUtil.SetField(Raw, "dragSpeed", value);
        }

        /// <summary>
        /// Gets or sets No Late Teacher's success point reward.
        /// </summary>
        public int SuccessPoints
        {
            get => ReflectionUtil.GetField<int>(Raw, "successPoints");
            set => ReflectionUtil.SetField(Raw, "successPoints", value);
        }

        /// <summary>
        /// Sets No Late Teacher to angry movement speed.
        /// </summary>
        public void SetAngrySpeed() =>
            ReflectionUtil.Call(Raw, "AngrySpeed");

        /// <summary>
        /// Starts No Late Teacher's attack behavior against a player.
        /// </summary>
        /// <param name="player">The target player.</param>
        public void Attack(BBPPlayerRef player)
        {
            if (player.Raw != null)
                ReflectionUtil.Call(Raw, "Attack", player.Raw);
        }

        /// <summary>
        /// Makes No Late Teacher call the player.
        /// </summary>
        public void CallPlayer() =>
            ReflectionUtil.Call(Raw, "CallPlayer");

        /// <summary>
        /// Gets whether No Late Teacher can drag the given player.
        /// </summary>
        /// <param name="player">The player to check.</param>
        public bool CanDrag(BBPPlayerRef player)
        {
            if (player.Raw == null)
                return false;

            return ReflectionUtil.Call<bool>(Raw, "CanDrag", player.Raw);
        }

        /// <summary>
        /// Sets No Late Teacher to chase speed.
        /// </summary>
        public void ChaseSpeed() =>
            ReflectionUtil.Call(Raw, "ChaseSpeed");

        /// <summary>
        /// Dismisses No Late Teacher's current classroom objective.
        /// </summary>
        public void Dismiss() =>
            ReflectionUtil.Call(Raw, "Dismiss");

        /// <summary>
        /// Attempts to drag the given player.
        /// </summary>
        /// <param name="player">The player to drag.</param>
        /// <returns>True if dragging continued.</returns>
        public bool Drag(BBPPlayerRef player)
        {
            if (player.Raw == null)
                return false;

            return ReflectionUtil.Call<bool>(Raw, "Drag", player.Raw);
        }

        /// <summary>
        /// Gets No Late Teacher's current class destination world position.
        /// </summary>
        public Vector3 GetClassPosition() =>
            ReflectionUtil.Call<Vector3>(Raw, "GetClassPosition");

        /// <summary>
        /// Sends No Late Teacher and the player toward the class objective.
        /// </summary>
        /// <param name="player">The target player.</param>
        public void HeadToClass(BBPPlayerRef player)
        {
            if (player.Raw != null)
                ReflectionUtil.Call(Raw, "HeadToClass", player.Raw);
        }

        /// <summary>
        /// Rewards the player for arriving in time.
        /// </summary>
        /// <param name="scoreMultiplier">Score multiplier.</param>
        public void InTime(int scoreMultiplier) =>
            ReflectionUtil.Call(Raw, "InTime", scoreMultiplier);

        /// <summary>
        /// Handles No Late Teacher catching a player.
        /// </summary>
        /// <param name="player">The caught player.</param>
        public void PlayerCaught(BBPPlayerRef player)
        {
            if (player.Raw != null)
                ReflectionUtil.Call(Raw, "PlayerCaught", player.Raw);
        }

        /// <summary>
        /// Updates No Late Teacher's timer display.
        /// </summary>
        /// <param name="time">Time remaining.</param>
        public void UpdateTimer(float time) =>
            ReflectionUtil.Call(Raw, "UpdateTimer", time);
    }
}
