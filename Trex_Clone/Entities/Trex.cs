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
    public class Trex : IGameEntity
    {
        private const int Default_POS_X = 848;
        private const int Default_POS_Y = 0;
        public const int Default_Width = 44;
        public const int Default_Height = 49;
        private const int IDLE_DEFAULT_POS_X = 40;
        private const int IDLE_DEFAULT_POS_Y = 0;

        public int DrawOrder { get; set; }
        //public Sprite Sprite { get; private set; }
        private Sprite _idleBackground;

        private Sprite _idleSprite;
        private Sprite _idleBlink;
        private SpriteAnimation _blinkAnimation;
        private Random _random;
        public Vector2 Position { get; set; }
        public TrexState State { get; private set; }
        public bool IsAlive { get; private set; }
        public float Speed { get; private set; }

        public Trex(Texture2D texture2D, Vector2 position)
        {
            //Sprite = new Sprite(texture2D, Default_POS_X, Default_POS_Y, Default_Width, Default_Height, Color.White);
            Position = position;
            _idleBackground = new Sprite(texture2D, IDLE_DEFAULT_POS_X, IDLE_DEFAULT_POS_Y, Default_Width, Default_Height, Color.White);
            State = TrexState.Idle;

            _random = new Random();
            _idleSprite = new Sprite(texture2D, Default_POS_X, Default_POS_Y, Default_Width, Default_Height, Color.White);
            _idleBlink = new Sprite(texture2D, Default_POS_X + Default_Width, Default_POS_Y, Default_Width, Default_Height, Color.White);

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (State == TrexState.Idle)
            {
                _idleBackground.Draw(spriteBatch, Position);
                _blinkAnimation.Draw(spriteBatch, Position);
            }

        }

        public void Update(GameTime gameTime)
        {
            if (State == TrexState.Idle)
            {
                _blinkAnimation.Update(gameTime);
            }
        }

        private SpriteAnimation CreateBlinkAnimation(Texture2D texture2D)
        {

            _blinkAnimation = new SpriteAnimation();
            _blinkAnimation.ShouldLoop = false;


            double blinkTimeStamp = 2f + _random.NextDouble() * (10f - 2f);

            _blinkAnimation.AddFrame(_idleSprite, 0);
            _blinkAnimation.AddFrame(_idleBlink,(float)blinkTimeStamp);
            _blinkAnimation.AddFrame(_idleSprite, 1 / 20f * 2);
        }
    }
}
