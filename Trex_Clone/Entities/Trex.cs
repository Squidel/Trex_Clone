using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        private const float BlinkDuration = 0.5f;

        private const float JUMP_START_VELOCITY = -200f;

        public int DrawOrder { get; set; }
        //public Sprite Sprite { get; private set; }
        private Sprite _idleBackground;

        private Sprite _idleSprite;
        private Sprite _idleBlink;
        private SpriteAnimation _blinkAnimation;
        private Random _random;

        private float _verticalVelocity;
        public Vector2 Position { get; set; }
        public TrexState State { get; private set; }
        public bool IsAlive { get; private set; }
        public float Speed { get; private set; }
        private SoundEffect _jumpSound;
        

        public Trex(Texture2D texture2D, Vector2 position, SoundEffect jumpSound)
        {
            //Sprite = new Sprite(texture2D, Default_POS_X, Default_POS_Y, Default_Width, Default_Height, Color.White);
            _blinkAnimation = new SpriteAnimation();
            Position = position;
            _idleBackground = new Sprite(texture2D, IDLE_DEFAULT_POS_X, IDLE_DEFAULT_POS_Y, Default_Width, Default_Height, Color.White);
            State = TrexState.Idle;
            _jumpSound = jumpSound;

            _random = new Random();
            _idleSprite = new Sprite(texture2D, Default_POS_X, Default_POS_Y, Default_Width, Default_Height, Color.White);
            _idleBlink = new Sprite(texture2D, Default_POS_X + Default_Width, Default_POS_Y, Default_Width, Default_Height, Color.White);

            CreateBlinkAnimation();
            _blinkAnimation.Play();

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (State == TrexState.Idle)
            {
                _idleBackground.Draw(spriteBatch, Position);
                _blinkAnimation.Draw(spriteBatch, Position);
            }else if (State == TrexState.Jumping || State == TrexState.Falling)
            {
                _idleSprite.Draw(spriteBatch, Position);
            }

        }

        public void Update(GameTime gameTime)
        {
            if (State == TrexState.Idle)
            {

                if (!_blinkAnimation.IsPlaying)
                {
                    CreateBlinkAnimation();
                    _blinkAnimation.Play();
                }
                _blinkAnimation.Update(gameTime);
            }else if(State == TrexState.Jumping || State == TrexState.Falling)
            {
                Position = new Vector2(Position.X, Position.Y + _verticalVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }

        private void CreateBlinkAnimation()
        {

            _blinkAnimation.Clear();
            _blinkAnimation.ShouldLoop = false;


            double blinkTimeStamp = 2f + _random.NextDouble() * (10f - 2f);

            _blinkAnimation.AddFrame(_idleSprite, 0);
            _blinkAnimation.AddFrame(_idleBlink, (float)blinkTimeStamp);
            _blinkAnimation.AddFrame(_idleSprite, (float)blinkTimeStamp + BlinkDuration);


        }
        public bool BeginJump()
        {
            if(State == TrexState.Jumping ||  State == TrexState.Falling)
            {
                return false;
            }
            _jumpSound.Play();
            State = TrexState.Jumping;
            _verticalVelocity = JUMP_START_VELOCITY;
            return true;
        }
        public bool EndJump()
        {
            return true;
        }
    }
}
