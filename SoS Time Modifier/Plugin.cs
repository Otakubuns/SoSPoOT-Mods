using System;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using BokuMono;
using HarmonyLib;

namespace SoS_Time_Modifier;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    private static ConfigEntry<int> _timeScale;
    private static ConfigEntry<bool> _timeInside;
    private static bool _isEvent;

    public override void Load()
    {
        _timeScale = Config.Bind("General", "TimeScale", 60,
            "The speed of time in the game(60 = 1 in game minute per second & 30 = 1 minute per 2 seconds)");
        _timeInside = Config.Bind("General", "TimeInside", false, "Whether time passes inside buildings");
        // Plugin startup logic
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
    }

    [HarmonyPatch]
    public class LoadTimePatch
    {
        [HarmonyPatch(typeof(DateManager), "Init")]
        [HarmonyPostfix]
        public static void TimePatch(DateManager __instance)
        {
            __instance.TimeScale = _timeScale.Value;
        }

        [HarmonyPatch(typeof(GameController), "PostSucceededChangeField")]
        [HarmonyPostfix]
        public static void AreaPatch(GameController __instance, uint __0)
        {
            if (_timeInside.Value) return;

            if (!FieldManager.Instance.IsIndoorField(__0))
            {
                if (DateManager.Instance.IsPlay()) return;
                DateManager.Instance.Play();
            }
            else
            {
                if (!DateManager.Instance.IsPlay()) return;
                DateManager.Instance.Pause();
            }
        }

        [HarmonyPatch(typeof(DateManager), "SetDate")]
        [HarmonyPostfix]
        public static void FestivalWorkaround()
        {
            // Try incase the game is not loaded yet and festival manager is not initialized
            try
            {
                if (FestivalManager.Instance.openFestivalState == FestivalState.Finish &&
                    FestivalManager.Instance.IsInSession)
                {
                    _isEvent = false;
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        [HarmonyPatch(typeof(DateManager), "Play")]
        [HarmonyPostfix]
        public static void TimeInsidePatch(DateManager __instance)
        {
            if (_timeInside.Value) return;
            
            // Festival workaround
            if (GameController.Instance.isEvent) _isEvent = true;
            if (_isEvent) return;
            
            var id = GameController.Instance.FM.currentFieldId;

            if (!FieldManager.Instance.IsIndoorField(id))
            {
                if (__instance.IsPlay()) return;
                __instance.Play();
            }
            else
            {
                if (!__instance.IsPlay()) return;
                __instance.Pause();
            }
        }
    }
}