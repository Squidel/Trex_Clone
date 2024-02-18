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
    public class GroundManager : IGameEntity
    {
        private const int SPRITE_WIDTH = 600;
        private const int SPRITE_HEIGHT = 12;


        //position of the sprite in the texture
        private const int SPRITE_X = 2;
        private const int SPRITE_Y = 54;

        //position we want on the screen
        private const float GROUND_TILE_POS_Y = 120.995f;

        private Texture2D _spriteTexture;
        private Sprite _regularSprite;
        private Sprite _bumpySprite;

        private readonly List<GroundTile> _groundTiles;
        private readonly EntityManager _entityManager;
        public int DrawOrder { get; set; }

        public GroundManager(Texture2D texture2D, EntityManager entityManager)
        {
            _spriteTexture = texture2D;
            _groundTiles = new List<GroundTile>();
            _entityManager = entityManager;
            _regularSprite = new Sprite(_spriteTexture, SPRITE_X, SPRITE_Y, SPRITE_WIDTH, SPRITE_HEIGHT, Color.White);
            _bumpySprite = new Sprite(_spriteTexture, SPRITE_X + SPRITE_WIDTH, SPRITE_Y, SPRITE_WIDTH, SPRITE_HEIGHT, Color.White);
        }

        public void Initialize()
        {
            var groundTile = CreateRegularTile(0);
            _groundTiles.Add(groundTile);
            _entityManager.AddEntity(groundTile);
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            
        }

        public void Update(GameTime gameTime)
        {
            
        }
        private GroundTile CreateRegularTile(float positionx)
        {
            GroundTile groundTile = new GroundTile(positionx,_regularSprite, GROUND_TILE_POS_Y);
            return groundTile;
        }
        private GroundTile CreateBumpyTile(float positionx)
        {
            GroundTile groundTile = new GroundTile(positionx, _bumpySprite, GROUND_TILE_POS_Y);
            return groundTile;
        }
    }
}
