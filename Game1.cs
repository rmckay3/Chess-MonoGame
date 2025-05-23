using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Managers;

namespace MonoGame;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpriteFont _font;
    private InputStateManager _inputStateManager;
    private BoardManager _boardManager;
    private TurnManager _turnManager;
    private StateManager _stateManager;
    private DisplayManager _displayManager;
    private TextureManager _textureManager;
    public static bool Quit = false;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _graphics.PreferredBackBufferWidth = 450;
        _graphics.PreferredBackBufferHeight = 450;
        _graphics.ApplyChanges();
        _inputStateManager = new InputStateManager();
        _boardManager = new BoardManager();
        _turnManager = new TurnManager();
        _stateManager = new StateManager();
        _displayManager = new DisplayManager();
        _textureManager = new TextureManager();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _font = Content.Load<SpriteFont>("Arial");

        // Load Texture Content
        _textureManager.AddTexture("DialogBox", Content.Load<Texture2D>("DialogBox"));
        _textureManager.AddTexture("Button", Content.Load<Texture2D>("Button"));
        _textureManager.AddTexture("Board", Content.Load<Texture2D>("board"));
        _textureManager.AddTexture("ChessPieceSprite", Content.Load<Texture2D>("Chess_Pieces_Sprite"));

        // Board should be able to fit 8 pieces across vertical and horizontal
        _inputStateManager.Load();
        _displayManager.Load(GraphicsDevice, _font, _font, 
        [
            Content.Load<Texture2D>("DialogBox"),
            Content.Load<Texture2D>("Button")
        ]);
        _boardManager.Load(GraphicsDevice, 
        [
            Content.Load<Texture2D>("board"),
            Content.Load<Texture2D>("Chess_Pieces_Sprite")
        ]);

        //_displayManager.CreateDisplayBox("Test", "Header", "This is a test", 300, 150, 30, 9 * 45 / 4, 9 * 45 / 4);
    }

    protected override void Update(GameTime gameTime)
    {
        if (Quit || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        _inputStateManager.Update();
        _boardManager.Update(_inputStateManager, _turnManager, _stateManager, _displayManager);
        _displayManager.Update(gameTime, _inputStateManager);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        // TODO: Add your drawing code here
        _spriteBatch.Begin(SpriteSortMode.FrontToBack);
        
        _boardManager.Draw(_spriteBatch, _turnManager);
        _displayManager.Draw(_spriteBatch);

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
