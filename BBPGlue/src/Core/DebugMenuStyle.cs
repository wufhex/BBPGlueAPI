using UnityEngine;

namespace BBPGlue.Core
{
    public static class DebugMenuStyle
    {
        public static GUIStyle WindowStyle = null!;
        public static GUIStyle HeaderLabel = null!;
        public static GUIStyle SubHeaderLabel = null!;
        public static GUIStyle SmallLabel = null!;
        public static GUIStyle WarningLabel = null!;

        public static GUIStyle CategoryButton = null!;
        public static GUIStyle ModuleButton = null!;

        public static GUIStyle Box = null!;
        public static GUIStyle InnerBox = null!;

        public static GUIStyle ToggleStyle = null!;
        public static GUIStyle TextFieldStyle = null!;

        public static GUIStyle SliderStyle = null!;
        public static GUIStyle SliderThumbStyle = null!;

        public static GUIStyle ScrollViewStyle = null!;

        public static GUIStyle VerticalScrollbar = null!;
        public static GUIStyle VerticalScrollbarThumb = null!;

        public static GUIStyle HorizontalScrollbar = null!;
        public static GUIStyle HorizontalScrollbarThumb = null!;
        public static GUIStyle HiddenHorizontalScrollbar = null!;

        public static GUIStyle ResizeHandleStyle = null!;

        private static GUIStyle _scrollbarButtonStyle = null!;

        private static Texture2D? _windowBg;
        private static Texture2D? _boxBg;
        private static Texture2D? _innerBoxBg;

        private static Texture2D? _categoryBg;
        private static Texture2D? _categoryHoverBg;

        private static Texture2D? _moduleBg;
        private static Texture2D? _moduleHoverBg;

        private static Texture2D? _toggleOffBg;
        private static Texture2D? _toggleOffHoverBg;
        private static Texture2D? _toggleOnBg;
        private static Texture2D? _toggleOnHoverBg;

        private static Texture2D? _inputBg;

        private static Texture2D? _sliderBg;
        private static Texture2D? _sliderThumbBg;

        private static Texture2D? _scrollbarBg;
        private static Texture2D? _scrollbarThumbBg;

        public static bool Initialized { get; private set; }

        public static void Initialize()
        {
            if (Initialized)
                return;

            CreateTextures();
            CreateStyles();

            Initialized = true;
        }

        public static void ApplyRuntimeSkin()
        {
            Initialize();

            GUI.skin.window = WindowStyle;
            GUI.skin.label = SmallLabel;
            GUI.skin.box = Box;
            GUI.skin.button = ModuleButton;

            GUI.skin.textField = TextFieldStyle;
            GUI.skin.toggle = ToggleStyle;

            GUI.skin.horizontalSlider = SliderStyle;
            GUI.skin.horizontalSliderThumb = SliderThumbStyle;

            GUI.skin.scrollView = ScrollViewStyle;

            GUI.skin.verticalScrollbar = VerticalScrollbar;
            GUI.skin.verticalScrollbarThumb = VerticalScrollbarThumb;
            GUI.skin.verticalScrollbarUpButton = _scrollbarButtonStyle;
            GUI.skin.verticalScrollbarDownButton = _scrollbarButtonStyle;

            GUI.skin.horizontalScrollbar = HiddenHorizontalScrollbar;
            GUI.skin.horizontalScrollbarThumb = HiddenHorizontalScrollbar;
            GUI.skin.horizontalScrollbarLeftButton = _scrollbarButtonStyle;
            GUI.skin.horizontalScrollbarRightButton = _scrollbarButtonStyle;
        }

        public static void Header(string text)
        {
            GUILayout.Space(8f);
            GUILayout.Label(text, HeaderLabel);
            GUILayout.Space(4f);
        }

        public static void Label(string text)
        {
            GUILayout.Label(text, SmallLabel);
        }

