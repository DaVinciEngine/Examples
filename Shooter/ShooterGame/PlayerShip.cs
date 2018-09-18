using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static ShooterGame.Extensions;

namespace ShooterGame
{
    class PlayerShip : Entity
    {
        private static PlayerShip instance;
        const int cooldownFrames = 6;
        int cooldownRemaining = 0;
        static Random rand = new Random();
        private int framesUntilRespawn = 0;
        public bool IsDead
        {
            get { return framesUntilRespawn > 0;  }
        }

        public static PlayerShip Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlayerShip();
                }

                return instance;
            }
        }

        private PlayerShip()
        {
            image = Art.Player;
            Position = ShooterGame.ScreenSize / 2;
            Radius = 10;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsDead)
                base.Draw(spriteBatch);
        }
        public void Kill()
        {
            if(PlayerStatus.Lives != 0)
            PlayerStatus.RemoveLive();
            else {
                                PlayerStatus.Reset();
            framesUntilRespawn = 60;
            EnemySpawner.Reset();
            }

        }
        public override void Update()
        {
            if (IsDead)
            {
                framesUntilRespawn--;
                return;
            }
            const float speed = 8;
            Velocity = speed * Input.GetMovementDirection();
            Position += Velocity;
            Position = Vector2.Clamp(Position, Size / 2, ShooterGame.ScreenSize - Size / 2);

            if (Velocity.LengthSquared() > 0)
                Orientation = Velocity.ToAngle();
            var aim = Input.GetAimDirection();
            if (aim.LengthSquared() > 0 && cooldownRemaining <= 0)
            {
                cooldownRemaining = cooldownFrames;
                float aimAngle = aim.ToAngle();
                Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);

                float randomSpread = rand.NextFloat(-0.04f, 0.04f) + rand.NextFloat(-0.04f, 0.04f);
                Vector2 vel = MathUtil.FromPolar(aimAngle + randomSpread, 11f);

                Vector2 offset = Vector2.Transform(new Vector2(25, -8), aimQuat);
                EntityManager.Add(new Bullet(Position + offset, vel));

                offset = Vector2.Transform(new Vector2(25, 8), aimQuat);
                EntityManager.Add(new Bullet(Position + offset, vel));
            }

            if (cooldownRemaining > 0)
                cooldownRemaining--;
        }
    }


}
