using UnityEngine;
using BBPGlue.API;

namespace BBPGlue.Tests
{
    internal static class CustomPrefabsTest
    {
        private static bool _registered;

        private static bool _boostActive;
        private static float _boostTimer;
        private static float _oldWalkSpeed;
        private static float _oldRunSpeed;
        private static float _oldStaminaDrop;

        public static void Register()
        {
            if (_registered)
                return;

            _registered = true;

            BBP.Callbacks.Game.OnLevelReady += OnLevelReady;
            BBP.Callbacks.Items.OnItemUse += OnItemUse;
        }

        public static void Update()
        {
            if (!_boostActive)
                return;

            _boostTimer -= Time.deltaTime;

            BBP.Player.Stamina = BBP.Player.StaminaMax;

            if (_boostTimer > 0f)
                return;

            BBP.Player.WalkSpeed = _oldWalkSpeed;
            BBP.Player.RunSpeed = _oldRunSpeed;
            BBP.Player.StaminaDrop = _oldStaminaDrop;

            _boostActive = false;

            BBPConsole.Log("[CustomPrefabsTest] Monster BSoda boost ended.");
        }

        private static void OnLevelReady()
        {
            BBP.Prefabs.Refresh();

            BBP.Authoring.CloneNpc(
                "test:old_principal",
                "Principal",
                npc =>
                {
                    npc.Name = "Old Principal";
                    npc.SetSpriteColor(Color.gray);
                    npc.MaxSpeed = 10f;
                    npc.Acceleration = 15f;
                }
            );

            BBP.Authoring.CloneItem(
                "test:monster_bsoda",
                BBPItemId.Bsoda,
                item =>
                {
                    item.NameKey = "Monster BSoda";
                    item.DescriptionKey = "UNLEASH THE BEAST!!!";
                    item.Price = 0;
                    item.Value = 999;

                    if (item.SmallSprite != null)
                        item.SmallSprite = CreateTintedSprite(item.SmallSprite, Color.green);

                    if (item.LargeSprite != null)
                        item.LargeSprite = CreateTintedSprite(item.LargeSprite, Color.green);
                }
            );

            BBPConsole.Log(
                $"[CustomPrefabsTest] Registered custom prefabs: {BBP.Prefabs.CustomPrefabs.Count}"
            );
        }

        private static void OnItemUse(BBPItemObject item)
        {
            if (item.NameKey != "Monster BSoda")
                return;

            BBPCallbacks.Cancel();

            BBP.Items.RemoveSlot(BBP.Items.SelectedSlot);

            if (!_boostActive)
            {
                _oldWalkSpeed = BBP.Player.WalkSpeed;
                _oldRunSpeed = BBP.Player.RunSpeed;
                _oldStaminaDrop = BBP.Player.StaminaDrop;
            }

            _boostActive = true;
            _boostTimer = 20f;

            BBP.Player.WalkSpeed = _oldWalkSpeed * 4f;
            BBP.Player.RunSpeed = _oldRunSpeed * 4f;
            BBP.Player.StaminaDrop = 0f;
            BBP.Player.Stamina = BBP.Player.StaminaMax;

            BBPConsole.Log("[CustomPrefabsTest] Monster BSoda boost applied for 20 seconds.");
        }

        private static Sprite CreateTintedSprite(Sprite source, Color tint)
        {
            Texture2D src = source.texture;

            RenderTexture rt = RenderTexture.GetTemporary(
                src.width,
                src.height,
                0,
                RenderTextureFormat.Default,
                RenderTextureReadWrite.Default
            );

            Graphics.Blit(src, rt);

            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = rt;

            Texture2D readable = new Texture2D(
                src.width,
                src.height,
                TextureFormat.RGBA32,
                false
            );

            readable.ReadPixels(new Rect(0, 0, src.width, src.height), 0, 0);
            readable.Apply();

            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(rt);

            Color[] pixels = readable.GetPixels();

            for (int i = 0; i < pixels.Length; i++)
                pixels[i] *= tint;

            readable.SetPixels(pixels);
            readable.Apply();
            readable.filterMode = src.filterMode;

            return Sprite.Create(
                readable,
                source.rect,
                new Vector2(
                    source.pivot.x / source.rect.width,
                    source.pivot.y / source.rect.height
                ),
                source.pixelsPerUnit
            );
        }
    }
}