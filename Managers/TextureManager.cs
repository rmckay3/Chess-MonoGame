using System.Collections;
using Microsoft.Xna.Framework.Graphics;

namespace Managers {
    public class TextureManager
    {
        private Hashtable _textureDictionary;

        private static TextureManager _instance;

        public static TextureManager Instance { get { if (_instance == null) _instance = new TextureManager(); return _instance; } }

        private TextureManager()
        {
            _textureDictionary = [];
        }

        public void AddTexture(string name, Texture2D texture)
        {
            if (_textureDictionary.ContainsKey(name))
                _textureDictionary[name] = texture;
            else
                _textureDictionary.Add(name, texture);
        }

        public Texture2D GetTexture(string name)
        {
            if (_textureDictionary.ContainsKey(name))
                return (Texture2D)_textureDictionary[name];

            return null;
        }
    }
}