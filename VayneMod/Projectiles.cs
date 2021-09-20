using R2API;
using UnityEngine;


namespace VayneMod
{
    internal static class Projectiles
    {
        private static GameObject boltprefab;
        public static  DamageAPI.ModdedDamageType Silverbolts { get; private set; }
        internal static void Initialize()
        {
            CallDamageAPI();
            ModifyProjectiles();
        }

        private static void CallDamageAPI()
        {
            Silverbolts = DamageAPI.ReserveDamageType();
        }
        private static void ModifyProjectiles()
        {
            boltprefab = Assets.mainAssetBundle.LoadAsset<GameObject>("Boltprefab");
            boltprefab.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>().Add(Silverbolts);
        }
        
    }
}