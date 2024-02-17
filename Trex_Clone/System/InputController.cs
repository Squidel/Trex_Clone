using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trex_Clone.Entities;

namespace Trex_Clone.System
{
    public class InputController
    {
        private Trex _trex;
        private KeyboardState _previousKeyboardState;

        public InputController(Trex trex)
        {
            _trex = trex;
        }
        public void ProcessControls(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            bool isJumpKeyPressed = keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.Space);
            bool wasJumpKeyPressed = _previousKeyboardState.IsKeyDown(Keys.Up) || _previousKeyboardState.IsKeyDown(Keys.Space);
            if (!wasJumpKeyPressed && isJumpKeyPressed)
            {
                if (_trex.State != TrexState.Jumping)
                {
                    _trex.BeginJump();
                }

            }
            else if (!isJumpKeyPressed && _trex.State == TrexState.Jumping)
            {
                _trex.EndJump();
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                if (_trex.State == TrexState.Jumping || _trex.State == TrexState.Falling)
                {
                    _trex.Drop();
                }
                else
                {
                    _trex.BeginDucking();
                }

            }
            else if (!keyboardState.IsKeyDown(Keys.Down) && _trex.State == TrexState.Ducking)
            {
                _trex.EndDucking();
            }

            _previousKeyboardState = keyboardState;
        }
    }
}
