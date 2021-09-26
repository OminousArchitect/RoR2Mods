using RoR2;
using RoR2.CharacterSpeech;
using System.Collections.ObjectModel;
using HarmonyLib;
using UnityEngine;
using BepInEx;
using System.Linq;


namespace VayneMod
{
    [HarmonyPatch]
    public class HarmonyPatches
    {
        [HarmonyPrefix, HarmonyPatch(typeof(CharacterBody), "OnBuffFirstStackGained")]
        public static void VayneUltimate(CharacterBody __instance, BuffDef buffDef)
        {
            if (__instance.HasBuff(FHbuff))
            {
                Debug.Log("HookStack");
                animator.SetBool("isFinalHour", true);
            }
        }
        
        [HarmonyPrefix, HarmonyPatch(typeof(CharacterBody), "OnBuffFinalStackLost")]
        public static void VayneUltimateEnd(CharacterBody __instance, BuffDef buffDef)
        {
            if (__instance.HasBuff(FHbuff))
            {
                Debug.Log("HookLost");
                animator.SetBool("isFinalHour", false);
            }
            
        }

        private static BuffDef FHbuff = Assets.serialcontentpack.buffDefs[0];
        private static Animator animator = Prefabs.vayneprefab.GetComponent<ModelLocator>()._modelTransform.GetComponent<Animator>();

    }
}