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
        private const float GROUND_TILE_POS_Y = 122f;

        private Random _random = new Random();

        private Texture2D _spriteTexture;
        private Sprite _regularSprite;
        private Sprite _bumpySprite;

        private readonly List<GroundTile> _groundTiles;
        private readonly EntityManager _entityManager;
        private Trex _trex;
        public int DrawOrder { get; set; }

        public GroundManager(Texture2D texture2D, EntityManager entityManager, Trex trex)
        {
            _spriteTexture = texture2D;
            _groundTiles = new List<GroundTile>();
            _entityManager = entityManager;
            _trex = trex;
            _regularSprite = new Sprite(_spriteTexture, SPRITE_X, SPRITE_Y, SPRITE_WIDTH, SPRITE_HEIGHT, Color.White);
            _bumpySprite = new Sprite(_spriteTexture, SPRITE_X + SPRITE_WIDTH, SPRITE_Y, SPRITE_WIDTH, SPRITE_HEIGHT, Color.White);
        }

        public void Initialize()
        {
            //clears all tiles within the class
            _groundTiles.Clear();

            //clears all tiles from the entity manager
            foreach (GroundTile tile in _entityManager.GetEntititesOfType<GroundTile>())
            {
                _entityManager.RemoveEntity(tile);
            }
            var groundTile = CreateRegularTile(0);
            _groundTiles.Add(groundTile);
            _entityManager.AddEntity(groundTile);
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

        }

        public void Update(GameTime gameTime)
        {
            if (_groundTiles.Any())
            {
                var maxPOSX = _groundTiles.Max(x => x.PositionX);
                if (maxPOSX < 0)
                {
                    SpawnTile(maxPOSX);
                }
            }
            List<GroundTile> tilesToRemove = new List<GroundTile>();
            foreach (var tile in _groundTiles)
            {
                tile.PositionX -= _trex.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (tile.PositionX < -SPRITE_WIDTH)
                {
                    _entityManager.RemoveEntity(tile);
                    tilesToRemove.Add(tile);
                }
            }
            foreach (var item in tilesToRemove)
            {
                _groundTiles.Remove(item);
            }
        }
        private GroundTile CreateRegularTile(float positionx)
        {
            GroundTile groundTile = new GroundTile(positionx, _regularSprite, GROUND_TILE_POS_Y);
            return groundTile;
        }
        private GroundTile CreateBumpyTile(float positionx)
        {
            GroundTile groundTile = new GroundTile(positionx, _bumpySprite, GROUND_TILE_POS_Y);
            return groundTile;
        }
        private void SpawnTile(float maxPosX)
        {
            var r = _random.NextDouble();
            var posX = maxPosX + SPRITE_WIDTH;

            GroundTile groundTile;
            if (r > 0.5)
            {
                groundTile = CreateBumpyTile(posX);
            }
            else
            {
                groundTile = CreateRegularTile(posX);
            }
            _entityManager.AddEntity(groundTile);
            _groundTiles.Add(groundTile);
        }
    }
}
