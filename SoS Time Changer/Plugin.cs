using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using BokuMono;
using HarmonyLib;
using Il2CppSystem.Collections.Generic;

namespace SoS_Time_Changer;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    private static ConfigEntry<int> _timeScale;
    private static ConfigEntry<bool> _timeInside;
    private static List<uint> _areas = FillList();

    private static List<uint> FillList()
    {
        var list = new List<uint>();
        // Town
        list.Add(10102);
        // Farm
        list.Add(102);
        // Earthsprite Village
        list.Add(20200);
        // Lava Caves
        list.Add(20300);
        // Beanstalk Island
        list.Add(20400);

        // DLC Areas
        // Terracotta Oasis
        list.Add(21212);
        // Windswept Farms
        list.Add(21112);
        // Twilight Isle
        list.Add(21012);
        return list;
    }

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

            if (_areas.Contains(__0))
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

        [HarmonyPatch(typeof(DateManager), "Play")]
        [HarmonyPostfix]
        public static void TimeInsidePatch(DateManager __instance)
        {
            if (_timeInside.Value) return;

            var areaid = GameController.Instance.FM.currentFieldId;


            if (_areas.Contains(areaid))
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