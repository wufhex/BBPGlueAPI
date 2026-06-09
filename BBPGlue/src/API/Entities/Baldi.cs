using UnityEngine;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Wraps the BB+ Baldi NPC and exposes Baldi-specific fields,
    /// sounds, detention behavior, and helper methods.
    /// </summary>
    public sealed class BBPBaldi : BBPNpc
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BBPBaldi"/> class wrapping a raw object.
        /// </summary>
        /// <param name="raw">The raw underlying object instance or null.</param>
        public BBPBaldi(object? raw) : base(raw)
        {
        }

        /// <summary>
        /// Gets the audio manager associated with Baldi.
        /// </summary>
        /// <returns>A <see cref="BBPAudioManager"/> instance wrapping the underlying audio manager.</returns>
        public BBPAudioManager Audio =>
            new BBPAudioManager(ReflectionUtil.GetProperty<object>(Raw, "AudMan"));

        /// <summary>
        /// Gets or sets the base anger value for Baldi.
        /// </summary>
        /// <returns>The base anger as a float.</returns>
        public float BaseAnger
        {
            get => ReflectionUtil.GetField<float>(Raw, "baseAnger");
            set => ReflectionUtil.SetField(Raw, "baseAnger", value);
        }

        /// <summary>
        /// Gets or sets the base movement speed for Baldi.
        /// </summary>
        /// <returns>The base speed as a float.</returns>
        public float BaseSpeed
        {
            get => ReflectionUtil.GetField<float>(Raw, "baseSpeed");
            set => ReflectionUtil.SetField(Raw, "baseSpeed", value);
        }

        /// <summary>
        /// Gets or sets the multiplier applied to Baldi's base speed.
        /// </summary>
        /// <returns>The speed multiplier as a float.</returns>
        public float SpeedMultiplier
        {
            get => ReflectionUtil.GetField<float>(Raw, "speedMultiplier");
            set => ReflectionUtil.SetField(Raw, "speedMultiplier", value);
        }

        /// <summary>
        /// Gets or sets the scale applied to speed when Baldi slaps.
        /// </summary>
        /// <returns>The slap speed scale as a float.</returns>
        public float SlapSpeedScale
        {
            get => ReflectionUtil.GetField<float>(Raw, "slapSpeedScale");
            set => ReflectionUtil.SetField(Raw, "slapSpeedScale", value);
        }

        /// <summary>
        /// Gets or sets the extra anger drain applied to Baldi.
        /// </summary>
        /// <returns>The extra anger drain as a float.</returns>
        public float ExtraAngerDrain
        {
            get => ReflectionUtil.GetField<float>(Raw, "extraAngerDrain");
            set => ReflectionUtil.SetField(Raw, "extraAngerDrain", value);
        }

        /// <summary>
        /// Gets or sets the total distance Baldi has traveled.
        /// </summary>
        /// <returns>The total distance as a float.</returns>
        public float TotalDistance
        {
            get => ReflectionUtil.GetField<float>(Raw, "totalDistance");
            set => ReflectionUtil.SetField(Raw, "totalDistance", value);
        }

        /// <summary>
        /// Gets or sets the timer related to Baldi's apple interactions.
        /// </summary>
        /// <returns>The apple time as a float.</returns>
        public float AppleTime
        {
            get => ReflectionUtil.GetField<float>(Raw, "appleTime");
            set => ReflectionUtil.SetField(Raw, "appleTime", value);
        }

        /// <summary>
        /// Gets or sets the current anger level for Baldi.
        /// Setting this value invokes the underlying SetAnger method.
        /// </summary>
        /// <returns>The current anger as a float.</returns>
        public float Anger
        {
            get => ReflectionUtil.GetField<float>(Raw, "anger");
            set => ReflectionUtil.Call(Raw, "SetAnger", value);
        }

        /// <summary>
        /// Gets or sets the extra anger applied to Baldi.
        /// </summary>
        /// <returns>The extra anger as a float.</returns>
        public float ExtraAnger
        {
            get => ReflectionUtil.GetField<float>(Raw, "extraAnger");
            set => ReflectionUtil.SetField(Raw, "extraAnger", value);
        }

        /// <summary>
        /// Gets or sets the current slap distance threshold for Baldi.
        /// </summary>
        /// <returns>The slap distance as a float.</returns>
        public float SlapDistance
        {
            get => ReflectionUtil.GetField<float>(Raw, "slapDistance");
            set => ReflectionUtil.SetField(Raw, "slapDistance", value);
        }

        /// <summary>
        /// Gets or sets the next slap distance threshold for Baldi.
        /// </summary>
        /// <returns>The next slap distance as a float.</returns>
        public float NextSlapDistance
        {
            get => ReflectionUtil.GetField<float>(Raw, "nextSlapDistance");
            set => ReflectionUtil.SetField(Raw, "nextSlapDistance", value);
        }

        /// <summary>
        /// Gets or sets the pause time for Baldi's behavior.
        /// </summary>
        /// <returns>The pause time as a float.</returns>
        public float PauseTime
        {
            get => ReflectionUtil.GetField<float>(Raw, "pauseTime");
            set => ReflectionUtil.SetField(Raw, "pauseTime", value);
        }

        /// <summary>
        /// Gets the current delay value for Baldi.
        /// </summary>
        /// <returns>The delay as a float.</returns>
        public float Delay =>
            ReflectionUtil.GetProperty<float>(Raw, "Delay");

        /// <summary>
        /// Gets the portion of time Baldi spends moving.
        /// </summary>
        /// <returns>The movement portion as a float.</returns>
        public float MovementPortion =>
            ReflectionUtil.GetProperty<float>(Raw, "MovementPortion");

        /// <summary>
        /// Gets the current effective speed for Baldi.
        /// </summary>
        /// <returns>The speed as a float.</returns>
        public float Speed =>
            ReflectionUtil.GetProperty<float>(Raw, "Speed");

        /// <summary>
        /// Gets or sets whether Baldi uses smooth movement.
        /// </summary>
        /// <returns>True if smooth movement is enabled; otherwise false.</returns>
        public bool SmoothMove
        {
            get => ReflectionUtil.GetField<bool>(Raw, "smoothMove");
            set => ReflectionUtil.SetField(Raw, "smoothMove", value);
        }

        /// <summary>
        /// Gets or sets whether Baldi is in tutorial mode.
        /// </summary>
        /// <returns>True if tutorial mode is active; otherwise false.</returns>
        public bool TutorialMode
        {
            get => ReflectionUtil.GetField<bool>(Raw, "tutorialMode");
            set => ReflectionUtil.SetField(Raw, "tutorialMode", value);
        }

        /// <summary>
        /// Gets the current destination interaction object Baldi is targeting.
        /// </summary>
        /// <returns>The destination interaction as an object, or null.</returns>
        public object? CurrentDestinationInteraction =>
            ReflectionUtil.GetProperty<object>(Raw, "CurrentDestinationInteraction");

        /// <summary>
        /// Sets Baldi's anger to the specified value by invoking the underlying method.
        /// </summary>
        /// <param name="value">The anger value to set.</param>
        public void SetAnger(float value) =>
            ReflectionUtil.Call(Raw, "SetAnger", value);

        /// <summary>
        /// Triggers Baldi to get angry by invoking the underlying GetAngry method.
        /// </summary>
        /// <param name="value">The amount of anger to apply.</param>
        public void GetAngry(float value) =>
            ReflectionUtil.Call(Raw, "GetAngry", value);

        /// <summary>
        /// Adds extra anger to Baldi by invoking the underlying GetExtraAnger method.
        /// </summary>
        /// <param name="value">The extra anger amount to add.</param>
        public void AddExtraAnger(float value) =>
            ReflectionUtil.Call(Raw, "GetExtraAnger", value);

        /// <summary>
        /// Invokes the underlying BreakRuler behavior for Baldi.
        /// </summary>
        public void BreakRuler() =>
            ReflectionUtil.Call(Raw, "BreakRuler");

        /// <summary>
        /// Invokes the underlying RestoreRuler behavior for Baldi.
        /// </summary>
        public void RestoreRuler() =>
            ReflectionUtil.Call(Raw, "RestoreRuler");

        /// <summary>
        /// Invokes Baldi's Slap action.
        /// </summary>
        public void Slap() =>
            ReflectionUtil.Call(Raw, "Slap");

        /// <summary>
        /// Invokes Baldi's normal slap action.
        /// </summary>
        public void SlapNormal() =>
            ReflectionUtil.Call(Raw, "SlapNormal");

        /// <summary>
        /// Invokes Baldi's broken slap action.
        /// </summary>
        public void SlapBroken() =>
            ReflectionUtil.Call(Raw, "SlapBroken");

        /// <summary>
        /// Invokes Baldi's slap break action.
        /// </summary>
        public void SlapBreak() =>
            ReflectionUtil.Call(Raw, "SlapBreak");

        /// <summary>
        /// Invokes Baldi's slap rumble action.
        /// </summary>
        public void SlapRumble() =>
            ReflectionUtil.Call(Raw, "SlapRumble");

        /// <summary>
        /// Ends Baldi's slap action by invoking the underlying EndSlap method.
        /// </summary>
        public void EndSlap() =>
            ReflectionUtil.Call(Raw, "EndSlap");

        /// <summary>
        /// Resets the slap distance threshold for Baldi.
        /// </summary>
        public void ResetSlapDistance() =>
            ReflectionUtil.Call(Raw, "ResetSlapDistance");

        /// <summary>
        /// Updates the slap distance calculation for Baldi.
        /// </summary>
        public void UpdateSlapDistance() =>
            ReflectionUtil.Call(Raw, "UpdateSlapDistance");

        /// <summary>
        /// Causes Baldi to become distracted by invoking the underlying Distract method.
        /// </summary>
        public void Distract() =>
            ReflectionUtil.Call(Raw, "Distract");

        /// <summary>
        /// Clears tracked sound locations for Baldi.
        /// </summary>
        public void ClearSoundLocations() =>
            ReflectionUtil.Call(Raw, "ClearSoundLocations");

        /// <summary>
        /// Clears the current destination interaction for Baldi.
        /// </summary>
        public void ClearDestinationInteraction() =>
            ReflectionUtil.Call(Raw, "ClearDestinationInteraction");

        /// <summary>
        /// Updates Baldi's sound target by invoking the underlying UpdateSoundTarget method.
        /// </summary>
        public void UpdateSoundTarget() =>
            ReflectionUtil.Call(Raw, "UpdateSoundTarget");

        /// <summary>
        /// Triggers Baldi's eat sound behavior.
        /// </summary>
        public void EatSound() =>
            ReflectionUtil.Call(Raw, "EatSound");

        /// <summary>
        /// Praises the player for a duration and optionally rewards a sticker.
        /// </summary>
        /// <param name="time">The duration of the praise.</param>
        /// <param name="rewardSticker">If true, reward a sticker.</param>
        public void Praise(float time, bool rewardSticker = false) =>
            ReflectionUtil.Call(Raw, "Praise", time, rewardSticker);

        /// <summary>
        /// Plays Baldi's praise animation.
        /// </summary>
        public void PraiseAnimation() =>
            ReflectionUtil.Call(Raw, "PraiseAnimation");

        /// <summary>
        /// Resets Baldi's sprite to its default state.
        /// </summary>
        public void ResetSprite() =>
            ReflectionUtil.Call(Raw, "ResetSprite");

        /// <summary>
        /// Causes Baldi to take an apple.
        /// </summary>
        public void TakeApple() =>
            ReflectionUtil.Call(Raw, "TakeApple");

        /// <summary>
        /// Resumes Baldi's apple-related behavior.
        /// </summary>
        public void ResumeApple() =>
            ReflectionUtil.Call(Raw, "ResumeApple");

        /// <summary>
        /// Invokes Baldi's tutorial catch behavior.
        /// </summary>
        public void TutorialCatch() =>
            ReflectionUtil.Call(Raw, "TutorialCatch");

        /// <summary>
        /// Notifies Baldi that a player has been caught.
        /// </summary>
        /// <param name="player">Reference to the player that was caught.</param>
        public void CaughtPlayer(BBPPlayerRef player)
        {
            if (player.Raw != null)
                ReflectionUtil.Call(Raw, "CaughtPlayer", player.Raw);
        }

        /// <summary>
        /// Informs Baldi of a sound source at a given position.
        /// </summary>
        /// <param name="source">The GameObject source of the sound (may be null).</param>
        /// <param name="position">The position where the sound originated.</param>
        /// <param name="value">An integer value associated with the sound.</param>
        /// <param name="indicator">Whether to show an indicator for the sound.</param>
        public void Hear(GameObject? source, Vector3 position, int value, bool indicator = true) {
            if (source == null) return;
            ReflectionUtil.Call(Raw, "Hear", source!, position, value, indicator);
        }
            
    }
}