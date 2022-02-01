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
        int brickDown = 1;
        int brickLeft = 3;
        int brickRight = 3;

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

            DropRectangle();

            base.Update(gameTime);
        }

        protected void DropRectangle()
        {
            brickTemp = bricks[brickPos];
            brickTemp.Y += brickDown;
            
            keyboardTwo = keyboardOne;
            keyboardOne = Keyboard.GetState();

            bool isTouching = isTouchingBrick(brickTemp);

            if (brickPos < bricks.Count)
            {
                if (!isTouching && (brickTemp.Y < windowHeight - brickTemp.Height))
                {
                    brickDown = 1;

                    if (keyboardOne.IsKeyDown(Keys.Left))
                    {
                        brickTemp.X -= brickLeft;

                        if (brickTemp.X < 0)
                        {
                            brickLeft = 0;
                        }
                        else
                        {
                            brickLeft = 3;
                        }
                    }

                    if (keyboardOne.IsKeyDown(Keys.Right))
                    {
                        brickTemp.X += brickRight;

                        if (brickTemp.X > windowWidth - brickTemp.Width)
                        {
                            brickRight = 0;
                        }
                        else
                        {
                            brickRight = 3;
                        }
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
                else if (brickPos < bricks.Count - 1)
                {
                    brickPos++;
                }
                else
                {
                    brickDown = 0;
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
