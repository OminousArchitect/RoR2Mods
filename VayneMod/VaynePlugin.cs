using System;
using BepInEx;
using R2API.Utils;

namespace VayneMod
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    [R2APISubmoduleDependency(new string[]
    {
        "LanguageAPI",
        "DamageAPI",
        "SoundAPI",
    })]
    
    public class VaynePlugin : BaseUnityPlugin
    {
        public const string MODUID = "com.Nines.Vayne";
        public const string MODNAME = "Vayne";
        public const string MODVERSION = "1.0.0";

        private void Awake()
        {
            Assets.Initialize();
            Projectiles.Initialize();
        }
    }
    
    
}