using System;
using System.Security;
using System.Security.Permissions;
using BepInEx;
using R2API.Utils;
using RoR2;
using HarmonyLib;
using R2API;

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
            Buffs.Initialize();

            GlobalEventManager.onCharacterDeathGlobal += GlobalEventManagerOnonCharacterDeathGlobal;
            
            var harmony = new Harmony("nines.vaynemod");
            harmony.PatchAll();

            //Hook();
        }

        private void Hook()
        {
            RecalculateStatsAPI.GetStatCoefficients += (sender, args) =>
            {
                //args.baseMoveSpeedAdd += sender.HasBuff(Buffs.FinalHour)
            };
        }

        private void GlobalEventManagerOnonCharacterDeathGlobal(DamageReport damageReport)
        {
            BodyIndex VayneIndex = Prefabs.vayneprefab.GetComponent<CharacterBody>().bodyIndex;
            if (damageReport.victimIsElite && damageReport.attackerBodyIndex == VayneIndex && damageReport.attackerBody.HasBuff(Buffs.FinalHour))
            {
                damageReport.attackerBody.AddTimedBuff(RoR2Content.Buffs.AffixLunar, 3f);
                damageReport.attackerBody.AddTimedBuff(Buffs.FinalHour, 5f);
            }
        }
    }
    
    
}