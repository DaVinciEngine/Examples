using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShooterGame
{
    static class EntityManager
    {
        static List<Entity> entities = new List<Entity>();
        private static bool isUpdating; 
        static List<Entity> addedEntities = new List<Entity>();
        static List<Enemy> enemies = new List<Enemy>();
        public static List<BlackHole> blackHoles = new List<BlackHole>();
        static List<Bullet> bullets = new List<Bullet>();

        public static int Count { get { return entities.Count; } }
        public static int BlackHoleCount { get; set; }

        public static void Add(Entity entity)
        {
            if (!isUpdating)
            {
                AddEntity(entity);
            }
            else
                addedEntities.Add(entity);
        }

        public static void Update()
        {
            isUpdating = true;
            HandleCollisions();

            foreach (var entity in entities)
                entity.Update();

            isUpdating = false;
            foreach (var entity in addedEntities)
            {
                AddEntity(entity);
            }
            addedEntities.Clear();
            //Remove dead
            entities = entities.Where(x => !x.isExpired).ToList();
            bullets = bullets.Where(x => !x.isExpired).ToList();
            enemies = enemies.Where(x => !x.isExpired).ToList();

        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach(var entity in entities)
            {
                entity.Draw(spriteBatch);
            }
        }

        public static void AddEntity(Entity entity)
        {
            entities.Add(entity);
            if (entity is Bullet)
                bullets.Add(entity as Bullet);
            else if(entity is Enemy) 
                enemies.Add(entity as Enemy);

        }

        private static bool IsColliding(Entity a, Entity b)
        {
            float radius = a.Radius + b.Radius;
            return !a.isExpired && !b.isExpired && Vector2.DistanceSquared(a.Position, b.Position) < radius * radius;
        }
        public static IEnumerable<Entity> GetNearbyEntities(Vector2 position, float radius)
        {
            return entities.Where(x => Vector2.DistanceSquared(position, x.Position) < radius * radius);
        }

        static void HandleCollisions()
        {
            for (int i = 0; i < enemies.Count; i++)
                for (int j = i + 1; j < enemies.Count; j++)
                {
                    if (IsColliding(enemies[i], enemies[j]))
                    {
                        enemies[i].HandleCollision(enemies[j]);
                        enemies[j].HandleCollision(enemies[i]);
                    }
                }

            for (int i = 0; i < enemies.Count; i++)
            {
                for (int j = 0; j < bullets.Count; j++)
                {
                    if (IsColliding(enemies[i], bullets[j]))
                    {
                        enemies[i].WasShot();
                        bullets[j].isExpired = true;
                    }
                }
            }
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].IsActive && IsColliding(PlayerShip.Instance, enemies[i]))
                {
                    PlayerShip.Instance.Kill();
                    enemies.ForEach(x => x.WasShot());
                    break;
                }
            }
            for (int i = 0; i < blackHoles.Count; i++)
            {
                for (int j = 0; j < enemies.Count; j++)
                    if (enemies[j].IsActive && IsColliding(blackHoles[i], enemies[j]))
                        enemies[j].WasShot();

                for (int j = 0; j < bullets.Count; j++)
                {
                    if (IsColliding(blackHoles[i], bullets[j]))
                    {
                        bullets[j].isExpired = true;
                        blackHoles[i].WasShot();
                    }
                }

                if (IsColliding(PlayerShip.Instance, blackHoles[i]))
                {
                     PlayerShip.Instance.Kill();
                    break;
                }
            }
        }

    }
}
