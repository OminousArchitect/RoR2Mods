using RoR2;
using UnityEngine;

namespace VayneMod
{
    public static class Prefabs
    {
        internal static void Init()
        {
            ForEachBody();
        }
        private static void ForEachBody()
        {
            foreach (var body in Assets.maincontentpack.bodyPrefabs)
            {
                var cb = body.GetComponent<CharacterBody>();
                cb.crosshairPrefab = Resources.Load<GameObject>("Prefabs/Crosshair/Bandit2CrosshairPrepRevolverFire");
                cb.preferredPodPrefab = Resources.Load<GameObject>("prefabs/networkedobjects/SurvivorPod");

                var footstep = body.GetComponentInChildren<FootstepHandler>();
                if (footstep != null)
                    footstep.footstepDustPrefab = Resources.Load<GameObject>("prefabs/GenericFootstepDust");
            }
        }
    }
}