using System;
using EntityStates;
using RoR2;
using UnityEngine;

namespace VayneMod
{
    internal static class Prefabs
    {
        internal static GameObject vayneprefab;
        internal static void Initialize()
        {
            ForEachBody();
            InitializeSkills();
        }
        private static void ForEachBody()
        {
            foreach (var prefab in Assets.maincontentpack.bodyPrefabs)
            {
                var characterBody = prefab.GetComponent<CharacterBody>();
                characterBody.crosshairPrefab = Resources.Load<GameObject>("Prefabs/Crosshair/Bandit2CrosshairPrepRevolverFire");
                characterBody.preferredPodPrefab = Resources.Load<GameObject>("prefabs/networkedobjects/SurvivorPod");

                var footstep = prefab.GetComponentInChildren<FootstepHandler>();
                if (footstep != null)
                    footstep.footstepDustPrefab = Resources.Load<GameObject>("prefabs/GenericFootstepDust");

                //var deathBehavior = body.GetComponent<CharacterDeathBehavior>();
                //deathBehavior.deathState = new SerializableEntityStateType(typeof(VayneMod.DeathState));
            }
            vayneprefab = Assets.maincontentpack.bodyPrefabs[0];
            
            //TODO vayneprefab.AddComponent<LobbyTaunt>();
        }

        private static void InitializeSkills()
        {
            foreach (var skillDef in  (Assets.maincontentpack.skillDefs))
            {
                skillDef.activationState = new SerializableEntityStateType(FindType(skillDef.skillName));
            }
        }

        private static Type[] _types;
        private static Type FindType(string camelCaseName)
        {
            if (_types == null) _types = typeof(VaynePlugin).Assembly.GetTypes();
            foreach (var type in _types)
            {
                if (type.Name.StartsWith(camelCaseName)) return type;
            }
            Debug.LogWarning("EntityState class not found for " + camelCaseName);
            return null;
        }
    }
}