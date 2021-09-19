﻿using System;
using EntityStates;
using RoR2;
using UnityEngine;

namespace VayneMod
{
    public static class Prefabs
    {
        internal static void Init()
        {
            ForEachBody();
            InitializeSkills();
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