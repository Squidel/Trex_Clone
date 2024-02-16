using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Trex_Clone.Visuals
{
    public class Sprite
    {
        public Texture2D Texture { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Color Color { get; set; }
        public Sprite(Texture2D texture, int x, int y, int width, int height, Color color)
        {
            Texture = texture;
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Color = color;

        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(Texture, position, new Rectangle(X, Y, Width, Height), Color);
        }
    }
}
