using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Trex_Clone.Entities;
using Trex_Clone.Visuals;

namespace Trex_Clone
{
    public class Trex_Clone : Game
    {
        public const int WindowWidth = 600;
        public const int WindowHeight = 150;

        public const int TREX_START_POS_X = 1;
        public const int TREX_START_POS_Y = WindowHeight - 16;

        private const string ASSET_NAME_SPRITESHEET = "100-offline-sprite";
        private const string ASSET_NAME_HIT = "1374573";
        private const string ASSET_NAME_SCORED = "1374572";
        private const string ASSET_NAME_JUMP = "1374571";

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SoundEffect _sfxHit;
        private SoundEffect _sfxJump;
        private SoundEffect _sfxScored;

        private Texture2D _spriteTexture;

        private Trex _trex;


        public Trex_Clone()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
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

            _trex = new Trex(_spriteTexture, new Vector2(TREX_START_POS_X, TREX_START_POS_Y - Trex.Default_Height));
            

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);

            _trex.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            _trex.Draw(_spriteBatch, gameTime);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}