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
        public const int DUCKING_SPRITE_WIDTH = 59;
        public const int DUCKING_SPRITE_HEIGHT = 49;

        private const int IDLE_DEFAULT_POS_X = 40;
        private const int IDLE_DEFAULT_POS_Y = 0;

        private const float BlinkDuration = 0.5f;
        private const float MIN_JUMP_HEIGHT = 40f;

        private const float JUMP_START_VELOCITY = -480f;
        private const float GRAVITY = 1300f;
        private const float END_JUMP_VELOCITY = -60f;
        private const float DROP_VELOCITY = 600f;

        private const int RUNNING_ANIMATION_FRAME_ONE_X = Default_POS_X + (Default_Width * 2);
        //private const float RUNNING_ANIMATION_FRAME_ONE_Y;
        private const int RUNNING_ANIMATION_FRAME_TWO_X = Default_POS_X + (Default_Width * 3);

        private const int DUCKING_ANIMATION_FRAME_ONE_X = RUNNING_ANIMATION_FRAME_TWO_X + (Default_Width * 3);
        private const int DUCKING_ANIMATION_FRAME_ONE_Y = 0;
        private const int DUCKING_ANIMATION_FRAME_TWO_X = DUCKING_ANIMATION_FRAME_ONE_X + DUCKING_SPRITE_WIDTH;

        private const int TREX_DEAD_SPRITE_POS_X = 1068;

        private const float ACCELERATION_PPS_PER_SECOND = 5f;

        public const float START_SPEED = 250f;
        public const float MAX_SPEED = 900f;

        public int DrawOrder { get; set; }

        private Sprite _idleBackground;
        private Sprite _idleSprite;
        private Sprite _idleBlink;
        private Sprite _runningFrameOne;
        private Sprite _runningFrameTwo;
        private Sprite _duckingFrameOne;
        private Sprite _duckingFrameTwo;
        private Sprite _deadSprite;

        private SpriteAnimation _blinkAnimation;
        private SpriteAnimation _runningAnimation;
        private SpriteAnimation _duckingAnimation;

        private SoundEffect _jumpSound;

        private Random _random;

        private float _verticalVelocity;
        private float _dropVelocity;
        private float _startPositionY;

        public event EventHandler JumpComplete;
        public event EventHandler TrexDied;

        public Vector2 Position { get; set; }
        public TrexState State { get; private set; }
        public bool IsAlive { get; private set; }
        public float Speed { get; private set; }
        public Rectangle CollisionBox
        {
            get
            {
                Rectangle box = new Rectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), Default_Width, Default_Height);
                box.Inflate(-4, -4);
                if(State == TrexState.Ducking)
                {
                    box.Y += 20;
                    box.Height -= 20;
                }
                return box;
            }
        }

        public Trex(Texture2D texture2D, Vector2 position, SoundEffect jumpSound)
        {
            //Sprite = new Sprite(texture2D, Default_POS_X, Default_POS_Y, Default_Width, Default_Height, Color.White);
            _blinkAnimation = new SpriteAnimation();
            _runningAnimation = new SpriteAnimation();
            _duckingAnimation = new SpriteAnimation();
            Position = position;
            _startPositionY = Position.Y;
            _idleBackground = new Sprite(texture2D, IDLE_DEFAULT_POS_X, IDLE_DEFAULT_POS_Y, Default_Width, Default_Height, Color.White);
            State = TrexState.Idle;
            _jumpSound = jumpSound;

            _random = new Random();
            _idleSprite = new Sprite(texture2D, Default_POS_X, Default_POS_Y, Default_Width, Default_Height, Color.White);
            _idleBlink = new Sprite(texture2D, Default_POS_X + Default_Width, Default_POS_Y, Default_Width, Default_Height, Color.White);

            _runningFrameOne = new Sprite(texture2D, RUNNING_ANIMATION_FRAME_ONE_X, Default_POS_Y, Default_Width, Default_Height, Color.White);
            _runningFrameTwo = new Sprite(texture2D, RUNNING_ANIMATION_FRAME_TWO_X, Default_POS_Y, Default_Width, Default_Height, Color.White);

            _duckingFrameOne = new Sprite(texture2D, DUCKING_ANIMATION_FRAME_ONE_X, DUCKING_ANIMATION_FRAME_ONE_Y, DUCKING_SPRITE_WIDTH, DUCKING_SPRITE_HEIGHT, Color.White);
            _duckingFrameTwo = new Sprite(texture2D, DUCKING_ANIMATION_FRAME_TWO_X, DUCKING_ANIMATION_FRAME_ONE_Y, DUCKING_SPRITE_WIDTH, DUCKING_SPRITE_HEIGHT, Color.White);

            _deadSprite = new Sprite(texture2D, TREX_DEAD_SPRITE_POS_X, DUCKING_ANIMATION_FRAME_ONE_Y, Default_Width, Default_Height, Color.White);

            CreateBlinkAnimation();
            _blinkAnimation.Play();

            CreateRunningAnimation();
            _runningAnimation.Play();

            CreateDuckingAnimation();
            _duckingAnimation.Play();

            IsAlive = true;

        }

        #region AnimationObjects
        private void CreateBlinkAnimation()
        {
            _blinkAnimation.Clear();
            _blinkAnimation.ShouldLoop = false;

            double blinkTimeStamp = 2f + _random.NextDouble() * (10f - 2f);

            _blinkAnimation.AddFrame(_idleSprite, 0);
            _blinkAnimation.AddFrame(_idleBlink, (float)blinkTimeStamp);
            _blinkAnimation.AddFrame(_idleSprite, (float)blinkTimeStamp + BlinkDuration);

        }
        private void CreateRunningAnimation()
        {
            _runningAnimation.Clear();
            _runningAnimation.ShouldLoop = true;

            _runningAnimation.AddFrame(_runningFrameOne, 0);
            _runningAnimation.AddFrame(_runningFrameTwo, 1 / 10f);
            _runningAnimation.AddFrame(_runningFrameOne, 1 / 10f * 2);
        }
        private void CreateDuckingAnimation()
        {
            _duckingAnimation.Clear();
            _duckingAnimation.ShouldLoop = true;

            _duckingAnimation.AddFrame(_duckingFrameOne, 0);
            _duckingAnimation.AddFrame(_duckingFrameTwo, 1 / 10f);
            _duckingAnimation.AddFrame(_duckingFrameOne, 1 / 10f * 2);
        }
        #endregion


        //This section should deal with drawing and upddating the entity
        #region EntityEvents
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (IsAlive)
            {
                if (State == TrexState.Idle)
                {
                    _idleBackground.Draw(spriteBatch, Position);
                    _blinkAnimation.Draw(spriteBatch, Position);
                }
                else if (State == TrexState.Jumping || State == TrexState.Falling)
                {
                    _idleSprite.Draw(spriteBatch, Position);
                }
                else if (State == TrexState.Running)
                {
                    _runningAnimation.Draw(spriteBatch, Position);
                }
                else if (State == TrexState.Ducking)
                {
                    _duckingAnimation.Draw(spriteBatch, Position);
                }
            }
            else
            {
                _deadSprite.Draw(spriteBatch, Position);
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
            }
            else if (State == TrexState.Jumping || State == TrexState.Falling)
            {

                var diffentialPosition = _verticalVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds + _dropVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Position = new Vector2(Position.X, Position.Y + diffentialPosition);
                _verticalVelocity += GRAVITY * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_verticalVelocity >= 0)
                {
                    State = TrexState.Falling;
                }
                if (Position.Y >= _startPositionY)
                {
                    Position = new Vector2(Position.X, _startPositionY);
                    _verticalVelocity = 0;
                    State = TrexState.Running;
                    OnJumpComplete();
                }
            }

            else if (State == TrexState.Running)
            {
                Position = new Vector2(Position.X, Position.Y);
                _runningAnimation.Update(gameTime);
            }
            else if (State == TrexState.Ducking)
            {
                _duckingAnimation.Update(gameTime);
            }

            if (State != TrexState.Idle)
            {
                Speed += ACCELERATION_PPS_PER_SECOND * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (Speed > MAX_SPEED)
            {
                Speed = MAX_SPEED;
            }
            _dropVelocity = 0;
        }

        public void Initialize()
        {
            Speed = START_SPEED;
            IsAlive = true;
            State = TrexState.Running;
            Position = new Vector2(Position.X, _startPositionY);
        }
        #endregion


        #region Entity Actions
        public bool BeginJump()
        {
            if (State == TrexState.Jumping || State == TrexState.Falling)
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
            if (State != TrexState.Jumping || (_startPositionY - Position.Y) < MIN_JUMP_HEIGHT)
            {
                return false;
            }
            //State = TrexState.Falling;
            _verticalVelocity = _verticalVelocity < END_JUMP_VELOCITY ? END_JUMP_VELOCITY : 0;
            return true;
        }
        public bool BeginDucking()
        {
            if (State != TrexState.Running)
            {
                return false;
            }
            State = TrexState.Ducking;
            return true;
        }
        public bool EndDucking()
        {
            if (State != TrexState.Ducking)
            {
                return false;
            }
            State = TrexState.Running;
            return true;
        }
        public bool Drop()
        {
            if (State != TrexState.Falling && State != TrexState.Jumping)
            {
                return false;
            }
            State = TrexState.Falling;
            _dropVelocity = DROP_VELOCITY;
            return true;
        }

        public bool Die()
        {
            if (!IsAlive) return false;

            State = TrexState.Idle;
            Speed = 0;

            IsAlive = false;

            OnDeath();
            return true;
        }


        #endregion

        #region Events

        protected virtual void OnJumpComplete()
        {
            EventHandler handler = JumpComplete;
            handler?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnDeath()
        {
            EventHandler eventHandler = TrexDied;
            eventHandler?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}


