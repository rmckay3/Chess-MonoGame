using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Managers
{
    public class InputStateManager
    {

        KeyboardState _oldKeyboardState;
        KeyboardState _newKeyboardState;
        MouseState _oldMouseState;
        MouseState _newMouseState;
        public KeyboardState OldKeyBoardState { get { return _oldKeyboardState; } }
        public KeyboardState CurrentKeyBoardState { get { return _newKeyboardState; } }
        public MouseState OldMouseState { get { return _oldMouseState; } }
        public MouseState CurrentMouseState { get { return _newMouseState; } }
        public bool LeftClickEvent { get { return _oldMouseState.LeftButton == ButtonState.Released && _newMouseState.LeftButton == ButtonState.Pressed; }}

        public InputStateManager()
        {

        }

        public void Initialize()
        {

        }

        public void Load()
        {
            _newKeyboardState = Keyboard.GetState();
            _newMouseState = Mouse.GetState();
        }

        public void Update()
        {
            _oldKeyboardState = _newKeyboardState;
            _newKeyboardState = Keyboard.GetState();

            _oldMouseState = _newMouseState;
            _newMouseState = Mouse.GetState();
        }
    }
}