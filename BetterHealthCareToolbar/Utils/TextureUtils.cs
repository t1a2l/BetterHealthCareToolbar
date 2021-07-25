using ColossalFramework.UI;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;
using static ColossalFramework.Plugins.PluginManager;
using ColossalFramework.Plugins;

namespace BetterHealthCareToolbar
{
	internal static class TextureUtils
	{
        static string PATH => "BetterHealthCareToolbar.BetterHealthCareToolbar.Utils.Atlas.";
        static string ModPath => GetPlugin().modPath;
        public static string FILE_PATH = ModPath;
        public static bool EmbededResources = true;
       
        static PluginManager man => PluginManager.instance;

        public static UITextureAtlas CreateTextureAtlas(string textureFile, string atlasName, string[] spriteNames) {
            Texture2D texture2D;
            if (!EmbededResources)
                texture2D = GetTextureFromFile(textureFile);
            else
                texture2D = GetTextureFromAssemblyManifest(textureFile);
            return CreateTextureAtlas(texture2D, atlasName, spriteNames);
        }

        public static UITextureAtlas CreateTextureAtlas(Texture2D texture2D, string atlasName, string[] spriteNames) {
            UITextureAtlas uitextureAtlas = ScriptableObject.CreateInstance<UITextureAtlas>();
            Assert(uitextureAtlas != null, "uitextureAtlas");
            Material material = Object.Instantiate<Material>(UIView.GetAView().defaultAtlas.material);
            Assert(material != null, "material");
            material.mainTexture = texture2D;
            uitextureAtlas.material = material;
            uitextureAtlas.name = atlasName;
            UITextureAtlas.SpriteInfo spriteInfo;
            float width;
            int n = spriteNames.Length;
            int j = 0;
            for (int i = 0; i < n; i++) {
                float num;
                if(i < 5)
                {
                    num = 1f / (float)15;
                    width = 1f / (float)15;
                    spriteInfo = new UITextureAtlas.SpriteInfo {
                        name = spriteNames[i],
                        texture = texture2D,
                        region = new Rect(i * num, 0f, width, 1f)
                    };
                } 
                else
                {
                    num = (1f + j) / (float)15;
                    width = 1f / (float)15;
                    spriteInfo = new UITextureAtlas.SpriteInfo {
                        name = spriteNames[i],
                        texture = texture2D,
                        region = new Rect(i * num, 0f, width, 1f)
                    };
                    j++;
                }
                uitextureAtlas.AddSprite(spriteInfo);
            }
            return uitextureAtlas;
        }

        public static Texture2D GetTextureFromFile(string file) {
            using (Stream stream = GetFileStream(file))
                return GetTextureFromStream(stream);
        }

        public static Texture2D GetTextureFromAssemblyManifest(string file) {
            using (Stream stream = GetManifestResourceStream(file))
                return GetTextureFromStream(stream);
        }

        public static Stream GetManifestResourceStream(string file) {
            try {
                var d = Assembly.GetExecutingAssembly().GetManifestResourceNames();
                string path = string.Concat(PATH, file);
                return Assembly.GetExecutingAssembly().GetManifestResourceStream(path)
                    ?? throw new Exception(path + " not found");
            } catch (Exception ex) {
                LogHelper.Error(ex.ToString());
                throw ex;
            }
        }

        internal static void Assert(bool con, string m = "") {
            if (!con) {
                m = "Assertion failed: " + m;
                LogHelper.Error(m);
                throw new System.Exception(m);
            }
        }

        public static Texture2D GetTextureFromStream(Stream stream) {
            Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            byte[] array = new byte[stream.Length];
            stream.Read(array, 0, array.Length);
            texture2D.filterMode = FilterMode.Bilinear;
            texture2D.LoadImage(array);
            texture2D.wrapMode = TextureWrapMode.Clamp; // for cursor.
            texture2D.Apply();
            return texture2D;
        }

        public static Stream GetFileStream(string file) {
            try {
                string path = Path.Combine(FILE_PATH, file);
                return File.OpenRead(path) ?? throw new Exception(path + "not found");
            } catch (Exception ex) {
                LogHelper.Error(ex.ToString());
                throw ex;
            }
        }

        public static PluginInfo GetPlugin(Assembly assembly = null) {
            if (assembly == null)
                assembly = Assembly.GetExecutingAssembly();
            foreach (PluginInfo current in man.GetPluginsInfo()) {
                if (current.ContainsAssembly(assembly))
                    return current;
            }
            return null;
        }
	}
}
