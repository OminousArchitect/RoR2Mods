using System.Security;
using System.Security.Permissions;
using BepInEx;
using R2API.Utils;
using RoR2;
using HarmonyLib;
using R2API;
using UnityEngine;

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
        "RecalculateStatsAPI",
    })]
    
    public class VaynePlugin : BaseUnityPlugin
    {
        public const string MODUID = "com.Nines.Vayne";
        public const string MODNAME = "Vayne";
        public const string MODVERSION = "1.0.0";

        private void Awake()
        {
            Assets.Initialize();
            Buffs.Initialize();
            
            ThunderkitMadeArtifact.InitializeArtifact();

            GlobalEventManager.onCharacterDeathGlobal += GlobalEventManager_onCharacterDeathGlobal;
            GlobalEventManager.onCharacterLevelUp += GlobalEventManager_onCharacterLevelUp;

            var harmony = new Harmony("nines.vaynemod");
            harmony.PatchAll();
            
            RecalculateStatsAPI.GetStatCoefficients += (sender, args) =>
            {
                var component = sender.GetComponent<NightHunter>();
                if (component)
                    if(component.changeInDirection == NightHunter.Direction.Closer)
                        args.baseMoveSpeedAdd += component.speedincrease;

                if (sender.HasBuff(Buffs.FinalHour))
                {
                    args.baseAttackSpeedAdd += 0.30f;
                }
            };
        }

        private void GlobalEventManager_onCharacterLevelUp(CharacterBody obj)
        {
            BodyIndex vayneIndex = Prefabs.vayneprefab.GetComponent<CharacterBody>().bodyIndex;
            if (obj.bodyIndex == vayneIndex)
            {
                Debug.Log("Leveled up");
            }
        }

        private void GlobalEventManager_onCharacterDeathGlobal(DamageReport damageReport)
        {
            BodyIndex vayneIndex = Prefabs.vayneprefab.GetComponent<CharacterBody>().bodyIndex;
            if (damageReport.victimIsElite && damageReport.attackerBodyIndex == vayneIndex && damageReport.attackerBody.HasBuff(Buffs.FinalHour))
            {
                damageReport.attackerBody.AddTimedBuff(Buffs.FinalHour, 5f);
            }
        }
    }
}