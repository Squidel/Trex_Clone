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
    public class ScoreBoard : IGameEntity
    {
        private const int TEXTURE_NUMS_X = 655;
        private const int TEXTURE_NUMS_Y = 0;
        private const int TEXTURE_NUMS_WIDTH = 10;
        private const int TEXTURE_NUMS_HEIGHT = 13;

        private const byte NUM_DIGITS_TO_DRAW = 5;
        private const float SCOREBOARD_RATE = 0.05f;
        private Rectangle SOURCE_HI = new Rectangle(755, 0, 20, 13);

        private Texture2D _texture;
        private Trex _trex;

        public double Score { get; set; }
        public int DisplayScore
        {
            get
            {
                return (int)Math.Floor(Score);
            }
        }
        public int HighScore { get; set; }
        public int DrawOrder { get; } = 100;
        public Vector2 Position { get; set; }
        public ScoreBoard(Texture2D texture2D, Vector2 position, Trex trex)
        {
            _texture = texture2D;
            Position = position;
            _trex = trex;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            DrawScore(spriteBatch, DisplayScore, Position.X + 65);            
            if (HasHighScore())
            {
                spriteBatch.Draw(_texture, new Vector2(Position.X - 20, Position.Y), SOURCE_HI, Color.White);
                //spriteBatch.Draw(_texture, new Vector2(Position.X - 10, Position.Y), new Rectangle(765, 0, 15, 13), Color.White);
                DrawScore(spriteBatch, HighScore, Position.X + 2);
            }
            DrawSpeed(spriteBatch);
        }

        private void DrawScore(SpriteBatch spriteBatch, int score, float startPosX)
        {
            int[] scoreDigits = SplitDigits(score);
            float posX = startPosX;
            foreach (int scoreDigit in scoreDigits)
            {
                Rectangle coords = GetDigitTexture(scoreDigit);

                Vector2 screenPos = new Vector2(posX, Position.Y);

                spriteBatch.Draw(_texture, screenPos, coords, Color.White);

                posX += TEXTURE_NUMS_WIDTH;

            }
        }
        //This method is mainly for debugging as I want to see the speed updating on the screen
        private void DrawSpeed(SpriteBatch spriteBatch)
        {
            int trexSpeed = (int)Math.Round(_trex.Speed);
            DrawScore(spriteBatch, trexSpeed, 10);
        }

        public void Update(GameTime gameTime)
        {
            Score += _trex.Speed * SCOREBOARD_RATE * gameTime.ElapsedGameTime.TotalSeconds;
        }
        public bool HasHighScore()
        {
            return HighScore > 0;
        }

        private Rectangle GetDigitTexture(int digit)
        {
            if (digit < 0 || digit > 9)
            {
                throw new ArgumentOutOfRangeException(nameof(digit), "Digit value out of range; must be between 0 and 9");
            }
            int posX = TEXTURE_NUMS_X + (digit * TEXTURE_NUMS_WIDTH);
            int posY = TEXTURE_NUMS_Y;

            return new Rectangle(posX, posY, TEXTURE_NUMS_WIDTH, TEXTURE_NUMS_HEIGHT);
        }
        private int[] SplitDigits(int input)
        {
            string digitString = input.ToString().PadLeft(NUM_DIGITS_TO_DRAW, '0');
            char[] individualCharacters = digitString.ToCharArray();
            return individualCharacters.Select(x => (int)Char.GetNumericValue(x)).ToArray();
        }
    }
}
