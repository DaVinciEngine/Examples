using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2
{
    public class Enemy : Player
    {
//        public enum direction { left, right, up, down };

        public direction direction;
        public Vector2 FrameSize;
        public Vector2 Position;
        public int score;

        public int speed;
        public Texture2D Texture;

        public Enemy()
        {
            FrameSize.X = (float) (screenSize.X * 0.11);
            FrameSize.Y = (float) (screenSize.Y * 0.2);
            Position.X = screenSize.X - FrameSize.X;
            Position.Y = screenSize.Y - FrameSize.Y;
            speed = 10;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            if (Position.X >= screenSize.X - FrameSize.X)
            {
                direction = direction.right;
                score += 1;
                speed += 1;
            }
            else if (Position.X < 0)
            {
                direction = direction.left;
                score += 1;
                speed += 1;
            }

            spriteBatch.Draw(texture,
                new Rectangle((int) Position.X, (int) Position.Y, (int) FrameSize.X,
                    (int) FrameSize.Y), Color.White);
            // TODO: Add your drawing code here
        }
    }
}