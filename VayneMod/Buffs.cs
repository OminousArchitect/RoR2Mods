using System.ComponentModel;
using RoR2;
using UnityEngine;

namespace VayneMod
{
    internal static class Buffs
    {
        internal static BuffDef FinalHour;
        internal static BuffDef Tumble;
        internal static void Initialize()
        {
            FinalHour = Assets.maincontentpack.buffDefs[0];
            Tumble = Assets.maincontentpack.buffDefs[1];
            FinalHour.buffColor = Color.white;
        }
    }
}