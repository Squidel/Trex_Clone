using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Trex_Clone.Entities
{
    public class ObstacleManager : IGameEntity
    {
        private const float MIN_SPAWN_DISTANCE = 40f;
        private const int MAX_DISTANCE_BETWEEN_OBSTACLES = 50;
        private const int MIN_DISTANCE_BETWEEN_OBSTACLES = 10;

        private const int OBSTACLE_TOLERANCE_VALUE = 5;

        private const int OBSTACLE_DRAW_ORDER = 12;

        private readonly EntityManager _entityManager;
        private readonly Trex _trex;
        private readonly ScoreBoard _scoreboard;
        private Random _random;

        private double _lastSpawnScore = -1;
        private double _currentTargetDistance;

        private Texture2D _texture;


        public bool isEnabled { get; set; }
        public int DrawOrder => 0;

        public bool CanSpawnObstacles => isEnabled && _scoreboard.Score >= MIN_SPAWN_DISTANCE;
        public ObstacleManager(EntityManager entityManager, Trex trex, ScoreBoard scoreBoard, Texture2D texture2D)
        {
            _entityManager = entityManager;
            _trex = trex;
            _scoreboard = scoreBoard;
            _random = new Random();
            _texture = texture2D;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

        }

        public void Update(GameTime gameTime)
        {
            if (!isEnabled) return;
            if (CanSpawnObstacles && (_lastSpawnScore <=0 ||  (_scoreboard.Score - _lastSpawnScore >= _currentTargetDistance)))
            {
                _currentTargetDistance = _random.NextDouble() * (MAX_DISTANCE_BETWEEN_OBSTACLES - MIN_DISTANCE_BETWEEN_OBSTACLES) + MIN_DISTANCE_BETWEEN_OBSTACLES;
                _currentTargetDistance *= (_trex.Speed - (Trex.START_SPEED-1)) / (Trex.MAX_SPEED - Trex.START_SPEED) * OBSTACLE_TOLERANCE_VALUE;
                _lastSpawnScore = _scoreboard.Score;
                SpawnObstacles();
            }
            foreach(var item in _entityManager.GetEntititesOfType<Obstacle>())
            {
                if(item.Position.X < -200)
                {
                   // _entityManager.RemoveEntity(item);
                }
            }
        }

        private void SpawnObstacles()
        {
            Obstacle obstacle = null;
            //TODO: create obstacles
            var cactusSize = _random.Next(0, 3);
            bool isLarge = _random.NextDouble() > 0.5;
            float posY = isLarge ? 85 : 95;
            obstacle = new CactusGroup(_trex, new Vector2(Trex_Clone.WindowWidth+20, posY), isLarge, (CactusGroup.GroupSize)cactusSize, _texture);
            obstacle.DrawOrder = OBSTACLE_DRAW_ORDER;
            _entityManager.AddEntity(obstacle);

        }
        public void Reset()
        {
            var entities = _entityManager.GetEntititesOfType<Obstacle>();
            foreach (var entity in entities)
            {
                _entityManager.RemoveEntity(entity);
            }
            _lastSpawnScore = -1;
            _currentTargetDistance = 0;
        }
    }
}