        public static void Label(string text, params GUILayoutOption[] options)
        {
            GUILayout.Label(text, SmallLabel, options);
        }

        public static void Warning(string text)
        {
            GUILayout.Label(text, WarningLabel);
        }

        public static bool Button(string text)
        {
            return GUILayout.Button(text, ModuleButton);
        }

        public static bool Button(string text, params GUILayoutOption[] options)
        {
            return GUILayout.Button(text, ModuleButton, options);
        }

        public static bool Tab(bool selected, string text, params GUILayoutOption[] options)
        {
            return GUILayout.Toggle(
                selected,
                text,
                selected ? CategoryButton : ModuleButton,
                options
            );
        }

        public static string TextField(string value)
        {
            return GUILayout.TextField(value, TextFieldStyle);
        }

        public static string TextField(string value, params GUILayoutOption[] options)
        {
            return GUILayout.TextField(value, TextFieldStyle, options);
        }

        public static bool Toggle(bool value, string label, params GUILayoutOption[] options)
        {
            string prefix = value ? "✓  " : "   ";
            return GUILayout.Toggle(value, prefix + label, ToggleStyle, options);
        }

        public static Vector2 ScrollViewBegin(Vector2 scroll)
        {
            return GUILayout.BeginScrollView(
                scroll,
                false,
                true,
                HiddenHorizontalScrollbar,
                VerticalScrollbar,
                ScrollViewStyle
            );
        }

        public static void ScrollViewEnd()
        {
            GUILayout.EndScrollView();
        }

        public static void BoxBegin()
        {
            GUILayout.BeginVertical(Box);
        }

        public static void BoxEnd()
        {
            GUILayout.EndVertical();
        }

        public static void RowBegin()
        {
            GUILayout.BeginHorizontal();
        }

        public static void RowEnd()
        {
            GUILayout.EndHorizontal();
        }

        private static void CreateTextures()
        {
            _windowBg = MakeRoundedTex(64, 64, 12, new Color(0.055f, 0.060f, 0.075f, 0.98f));
            _boxBg = MakeRoundedTex(64, 64, 9, new Color(0.085f, 0.092f, 0.115f, 1f));
            _innerBoxBg = MakeRoundedTex(64, 64, 8, new Color(0.070f, 0.076f, 0.095f, 1f));

            _categoryBg = MakeRoundedTex(64, 64, 8, new Color(0.120f, 0.135f, 0.170f, 1f));
            _categoryHoverBg = MakeRoundedTex(64, 64, 8, new Color(0.170f, 0.195f, 0.250f, 1f));

            _moduleBg = MakeRoundedTex(64, 64, 7, new Color(0.105f, 0.115f, 0.145f, 1f));
            _moduleHoverBg = MakeRoundedTex(64, 64, 7, new Color(0.150f, 0.170f, 0.220f, 1f));

            _toggleOffBg = MakeRoundedTex(64, 64, 7, new Color(0.105f, 0.115f, 0.145f, 1f));
            _toggleOffHoverBg = MakeRoundedTex(64, 64, 7, new Color(0.150f, 0.165f, 0.210f, 1f));
            _toggleOnBg = MakeRoundedTex(64, 64, 7, new Color(0.190f, 0.330f, 0.460f, 1f));
            _toggleOnHoverBg = MakeRoundedTex(64, 64, 7, new Color(0.240f, 0.420f, 0.580f, 1f));

            _inputBg = MakeRoundedTex(64, 64, 6, new Color(0.14f, 0.16f, 0.21f, 1f));

            _sliderBg = MakeRoundedTex(64, 12, 6, new Color(0.115f, 0.125f, 0.155f, 1f));
            _sliderThumbBg = MakeRoundedTex(18, 18, 9, new Color(0.520f, 0.700f, 0.950f, 1f));

            _scrollbarBg = MakeRoundedTex(8, 64, 4, new Color(0.045f, 0.050f, 0.065f, 1f));
            _scrollbarThumbBg = MakeRoundedTex(8, 64, 4, new Color(0.260f, 0.300f, 0.380f, 1f));
        }

