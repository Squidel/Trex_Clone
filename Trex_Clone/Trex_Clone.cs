using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Trex_Clone.Entities;
using Trex_Clone.System;
using Trex_Clone.Visuals;

namespace Trex_Clone
{
    public class Trex_Clone : Game
    {
        public const int WindowWidth = 600;
        public const int WindowHeight = 150;

        public const int TREX_START_POS_X = 1;
        public const int TREX_START_POS_Y = WindowHeight - 16;

        private const string MOUSE_TEXTURE = "6_8mouse_pointer";
        private const string ASSET_NAME_SPRITESHEET = "100-offline-sprite";
        private const string ASSET_NAME_HIT = "1374573";
        private const string ASSET_NAME_SCORED = "1374572";
        private const string ASSET_NAME_JUMP = "1374571";
        private const float FADE_IN_ANIMATION_SPEED = 800f;
        private const int SCORE_BOARD_POS_X = WindowWidth - 150;
        private const int SCORE_BOARD_POS_Y = 10;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SoundEffect _sfxHit;
        private SoundEffect _sfxJump;
        private SoundEffect _sfxScored;

        private Texture2D _spriteTexture;
        private Texture2D _fadeInTexture;

        private float _fadeInTexturePOSX;

        private Trex _trex;
        private GroundManager _groundManager;

        private InputController _controller;
        private EntityManager _entityManager;

        private ScoreBoard _scoreBoard;
        private ObstacleManager _obstacleManager;

        private GameOver _gameOverScreen;

        private CloudManager _cloudManager;

        private KeyboardState _previousKeyBoardState;

        public GameState gameState { get; set; }


        public Trex_Clone()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _entityManager = new EntityManager();
            gameState = GameState.Initial;
            _fadeInTexturePOSX = Trex.Default_Width;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            // set screen size
            _graphics.PreferredBackBufferWidth = WindowWidth;
            _graphics.PreferredBackBufferHeight = WindowHeight;
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _sfxHit = Content.Load<SoundEffect>(ASSET_NAME_HIT);
            _sfxJump = Content.Load<SoundEffect>(ASSET_NAME_JUMP);
            _sfxScored = Content.Load<SoundEffect>(ASSET_NAME_SCORED);

            _spriteTexture = Content.Load<Texture2D>(ASSET_NAME_SPRITESHEET);

            _fadeInTexture = new Texture2D(GraphicsDevice, 1, 1);
            _fadeInTexture.SetData(new Color[] { Color.White });

            _trex = new Trex(_spriteTexture, new Vector2(TREX_START_POS_X, TREX_START_POS_Y - Trex.Default_Height), _sfxJump);
            _trex.DrawOrder = 10;
            _trex.JumpComplete += trex_JumpComplete;
            _trex.TrexDied += trex_Died;

            _scoreBoard = new ScoreBoard(_spriteTexture, new Vector2(SCORE_BOARD_POS_X, SCORE_BOARD_POS_Y), _trex);
            //_scoreBoard.Score = 498;
            //_scoreBoard.HighScore = 25;

            _controller = new InputController(_trex);

            _groundManager = new GroundManager(_spriteTexture, _entityManager, _trex);

            _obstacleManager = new ObstacleManager(_entityManager, _trex, _scoreBoard, _spriteTexture);

            _cloudManager = new CloudManager(_spriteTexture, new Vector2(WindowWidth, 15), _entityManager, _scoreBoard);

            var mouseTexture = Content.Load<Texture2D>(MOUSE_TEXTURE);
            _gameOverScreen = new GameOver(_spriteTexture, mouseTexture);
            _gameOverScreen.Position = new Vector2(WindowWidth / 2 - (GameOver.GAME_OVER_WIDTH/2), WindowHeight/2-30);
            _gameOverScreen.Reset += trex_clone_Reset;

            _entityManager.AddEntity(_trex);
            _entityManager.AddEntity(_groundManager);
            _entityManager.AddEntity(_scoreBoard);
            _entityManager.AddEntity(_obstacleManager);
            _entityManager.AddEntity(_gameOverScreen);
            _entityManager.AddEntity(_cloudManager);

            _groundManager.Initialize();



        }

        private void trex_JumpComplete(object sender, EventArgs e)
        {
            if (gameState == GameState.Transition)
            {
                gameState = GameState.Playing;
                _trex.Initialize();

                _obstacleManager.isEnabled = true;
            }
        }

        private void trex_Died(object sender, EventArgs e)
        {
            gameState = GameState.GameOver;
            _sfxHit.Play();
            _obstacleManager.isEnabled = false;
            _gameOverScreen.IsEnabled = true;
            

        }
        private void trex_clone_Reset(object sender, EventArgs e)
        {
            gameState = GameState.GameOver;
            Replay();
        }

        public bool Replay()
        {
            if(gameState != GameState.GameOver)
            {
                return false;
            }
            gameState = GameState.Playing;
            _trex.Initialize();
            _scoreBoard.Score = 0;
            _obstacleManager.Reset();
            _obstacleManager.isEnabled=true;
            _gameOverScreen.IsEnabled = false;
            _groundManager.Initialize();
            return true;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            var keyboardState = Keyboard.GetState();
            // TODO: Add your update logic here

            base.Update(gameTime);
            if (gameState == GameState.Playing)
            {
                _controller.ProcessControls(gameTime);
            }
            else if (gameState == GameState.Transition)
            {
                _fadeInTexturePOSX += (float)gameTime.ElapsedGameTime.TotalSeconds * FADE_IN_ANIMATION_SPEED;
            }
            else if (gameState == GameState.Initial)
            {
                bool isStartKeyPressed = keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.Space);
                bool wasStartKeyPressed = _previousKeyBoardState.IsKeyDown(Keys.Up) || _previousKeyBoardState.IsKeyDown(Keys.Space);
                if (isStartKeyPressed)
                {
                    StartGame();
                }
            }

            _entityManager.Update(gameTime);
            _previousKeyBoardState = keyboardState;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            _entityManager.Draw(_spriteBatch, gameTime);
            if (gameState == GameState.Initial || gameState == GameState.Transition)
            {
                _spriteBatch.Draw(_fadeInTexture, new Rectangle((int)Math.Round(_fadeInTexturePOSX), 0, WindowWidth, WindowHeight), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
        public bool StartGame()
        {
            if (gameState != GameState.Initial)
            {
                return false;
            }
            gameState = GameState.Transition;
            _trex.BeginJump();
            return true;
        }
    }
}