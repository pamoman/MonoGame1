using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace MonoGame1
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont arialFont;
        Texture2D brick, parrot, laser, spaceBackground, playImage;
        Rectangle parrotRectangle, spaceRectangle, playButton, laser1, laser2, alienLaser;
        Vector2 welcomePosition;

        KeyboardState keyboardOne = Keyboard.GetState();
        KeyboardState keyboardTwo = Keyboard.GetState();

        MouseState mouseOne = Mouse.GetState();
        MouseState mouseTwo = Mouse.GetState();

        int windowWidth;
        int windowHeight;

        List<Rectangle> bricks = new List<Rectangle>();

        // Globals
        string welcome = "Space Invaders";
        int gun = 2;
        int scene = 0;
        bool shoot1 = false;
        bool shoot2 = false;
        int moveYtimer = 0;
        bool moveYtimerOn = false;

        bool shouldMoveX = true;

        Dictionary<string, int> settings = new Dictionary<string, int>();

        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;

            windowWidth = graphics.PreferredBackBufferWidth;
            windowHeight = graphics.PreferredBackBufferHeight;

            for (int i = 0; i < 10; i++)
            {
                bricks.Add(new Rectangle((50 + (i * 70)), 100, 60, 60));
            }

            for (int j = 0; j < 10; j++)
            {
                bricks.Add(new Rectangle((50 + (j * 70)), 200, 60, 60));
            }

            /* FullScreen();*/

            settings["shootSpeed"] = 20;
            settings["moveX"] = 2;
            settings["alienShootSpeed"] = 10;

            base.Initialize();
        }

        protected void FullScreen()
        {
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.IsFullScreen = true;

            windowWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            windowHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            spaceBackground = Content.Load<Texture2D>("space");
            spaceRectangle = new Rectangle(0, 0, windowWidth, windowHeight);

            brick = Content.Load<Texture2D>("ufo");
            arialFont = Content.Load<SpriteFont>("arial");

            parrot = Content.Load<Texture2D>("parrot");
            parrotRectangle = new Rectangle(0, (windowHeight - (parrot.Height / 2)), parrot.Width / 2, parrot.Height / 2);

            laser = Content.Load<Texture2D>("laser");
            laser1 = new Rectangle(parrotRectangle.X, parrotRectangle.Y, laser.Width, laser.Height);
            laser2 = new Rectangle(parrotRectangle.X + parrotRectangle.Width - 10, parrotRectangle.Y, laser.Width, laser.Height);
            alienLaser = new Rectangle(-100, -100, laser.Width, laser.Height);

            playImage = Content.Load<Texture2D>("button");
            playButton = new Rectangle((windowWidth / 2) - (playImage.Width / 2), (windowHeight / 4) * 3, playImage.Width, playImage.Height);
            welcomePosition = new Vector2((windowWidth / 2) - (arialFont.MeasureString(welcome).X / 2), (windowHeight / 4) * 1);
        }

        void DrawCanvas()
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            spriteBatch.Draw(spaceBackground, spaceRectangle, Color.White);

            spriteBatch.End();
        }

        void DrawMenu()
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(arialFont, welcome, welcomePosition, Color.White);
            spriteBatch.Draw(playImage, playButton, Color.White);
            spriteBatch.End();
        }

        void UpdateMenu()
        {
            if (LeftClick() && playButton.Contains(mouseOne.Position))
            {
                ChangeScene(1);
            }

            if (bricks.Count == 0)
            {
                ChangeScene(0);
            }
        }

        void DrawGame()
        {
            spriteBatch.Begin();

            foreach (Rectangle b in bricks)
            {
                spriteBatch.Draw(brick, b, Color.White);
            }

            spriteBatch.Draw(parrot, parrotRectangle, Color.White);

            spriteBatch.Draw(laser, laser1, Color.White);

            spriteBatch.Draw(laser, laser2, Color.White);

            spriteBatch.Draw(laser, alienLaser, Color.White);

            spriteBatch.End();
        }

        void UpdateGame()
        {
            if (bricks.Count > 0)
            {
                PlaySpaceInvaders(5, 5);
            }
        }

        bool LeftClick()
        {
            if (mouseOne.LeftButton == ButtonState.Pressed && mouseTwo.LeftButton == ButtonState.Released)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void ChangeScene(int newScene)
        {
            scene = newScene;
        }

        Rectangle ControlHit(Rectangle laser, int shoot)
        {
            for (int i = 0; i < bricks.Count; i++)
            {
                Rectangle currentBrick = bricks[i];

                if (laser.Intersects(currentBrick))
                {
                    bricks.Remove(currentBrick);

                    laser.Y = parrotRectangle.Y;

                    if (shoot == 1)
                    {
                        shoot1 = false;
                    } else
                    {
                        shoot2 = false;
                    }
                }
            }

            return laser;
        }

        void MoveUfos()
        {
            shouldMoveX = false;
            Rectangle first = bricks[0];
            Rectangle last = bricks[bricks.Count - 1];

            for (int i = 0; i < bricks.Count; i++)
            {
                Rectangle temp = bricks[i];

                if (!moveYtimerOn && (temp.X > 0 || temp.X < (windowWidth - temp.Width)))
                {
                    temp.X += settings["moveX"];

                    bricks[i] = temp;
                }
                
                if ((temp.X > (windowWidth - temp.Width) || temp.X < 0))
                {
                    settings["moveX"] *= -1;
                    moveYtimerOn = true;
                }
            }

            /*if (last.X > (windowWidth - last.Width) || first.X < 0)
            {
                settings["moveX"] *= -1;
                moveYtimerOn = true;
            }*/

            if (moveYtimer >= 6)
            {
                MoveUfosY(10);
                moveYtimer = 0;
                moveYtimerOn = false;
            }

            if (moveYtimerOn == true)
            {
                moveYtimer++;
            }

            shouldMoveX = true;
        }

        void MoveUfosY(int move)
        {
            for (int i = 0; i < bricks.Count; i++)
            {
                Rectangle temp = bricks[i];

                if (temp.Y > 0 || temp.Y < (windowHeight - temp.Height))
                {
                    temp.Y += move;

                    bricks[i] = temp;
                }
            }
        }

        void AlienShoot()
        {
            Random rand = new Random();

            int alienIndex = rand.Next(0, bricks.Count - 1);

            if (alienLaser.Y > windowHeight)
            {
                Rectangle Alien = bricks[alienIndex];

                alienLaser.X = Alien.X + Alien.Width / 2;
                alienLaser.Y = Alien.Y + Alien.Height;
            }
            else
            {
                alienLaser.Y += settings["alienShootSpeed"];
            }
        }

        protected void PlaySpaceInvaders(int left, int right)
        {
            keyboardTwo = keyboardOne;
            keyboardOne = Keyboard.GetState();

            laser1 = ControlHit(laser1, 1);
            laser2 = ControlHit(laser2, 2);

            if (shouldMoveX)
            {
                MoveUfos();
            }

            AlienShoot();

            if (shoot1 && gun == 1)
            {
                if (laser1.Y > 0)
                {
                    laser1.Y -= settings["shootSpeed"];
                }
                else if (laser1.Y <= 0)
                {
                    shoot1 = false;
                    laser1.Y = parrotRectangle.Y;
                }
            }
            else if (shoot2 && gun == 2)
            {
                if (laser2.Y > 0)
                {
                    laser2.Y -= settings["shootSpeed"];
                }
                else if (laser2.Y <= 0)
                {
                    shoot2 = false;
                    laser2.Y = parrotRectangle.Y;
                }
            }

            if (keyboardOne.IsKeyDown(Keys.Left) && (parrotRectangle.X > 0))
            {
                parrotRectangle.X -= left;
                laser1.X -= left;
                laser2.X -= left;
            }

            if (keyboardOne.IsKeyDown(Keys.Right) && (parrotRectangle.X < windowWidth - (parrot.Width / 2)))
            {
                parrotRectangle.X += right;
                laser1.X += right;
                laser2.X += right;
            }

            if ((keyboardOne.IsKeyDown(Keys.Space)) && (keyboardTwo.IsKeyUp(Keys.Space)))
            {
                if (!shoot1 && gun == 1)
                {
                    shoot1 = true;
                    gun = 2;
                } else if (!shoot2 && gun == 2)
                {
                    shoot2 = true;
                    gun = 1;
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            mouseTwo = mouseOne;
            mouseOne = Mouse.GetState();

            switch (scene) 
            {
                case 0:
                    UpdateMenu();
                    break;
                case 1:
                    UpdateGame();
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            DrawCanvas();

            switch (scene)
            {
                case 0:
                    DrawMenu();
                    break;
                case 1:
                    DrawGame();
                    break;
            }


            base.Draw(gameTime);
        }
    }
}