        private static void CreateStyles()
        {
            WindowStyle = new GUIStyle(GUI.skin.window)
            {
                padding = new RectOffset(14, 14, 34, 14),
                border = new RectOffset(12, 12, 12, 12),
                alignment = TextAnchor.UpperCenter
            };
            ApplyAllStates(WindowStyle, _windowBg, Color.white);

            HeaderLabel = new GUIStyle(GUI.skin.label)
            {
                fontSize = 13,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                wordWrap = false
            };
            ApplyAllStates(HeaderLabel, null, new Color(0.880f, 0.920f, 1f, 1f));

            SubHeaderLabel = new GUIStyle(GUI.skin.label)
            {
                fontSize = 11,
                alignment = TextAnchor.MiddleCenter,
                wordWrap = false
            };
            ApplyAllStates(SubHeaderLabel, null, new Color(0.550f, 0.610f, 0.720f, 1f));

            SmallLabel = new GUIStyle(GUI.skin.label)
            {
                fontSize = 11,
                alignment = TextAnchor.MiddleLeft,
                wordWrap = true
            };
            ApplyAllStates(SmallLabel, null, new Color(0.760f, 0.800f, 0.880f, 1f));

            WarningLabel = new GUIStyle(SmallLabel)
            {
                fontStyle = FontStyle.Bold
            };
            ApplyAllStates(WarningLabel, null, new Color(1f, 0.58f, 0.45f, 1f));

            CategoryButton = CreateButtonStyle(
                13,
                _categoryBg,
                _categoryHoverBg,
                new Color(0.920f, 0.950f, 1f, 1f)
            );

            CategoryButton.fontStyle = FontStyle.Bold;

            ModuleButton = CreateButtonStyle(
                12,
                _moduleBg,
                _moduleHoverBg,
                new Color(0.880f, 0.910f, 1f, 1f)
            );

            Box = new GUIStyle(GUI.skin.box)
            {
                padding = new RectOffset(9, 9, 8, 8),
                margin = new RectOffset(5, 5, 4, 8),
                border = new RectOffset(10, 10, 10, 10)
            };
            ApplyAllStates(Box, _boxBg, Color.white);

            InnerBox = new GUIStyle(GUI.skin.box)
            {
                padding = new RectOffset(10, 10, 8, 8),
                margin = new RectOffset(10, 6, 4, 8),
                border = new RectOffset(9, 9, 9, 9)
            };
            ApplyAllStates(InnerBox, _innerBoxBg, Color.white);

            TextFieldStyle = new GUIStyle(GUI.skin.textField)
            {
                fontSize = 12,
                padding = new RectOffset(8, 8, 5, 5),
                margin = new RectOffset(4, 4, 4, 4),
                border = new RectOffset(7, 7, 7, 7)
            };
            ApplyAllStates(TextFieldStyle, _inputBg, new Color(0.900f, 0.930f, 1f, 1f));

            ToggleStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleLeft,
                padding = new RectOffset(12, 12, 7, 7),
                margin = new RectOffset(4, 4, 4, 4),
                border = new RectOffset(8, 8, 8, 8)
            };

            ApplyAllStates(ToggleStyle, _toggleOffBg, new Color(0.860f, 0.900f, 1f, 1f));

            ToggleStyle.hover.background = _toggleOffHoverBg;
            ToggleStyle.active.background = _toggleOffHoverBg;
            ToggleStyle.focused.background = _toggleOffHoverBg;

            ToggleStyle.onNormal.background = _toggleOnBg;
            ToggleStyle.onHover.background = _toggleOnHoverBg;
            ToggleStyle.onActive.background = _toggleOnHoverBg;
            ToggleStyle.onFocused.background = _toggleOnHoverBg;

