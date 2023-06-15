using System;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using BokuMono;
using HarmonyLib;
using SoSTestMod;
using UnityEngine;
using UnityEngine.UI;

namespace SoSPortraitMod;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    private static ManualLogSource _log;
    private static string SpritePath;

    public override void Load()
    {
        // Plugin startup logic
        _log = Log;
        _log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        SpritePath = Path.Combine(Paths.PluginPath, "SoS Portrait Mod", "assets");
    }


    [HarmonyPatch]
    public class LoadPatch
    {
        [HarmonyPatch(typeof(UISimpleMessage), "OpenMessage")]
        [HarmonyPostfix]
        public static void MessagePatch(UISimpleMessage __instance)
        {
            try
            {
                var charName = __instance.gameObject.transform.GetChild(3).GetChild(0).gameObject
                    .GetComponent<UITextMeshProOneLine>();


                var checkPortrait = __instance.transform.parent.Find("CharPortrait")?.gameObject;
                if (checkPortrait != null)
                {
                    // Set to false in case sprite isn't valid and it was true before
                    var portrait = checkPortrait.GetComponent<Image>();
                    portrait.sprite = ChangeAssets(charName.text);

                    checkPortrait.active = portrait.sprite != null;
                }
                else
                {
                    CreatePortrait(__instance, charName.text);
                }
            }
            catch (Exception e)
            {
                _log.LogError(e);
            }
        }

        [HarmonyPatch(typeof(UIEventMessageManager), "SetFeedMessageAnimation")]
        [HarmonyPostfix]
        public static void EventPortraitPatch(UIEventMessageManager __instance)
        {
            var charName = __instance.transform.GetChild(4).GetChild(0).gameObject.GetComponent<UITextMeshProOneLine>();
            var checkPortrait = __instance.transform.parent.Find("CharPortrait")?.gameObject;
            
            if (checkPortrait != null)
            {
                // Set to false in case sprite isn't valid and it was true before
                var portrait = checkPortrait.GetComponent<Image>();
                portrait.sprite = ChangeAssets(charName.text);

                checkPortrait.active = portrait.sprite != null;
            }
            else
            {
                CreatePortrait(__instance, charName.text);
            }
        }

        [HarmonyPatch(typeof(UIEventMessageManager), "CloseMessage")]
        [HarmonyPostfix]
        public static void CloseEventPatch(UIEventMessageManager __instance)
        {
            var checkPortrait = __instance.transform.parent.Find("CharPortrait")?.gameObject;
            if (checkPortrait != null)
            {
                checkPortrait.active = false;
            }
        }
        

        [HarmonyPatch(typeof(UISimpleMessage), "CloseMessage")]
        [HarmonyPrefix]
        public static void CloseMessagePatch(UISimpleMessage __instance)
        {
            var checkPortrait = __instance.transform.parent.Find("CharPortrait")?.gameObject;
            if (checkPortrait == null) return;
            checkPortrait.active = false;
        }

        private static void CreatePortrait(Component uiMessage, string charName)
        {
            var gameObject = new GameObject();
            var portrait = gameObject.AddComponent<Image>();
            var mask = gameObject.AddComponent<Mask>();
            mask.name = "CharMask";

            var transform = portrait.transform;

            if (uiMessage.name == "EventMessage")
                transform.parent = uiMessage.transform.parent.parent;
            transform.parent = uiMessage.transform.parent.transform;
            portrait.name = "CharPortrait";

            transform.position = new Vector3(2003.3f, -0.1964f, 100);
            transform.localScale = new Vector3(4.3f, 4, 108);

            transform.SetAsFirstSibling();

            portrait.sprite = ChangeAssets(charName);
            gameObject.active = portrait.sprite != null;
        }

        private static Sprite ChangeAssets(string pText, string extra = "")
        {
            // help from https://forum.unity.com/threads/generating-sprites-dynamically-from-png-or-jpeg-files-in-c.343735/
            Texture2D texture2D;
            byte[] fileBytes;
            var files = Directory.GetFiles(SpritePath, pText + extra + ".png", SearchOption.AllDirectories);
            var file = files.Length > 0 ? files[0] : null;

            if (!File.Exists(file)) return null;
            // Read all bytes from file and convert to sprite and add to dictionary for easier loading in dialog
            fileBytes = File.ReadAllBytes(file);
            texture2D = new Texture2D(2, 2);
            return !texture2D.LoadImage(fileBytes)
                ? null
                : Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
        }
    }
}