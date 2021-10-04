using R2API;
using RoR2;

namespace VayneMod
{
    internal static class Tokens
    {
        internal static string Prefix = "NINES_";
        
        internal static void Init()
        {
            Reg(Prefix + "VAYNE_PRIMARY_NAME", "BasicAttack");
            Reg(Prefix + "VAYNE_SECONDARY_NAME", "Tumble");
            Reg(Prefix + "VAYNE_UTILITY_NAME", "Condemn");
            Reg(Prefix + "VAYNE_SPECIAL_NAME", "Final Hour");
            Reg(Prefix + "VAYNE_PRIMARY_DESC", "I'm not describing this rn");
            Reg(Prefix + "VAYNE_SECONDARY_NAME", "Condemn");
            //Reg(Prefix + "VAYNE_SENTINEL_SKIN", "Sentinel"); //I'll do this eventually
            Reg(Prefix + "VAYNE_BASE_SKIN", "Project");
        }

        internal static void Reg(string key , string val)
        {
            LanguageAPI.Add(Prefix + key , val);
        }
    }
}