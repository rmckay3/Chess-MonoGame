using System;
using Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame;

namespace Utilities
{
    public static class ButtonFactory {
        public static Button Create(ButtonTypeEnum buttonType, int x, int y, int width, int height, SpriteFont textFont, Color color, string text = "", Action onClick = null)
        {
            switch (buttonType) {
                case ButtonTypeEnum.OK:
                    return new Button(x, y, width, height, "OK", textFont,() => { Game1.Quit = true; });
                case ButtonTypeEnum.Cancel:
                    return new Button(x, y, width, height, "Cancel", textFont,() => { Game1.Quit = true; });
                case ButtonTypeEnum.Next:
                case ButtonTypeEnum.Previous:
                    throw new NotImplementedException();
                case ButtonTypeEnum.Custom:
                    return new Button(x, y, width, height, text, textFont, onClick);
                default:
                    throw new ArgumentException("Please input a Valid Button Type");
            }
        }
    }
}