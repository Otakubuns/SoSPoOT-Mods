using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using BokuMono;
using HarmonyLib;

namespace SoS_Craft_Limit;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    // Config - Craft Limit
    private static ConfigEntry<bool> configCraftLimitEnabled;
    private static ConfigEntry<uint> configCraftLimit;
    // This enables user to set craft limit to specific makers
    private static ConfigEntry<bool> configLimitAdvanced;
    private static ConfigEntry<uint> configLimitLumber; private static ConfigEntry<uint> configLimitThread;
    private static ConfigEntry<uint> configLimitTextile; private static ConfigEntry<uint> configLimitIngot;
    private static ConfigEntry<uint> configLimitJewel; private static ConfigEntry<uint> configLimitBrick;
    private static ConfigEntry<uint> configLimitPowder; private static ConfigEntry<uint> configLimitDye;
    private static ConfigEntry<uint> configLimitSeed; private static ConfigEntry<uint> configLimitSeedling;
    private static ConfigEntry<uint> configLimitJam; private static ConfigEntry<uint> configLimitSpore;
    private static ConfigEntry<uint> configLimitHoney; private static ConfigEntry<uint> configLimitMayonnaise;
    private static ConfigEntry<uint> configLimitYogurt; private static ConfigEntry<uint> configLimitButter;
    private static ConfigEntry<uint> configLimitYarn; private static ConfigEntry<uint> configLimitCheese;
    private static ConfigEntry<uint> configLimitCloth; private static ConfigEntry<uint> configLimitSeasoning;
    private static ConfigEntry<uint> configLimitCondiment; private static ConfigEntry<uint> configLimitEssence;

    // Config - Craft Time
    private static ConfigEntry<bool> configCraftTimeEnabled;
    private static ConfigEntry<int> configCraftTime;
    // This enables user to set craft time to specific makers
    private static ConfigEntry<bool> configTimeAdvanced;
    private static ConfigEntry<int> configTimeLumber; private static ConfigEntry<int> configTimeThread; 
    private static ConfigEntry<int> configTimeTextile; private static ConfigEntry<int> configTimeIngot; 
    private static ConfigEntry<int> configTimeJewel; private static ConfigEntry<int> configTimeBrick; 
    private static ConfigEntry<int> configTimePowder; private static ConfigEntry<int> configTimeDye;
    private static ConfigEntry<int> configTimeSeed; private static ConfigEntry<int> configTimeSeedling; 
    private static ConfigEntry<int> configTimeJam; private static ConfigEntry<int> configTimeSpore; 
    private static ConfigEntry<int> configTimeHoney; private static ConfigEntry<int> configTimeMayonnaise; 
    private static ConfigEntry<int> configTimeYogurt; private static ConfigEntry<int> configTimeButter; 
    private static ConfigEntry<int> configTimeYarn; private static ConfigEntry<int> configTimeCheese; 
    private static ConfigEntry<int> configTimeCloth; private static ConfigEntry<int> configTimeSeasoning; 
    private static ConfigEntry<int> configTimeCondiment; private static ConfigEntry<int> configTimeEssence;


    private static ManualLogSource _log;

    public override void Load()
    {
        // Seperate function just to make it look cleaner
        SetUpConfig();
        _log = Log;

        // Plugin startup logic
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
    }

    private void SetUpConfig()
    {
        // Setup config
        configCraftLimitEnabled = Config.Bind("!!Basic", "Craft Limit Enabled", true, "Enable/Disable craft limit");
        configCraftLimit = Config.Bind("!!Basic", "Craft Limit", (uint) 999, "Max amount of items to craft");
        configCraftTimeEnabled = Config.Bind("!!Basic", "Craft Time Enabled", true, "Enable/Disable craft time");
        configCraftTime = Config.Bind("!!Basic", "Craft Time", 1, "Minutes to craft an item");
        
        //Advanced config
        configLimitAdvanced = Config.Bind("!Advanced", "Limit Advanced Enabled", false, "Enable/Disable individual maker limit(not including giant)");
        configTimeAdvanced = Config.Bind("!Advanced", "Time Advanced Enabled", false, "Enable/Disable individual maker time(not including giant)");
        
        // Craft Limit
        configLimitLumber = Config.Bind("Advanced (Craft Limit)", "Lumber", (uint) 999, "Lumber Limit");
        configLimitThread = Config.Bind("Advanced (Craft Limit)", "Thread", (uint) 999, "Thread Limit");
        configLimitTextile = Config.Bind("Advanced (Craft Limit)", "Textile", (uint) 999, "Textile Limit");
        configLimitIngot = Config.Bind("Advanced (Craft Limit)", "Ingot", (uint) 999, "Ingot Limit");
        configLimitJewel = Config.Bind("Advanced (Craft Limit)", "Jewel", (uint) 999, "Jewel Limit");
        configLimitBrick = Config.Bind("Advanced (Craft Limit)", "Brick", (uint) 999, "Brick Limit");
        configLimitPowder = Config.Bind("Advanced (Craft Limit)", "Powder", (uint) 999, "Powder Limit");
        configLimitDye = Config.Bind("Advanced (Craft Limit)", "Dye", (uint) 999, "Dye Limit");
        configLimitSeed = Config.Bind("Advanced (Craft Limit)", "Seed", (uint) 999, "Seed Limit");
        configLimitSeedling = Config.Bind("Advanced (Craft Limit)", "Seedling", (uint) 999, "Seedling Limit");
        configLimitJam = Config.Bind("Advanced (Craft Limit)", "Jam", (uint) 999, "Jam Limit");
        configLimitSpore = Config.Bind("Advanced (Craft Limit)", "Spore", (uint) 999, "Spore Limit");
        configLimitHoney = Config.Bind("Advanced (Craft Limit)", "Honey", (uint) 999, "Honey Limit");
        configLimitMayonnaise = Config.Bind("Advanced (Craft Limit)", "Mayonnaise", (uint) 999, "Mayonnaise Limit");
        configLimitYogurt = Config.Bind("Advanced (Craft Limit)", "Yogurt", (uint) 999, "Yogurt Limit");
        configLimitButter = Config.Bind("Advanced (Craft Limit)", "Butter", (uint) 999, "Butter Limit");
        configLimitYarn = Config.Bind("Advanced (Craft Limit)", "Yarn", (uint) 999, "Yarn Limit");
        configLimitCheese = Config.Bind("Advanced (Craft Limit)", "Cheese", (uint) 999, "Cheese Limit");
        configLimitCloth = Config.Bind("Advanced (Craft Limit)", "Cloth", (uint) 999, "Cloth Limit");
        configLimitSeasoning = Config.Bind("Advanced (Craft Limit)", "Seasoning", (uint) 999, "Seasoning Limit");
        configLimitCondiment = Config.Bind("Advanced (Craft Limit)", "Condiment", (uint) 999, "Condiment Limit");
        configLimitEssence = Config.Bind("Advanced (Craft Limit)", "Essence", (uint) 999, "Essence Limit");

        // Craft Time
        configTimeLumber = Config.Bind("Advanced (Craft Time)", "Lumber", 1, "Minutes to craft an item");
        configTimeThread = Config.Bind("Advanced (Craft Time)", "Thread", 1, "Minutes to craft an item");
        configTimeTextile = Config.Bind("Advanced (Craft Time)", "Textile", 1, "Minutes to craft an item");
        configTimeIngot = Config.Bind("Advanced (Craft Time)", "Ingot", 1, "Minutes to craft an item");
        configTimeJewel = Config.Bind("Advanced (Craft Time)", "Jewel", 1, "Minutes to craft an item");
        configTimeBrick = Config.Bind("Advanced (Craft Time)", "Brick", 1, "Minutes to craft an item");
        configTimePowder = Config.Bind("Advanced (Craft Time)", "Powder", 1, "Minutes to craft an item");
        configTimeDye = Config.Bind("Advanced (Craft Time)", "Dye", 1, "Minutes to craft an item");
        configTimeSeed = Config.Bind("Advanced (Craft Time)", "Seed", 1, "Minutes to craft an item");
        configTimeSeedling = Config.Bind("Advanced (Craft Time)", "Seedling", 1, "Minutes to craft an item");
        configTimeJam = Config.Bind("Advanced (Craft Time)", "Jam", 1, "Minutes to craft an item");
        configTimeSpore = Config.Bind("Advanced (Craft Time)", "Spore", 1, "Minutes to craft an item");
        configTimeHoney = Config.Bind("Advanced (Craft Time)", "Honey", 1, "Minutes to craft an item");
        configTimeMayonnaise = Config.Bind("Advanced (Craft Time)", "Mayonnaise", 1, "Minutes to craft an item");
        configTimeYogurt = Config.Bind("Advanced (Craft Time)", "Yogurt", 1, "Minutes to craft an item");
        configTimeButter = Config.Bind("Advanced (Craft Time)", "Butter", 1, "Minutes to craft an item");
        configTimeYarn = Config.Bind("Advanced (Craft Time)", "Yarn", 1, "Minutes to craft an item");
        configTimeCheese = Config.Bind("Advanced (Craft Time)", "Cheese", 1, "Minutes to craft an item");
        configTimeCloth = Config.Bind("Advanced (Craft Time)", "Cloth", 1, "Minutes to craft an item");
        configTimeSeasoning = Config.Bind("Advanced (Craft Time)", "Seasoning", 1, "Minutes to craft an item");
        configTimeCondiment = Config.Bind("Advanced (Craft Time)", "Condiment", 1, "Minutes to craft an item");
        configTimeEssence = Config.Bind("Advanced (Craft Time)", "Essence", 1, "Minutes to craft an item");
    }

    [HarmonyPatch]
    public class LoadPatch
    {
        [HarmonyPatch(typeof(MasterDataManager), "LoadComplete")]
        [HarmonyPostfix]
        public static void MakerPatch(MasterDataManager __instance)
        {
            foreach (var maker in __instance.MakerMasterData)
            {
                if (configCraftLimitEnabled.Value)
                {
                    if (configLimitAdvanced.Value)
                    {
                        maker.MaxCraftNum = maker.Category switch
                        {
                            1 => configLimitLumber.Value,
                            2 => configLimitThread.Value,
                            3 => configLimitTextile.Value,
                            4 => configLimitIngot.Value,
                            5 => configLimitJewel.Value,
                            6 => configLimitBrick.Value,
                            7 => configLimitSeed.Value,
                            8 => configLimitPowder.Value,
                            9 => configLimitSeedling.Value,
                            10 => configLimitJam.Value,
                            11 => configLimitSpore.Value,
                            12 => configLimitHoney.Value,
                            13 => configLimitYarn.Value,
                            14 => configLimitCloth.Value,
                            15 => configLimitCheese.Value,
                            16 => configLimitButter.Value,
                            17 => configLimitYogurt.Value,
                            18 => configLimitMayonnaise.Value,
                            19 => configLimitSeasoning.Value,
                            20 => configLimitCondiment.Value,
                            21 => configLimitEssence.Value,
                            22 => configLimitDye.Value,
                            _ => 999
                        };
                    }
                    else
                    {
                        // Change the max craft limit to config value
                        maker.MaxCraftNum = configCraftLimit.Value;
                    }
                }

                if (!configCraftTimeEnabled.Value) continue;
                if (configTimeAdvanced.Value)
                {
                    maker.Time = maker.Category switch
                    {
                        1 => configTimeLumber.Value,
                        2 => configTimeThread.Value,
                        3 => configTimeTextile.Value,
                        4 => configTimeIngot.Value,
                        5 => configTimeJewel.Value,
                        6 => configTimeBrick.Value,
                        7 => configTimeSeed.Value,
                        8 => configTimePowder.Value,
                        9 => configTimeSeedling.Value,
                        10 => configTimeJam.Value,
                        11 => configTimeSpore.Value,
                        12 => configTimeHoney.Value,
                        13 => configTimeYarn.Value,
                        14 => configTimeCloth.Value,
                        15 => configTimeCheese.Value,
                        16 => configTimeButter.Value,
                        17 => configTimeYogurt.Value,
                        18 => configTimeMayonnaise.Value,
                        19 => configTimeSeasoning.Value,
                        20 => configTimeCondiment.Value,
                        21 => configTimeEssence.Value,
                        22 => configTimeDye.Value,
                        _ => 1
                    };
                }
                else
                {
                    // Change the time to craft to config value
                    maker.Time = configCraftTime.Value;
                }
            }
        }
    }
}