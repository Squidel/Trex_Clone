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

            if (!_previousKeyboardState.IsKeyDown(Keys.Space) && keyboardState.IsKeyDown(Keys.Space))
            {
                if(_trex.State != TrexState.Jumping)
                {
                    _trex.BeginJump();
                }
                else
                {
                    _trex.EndJump();
                }
                
            }
            _previousKeyboardState = keyboardState;
        }
    }
}
