using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Utilities.Extensions {
    public static class SpriteBatchExtensions {
        private static Texture2D _blankTexture = null;
        public static Texture2D BlankTexture(this SpriteBatch spriteBatch)
        {
            if (_blankTexture != null) return _blankTexture;

            _blankTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            _blankTexture.SetData(new Color[] { Color.White });

            return _blankTexture;
        }
        public static void DrawWithBorder(this SpriteBatch spriteBatch, Texture2D texture, Rectangle destinationRectangle, int borderWidth, Color borderColor, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            Rectangle newDestinationRectangle = new Rectangle(
                destinationRectangle.X + (borderWidth / 2),
                destinationRectangle.Y + (borderWidth / 2),
                destinationRectangle.Width - borderWidth,
                destinationRectangle.Height - borderWidth
            );

            Color[] colors = new Color[newDestinationRectangle.Width * newDestinationRectangle.Height]; 
            Color[] test = new Color[destinationRectangle.Width * destinationRectangle.Height];
            int startIndexForCopy = (destinationRectangle.Width * destinationRectangle.Height) - colors.Length;
            Texture2D updatedTexture = new Texture2D(texture.GraphicsDevice, texture.Width - borderWidth, texture.Height - borderWidth);
            //texture.GetData(colors, startIndexForCopy + 1, newDestinationRectangle.Width * newDestinationRectangle.Height);
            texture.GetData(test);
            colors = test.Skip(startIndexForCopy).ToArray();
            updatedTexture.SetData(colors);
            Texture2D borderTexture = new Texture2D(texture.GraphicsDevice, texture.Width, texture.Height);
            borderTexture.SetData(Enumerable.Repeat(borderColor, borderTexture.Width * borderTexture.Height).ToArray());

            float newDepth = layerDepth - 0.001f;
            // Draw border rectangle using a blank Texture and letting the Rectangle fill in the color
            spriteBatch.Draw(borderTexture, destinationRectangle, null, color, rotation, origin, effects, newDepth);

            // Reuse existing draw function, we are simply drawing a small border around the pulled in sprite
            spriteBatch.Draw(updatedTexture, newDestinationRectangle, null, color, rotation, origin, effects, layerDepth);


        }
    }
}