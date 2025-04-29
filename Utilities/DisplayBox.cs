using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        private Rectangle background;
        private Rectangle body;
        private Texture2D backgroundTexture;
        private Texture2D bodyTexture;
        private Vector2 fontOriginHeader;
        private Vector2 fontOriginBody;

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
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(this._headerFont, this._header, new Vector2((2*this.background.X + this._width) / 2, this.background.Y + 10), Color.Black, 0, this.fontOriginHeader, 1.0f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(this._bodyFont, this._body, new Vector2((2*this.body.X + this._width) / 2, (2*this.body.Y + this._height - this._margin) / 2), Color.Black, 0, this.fontOriginBody, 1.0f, SpriteEffects.None, 1f);
            spriteBatch.Draw(backgroundTexture, this.background, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.98f);
            spriteBatch.Draw(bodyTexture, this.body, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.99f);
        }
    }
}