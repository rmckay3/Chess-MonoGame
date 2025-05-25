using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utilities;

namespace Managers {
    public class DisplayManager {
        private Hashtable _displayBoxes = new Hashtable();
        private GraphicsDevice graphicsDevice;
        private SpriteFont headerFont;
        private SpriteFont bodyFont;

        public DisplayManager() {

        }

        public void Load(GraphicsDevice graphicsDevice, SpriteFont headerFont, SpriteFont bodyFont)
        {
            this.graphicsDevice = graphicsDevice;
            this.headerFont = headerFont;
            this.bodyFont = bodyFont;
        }

        public void CreateDisplayBox(string name, string header, string body, int width, int height, int margin, int x, int y)
        {
            this._displayBoxes.Add(name, new DisplayBox(headerFont, bodyFont, header, body, width, height, x, y, margin));
        }

        public void UpdateDisplayBox(string name, string header, string body, int width, int height, int margin, int x, int y)
        {
            this._displayBoxes[name] = new DisplayBox(headerFont, bodyFont, header, body, width, height, x, y, margin);
        }

        public void AddButtonToDisplayBox(string nameOfDisplayBox, ButtonTypeEnum buttonType, int x, int y, int width, int height, SpriteFont textFont, Color color, string text = "", Action onClick = null)
        {
            if (!this._displayBoxes.ContainsKey(nameOfDisplayBox)) throw new ArgumentException("Invalid Display Box");

            ((DisplayBox)this._displayBoxes[nameOfDisplayBox]).AddButton(ButtonFactory.Create(buttonType, x, y, width, height, textFont, color, text, onClick));
        }

        public bool CheckIfDisplayBoxExists(string name)
        {
            return this._displayBoxes.ContainsKey(name);
        }

        public void Update(GameTime gameTime, InputStateManager inputStateManager)
        {
            foreach(DisplayBox box in this._displayBoxes.Values) {
                box.Update(gameTime, inputStateManager);
            }
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            foreach(DisplayBox box in this._displayBoxes.Values) {
                box.Draw(spriteBatch);
            }
        }
    }
}