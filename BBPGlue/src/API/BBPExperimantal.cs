namespace BBPGlue.API
{
    /// <summary>
    /// Contains experimental API features that may change or be removed in future versions.
    /// </summary>
    public sealed class BBPExperimental
    {
        /// <summary>
        /// Gets the experimental player features.
        /// </summary>
        public ExperimentalPlayer Player { get; } = new ExperimentalPlayer();

        /// <summary>
        /// Provides experimental player-related options.
        /// </summary>
        public sealed class ExperimentalPlayer
        {
            /// <summary>
            /// Gets or sets whether vertical camera rotation is unrestricted.
            /// </summary>
            /// <remarks>
            /// When enabled, the player can rotate the camera fully.
            /// </remarks>
            public bool PitchUnlocked { get; set; } = false;
        }
    }
}