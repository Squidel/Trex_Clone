using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Trex_Clone.Visuals;

namespace Trex_Clone.Entities
{
    public class GameOver : IGameEntity
    {
        private const int GAME_OVER_DRAW_ORDER = 100;
        private const int GAME_OVER_POS_X = 655;
        private const int GAME_OVER_POS_Y = 15;
        public const int GAME_OVER_WIDTH = 192;
        private const int GAME_OVER_HEIGHT = 11;

        private const int GAME_OVER_IMAGE_POS_X = 1;
        private const int GAME_OVER_IMAGE_POS_Y = 1;
        private const int GAME_OVER_IMAGE_WIDTH = 38;
        private const int GAME_OVER_IMAGE_HEIGHT = 33;

        

        private Texture2D _texture;
        private Texture2D _mouseTexture;
        private MouseCursor _pointerCursor;
        private MouseCursor _originalCursor;

        private Sprite _textSprite;
        private Sprite _imageSprite;
        private Vector2 _buttonPos => new Vector2((Position.X + (GAME_OVER_WIDTH/2)- GAME_OVER_IMAGE_WIDTH/2), Position.Y+25);
        private Rectangle _gameOverImageBox => new Rectangle((int)Math.Round(_buttonPos.X), (int)Math.Round(_buttonPos.Y), GAME_OVER_IMAGE_WIDTH, GAME_OVER_IMAGE_HEIGHT);
        public int DrawOrder => 100;
        public Vector2 Position { get; set; }
        public bool IsEnabled { get; set; }
        public event EventHandler Reset;

        public GameOver(Texture2D texture2D, Texture2D mouseTexture)
        {
            _texture = texture2D;
            _textSprite = new Sprite(_texture, GAME_OVER_POS_X, GAME_OVER_POS_Y, GAME_OVER_WIDTH, GAME_OVER_HEIGHT, Color.White);
            _imageSprite = new Sprite(_texture, GAME_OVER_IMAGE_POS_X, GAME_OVER_IMAGE_POS_Y, GAME_OVER_IMAGE_WIDTH, GAME_OVER_IMAGE_HEIGHT, Color.White);
            _mouseTexture = mouseTexture;
            _originalCursor = MouseCursor.Arrow;
            _pointerCursor = MouseCursor.Hand;
            //_pointerCursor = MouseCursor.FromTexture2D(mouseTexture, 8,8);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if(!IsEnabled) return;
            _textSprite.Draw(spriteBatch, Position);
            _imageSprite.Draw(spriteBatch, _buttonPos);
        }

        public void Update(GameTime gameTime)
        {
            if(!IsEnabled) { return; }
            MouseState mouseState = Mouse.GetState();
            //Rectangle mouseRect = new Rectangle(mouseState.X, mouseState.Y, 32, 32);
            if (_gameOverImageBox.Contains(mouseState.Position))
            {
                Mouse.SetCursor(_pointerCursor);
            }
            else
            {
                Mouse.SetCursor(_originalCursor);
            }
            if (_gameOverImageBox.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed)
            {
                OnResetClicked();
                Mouse.SetCursor(_originalCursor);
            }

        }
        public void OnResetClicked()
        {
            EventHandler eventHandler = Reset;
            eventHandler?.Invoke(this, EventArgs.Empty);
        }
    }
}