            ToggleStyle.hover.textColor = Color.white;
            ToggleStyle.active.textColor = Color.white;
            ToggleStyle.onNormal.textColor = Color.white;
            ToggleStyle.onHover.textColor = Color.white;
            ToggleStyle.onActive.textColor = Color.white;
            ToggleStyle.onFocused.textColor = Color.white;

            SliderStyle = new GUIStyle(GUI.skin.horizontalSlider)
            {
                fixedHeight = 18f,
                margin = new RectOffset(6, 6, 8, 8),
                border = new RectOffset(6, 6, 6, 6)
            };
            ApplyAllStates(SliderStyle, _sliderBg, Color.white);

            SliderThumbStyle = new GUIStyle(GUI.skin.horizontalSliderThumb)
            {
                fixedWidth = 18f,
                fixedHeight = 18f,
                border = new RectOffset(9, 9, 9, 9)
            };
            ApplyAllStates(SliderThumbStyle, _sliderThumbBg, Color.white);

            ScrollViewStyle = new GUIStyle(GUI.skin.scrollView)
            {
                padding = new RectOffset(0, 2, 0, 0),
                margin = new RectOffset(0, 0, 0, 0)
            };
            ApplyAllStates(ScrollViewStyle, null, Color.white);

            VerticalScrollbar = new GUIStyle(GUI.skin.verticalScrollbar)
            {
                fixedWidth = 8f,
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(4, 0, 0, 0),
                border = new RectOffset(4, 4, 4, 4)
            };
            ApplyAllStates(VerticalScrollbar, _scrollbarBg, Color.white);

            VerticalScrollbarThumb = new GUIStyle(GUI.skin.verticalScrollbarThumb)
            {
                fixedWidth = 8f,
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 0, 0),
                border = new RectOffset(4, 4, 4, 4)
            };
            ApplyAllStates(VerticalScrollbarThumb, _scrollbarThumbBg, Color.white);

