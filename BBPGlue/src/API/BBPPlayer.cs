using UnityEngine;
using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// High-level access to the current player instance and player-related settings.
    /// </summary>
    public sealed class BBPPlayer
    {
        /// <summary>
        /// The raw Unity Component representing the player, if available.
        /// </summary>
        public Component? Raw => GameContext.Player;
        /// <summary>
        /// Whether a player component currently exists in the game context.
        /// </summary>
        public bool Exists => Raw != null;

        /// <summary>
        /// The Unity GameObject for the player, if present.
        /// </summary>
        public GameObject? GameObject => Raw?.gameObject;
        /// <summary>
        /// The Unity Transform of the player, if present.
        /// </summary>
        public Transform? Transform => Raw?.transform;

        /// <summary>
        /// World position of the player. Setting moves the player's transform.
        /// </summary>
        public Vector3 Position
        {
            get => Transform != null ? Transform.position : Vector3.zero;
            set => Transform?.SetPositionAndRotation(value, Transform.rotation);
        }

        /// <summary>
        /// The player's rotation. Setting updates the transform's rotation.
        /// </summary>
        public Quaternion Rotation
        {
            get => Transform != null ? Transform.rotation : Quaternion.identity;
            set
            {
                if (Transform != null)
                    Transform.rotation = value;
            }
        }

        /// <summary>
        /// The numeric player index used by the game.
        /// </summary>
        public int PlayerNumber
        {
            get => ReflectionUtil.GetField<int>(Raw, "playerNumber");
            set => ReflectionUtil.SetField(Raw, "playerNumber", value);
        }

        /// <summary>
        /// Text describing the player's last rule break.
        /// </summary>
        public string RuleBreakText
        {
            get => ReflectionUtil.GetField<string>(Raw, "ruleBreak") ?? "";
            set => ReflectionUtil.SetField(Raw, "ruleBreak", value);
        }

        /// <summary>
        /// Whether the player is currently disobeying rules.
        /// </summary>
        public bool Disobeying => ReflectionUtil.GetProperty<bool>(Raw, "Disobeying");
        /// <summary>
        /// The player's guilt sensitivity multiplier.
        /// </summary>
        public float GuiltSensitivity => ReflectionUtil.GetProperty<float>(Raw, "GuiltySensitivity");

        /// <summary>
        /// Gets or sets whether the player is invincible.
        /// </summary>
        public bool Invincible
        {
            get => ReflectionUtil.GetField<bool>(Raw, "invincible");
            set => ReflectionUtil.SetField(Raw, "invincible", value);
        }

        /// <summary>
        /// Whether the player is currently tagged.
        /// </summary>
        public bool Tagged => ReflectionUtil.GetProperty<bool>(Raw, "Tagged");

        /// <summary>
        /// Whether the player is reversed (game-specific meaning).
        /// </summary>
        public bool Reversed
        {
            get => ReflectionUtil.GetField<bool>(Raw, "reversed");
            set => ReflectionUtil.SetField(Raw, "reversed", value);
        }

        /// <summary>
        /// Whether the player is invisible.
        /// </summary>
        public bool Invisible => ReflectionUtil.GetProperty<bool>(Raw, "Invisible");

        /// <summary>
        /// The player's time scale modifier.
        /// </summary>
        public float PlayerTimeScale => ReflectionUtil.GetProperty<float>(Raw, "PlayerTimeScale");
        /// <summary>
        /// Maximum light level below which the player can hide.
        /// </summary>
        public float MaxHideableLightLevel => ReflectionUtil.GetProperty<float>(Raw, "MaxHideableLightLevel");

        /// <summary>
        /// Low-level controller managing environment interactions for the player.
        /// </summary>
        public object? EnvironmentController => ReflectionUtil.GetField<object>(Raw, "ec");
        /// <summary>
        /// Internal item manager reference.
        /// </summary>
        public object? ItemManager => ReflectionUtil.GetField<object>(Raw, "itm");
        /// <summary>
        /// Low-level player movement controller object.
        /// </summary>
        public object? PlayerMovement => ReflectionUtil.GetField<object>(Raw, "plm");
        /// <summary>
        /// Player click/input helper object.
        /// </summary>
        public object? PlayerClick => ReflectionUtil.GetField<object>(Raw, "pc");
        /// <summary>
        /// Base transform used for the player camera.
        /// </summary>
        public Transform? CameraBase => ReflectionUtil.GetField<Transform>(Raw, "cameraBase");

        /// <summary>
        /// Shortcut reference to the movement controller (same as PlayerMovement).
        /// </summary>
        public object? Movement => ReflectionUtil.GetField<object>(Raw, "plm");

        /// <summary>
        /// Movement walk speed value.
        /// </summary>
        public float WalkSpeed
        {
            get => ReflectionUtil.GetField<float>(Movement, "walkSpeed");
            set => ReflectionUtil.SetField(Movement, "walkSpeed", value);
        }

        /// <summary>
        /// Movement run speed value.
        /// </summary>
        public float RunSpeed
        {
            get => ReflectionUtil.GetField<float>(Movement, "runSpeed");
            set => ReflectionUtil.SetField(Movement, "runSpeed", value);
        }

        /// <summary>
        /// Current stamina value.
        /// </summary>
        public float Stamina
        {
            get => ReflectionUtil.GetField<float>(Movement, "stamina");
            set => ReflectionUtil.SetField(Movement, "stamina", value);
        }

        /// <summary>
        /// Maximum stamina available to the player.
        /// </summary>
        public float StaminaMax
        {
            get => ReflectionUtil.GetProperty<float>(Movement, "StaminaMax");
        }

        /// <summary>
        /// Rate at which stamina drops while running.
        /// </summary>
        public float StaminaDrop
        {
            get => ReflectionUtil.GetField<float>(Movement, "staminaDrop");
            set => ReflectionUtil.SetField(Movement, "staminaDrop", value);
        }

        /// <summary>
        /// Rate at which stamina recovers.
        /// </summary>
        public float StaminaRise
        {
            get => ReflectionUtil.GetField<float>(Movement, "staminaRise");
            set => ReflectionUtil.SetField(Movement, "staminaRise", value);
        }

        /// <summary>
        /// The player's collider height used by movement logic.
        /// </summary>
        public float Height
        {
            get => ReflectionUtil.GetField<float>(Movement, "height");
            set => ReflectionUtil.SetField(Movement, "height", value);
        }

        /// <summary>
        /// Current effective interaction reach distance.
        /// </summary>
        public float Reach
        {
            get
            {
                object? pc = ReflectionUtil.GetField<object>(Raw, "pc");
                return ReflectionUtil.GetProperty<float>(pc, "Reach");
            }
        }

        /// <summary>
        /// The base reach value before modifiers.
        /// </summary>
        public float BaseReach
        {
            get
            {
                object? pc = ReflectionUtil.GetField<object>(Raw, "pc");
                return ReflectionUtil.GetField<float>(pc, "reach");
            }
            set
            {
                object? pc = ReflectionUtil.GetField<object>(Raw, "pc");
                ReflectionUtil.SetField(pc, "reach", value);
            }
        }

        /// <summary>
        /// Whether the player currently sees a clickable object.
        /// </summary>
        public bool SeesClickable
        {
            get
            {
                object? pc = ReflectionUtil.GetField<object>(Raw, "pc");
                return ReflectionUtil.GetField<bool>(pc, "seesClickable");
            }
        }

        /// <summary>
        /// The GameObject that was clicked this frame, if any.
        /// </summary>
        public GameObject? ClickedThisFrame
        {
            get
            {
                object? pc = ReflectionUtil.GetField<object>(Raw, "pc");
                return ReflectionUtil.GetField<GameObject>(pc, "clickedThisFrame");
            }
        }

        /// <summary>
        /// The current clickable object under the player's cursor, if any.
        /// </summary>
        public object? CurrentClickable
        {
            get
            {
                object? pc = ReflectionUtil.GetField<object>(Raw, "pc");
                return ReflectionUtil.GetField<object>(pc, "currentClickable");
            }
        }

        /// <summary>
        /// Current real velocity reported by the movement controller.
        /// </summary>
        public float RealVelocity => ReflectionUtil.GetProperty<float>(Movement, "RealVelocity");
        /// <summary>
        /// Velocity recorded during the current frame.
        /// </summary>
        public float FrameVelocity => ReflectionUtil.GetField<float>(Movement, "frameVelocity");
        /// <summary>
        /// The entity wrapper corresponding to the movement controller.
        /// </summary>
        public BBPEntity MovementEntity => new BBPEntity(ReflectionUtil.GetProperty<object>(Movement, "Entity"));

        /// <summary>
        /// The player entity wrapper.
        /// </summary>
        public BBPEntity Entity
        {
            get
            {
                object? plm = PlayerMovement;
                object? rawEntity = ReflectionUtil.GetProperty<object>(plm, "Entity");
                return new BBPEntity(rawEntity);
            }
        }

        /// <summary>
        /// Refreshes internal cached references to the player.
        /// </summary>
        public void Refresh()
        {
            GameContext.Refresh();
        }

        /// <summary>
        /// Retrieves a player component by class name.
        /// </summary>
        /// <param name="className">The fully-qualified class name to look up.</param>
        /// <returns>The component instance or null.</returns>
        public Component? GetComponent(string className)
        {
            return GameContext.GetPlayerComponent(className);
        }

        /// <summary>
        /// Teleports the player to the specified position.
        /// </summary>
        /// <param name="position">Destination position.</param>
        public void Teleport(Vector3 position)
        {
            ReflectionUtil.Call(Raw, "Teleport", position);
        }

        /// <summary>
        /// Sets the player's hidden state.
        /// </summary>
        /// <param name="hidden">True to hide the player; false to show.</param>
        public void SetHidden(bool hidden)
        {
            ReflectionUtil.Call(Raw, "SetHidden", hidden);
        }

        /// <summary>
        /// Shows or hides the player's nametag.
        /// </summary>
        /// <param name="state">True to show the nametag; false to hide it.</param>
        public void SetNametag(bool state)
        {
            ReflectionUtil.Call(Raw, "SetNametag", state);
        }

        /// <summary>
        /// Reverses the player's movement or facing state (game-specific).
        /// </summary>
        public void Reverse()
        {
            ReflectionUtil.Call(Raw, "Reverse");
        }

        /// <summary>
        /// Triggers a rule break for the player with a linger time.
        /// </summary>
        /// <param name="rule">Identifier of the rule broken.</param>
        /// <param name="linger">How long the rule break lingers.</param>
        public void RuleBreak(string rule, float linger)
        {
            ReflectionUtil.Call(Raw, "RuleBreak", rule, linger);
        }

        /// <summary>
        /// Triggers a rule break with an explicit sensitivity parameter.
        /// </summary>
        /// <param name="rule">Identifier of the rule broken.</param>
        /// <param name="linger">How long the rule break lingers.</param>
        /// <param name="sensitivity">Guilt sensitivity override.</param>
        public void RuleBreak(string rule, float linger, float sensitivity)
        {
            ReflectionUtil.Call(Raw, "RuleBreak", rule, linger, sensitivity);
        }

        /// <summary>
        /// Clears the player's guilt state.
        /// </summary>
        public void ClearGuilt()
        {
            ReflectionUtil.Call(Raw, "ClearGuilt");
        }

        /// <summary>
        /// Adds a time scale modifier object to the player.
        /// </summary>
        /// <param name="timeScaleModifier">Modifier object understood by the game.</param>
        public void AddTimeScale(object timeScaleModifier)
        {
            ReflectionUtil.Call(Raw, "AddTimeScale", timeScaleModifier);
        }

        /// <summary>
        /// Removes a previously added time scale modifier.
        /// </summary>
        /// <param name="timeScaleModifier">Modifier object to remove.</param>
        public void RemoveTimeScale(object timeScaleModifier)
        {
            ReflectionUtil.Call(Raw, "RemoveTimeScale", timeScaleModifier);
        }

        /// <summary>
        /// Adds stamina to the player; limited controls whether addition respects caps.
        /// </summary>
        /// <param name="value">Amount of stamina to add.</param>
        /// <param name="limited">Whether addition should respect stamina caps.</param>
        public void AddStamina(float value, bool limited = true)
        {
            ReflectionUtil.Call(Movement, "AddStamina", value, limited);
        }
    }
}