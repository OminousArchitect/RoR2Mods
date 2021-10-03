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
        public static List<int> FlyingEnemies;
        public static List<int> cachedlist
        {
            get
            {
                if(FlyingEnemies == null)
                {
                    FlyingEnemies = new List<int>();
                    FlyingEnemies.Add( (int) BodyCatalog.FindBodyIndex("BellBody") );
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
            Run.onRunStartGlobal += ArtifactBehaviour;
            SceneDirector.onGenerateInteractableCardSelection += (director, selection) =>
            {
                selection.RemoveCardsThatFailFilter(c => cachedlist.Contains( (int) (c.spawnCard.prefab.GetComponent<CharacterBody>().bodyIndex) ) );
            };
        }
        private static void ArtifactBehaviour(Run run)
        {
            if(NetworkServer.active && RunArtifactManager.instance.IsArtifactEnabled(NoFlying))
            {
                Chat.AddMessage("All my homies hate flying enemies");
            }
        }
    }
}