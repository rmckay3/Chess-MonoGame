using System.Collections;
using Microsoft.Xna.Framework.Graphics;

namespace Managers {
    public class TextureManager {
        private static Hashtable _textureDictionary;

        public static Hashtable TextureDictionary { get { return _textureDictionary; } }

        public TextureManager() {
            _textureDictionary = [];
        }

        public void AddTexture(string name, Texture2D texture) {
            if (_textureDictionary.ContainsKey(name)) 
                _textureDictionary[name] = texture;
            else 
                _textureDictionary.Add(name, texture);
        }
    }
}