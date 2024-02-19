using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Trex_Clone.Visuals;

namespace Trex_Clone.Entities
{
    public class CactusGroup : Obstacle
    {
        private const int TINY_CACTUS_WIDTH = 17;
        private const int TINY_CACTUS_HEIGHT = 36;
        private const int TINY_CACTUS_POS_X = 228;
        private const int TINY_CACTUS_POS_Y = 0;

        private const int BIG_CACTUS_WIDTH = 17;
        private const int BIG_CACTUS_HEIGHT = 36;
        private const int BIG_CACTUS_POS_X = 228;
        private const int BIG_CACTUS_POS_Y = 0;
        private Texture2D _texture;
        public enum GroupSize
        {
            Small, 
            Medium,
            Large
        }
        public override Rectangle CollisionBox => throw new NotImplementedException();
        public bool isLarge { get; }
        public GroupSize groupSize { get; }
        public Sprite Sprite { get; }

        public CactusGroup(Trex trex, Vector2 position, bool Large, GroupSize Size, Texture2D texture) : base(trex, position)
        {
            isLarge = Large;
            groupSize = Size;
            _texture = texture;
            Sprite = GenerateSprite();
        }

        private Sprite GenerateSprite()
        {
            Sprite sprite = null;
            if(!isLarge)
            {
                int offset = 0;
                int width = TINY_CACTUS_WIDTH;
                switch(groupSize)
                {
                    case GroupSize.Small:
                        offset = 0;
                        width = TINY_CACTUS_WIDTH;
                        break;
                    case GroupSize.Medium:
                        offset = 1;
                        width = TINY_CACTUS_WIDTH * 2;
                        break;
                    case GroupSize.Large:
                        offset = 3;
                        width = TINY_CACTUS_WIDTH * 3;
                        break;

                }
                sprite = new Sprite(_texture, TINY_CACTUS_POS_X + (TINY_CACTUS_WIDTH * offset), TINY_CACTUS_POS_Y, width, TINY_CACTUS_HEIGHT, Color.White);
            }
            else
            {

            }
            return sprite;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Sprite.Draw(spriteBatch, Position);
        }
    }
}
