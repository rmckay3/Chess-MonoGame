using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utilities.Extensions;
using Managers;
using MonoGame;

namespace Utilities {
    public class DisplayBox {

        private string _header = String.Empty;
        private string _body = String.Empty;
        private int _width = 0;
        private int _height = 0;
        private int _margin = 0;
        private int _x = 0;
        private int _y = 0;
        private SpriteFont _headerFont;
        private SpriteFont _bodyFont;
        private List<Button> _buttons;

        private Rectangle background;
        private Rectangle body;
        private Texture2D backgroundTexture;
        private Texture2D bodyTexture;
        private Vector2 fontOriginHeader;
        private Vector2 fontOriginBody;
        private Vector2 headerLocation;
        private Vector2 bodyLocation;

        public string Header { get { return _header;} }
        public string Body { get { return _body;} }
        public int Width { get { return _width;} }
        public int Height { get { return _height;} }
        public int Margin { get { return _margin;} }
        public int X { get { return _x;} }
        public int Y { get { return _y;} }

        // Default Constructor
        public DisplayBox(GraphicsDevice graphicsDevice, SpriteFont headerFont, SpriteFont bodyFont) 
        {
            _headerFont = headerFont;
            _bodyFont = bodyFont;
            _header = "Header";
            _body = "Body";
            _width = 128;
            _height = 64;
            _margin = 20;

            Initialize(graphicsDevice);
        }

        ///
        /// Initialize Text for header and body
        /// 
        public DisplayBox(GraphicsDevice graphicsDevice, SpriteFont headerFont, SpriteFont bodyFont, string header, string body) 
        {
            _headerFont = headerFont;
            _bodyFont = bodyFont;
            _header = header;
            _body = body;
            _width = 128;
            _height = 64;
            _margin = 20;

            Initialize(graphicsDevice);
        }

        ///
        /// Set Text in header and body, as well as height and width of Display Box
        /// 
        public DisplayBox(GraphicsDevice graphicsDevice, SpriteFont headerFont, SpriteFont bodyFont, string header, string body, int width, int height, int margin = 20)
        {
            _headerFont = headerFont;
            _bodyFont = bodyFont;
            _header = header;
            _body = body;
            _width = width;
            _height = height;
            _margin = margin;

            Initialize(graphicsDevice);
        }

        ///
        /// Set Text in header and body, well as height and width of Display Box, and location of DisplayBox
        /// 
        public DisplayBox(GraphicsDevice graphicsDevice, SpriteFont headerFont, SpriteFont bodyFont, string header, string body, int width, int height, int x, int y, int margin = 20)
        {
            _headerFont = headerFont;
            _bodyFont = bodyFont;
            _header = header;
            _body = body;
            _width = width;
            _height = height;
            _margin = margin;
            _x = x;
            _y = y;

            Initialize(graphicsDevice);
        }

        private void Initialize(GraphicsDevice graphicsDevice)
        {
            this.background = new Rectangle(this._x, this._y, this._width, this._height);
            this.body = new Rectangle(this._x, this._y + this._margin, this._width, this._height - this._margin);
            this.backgroundTexture = new Texture2D(graphicsDevice, _width, _height);
            this.bodyTexture = new Texture2D(graphicsDevice, _width, _height - _margin);
            this.backgroundTexture.SetData(Enumerable.Repeat(new Color(Color.RoyalBlue, 1.0f), this._width * this._height).ToArray());
            this.bodyTexture.SetData(Enumerable.Repeat(new Color(Color.LightGray, 1.0f), this._width * (this._height - this._margin)).ToArray());
            this.fontOriginHeader = _headerFont.MeasureString(this._header) / 2;
            this.fontOriginBody = _bodyFont.MeasureString(this._body) / 2;
            this.headerLocation = new Vector2(this._x + (this._width / 2), this._y + (this._margin / 2));
            this.bodyLocation = new Vector2(this._x + (this._width / 2), this._y + ((this._height + this._margin) / 2));

            this._buttons = new List<Button>()
            {
                new Button(graphicsDevice, this._x + (this._width / 2) - 30, this._y + (9*(this._height - this._margin) / 10) + 10, 60, 20, "OK", this._bodyFont, Color.DarkGray, () => { Game1.Quit = true; })
            };
        }

        public void Update(GameTime gameTime, InputStateManager inputStateManager) {
            foreach (var button in this._buttons) {
                if (inputStateManager.LeftClickEvent && button.IsClicked(inputStateManager)) button.Click();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(this._headerFont, this._header, this.headerLocation, Color.Black, 0, this.fontOriginHeader, 1.0f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(this._bodyFont, this._body, this.bodyLocation, Color.Black, 0, this.fontOriginBody, 1.0f, SpriteEffects.None, 1f);
            spriteBatch.DrawWithBorder(backgroundTexture, this.background, 6, Color.Black, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.90f);
            spriteBatch.DrawWithBorder(bodyTexture, this.body, 6, Color.Black, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.91f);

            foreach (var button in this._buttons) {
                button.Draw(spriteBatch);
            }
        }
    }

    public class Button {
        private int _width = 0;
        private int _height = 0;
        private int _x = 0;
        private int _y = 0;
        private string _text = "";
        private Action _onClick = null;
        private SpriteFont textFont;

        private Rectangle btnBackground;
        private Texture2D btnTexture;
        private Color btnColor;
        private Vector2 fontOrigin;
        private Vector2 textLocation;

        public int Width { get { return _width;} }
        public int Height { get { return _height;} }
        public int X { get { return _x;} }
        public int Y { get { return _y;} }
        public string Text { get { return _text;} }
        public Action Click { get { return _onClick; } }

        public Button(GraphicsDevice graphicsDevice, int x, int y, int width, int height, string text, SpriteFont textFont, Color color) {
            this._width = width;
            this._height = height;
            this._x = x;
            this._y = y;
            this._text = text;
            this.textFont = textFont;
            this.btnColor = color;
            this.textFont = textFont;
            
            Initialize(graphicsDevice);
        }

        public Button(GraphicsDevice graphicsDevice, int x, int y, int width, int height, string text, SpriteFont textFont, Color color, Action onClick) {
            this._width = width;
            this._height = height;
            this._x = x;
            this._y = y;
            this._text = text;
            this.textFont = textFont;
            this.btnColor = color;
            this._onClick = onClick;

            Initialize(graphicsDevice);
        }

        private void Initialize(GraphicsDevice graphicsDevice) {
            this.btnBackground = new Rectangle(this._x, this._y, this._width, this._height);
            this.btnTexture = new Texture2D(graphicsDevice, this._width, this._height);
            this.btnTexture.SetData(Enumerable.Repeat(new Color(this.btnColor, 1.0f), this._width * this._height).ToArray());
            this.fontOrigin = this.textFont.MeasureString(this._text) / 2;
            this.textLocation = new Vector2(this._x + (this._width / 2), this._y + (this._height / 2));
        }

        public bool IsClicked(InputStateManager inputStateManager) {

            return  inputStateManager.CurrentMouseState.X >= this._x && 
                    inputStateManager.CurrentMouseState.X <= this._x + this._width &&
                    inputStateManager.CurrentMouseState.Y >= this._y &&
                    inputStateManager.CurrentMouseState.Y <= this._y + this._height;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(this.textFont, this._text, this.textLocation, Color.Black, 0, this.fontOrigin, 0.7f, SpriteEffects.None, 1f);
            spriteBatch.DrawWithBorder(this.btnTexture, this.btnBackground, 6, Color.Black, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.93f);
        }
    }
}