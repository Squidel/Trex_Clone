using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Trex_Clone.Visuals;

namespace Trex_Clone.Entities
{
    public class GroundTile : IGameEntity
    {
        private float PostionY;
        public float PositionX { get; set; }
        public Sprite Sprite { get; }

        public int DrawOrder {get;set; }

        public GroundTile(float position, Sprite sprite, float positiony)
        {
            PositionX = position;
            Sprite = sprite;
            PostionY = positiony;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Sprite.Draw(spriteBatch, new Vector2(PositionX, PostionY));
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
