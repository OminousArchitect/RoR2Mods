using EntityStates;
using EntityStates.Commando;
using RoR2;
using HarmonyLib;
using UnityEngine;

namespace VayneMod
{
    [HarmonyPatch]
    public class HarmonyPatches
    {
        [HarmonyPrefix, HarmonyPatch(typeof(CharacterBody), "OnBuffFirstStackGained")]
        public static void VayneUltimate(CharacterBody __instance, BuffDef buffDef)
        {
            if (buffDef == Buffs.FinalHour)
            {
                RetrieveAnimator(__instance).SetBool("inFinalHour", true);
            }

            if (buffDef == Buffs.Tumble)
            {
                RetrieveAnimator(__instance).SetBool("hasTumbled", true);
            }
        }

        [HarmonyPrefix, HarmonyPatch(typeof(CharacterBody), "OnBuffFinalStackLost")]
        public static void VayneUltimateEnd(CharacterBody __instance, BuffDef buffDef)
        {
            if (buffDef == Buffs.FinalHour)
            {
                RetrieveAnimator(__instance).SetBool("inFinalHour", false);
            }
            
            if (buffDef == Buffs.Tumble)
            {
                RetrieveAnimator(__instance).SetBool("hasTumbled", false);
            }
        }

        public static Animator RetrieveAnimator(CharacterBody body)
        {
            Animator animator = body.GetComponent<ModelLocator>().modelTransform.GetComponent<Animator>();
            return animator;
        }
    }
}
