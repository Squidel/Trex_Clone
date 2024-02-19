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
    internal class ScoreBoard : IGameEntity
    {
        private const int TEXTURE_NUMS_X = 655;
        private const int TEXTURE_NUMS_Y = 0;
        private const int TEXTURE_NUMS_WIDTH = 10;
        private const int TEXTURE_NUMS_HEIGHT = 13;

        private const byte NUM_DIGITS_TO_DRAW = 5;

        private Texture2D _texture;

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
        public ScoreBoard(Texture2D texture2D, Vector2 position)
        {
            _texture = texture2D;
            Position = position;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            int[] scoreDigits = SplitDigits(DisplayScore);
            float posX = Position.X;
            foreach(int scoreDigit in scoreDigits)
            {
                Rectangle coords = GetDigitTexture(scoreDigit);

                Vector2 screenPos = new Vector2(posX, Position.Y);

                spriteBatch.Draw(_texture, screenPos, coords, Color.White);

                posX += TEXTURE_NUMS_WIDTH;
                
            }
        }

        public void Update(GameTime gameTime)
        {

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
