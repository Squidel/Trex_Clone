using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trex_Clone.Visuals;

namespace Trex_Clone.Entities
{
    public class Pteradactly : Obstacle
    {
        private const int FRAME_ONE_POS_X = 133;
        private const int FRAME_ONE_POS_Y = 2;
        private const int FRAME_TWO_POS_X = 0;
        private const int FRAME_TWO_POS_Y = 0;

        private const int SPRITE_WIDTH = 46;
        private const int SPRITE_HEIGHT = 39;

        private Trex _trex;

        private Sprite _frameOne;
        private Sprite _frameTwo;

        private SpriteAnimation _animation;

        private Texture2D _texture;

        private Vector2 _position;
        Random _random;
        public Pteradactly(Texture2D texture, Trex trex, Vector2 position) : base(trex, position)
        {
            _random = new Random();
            _trex = trex;
            _texture = texture;
            _position = position;

            _frameOne = new Sprite(texture, FRAME_ONE_POS_X, FRAME_ONE_POS_Y, SPRITE_WIDTH, SPRITE_HEIGHT, Color.White);
            _frameTwo = new Sprite(texture, FRAME_ONE_POS_X+SPRITE_WIDTH, FRAME_ONE_POS_Y, SPRITE_WIDTH, SPRITE_HEIGHT, Color.White);

            //var valBetweenFrames = _random.NextDouble() * 1/10f;
            _animation = new SpriteAnimation();
            _animation.AddFrame(_frameOne, 0);
            _animation.AddFrame(_frameTwo, 0.2f);
            _animation.AddFrame(_frameTwo, 0.2f*2);
            _animation.ShouldLoop = true;
            _animation.Play();

        }

        public override Rectangle CollisionBox
        {
            get {
                Rectangle rectangle = new Rectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), SPRITE_WIDTH, SPRITE_HEIGHT);
                rectangle.Inflate(-3,-3);
                return rectangle;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _animation.Draw(spriteBatch, Position);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _animation.Update(gameTime);
            Position = new Vector2(Position.X - 80f * (float)gameTime.ElapsedGameTime.TotalSeconds, Position.Y);
        }
    }
}
