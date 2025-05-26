using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using Managers;

namespace MonoGame;

public class Game1 : Core
{
    private SpriteFont _font;
    private InputStateManager _inputStateManager = new InputStateManager();
    private BoardManager _boardManager = new BoardManager();
    private TurnManager _turnManager = new TurnManager();
    private StateManager _stateManager = new StateManager();
    private DisplayManager _displayManager = new DisplayManager();
    public static bool Quit = false;

    public Game1() : base("Chess", 450, 450, false)
    {

    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _font = Content.Load<SpriteFont>("Arial");

        // Load Texture Content
        TextureManager.Instance.AddTexture("DialogBox", Content.Load<Texture2D>("DialogBox"));
        TextureManager.Instance.AddTexture("Button", Content.Load<Texture2D>("Button"));
        TextureManager.Instance.AddTexture("Board", Content.Load<Texture2D>("board"));

        TextureManager.Instance.AddAtlasFromFile(Content, "chessPieces", "chessAtlas.xml");

        // Board should be able to fit 8 pieces across vertical and horizontal
        _inputStateManager.Load();
        _displayManager.Load(_font, _font);
        _boardManager.Load();

        //_displayManager.CreateDisplayBox("Test", "", "This is a test", 300, 150, 30, 9 * 45 / 4, 9 * 45 / 4);
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
        SpriteBatch.Begin(SpriteSortMode.FrontToBack);
        
        _boardManager.Draw(SpriteBatch, _turnManager);
        _displayManager.Draw(SpriteBatch);

        SpriteBatch.End();
        base.Draw(gameTime);
    }
}
