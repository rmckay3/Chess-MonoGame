namespace Utilities {
    public class DisplayBox {

        private string _header = String.Empty;
        private string _body = String.Empty;
        private int _width = 0;
        private int _height = 0;
        private int _margin = 0;
        private int _x = 0;
        private int _y = 0;

        private Rectangle background = null;
        private Rectangle body = null;
        private Vector2 fontOrigin = null;

        public string Header { get { return _header;} }
        public string Body { get { return _body;} }
        public int Width { get { return _width;} }
        public int Height { get { return _height;} }
        public int Margin { get { return _margin;} }
        public int X { get { return _x;} }
        public int Y { get { return _y;} }

        // Default Constructor
        public DisplayBox() 
        {
            _header = "Header";
            _body = "Body";
            _width = 128;
            _height = 64;
            _margin = 20;
        }

        ///
        /// Initialize Text for header and body
        /// 
        public DisplayBox(string header, string body) 
        {
            _header = header;
            _body = body;
            _width = 128;
            _height = 64
            _margin = 20;
        }

        ///
        /// Set Text in header and body, as well as height and width of Display Box
        /// 
        public DisplayBox(string header, string body, int width, int height, int margin = 20)
        {
            _header = header;
            _body = body;
            _width = width;
            _height = height;
            _margin = margin;
        }

        ///
        /// Set Text in header and body, well as height and width of Display Box, and location of DisplayBox
        /// 
        public DisplayBox(string header, string body, int width, int height, int x, int y, int margin = 20)
        {
            _header = header;
            _body = body;
            _width = width;
            _height = height;
            _margin = margin;
            _x = x;
            _y = y;
        }

        private void Initialize()
        {
            this.background = new Rectangle(x, y, width, height);
            this.body = new Rectangle(x, y - margin, width, height - margin);
            this.fontOrigin = 
        }
    }
}