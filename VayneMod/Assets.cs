using System.Collections;
using System.IO;
using System.Reflection;
using RoR2;
using RoR2.ContentManagement;
using UnityEngine;
using Path = System.IO.Path;
using System.Linq;
using EntityStates;
using R2API;
using VayneMod;

namespace VayneMod
{
    internal static class Assets
    {
        internal static AssetBundle mainAssetBundle;
        private static SerializableContentPack serialcontentpack;
        internal static ContentPack maincontentpack;
        internal static string SoundBankName = "VayneSounds";  
        
        internal static void Initialize()
        {
            LoadAssetBundle();
            LoadSoundBank();
            ContentPackProvider.Initialize();
            ApplyShaders();
        }

        private static void ApplyShaders()
        {
            var materials = Assets.mainAssetBundle.LoadAllAssets<Material>();
            foreach (Material material in materials) 
                if(material.shader.name.StartsWith("StubbedShader"))
                    material.shader = Resources.Load<Shader>("shaders" + material.shader.name.Substring(13));
        }

        private static void LoadSoundBank()
        {
            if (SoundBankName == "")
            {
                Debug.LogFormat("VayneMod: SoundBank name is blank. Skipping loading SoundBank.");
                return;
            }
            
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Stream stream = File.Open(Path.Combine(path, SoundBankName + ".bnk"), FileMode.Open);
            var array = new byte[stream.Length];
            stream.Read(array, 0, array.Length);
            Debug.Log("length of sndbnk: " + array.Length);
            
            R2API.SoundAPI.SoundBanks.Add(array);
        }

        private static void LoadAssetBundle()
        {
            if (mainAssetBundle != null) return;
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            mainAssetBundle = AssetBundle.LoadFromFile(Path.Combine(path, "vayneassets"));
            LoadContentPack();
        }

        private static void LoadContentPack()
        {
            serialcontentpack = mainAssetBundle.LoadAsset<SerializableContentPack>(ContentPackProvider.contentPackName);
            maincontentpack = serialcontentpack.CreateContentPack();
            AddEntityStates();
            ContentPackProvider.contentPack = maincontentpack;
            Prefabs.Initialize();
        }

        private static void AddEntityStates()
        {
            maincontentpack.entityStateTypes.Add(Assembly.GetExecutingAssembly().GetTypes().Where
                (type => typeof(EntityState).IsAssignableFrom(type)).ToArray());
        }
        
        public class ContentPackProvider : IContentPackProvider
        {
            public static SerializableContentPack serializedContentPack;
            public static ContentPack contentPack;
            public static string contentPackName = "MainContentPack";

            public string identifier => "VayneMod";

            internal static void Initialize()
            {
                contentPack = Assets.maincontentpack;
                //contentPack = serializedContentPack.CreateContentPack();
                ContentManager.collectContentPackProviders += AddCustomContent;
            }

            private static void AddCustomContent(ContentManager.AddContentPackProviderDelegate addContentPackProvider)
            {
                addContentPackProvider(new ContentPackProvider());
            }

            public IEnumerator LoadStaticContentAsync(LoadStaticContentAsyncArgs args)
            {
                args.ReportProgress(1f);
                yield break;
            }

            public IEnumerator GenerateContentPackAsync(GetContentPackAsyncArgs args)
            {
                ContentPack.Copy(contentPack, args.output);
                args.ReportProgress(1f);
                yield break;
            }

            public IEnumerator FinalizeAsync(FinalizeAsyncArgs args)
            {
                args.ReportProgress(1f);
                yield break;
            }
        }
    }
}