            HorizontalScrollbar = new GUIStyle(GUI.skin.horizontalScrollbar)
            {
                fixedHeight = 8f,
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 4, 0),
                border = new RectOffset(4, 4, 4, 4)
            };
            ApplyAllStates(HorizontalScrollbar, _scrollbarBg, Color.white);

            HorizontalScrollbarThumb = new GUIStyle(GUI.skin.horizontalScrollbarThumb)
            {
                fixedHeight = 8f,
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 0, 0),
                border = new RectOffset(4, 4, 4, 4)
            };
            ApplyAllStates(HorizontalScrollbarThumb, _scrollbarThumbBg, Color.white);

            HiddenHorizontalScrollbar = new GUIStyle(GUI.skin.horizontalScrollbar)
            {
                fixedHeight = 0f,
                stretchHeight = false,
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 0, 0),
                border = new RectOffset(0, 0, 0, 0)
            };
            ApplyAllStates(HiddenHorizontalScrollbar, null, Color.clear);

            _scrollbarButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fixedWidth = 0f,
                fixedHeight = 0f,
                stretchWidth = false,
                stretchHeight = false,
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 0, 0),
                border = new RectOffset(0, 0, 0, 0)
            };
            ApplyAllStates(_scrollbarButtonStyle, null, Color.clear);

            ResizeHandleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 15,
                alignment = TextAnchor.MiddleCenter,
                normal =
                {
                    textColor = new Color(0.55f, 0.62f, 0.75f, 0.95f)
                }
            };
            ApplyAllStates(ResizeHandleStyle, null, new Color(0.55f, 0.62f, 0.75f, 0.95f));
        }

        private static GUIStyle CreateButtonStyle(
            int fontSize,
            Texture2D? normalBg,
            Texture2D? hoverBg,
            Color textColor
        )
        {
            GUIStyle style = new GUIStyle(GUI.skin.button)
            {
                fontSize = fontSize,
                alignment = TextAnchor.MiddleLeft,
                padding = new RectOffset(12, 12, 7, 7),
                margin = new RectOffset(4, 4, 4, 4),
                border = new RectOffset(8, 8, 8, 8),
                wordWrap = false
            };

            ApplyAllStates(style, normalBg, textColor);

            style.hover.background = hoverBg;
            style.active.background = hoverBg;
            style.focused.background = hoverBg;

            style.onHover.background = hoverBg;
            style.onActive.background = hoverBg;
            style.onFocused.background = hoverBg;

            style.hover.textColor = Color.white;
            style.active.textColor = Color.white;
            style.focused.textColor = Color.white;

            style.onHover.textColor = Color.white;
            style.onActive.textColor = Color.white;
            style.onFocused.textColor = Color.white;

            return style;
        }

        public static bool Toggle(bool value, string label)
        {
            string prefix = value ? "✓  " : "   ";
            return GUILayout.Toggle(value, prefix + label, ToggleStyle);
        }

        public static float Slider(float value, float min, float max)
        {
            return GUILayout.HorizontalSlider(value, min, max, SliderStyle, SliderThumbStyle);
        }

        private static void ApplyAllStates(GUIStyle style, Texture2D? background, Color textColor)
        {
            style.normal.background = background;
            style.hover.background = background;
            style.active.background = background;
            style.focused.background = background;

            style.onNormal.background = background;
            style.onHover.background = background;
            style.onActive.background = background;
            style.onFocused.background = background;

            style.normal.textColor = textColor;
            style.hover.textColor = textColor;
            style.active.textColor = textColor;
            style.focused.textColor = textColor;

            style.onNormal.textColor = textColor;
            style.onHover.textColor = textColor;
            style.onActive.textColor = textColor;
            style.onFocused.textColor = textColor;
        }

        private static Texture2D MakeRoundedTex(int width, int height, int radius, Color color)
        {
            Texture2D tex = new Texture2D(width, height, TextureFormat.ARGB32, false)
            {
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Bilinear,
                hideFlags = HideFlags.HideAndDontSave
            };

            Color clear = new Color(0f, 0f, 0f, 0f);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tex.SetPixel(
                        x,
                        y,
                        IsInsideRoundedRect(x, y, width, height, radius) ? color : clear
                    );
                }
            }

            tex.Apply(false, true);
            return tex;
        }

        private static bool IsInsideRoundedRect(int x, int y, int width, int height, int radius)
        {
            int left = radius;
            int right = width - radius - 1;
            int bottom = radius;
            int top = height - radius - 1;

            int cx = x;
            int cy = y;

            if (x < left)
                cx = left;
            else if (x > right)
                cx = right;

            if (y < bottom)
                cy = bottom;
            else if (y > top)
                cy = top;

            int dx = x - cx;
            int dy = y - cy;

            return dx * dx + dy * dy <= radius * radius;
        }

        public static void Dispose()
        {
            DestroyTexture(ref _windowBg);
            DestroyTexture(ref _boxBg);
            DestroyTexture(ref _innerBoxBg);

            DestroyTexture(ref _categoryBg);
            DestroyTexture(ref _categoryHoverBg);

            DestroyTexture(ref _moduleBg);
            DestroyTexture(ref _moduleHoverBg);

            DestroyTexture(ref _toggleOffBg);
            DestroyTexture(ref _toggleOffHoverBg);
            DestroyTexture(ref _toggleOnBg);
            DestroyTexture(ref _toggleOnHoverBg);

            DestroyTexture(ref _inputBg);

            DestroyTexture(ref _sliderBg);
            DestroyTexture(ref _sliderThumbBg);

            DestroyTexture(ref _scrollbarBg);
            DestroyTexture(ref _scrollbarThumbBg);

            Initialized = false;
        }

        private static void DestroyTexture(ref Texture2D? tex)
        {
            if (tex == null)
                return;

            Object.Destroy(tex);
            tex = null;
        }
    }
}