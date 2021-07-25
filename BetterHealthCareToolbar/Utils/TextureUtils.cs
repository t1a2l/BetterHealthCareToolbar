using ColossalFramework.UI;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using static ColossalFramework.Plugins.PluginManager;
using ColossalFramework.Plugins;

namespace BetterHealthCareToolbar
{
	internal static class TextureUtils
	{
        static string ModPath => GetPlugin().modPath;
        public static string FILE_PATH = ModPath + "\\BetterHealthCareToolbar\\Utils\\Images\\";
        public static bool EmbededResources = false;
       
        static PluginManager man => PluginManager.instance;


        public static void AddTexturesInAtlas(UITextureAtlas atlas, Texture2D[] newTextures, bool locked = false) 
        {
            Texture2D[] textures = new Texture2D[atlas.count + newTextures.Length];

            for (int i = 0; i < atlas.count; i++) {
                Texture2D texture2D = atlas.sprites[i].texture;

                if (locked) {
                    // Locked textures workaround
                    RenderTexture renderTexture = RenderTexture.GetTemporary(texture2D.width, texture2D.height, 0);
                    Graphics.Blit(texture2D, renderTexture);

                    RenderTexture active = RenderTexture.active;
                    texture2D = new Texture2D(renderTexture.width, renderTexture.height);
                    RenderTexture.active = renderTexture;
                    texture2D.ReadPixels(new Rect(0f, 0f, (float)renderTexture.width, (float)renderTexture.height), 0, 0);
                    texture2D.Apply();
                    RenderTexture.active = active;

                    RenderTexture.ReleaseTemporary(renderTexture);
                }

                textures[i] = texture2D;
                textures[i].name = atlas.sprites[i].name;
            }

            for (int i = 0; i < newTextures.Length; i++)
                textures[atlas.count + i] = newTextures[i];

            Rect[] regions = atlas.texture.PackTextures(textures, atlas.padding, 4096, false);

            atlas.sprites.Clear();

            for (int i = 0; i < textures.Length; i++) {
                UITextureAtlas.SpriteInfo spriteInfo = atlas[textures[i].name];
                atlas.sprites.Add(new UITextureAtlas.SpriteInfo {
                    texture = textures[i],
                    name = textures[i].name,
                    border = (spriteInfo != null) ? spriteInfo.border : (i < 5 ? new RectOffset(1, 1, 0, 0) : new RectOffset()),
                    region = regions[i]
                });
            }

            atlas.RebuildIndexes();
        }

        public static UITextureAtlas GetAtlas(string name)
        {
            UITextureAtlas[] atlases = Resources.FindObjectsOfTypeAll(typeof(UITextureAtlas)) as UITextureAtlas[];
            for (int i = 0; i < atlases.Length; i++)
            {
                if (atlases[i].name == name)
                    return atlases[i];
            }

            return UIView.GetAView().defaultAtlas;
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
