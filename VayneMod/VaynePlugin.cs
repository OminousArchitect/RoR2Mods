using System;
using System.Security;
using System.Security.Permissions;
using BepInEx;
using R2API.Utils;
using RoR2;
using HarmonyLib;

#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete
[module: UnverifiableCode]

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

            var harmony = new Harmony("nines.vaynemod");
            harmony.PatchAll();
        }
    }
    
    
}