﻿using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;


namespace VayneMod
{
    internal static class Projectiles
    {
        public static GameObject boltprefab;
        public static GameObject silverboltprefab;
        public static BuffDef FinalHour;
        internal static void Initialize()
        {
            ModifyProjectiles();
        }
        
        private static void ModifyProjectiles()
        {
            boltprefab = Assets.mainAssetBundle.LoadAsset<GameObject>("Boltprefab");
            FinalHour = Assets.serialcontentpack.buffDefs[0];
        }
        
    }
}