using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AnimatedSprites
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D ringsTexture;
        Point ringsFrameSize = new Point(75, 75);
        Point ringsCurrentFrame = new Point(0, 0);
        Point ringsSheetSize = new Point(6, 8);
        int ringsTimeSinceLastFrame = 0;
        const int ringsMillisecondsPerFrame = 50;
        Vector2 ringsPosition = Vector2.Zero;
        const float ringsSpeed = 6;

        Texture2D skullTexture;
        Point skullFrameSize = new Point(75, 75);
        Point skullCurrentFrame = new Point(0, 0);
        Point skullSheetSize = new Point(6, 8);
        int skullTimeSinceLastFrame = 0;
        const int skullMillisecondsPerFrame = 50;
        Vector2 skullPosition = new Vector2(100, 100);

        int ringsCollisionRectOffset = 11;
        int skullCollisionRectOffset = 11;

        MouseState prevMouseState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ringsTexture = Content.Load<Texture2D>("Images/threerings");
            skullTexture = Content.Load<Texture2D>("Images/skullball");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            
            //  Animation
            ringsTimeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if(ringsTimeSinceLastFrame > ringsMillisecondsPerFrame)
            {
                ringsTimeSinceLastFrame -= ringsMillisecondsPerFrame;
                ++ringsCurrentFrame.X;
                if (ringsCurrentFrame.X >= ringsSheetSize.X)
                {
                    ringsCurrentFrame.X = 0;
                    ++ringsCurrentFrame.Y;
                    if (ringsCurrentFrame.Y >= ringsSheetSize.Y)
                        ringsCurrentFrame.Y = 0;
                }
            }

            skullTimeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (skullTimeSinceLastFrame > skullMillisecondsPerFrame)
            {
                skullTimeSinceLastFrame -= skullMillisecondsPerFrame;
                ++skullCurrentFrame.X;
                if (skullCurrentFrame.X >= skullSheetSize.X)
                {
                    skullCurrentFrame.X = 0;
                    ++skullCurrentFrame.Y;
                    if (skullCurrentFrame.Y >= skullSheetSize.Y)
                        skullCurrentFrame.Y = 0;
                }
            }

            //  Input
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Left))
                ringsPosition.X -= ringsSpeed;
            if (keyboardState.IsKeyDown(Keys.Right))
                ringsPosition.X += ringsSpeed;
            if (keyboardState.IsKeyDown(Keys.Up))
                ringsPosition.Y -= ringsSpeed;
            if (keyboardState.IsKeyDown(Keys.Down))
                ringsPosition.Y += ringsSpeed;

            MouseState mouseState = Mouse.GetState();
            if (mouseState.X != prevMouseState.X ||
                mouseState.Y != prevMouseState.Y)
                ringsPosition = new Vector2(mouseState.X, mouseState.Y);
            prevMouseState = mouseState;

            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            if (gamePadState.Buttons.A == ButtonState.Pressed)
            {
                ringsPosition.X += ringsSpeed * 2 * gamePadState.ThumbSticks.Left.X;
                ringsPosition.Y -= ringsSpeed * 2 * gamePadState.ThumbSticks.Left.Y;
                GamePad.SetVibration(PlayerIndex.One, 1f, 1f);
            }
            else
            {
                ringsPosition.X += ringsSpeed * gamePadState.ThumbSticks.Left.X;
                ringsPosition.Y -= ringsSpeed * gamePadState.ThumbSticks.Left.Y;
                GamePad.SetVibration(PlayerIndex.One, 0, 0);
            }

            if (ringsPosition.X < 0)
                ringsPosition.X = 0;
            if (ringsPosition.Y < 0)
                ringsPosition.Y = 0;
            if (ringsPosition.X > Window.ClientBounds.Width - ringsFrameSize.X)
                ringsPosition.X = Window.ClientBounds.Width - ringsFrameSize.X;
            if (ringsPosition.Y > Window.ClientBounds.Height - ringsFrameSize.Y)
                ringsPosition.Y = Window.ClientBounds.Height - ringsFrameSize.Y;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            spriteBatch.Draw(ringsTexture, ringsPosition,
                new Rectangle(
                    ringsCurrentFrame.X * ringsFrameSize.X,
                    ringsCurrentFrame.Y * ringsFrameSize.Y,
                    ringsFrameSize.X,
                    ringsFrameSize.Y),
                Color.White, 0, Vector2.Zero,
                1, SpriteEffects.None, 0);

            spriteBatch.Draw(skullTexture, skullPosition,
                new Rectangle(
                    skullCurrentFrame.X * skullFrameSize.X,
                    skullCurrentFrame.Y * skullFrameSize.Y,
                    skullFrameSize.X,
                    skullFrameSize.Y),
                Color.White, 0, Vector2.Zero,
                1, SpriteEffects.None, 0);

            spriteBatch.End();

            if (Collide())
                Exit();

            base.Draw(gameTime);
        }

        protected bool Collide()
        {
            Rectangle ringsRect = new Rectangle(
                (int)ringsPosition.X + ringsCollisionRectOffset,
                (int)ringsPosition.Y + ringsCollisionRectOffset,
                ringsFrameSize.X - (ringsCollisionRectOffset * 2),
                ringsFrameSize.Y - (ringsCollisionRectOffset * 2));
            Rectangle skullRect = new Rectangle(
                (int)skullPosition.X + skullCollisionRectOffset,
                (int)skullPosition.Y + skullCollisionRectOffset,
                skullFrameSize.X - (skullCollisionRectOffset * 2),
                skullFrameSize.Y - (skullCollisionRectOffset * 2));

            return ringsRect.Intersects(skullRect);
        }

    }
}
