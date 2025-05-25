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

        private Rectangle body;
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
        public DisplayBox(SpriteFont headerFont, SpriteFont bodyFont) 
        {
            _headerFont = headerFont;
            _bodyFont = bodyFont;
            _header = "Header";
            _body = "Body";
            _width = 128;
            _height = 64;
            _margin = 20;

            Initialize();
        }

        ///
        /// Initialize Text for header and body
        /// 
        public DisplayBox(SpriteFont headerFont, SpriteFont bodyFont, string header, string body) 
        {
            _headerFont = headerFont;
            _bodyFont = bodyFont;
            _header = header;
            _body = body;
            _width = 128;
            _height = 64;
            _margin = 20;

            Initialize();
        }

        ///
        /// Set Text in header and body, as well as height and width of Display Box
        /// 
        public DisplayBox(SpriteFont headerFont, SpriteFont bodyFont, string header, string body, int width, int height, int margin = 20)
        {
            _headerFont = headerFont;
            _bodyFont = bodyFont;
            _header = header;
            _body = body;
            _width = width;
            _height = height;
            _margin = margin;

            Initialize();
        }

        ///
        /// Set Text in header and body, well as height and width of Display Box, and location of DisplayBox
        /// 
        public DisplayBox(SpriteFont headerFont, SpriteFont bodyFont, string header, string body, int width, int height, int x, int y, int margin = 20)
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

            Initialize();
        }

        private void Initialize()
        {
            this.body = new Rectangle(this._x, this._y + this._margin, this._width, this._height - this._margin);
            this.fontOriginHeader = _headerFont.MeasureString(this._header) / 2;
            this.fontOriginBody = _bodyFont.MeasureString(this._body) / 2;
            this.headerLocation = new Vector2(this._x + (this._width / 2), this._y + (this._margin / 2));
            this.bodyLocation = new Vector2(this._x + (this._width / 2), this._y + ((this._height + this._margin) / 2));

            this._buttons = new List<Button>()
            {
                new Button(this._x + (this._width / 2) - 30, this._y + (9*(this._height - this._margin) / 10) + 10, 60, 20, "OK", this._bodyFont, () => { Game1.Quit = true; })
            };
        }

        public void AddButton(Button button)
        {
            this._buttons.Add(button);
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
            spriteBatch.Draw(TextureManager.Instance.GetTexture("DialogBox"), this.body, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.90f);

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
        private Vector2 fontOrigin;
        private Vector2 textLocation;

        public int Width { get { return _width;} }
        public int Height { get { return _height;} }
        public int X { get { return _x;} }
        public int Y { get { return _y;} }
        public string Text { get { return _text;} }
        public Action Click { get { return _onClick; } }

        public Button(int x, int y, int width, int height, string text, SpriteFont textFont) {
            this._width = width;
            this._height = height;
            this._x = x;
            this._y = y;
            this._text = text;
            this.textFont = textFont;
            this.textFont = textFont;
            
            Initialize();
        }

        public Button(int x, int y, int width, int height, string text, SpriteFont textFont, Action onClick) {
            this._width = width;
            this._height = height;
            this._x = x;
            this._y = y;
            this._text = text;
            this.textFont = textFont;
            this._onClick = onClick;

            Initialize();
        }

        private void Initialize() {
            this.btnBackground = new Rectangle(this._x, this._y, this._width, this._height);
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
            spriteBatch.Draw(TextureManager.Instance.GetTexture("Button"), this.btnBackground, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.93f);
        }
    }
}