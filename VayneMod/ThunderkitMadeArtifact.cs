using RoR2;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine.Networking;
using UnityEngine;

namespace VayneMod
{
    [HarmonyPatch]
    public class ThunderkitMadeArtifact
    {
        public static ArtifactDef NoFlying = Assets.ContentPackProvider.contentPack.artifactDefs.Find("NoFlyingDef");

        public static readonly List<string> banned = new List<string>
        {
            "WispMaster", 
            "VagrantMaster",
            "BellMaster",
            //"RoboBallBossMaster",
            "GreaterWispMaster",
            //"LunarWispMaster"
        };
        public static void InitializeArtifact()
        {
            Debug.Log("Initializing NoFlying");
        }

        public static bool FilterCards(DirectorCard c)
        {
            Debug.Log(c.spawnCard.prefab.name);
            var what = !banned.Contains(c.spawnCard.prefab.name);
            Debug.Log(what);
            return what;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(ClassicStageInfo), nameof(ClassicStageInfo.RebuildCards))]
        public static void RemoveCards(ClassicStageInfo __instance)
        {
            if (NetworkServer.active && RunArtifactManager.instance.IsArtifactEnabled(NoFlying))
            {
                var cards = __instance.monsterSelection;
                for (var i = 0; i < cards.Count; i++)
                {
                    var card = cards.GetChoice(i);
                    if (!FilterCards(card.value))
                        cards.RemoveChoice(i);
                }
            }
        }
    }
}