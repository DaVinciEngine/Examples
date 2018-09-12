using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimatedSprites
{
    class SpriteManager : DrawableGameComponent
    {
        SpriteBatch spriteBatch;

        Player player;
        List<Sprite> spriteList = new List<Sprite>();


        public SpriteManager(Game game)
            : base(game)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            player = new Player(
                Game.Content.Load<Texture2D>("Images/threerings"),
                Vector2.Zero, new Point(75, 75), 10, new Point(0, 0),
                new Point(6, 8), new Vector2(6, 6));

            spriteList.Add(new AutomatedSprite(
                Game.Content.Load<Texture2D>("Images/skullball"),
                new Vector2(150, 150), new Point(75, 75), 10, new Point(0, 0),
                new Point(6, 8), Vector2.Zero));
            spriteList.Add(new AutomatedSprite(
                Game.Content.Load<Texture2D>("Images/skullball"),
                new Vector2(300, 150), new Point(75, 75), 10, new Point(0, 0),
                new Point(6, 8), Vector2.Zero));
            spriteList.Add(new AutomatedSprite(
                Game.Content.Load<Texture2D>("Images/skullball"),
                new Vector2(150, 300), new Point(75, 75), 10, new Point(0, 0),
                new Point(6, 8), Vector2.Zero));
            spriteList.Add(new AutomatedSprite(
                Game.Content.Load<Texture2D>("Images/skullball"),
                new Vector2(600, 400), new Point(75, 75), 10, new Point(0, 0),
                new Point(6, 8), new Vector2(-1, 0)));

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime, Game.Window.ClientBounds);

            foreach (Sprite s in spriteList)
            {
                s.Update(gameTime, Game.Window.ClientBounds);

                // Check for Collisions, exit game if there is
                if (s.collisionRect.Intersects(player.collisionRect))
                    Game.Exit();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            player.Draw(gameTime, spriteBatch);

            foreach (Sprite s in spriteList)
                s.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
