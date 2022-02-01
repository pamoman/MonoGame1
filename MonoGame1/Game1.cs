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
        Texture2D parrotBild, zebraBild, brick;
        Rectangle zebraRectangle;
        Rectangle brickRectangle;
        Rectangle brickTemp;

        MouseState mouse;
        KeyboardState keyboardOne = Keyboard.GetState();
        KeyboardState keyboardTwo = Keyboard.GetState();

        Vector2 parrotPosition = new Vector2(100, 200);
        Vector2 zebraPosition = new Vector2(200, 100);
        Vector2 brickPosition = new Vector2(200, 480 - 127);

        int windowWidth = 800;
        int windowHeight = 480;

        string message = "Play Tetris!";
        double version = 0.1;
        Vector2 messagePosition = new Vector2(325, 0);

        int left = 3;
        int right = 3;
        int up = 3;
        int down = 3;

        int brickDown = 1;
        int brickLeft = 3;
        int brickRight = 3;

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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            parrotBild = Content.Load<Texture2D>("parrot");
            zebraBild = Content.Load<Texture2D>("zebra");
            brick = Content.Load<Texture2D>("brick_grey");

            brickRectangle = new Rectangle(100, 0, 40, 20);
            zebraRectangle = new Rectangle(100, 200, zebraBild.Width / 2, zebraBild.Height / 2);

            arialFont = Content.Load<SpriteFont>("arial");
        }

        protected override void Update(GameTime gameTime)
        {
            DropRectangle();

            /*parrotPosition = FollowMouse(parrotPosition);*/

            /*zebraRectangle = MakeBigger(zebraRectangle);*/

            /*parrotPosition = ControlKeyboard(parrotPosition);*/

            /*zebraPosition = ControlKeyboard(zebraPosition);*/

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

        protected Vector2 FollowMouse(Vector2 position)
        {
            mouse = Mouse.GetState();
            position.X = mouse.X;
            position.Y = mouse.Y;

            return position;
        }

        protected Vector2 ControlKeyboard(Vector2 position)
        {
            keyboardOne = Keyboard.GetState();

            if (keyboardOne.IsKeyDown(Keys.Left))
            {
                position.X -= left;

                if (position.X < 0)
                {
                    left = 0;
                }
                else
                {
                    left = 3;
                }
            }

            if (keyboardOne.IsKeyDown(Keys.Right))
            {
                position.X += right;

                if (position.X > windowWidth - parrotBild.Width)
                {
                    right = 0;
                }
                else
                {
                    right = 3;
                }
            }

            if (keyboardOne.IsKeyDown(Keys.Up))
            {
                position.Y -= up;

                if (position.Y < 0)
                {
                    up = 0;
                }
                else
                {
                    up = 3;
                }

            }

            if (keyboardOne.IsKeyDown(Keys.Down))
            {
                position.Y += down;

                if (position.Y > windowHeight - parrotBild.Height)
                {
                    down = 0;
                }
                else
                {
                    down = 3;
                }
            }

            return position;
        }

        protected Rectangle MakeBigger(Rectangle image)
        {
            image.Width += 1;
            image.Height += 1;

            return image;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkCyan);

            spriteBatch.Begin();
            /*spriteBatch.Draw(parrotBild, parrotPosition, Color.Orange);*/
            /*spriteBatch.Draw(zebraBild, zebraPosition, Color.Orange);*/
            /*spriteBatch.Draw(brick, brickRectangle, Color.White);*/

            /* spriteBatch.Draw(zebraBild, zebraRectangle, Color.White);*/

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
