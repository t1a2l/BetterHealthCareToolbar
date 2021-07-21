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
        static string PATH => typeof(TextureUtils).Assembly.GetName().Name + ".Resources.";
        static string ModPath => GetPlugin().modPath;
        public static string FILE_PATH = ModPath;
        public static bool EmbededResources = true;
        static PluginManager man => PluginManager.instance;

        public static UITextureAtlas CreateTextureAtlas(string textureFile, string atlasName, int spriteWidth, int spriteHeight, string[] spriteNames) {
            return CreateTextureAtlas(textureFile, atlasName, spriteNames);
        }

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

            int n = spriteNames.Length;
            for (int i = 0; i < n; i++) {
                float num = 1f / (float)spriteNames.Length;
                UITextureAtlas.SpriteInfo spriteInfo = new UITextureAtlas.SpriteInfo {
                    name = spriteNames[i],
                    texture = texture2D,
                    region = new Rect(i * num, 0f, num, 1f)
                };
                uitextureAtlas.AddSprite(spriteInfo);
            }
            return uitextureAtlas;
        }

        public static Texture2D GetTextureFromFile(string file) {
            using (Stream stream = GetFileStream(file))
                return GetTextureFromStream(stream);
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
                return File.OpenRead(path) ?? throw new Exception(path + "not find");
            } catch (Exception ex) {
                LogHelper.Error(ex.ToString());
                throw ex;
            }
        }

        public static Texture2D GetTextureFromAssemblyManifest(string file) {
            using (Stream stream = GetManifestResourceStream(file))
                return GetTextureFromStream(stream);
        }

        public static Stream GetManifestResourceStream(string file) {
            try {
                string path = string.Concat(PATH, file);
                return Assembly.GetExecutingAssembly().GetManifestResourceStream(path)
                    ?? throw new Exception(path + " not find");
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

        public static PluginInfo GetPlugin(Assembly assembly = null) {
            if (assembly == null)
                assembly = Assembly.GetExecutingAssembly();
            foreach (PluginInfo current in man.GetPluginsInfo()) {
                if (current.ContainsAssembly(assembly))
                    return current;
            }
            return null;
        }

        public static UITextureAtlas GetAtlas(string name) {
            UITextureAtlas[] atlases = Resources.FindObjectsOfTypeAll(typeof(UITextureAtlas)) as UITextureAtlas[];
            for (int i = 0; i < atlases.Length; i++) {
                if (atlases[i].name == name)
                    return atlases[i];
            }
            return UIView.GetAView().defaultAtlas;
        }
	}
}
