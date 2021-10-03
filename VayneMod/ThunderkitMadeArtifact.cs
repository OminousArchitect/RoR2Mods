using RoR2;
using R2API;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace VayneMod
{
    public class ThunderkitMadeArtifact
    {
        public static ArtifactDef NoFlying = Assets.ContentPackProvider.contentPack.artifactDefs.Find("NoFlyingDef");
        public static List<BodyIndex> FlyingEnemies;
        public static List<string> banned = new List<string> {"BellBody"}; //"GreaterWispBody", "RoboBallBossBody", "VultureBody", "VagrantBody"};
        public static List<BodyIndex> cachedlist
        { 
            get
            {
                if(FlyingEnemies == null)
                {
                    FlyingEnemies = new List<BodyIndex>();
                    banned.ForEach(x => FlyingEnemies.Add(BodyCatalog.FindBodyIndex(x)));
                }
                Debug.LogWarning("This is printing AAAAAAAAAAA");
                return FlyingEnemies;
            }
        }
        public static void InitializeArtifact()
        {
            NotHooks();
        }
        public static void NotHooks()
        {
            //Run.onRunStartGlobal += ArtifactBehaviour;
            SceneDirector.onGenerateInteractableCardSelection += (director, selection) =>
            {
                if (NetworkServer.active && RunArtifactManager.instance.IsArtifactEnabled(NoFlying))
                {
                    selection.RemoveCardsThatFailFilter(c =>
                    {
                        var component = c.spawnCard.prefab.GetComponent<CharacterBody>();
                        if (component == null) return true;
                        return !cachedlist.Contains(component.bodyIndex);
                    });
                }
            };
        }
    }
}