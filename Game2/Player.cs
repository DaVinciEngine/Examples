using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2
{
    public class Player : Game
    {
        public enum direction
        {
            left,
            right,
            up,
            down
        }

        public double elapsedAirTime;
        private bool falling;
        public Vector2 FrameSize;
        private bool jumping;
        private readonly double maxAirTime = 12;
        public Vector2 Position;
        public Vector2 screenSize;
        public int speed;
        public Texture2D Texture;

        public Player()
        {
            screenSize.X = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            screenSize.Y = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            Content.RootDirectory = "Content";
            FrameSize.X = (float) (screenSize.X * 0.1);
            FrameSize.Y = (float) (screenSize.Y * 0.1);
            Position.X = FrameSize.X;
            Position.Y = screenSize.Y - FrameSize.Y;
            speed = 10;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            if (jumping && elapsedAirTime < maxAirTime)
            {
                var calc = 20 - -4 * elapsedAirTime;
                elapsedAirTime += 1;
                Position.Y -= (int) calc;
            }
            else if (falling && Position.Y < screenSize.Y - FrameSize.Y ||
                     elapsedAirTime == maxAirTime && Position.Y < screenSize.Y - FrameSize.Y)
            {
                jumping = false;
                falling = true;
                var calc = 2 + 0.5 * elapsedAirTime;
                Position.Y += (int) calc;
            }
            else if (Position.Y >= screenSize.Y - FrameSize.Y)
            {
                falling = false;
                elapsedAirTime = 0;
            }

            spriteBatch.Draw(texture,
                new Rectangle((int) Position.X, (int) Position.Y, (int) FrameSize.X, (int) FrameSize.Y), Color.White);
            // TODO: Add your drawing code here
        }

        public void Move(direction Direction)
        {
            if (Direction == direction.left && Position.X > 0) Position.X -= speed;
            if (Direction == direction.right && Position.X < screenSize.X - FrameSize.X) Position.X += speed;
            if (Direction == direction.up)
            {
                if (elapsedAirTime < maxAirTime)
                {
                    jumping = true;
                }
                else if (Position.Y < (screenSize.Y - FrameSize.Y))
                {
                    jumping = false;
                    falling = true;
                }
            }
        }

        public bool Collide(Enemy target)
        {
            var ownRect = new Rectangle((int) Position.X,
                (int) Position.Y, (int) FrameSize.X, (int) FrameSize.Y);
            var targetRect = new Rectangle((int) target.Position.X,
                (int) target.Position.Y, (int) target.FrameSize.X, (int) target.FrameSize.Y);
            return ownRect.Intersects(targetRect);
        }
    }
}