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
    public class CloudTile : IGameEntity
    {
        private Sprite CloudTileSprite { get; set; }
        public Vector2 Position;
        public int DrawOrder { get; set; }
        public float PositionX { get { return Position.X; } }
        public CloudTile(Sprite cloudSprite, Vector2 position)
        {
            CloudTileSprite = cloudSprite;
            Position = position;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            CloudTileSprite.Draw(spriteBatch, Position);
        }

        public void Update(GameTime gameTime)
        {
        }
    }
    public class CloudManager : IGameEntity
    {
        private const int CLOUD_POS_X = 89;
        private const int CLOUD_POS_Y = 1;

        private const int CLOUD_POS_WIDTH = 42;
        private const int CLOUD_POS_HEIGHT = 15;

        private Random _random;

        private Texture2D _texture;
        private List<CloudTile> _cloudSprites;


        private EntityManager _entityManager;
        public int DrawOrder { get; set; }
        private Sprite _cloudSprite { get; set; }
        private Vector2 _position { get; set; }
        private CloudTile _cloudTile { get; set; }
        private ScoreBoard _scoreBoard;



        public CloudManager(Texture2D texture, Vector2 position, EntityManager entityManager, ScoreBoard scoreBoard)
        {
            _random = new Random();
            _cloudSprites = new List<CloudTile>();
            _position = position;
            _texture = texture;
            _cloudSprite = new Sprite(_texture, CLOUD_POS_X, CLOUD_POS_Y, CLOUD_POS_WIDTH, CLOUD_POS_HEIGHT, Color.White);
            _entityManager = entityManager;
            _cloudTile = new CloudTile(_cloudSprite, position);
            _scoreBoard = scoreBoard;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

        }

        public void Update(GameTime gameTime)
        {
            if (!_cloudSprites.Any() && _scoreBoard.Score > 80)
            {
                SpawnCloud();
            }
            if (_cloudSprites.Any())
            {
                float maxPOS = _cloudSprites.Max(x => x.PositionX);
                if (maxPOS < 50)
                {
                    SpawnCloud();
                }
            }
            //this secondary list is so that we don't modify the original list while iterating through it
            var _cloudSpritesToRemove = new List<CloudTile>();
            foreach (var item in _cloudSprites)
            {
                item.Position = new Vector2(item.Position.X - 20f * (float)gameTime.ElapsedGameTime.TotalSeconds, item.Position.Y);
                if (item.PositionX == 50)
                {
                    Console.WriteLine("Cloud reached 0 x position");
                }
                if (item.PositionX < -CLOUD_POS_WIDTH)
                {
                    _cloudSpritesToRemove.Add(item);
                    _entityManager.RemoveEntity(item);
                }
            }
            foreach (var item in _cloudSpritesToRemove)
            {
                _cloudSprites.Remove(item);
            }

        }
        private void SpawnCloud()
        {
            //_cloudSprites.Clear();
            //random number between 1 and 3
            int num = _random.Next(0, 4);
            if (num == 3)
            {
                _cloudTile = new CloudTile(_cloudSprite, _position);
                _cloudSprites.Add(_cloudTile);
                _entityManager.AddEntity(_cloudTile);
                int doubleCloud = _random.Next(0, 3);
                if (_cloudSprites.Any() && doubleCloud == 2)
                {
                    _cloudTile = new CloudTile(_cloudSprite, new Vector2(_position.X + CLOUD_POS_WIDTH + 5, _position.Y));
                    _cloudSprites.Add(_cloudTile);
                    _entityManager.AddEntity(_cloudTile);
                }
            }



        }
    }
}
