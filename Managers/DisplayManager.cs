using System.Collections;
using System.Collections.Generic;
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
            this._displayBoxes.Add(name, new DisplayBox(graphicsDevice, headerFont, bodyFont, header, body, width, height, x, y, margin));
        }

        public void UpdateDisplayBox(string name, string header, string body, int width, int height, int margin, int x, int y)
        {
            this._displayBoxes[name] = new DisplayBox(graphicsDevice, headerFont, bodyFont, header, body, width, height, x, y, margin);
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            foreach(DisplayBox box in this._displayBoxes.Values) {
                box.Draw(spriteBatch);
            }
        }
    }
}