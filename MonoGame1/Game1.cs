using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace MonoGame1
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont arialFont;
        Texture2D brick;

        KeyboardState keyboardOne = Keyboard.GetState();
        KeyboardState keyboardTwo = Keyboard.GetState();

        int windowWidth = 800;
        int windowHeight = 480;

        string message = "Play Tetris!";
        double version = 0.1;
        Vector2 messagePosition = new Vector2(325, 0);

        Rectangle brickTemp;
        List<Rectangle> bricks = new List<Rectangle>();

        int brickPos = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;

            for (int i = 0; i < 10; i++)
            {
                bricks.Add(new Rectangle((100 + (i * 70)), -100, 60, 30));
            }

            FullScreen();

            base.Initialize();
        }

        protected void FullScreen()
        {
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.IsFullScreen = true;

            graphics.ApplyChanges();

            windowWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            windowHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            brick = Content.Load<Texture2D>("brick_grey");
            arialFont = Content.Load<SpriteFont>("arial");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            PlayTetris(5, 10, 10);

            base.Update(gameTime);
        }

        protected void PlayTetris(int down, int left, int right)
        {
            if (brickPos < bricks.Count)
            {
                brickTemp = bricks[brickPos];
                brickTemp.Y += down;

                keyboardTwo = keyboardOne;
                keyboardOne = Keyboard.GetState();

                bool isTouching = isTouchingBrick(brickTemp);

                if (!isTouching && (brickTemp.Y < windowHeight - brickTemp.Height))
                {
                    if (keyboardOne.IsKeyDown(Keys.Left) && brickTemp.X > 0)
                    {
                        brickTemp.X -= left;
                    }

                    if (keyboardOne.IsKeyDown(Keys.Right) && brickTemp.X < windowWidth - brickTemp.Width)
                    {
                        brickTemp.X += right;
                    }

                    if ((keyboardOne.IsKeyDown(Keys.Space) && keyboardTwo.IsKeyUp(Keys.Space)))
                    {
                        int width = brickTemp.Width;
                        int height = brickTemp.Height;

                        brickTemp.Width = height;
                        brickTemp.Height = width;
                    }

                    bricks[brickPos] = brickTemp;
                }
                else
                {
                    brickPos++;
                }
            }
        }

        protected void PlayTetrisSC(int down, int left, int right)
        {
            if (brickPos < bricks.Count)
            {
                brickTemp = bricks[brickPos];
                brickTemp.Y += down;

                keyboardTwo = keyboardOne;
                keyboardOne = Keyboard.GetState();

                bool isTouching = isTouchingBrick(brickTemp);

                if (!isTouching && (brickTemp.Y < windowHeight - brickTemp.Height))
                {
                    switch (true)
                    {
                        case true when (keyboardOne.IsKeyDown(Keys.Left) && brickTemp.X > 0):
                            brickTemp.X -= left;
                            break;
                        case true when (keyboardOne.IsKeyDown(Keys.Right) && brickTemp.X < windowWidth - brickTemp.Width):
                            brickTemp.X += right;
                            break;
                        case true when (keyboardOne.IsKeyDown(Keys.Space) && keyboardTwo.IsKeyUp(Keys.Space)):
                            int width = brickTemp.Width;
                            int height = brickTemp.Height;

                            brickTemp.Width = height;
                            brickTemp.Height = width;
                            break;
                        default:
                            break;
                    }

                    bricks[brickPos] = brickTemp;
                }
                else
                {
                    brickPos++;
                }
            }
        }

        protected bool isTouchingBrick(Rectangle brick)
        {
            bool touching = false;

            for (int i = 0; i < bricks.Count; i++)
            {
                Rectangle currentBrick = bricks[i];

                if (brickPos != i && brick.Intersects(currentBrick))
                {
                    touching = true;
                }
            }

            return touching;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkCyan);

            spriteBatch.Begin();
            spriteBatch.DrawString(arialFont, (message + " v" + version.ToString()), messagePosition, Color.Yellow);

            foreach (Rectangle b in bricks)
            {
                spriteBatch.Draw(brick, b, Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
