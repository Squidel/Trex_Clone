using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trex_Clone.System;
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

        private Sprite _textSprite;
        private Sprite _imageSprite;
        private Vector2 _buttonPos => new Vector2((Position.X + (GAME_OVER_WIDTH/2)- GAME_OVER_IMAGE_WIDTH/2), Position.Y+25);
        public int DrawOrder => 100;
        public Vector2 Position { get; set; }
        public bool IsEnabled { get; set; }

        public GameOver(Texture2D texture2D)
        {
            _texture = texture2D;
            _textSprite = new Sprite(_texture, GAME_OVER_POS_X, GAME_OVER_POS_Y, GAME_OVER_WIDTH, GAME_OVER_HEIGHT, Color.White);
            _imageSprite = new Sprite(_texture, GAME_OVER_IMAGE_POS_X, GAME_OVER_IMAGE_POS_Y, GAME_OVER_IMAGE_WIDTH, GAME_OVER_IMAGE_HEIGHT, Color.White);
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
        }
    }
}
