using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;

namespace Managers {
    public class TextureManager
    {
        private Hashtable _textureDictionary;

        private Dictionary<string, TextureAtlas> _textureAtlases;

        private static TextureManager _instance;

        public static TextureManager Instance { get { if (_instance == null) _instance = new TextureManager(); return _instance; } }

        private TextureManager()
        {
            _textureDictionary = [];
            _textureAtlases = [];
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

        public void AddAtlasFromFile(ContentManager content, string name, string fileName)
        {
            if (_textureAtlases.ContainsKey(name)) throw new ArgumentException($"Texture Atlas {name} already exists");

            _textureAtlases[name] = TextureAtlas.FromFile(content, fileName);
        }

        public TextureAtlas GetTextureAtlas(string name)
        {
            if (!_textureAtlases.ContainsKey(name)) throw new ArgumentException($"Texture Atlas: {name} does not exist");

            return _textureAtlases[name];
        }

        public TextureRegion GetTextureRegionFromAtlas(string atlasName, string regionName)
        {
            if (!_textureAtlases.ContainsKey(atlasName)) throw new ArgumentException($"Texture Atlas: {atlasName} does not exist");

            return _textureAtlases[atlasName].GetRegion(regionName);
        }
    }
}