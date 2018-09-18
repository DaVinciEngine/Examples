using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace Game2
{
    /// <summary>
    ///     This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private Texture2D background;
        public Texture2D enemyTexture;
        private SpriteFont font;
        private readonly GraphicsDeviceManager graphics;
        public Texture2D moveAble;
        public Player Player1 = new Player();
        private int score = 0;
        public Vector2 screenSize;
        public Enemy Snoek = new Enemy();
        private SpriteBatch spriteBatch;

        public Game1()
        {
            screenSize.X = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            screenSize.Y = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth =
                (int) screenSize.X; // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight =
                (int) screenSize.Y; // set this value to the desired height of your window
            graphics.ApplyChanges();
        }

        /// <summary>
        ///     Allows the game to perform any initialization it needs to before starting to run.
        ///     This is where it can query for any required services and load any non-graphic
        ///     related content.  Calling base.Initialize will enumerate through any components
        ///     and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        ///     LoadContent will be called once per game and is the place to load
        ///     all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("davinci");
            moveAble = Content.Load<Texture2D>("enemy");
            enemyTexture = Content.Load<Texture2D>("Player");
            font = Content.Load<SpriteFont>("File");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        ///     UnloadContent will be called once per game and is the place to unload
        ///     game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        ///     Allows the game to run logic such as updating the world,
        ///     checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Player1.Collide(Snoek))
            {
                MessageBox.Show("score:" + Snoek.score);
                Exit();
            }

            // TODO: Add your update logic here
            if (Snoek.direction == Player.direction.left) Snoek.Position.X += Snoek.speed;
            if (Snoek.direction == Player.direction.right) Snoek.Position.X -= Snoek.speed;

            // Move our sprite based on arrow keys being pressed:
            if (state.IsKeyDown(Keys.Right))
                Player1.Move(Player.direction.right);
            if (state.IsKeyDown(Keys.Left))
                Player1.Move(Player.direction.left);
            if (state.IsKeyDown(Keys.Up))
                Player1.Move(Player.direction.up);
            if (state.IsKeyDown(Keys.Down))
                Player1.Move(Player.direction.down);
            base.Update(gameTime);
        }

        /// <summary>
        ///     This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
//            GraphicsDevice.Clear(Color.HotPink);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, (int) screenSize.X, (int) screenSize.Y), Color.White);
            Player1.Draw(spriteBatch, moveAble);
            Snoek.Draw(spriteBatch, enemyTexture);
            spriteBatch.DrawString(font, "Score" + Snoek.score, new Vector2(100, 100), Color.Black);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}