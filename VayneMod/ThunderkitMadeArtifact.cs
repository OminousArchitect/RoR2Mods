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

        public static string[] banned =
        {
            "WispBody", 
            "VagrantBody"
        };
        public static List<BodyIndex> cachedlist
        { 
            get
            {
                if(FlyingEnemies == null)
                {
                    FlyingEnemies = new List<BodyIndex>();
                    Array.ForEach(banned,x => FlyingEnemies.Add(BodyCatalog.FindBodyIndex(x)) );
                    Debug.LogWarning("Creating blacklist...");
                }
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
                        if (component == null)
                        {
                            return true;
                        }
                        return cachedlist.Contains(component.bodyIndex);
                    });
                }
            };
        }
    }
}