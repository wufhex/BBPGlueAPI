using BBPGlue.Core;

namespace BBPGlue.API
{
    /// <summary>
    /// Controls BB+ seed selection and generation.
    /// </summary>
    public sealed class BBPSeed
    {
        /// <summary>
        /// Gets whether a seed override is currently enabled.
        /// </summary>
        public bool OverrideEnabled { get; private set; }
        /// <summary>
        /// Gets the configured override seed value.
        /// </summary>
        public int OverrideSeed { get; private set; }

        /// <summary>
        /// Gets the current active seed from the game.
        /// </summary>
        public int CurrentSeed => BBP.Game.Seed;

        /// <summary>
        /// Enables a deterministic seed override with the provided seed.
        /// </summary>
        /// <param name="seed">The seed value to use as an override.</param>
        public void SetOverride(int seed)
        {
            OverrideSeed = seed;
            OverrideEnabled = true;
        }

        /// <summary>
        /// Disables any previously set seed override.
        /// </summary>
        public void ClearOverride()
        {
            OverrideEnabled = false;
        }

        internal bool TryApplyOverride(object coreGameManager)
        {
            if (!OverrideEnabled)
                return false;

            return ReflectionUtil.SetField(coreGameManager, "seed", OverrideSeed);;
        }
    }
}