using System;
using System.Collections.Generic;
using UnityEngine;

namespace BBPGlue.API
{
    /// <summary>
    /// Log level for BBPGlue console messages.
    /// </summary>
    public enum BBPConsoleLevel
    {
        Info,
        Warning,
        Error
    }

    /// <summary>
    /// Represents a single console message with timestamp and level.
    /// </summary>
    public readonly struct BBPConsoleMessage
    {
        public readonly DateTime Time;
        public readonly BBPConsoleLevel Level;
        public readonly string Text;

        public BBPConsoleMessage(BBPConsoleLevel level, string text)
        {
            Time = DateTime.Now;
            Level = level;
            Text = text;
        }
    }

    /// <summary>
    /// In-game console used for logging messages to a small overlay and capturing history.
    /// </summary>
    public static class BBPConsole
    {
        public static bool Enabled { get; set; } = true;

        public static int  MaxMessages { get; set; } = 512;

        private static readonly Queue<BBPConsoleMessage> Messages =
            new Queue<BBPConsoleMessage>(MaxMessages);

        private static bool _visible;
        private static Vector2 _scroll;

        private static Texture2D? _backgroundTexture;

        /// <summary>
        /// Read-only view of console message history.
        /// </summary>
        public static IReadOnlyCollection<BBPConsoleMessage> Entries => Messages;

        /// <summary>
        /// Update loop to handle console visibility hotkeys.
        /// </summary>
        public static void Update()
        {
            if (!Enabled) return;

            if (
                Input.GetKeyDown(KeyCode.F1) &&
                (Input.GetKey(KeyCode.LeftShift) ||
                Input.GetKey(KeyCode.RightShift))
            )
            {
                _visible = !_visible;
            }

            if (_visible)
            {
                if (
                    Input.GetKeyDown(KeyCode.C) &&
                    (Input.GetKey(KeyCode.LeftControl) ||
                    Input.GetKey(KeyCode.RightControl))
                )
                {
                    CopyAll();
                }
            }
        }

        /// <summary>
        /// Renders the console GUI when visible.
        /// </summary>
        public static void OnGUI()
        {
            if (!_visible || !Enabled)
                return;

            EnsureTextures();

            Rect rect = new Rect(
                20f,
                20f,
                Screen.width - 40f,
                Screen.height * 0.65f
            );

            GUI.DrawTexture(rect, _backgroundTexture);

            GUILayout.BeginArea(rect);
            GUILayout.Space(8f);

            GUILayout.Label("BBPGlue Console");

            _scroll = GUILayout.BeginScrollView(_scroll);

            foreach (BBPConsoleMessage msg in Messages)
            {
                GUILayout.Label(FormatMessage(msg));
            }

            GUILayout.EndScrollView();

            GUILayout.EndArea();
        }

        /// <summary>
        /// Logs an informational message to the BBPGlue console and Unity logger.
        /// </summary>
        /// <param name="message">Message text to log.</param>
        public static void Log(string message)
        {
            if (!Enabled) return;

            Push(BBPConsoleLevel.Info, message);
            BBP.Logger?.LogInfo(message);
            Debug.Log($"[BBPGlue] {message}");
        }

        /// <summary>
        /// Logs a warning message to the BBPGlue console and Unity logger.
        /// </summary>
        /// <param name="message">Warning message text.</param>
        public static void Warn(string message)
        {
            if (!Enabled) return;

            Push(BBPConsoleLevel.Warning, message);
            BBP.Logger?.LogWarning(message);
            Debug.LogWarning($"[BBPGlue] {message}");
        }

        /// <summary>
        /// Logs an error message to the BBPGlue console and Unity logger.
        /// </summary>
        /// <param name="message">Error message text.</param>
        public static void Error(string message)
        {
            if (!Enabled) return;

            Push(BBPConsoleLevel.Error, message);
            BBP.Logger?.LogError(message);
            Debug.LogError($"[BBPGlue] {message}");
        }

                /// <summary>
        /// Copies the entire console log to the system clipboard.
        /// </summary>
        public static void CopyAll()
        {
            GUIUtility.systemCopyBuffer = GetFullLogText();
            Log("Console copied to clipboard.");
        }

        /// <summary>
        /// Returns the full console contents as a single string.
        /// </summary>
        /// <returns>Concatenated log text.</returns>
        public static string GetFullLogText()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();

            foreach (BBPConsoleMessage msg in Messages)
                builder.AppendLine(FormatMessage(msg));

            return builder.ToString();
        }

        /// <summary>
        /// Clears the console history.
        /// </summary>
        public static void Clear()
        {
            Messages.Clear();
        }

        private static void Push(BBPConsoleLevel level, string message)
        {
            if (Messages.Count >= MaxMessages)
                Messages.Dequeue();

            Messages.Enqueue(new BBPConsoleMessage(level, message));

            _scroll.y = float.MaxValue;
        }

        private static string FormatMessage(BBPConsoleMessage msg)
        {
            return $"[{msg.Time:HH:mm:ss}] [{msg.Level}] {msg.Text}";
        }

        private static void EnsureTextures()
        {
            if (_backgroundTexture != null)
                return;

            _backgroundTexture = new Texture2D(1, 1);
            _backgroundTexture.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.72f));
            _backgroundTexture.Apply();
        }
    }
}