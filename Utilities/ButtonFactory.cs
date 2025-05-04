using System;
using Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame;

namespace Utilities
{
    public static class ButtonFactory {
        public static Button Create(ButtonTypeEnum buttonType, GraphicsDevice graphicsDevice, int x, int y, int width, int height, SpriteFont textFont, Color color, string text = "", Action onClick = null)
        {
            switch (buttonType) {
                case ButtonTypeEnum.OK:
                    return new Button(graphicsDevice, x, y, width, height, "OK", textFont, color, () => { Game1.Quit = true; });
                case ButtonTypeEnum.Cancel:
                    return new Button(graphicsDevice, x, y, width, height, "Cancel", textFont, color, () => { Game1.Quit = true; });
                case ButtonTypeEnum.Next:
                case ButtonTypeEnum.Previous:
                    throw new NotImplementedException();
                case ButtonTypeEnum.Custom:
                    return new Button(graphicsDevice, x, y, width, height, text, textFont, color, onClick);
                default:
                    throw new ArgumentException("Please input a Valid Button Type");
            }
        }
    }
}