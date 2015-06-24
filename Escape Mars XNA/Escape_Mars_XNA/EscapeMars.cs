using System;
using System.Linq;
using Escape_Mars_XNA.Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Escape_Mars_XNA
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class EscapeMars : Game
    {
        // Flag for displaying the graph or not
        private bool _displayGraph;
        private bool _displayRobotBrainStack;
        private int _brainStackLevel = 2;

        // 
        private bool _bKeyPressed;
        private bool _gKeyPressed;
        private bool _rKeyPressed;
        private bool _leftClick;

        public GraphicsDeviceManager Graphics { get; private set; }
        SpriteBatch _spriteBatch;

        private readonly World _world;

        public EscapeMars()
        {
            Graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 768,
                PreferredBackBufferHeight = 648
            };

            Vector2Helper.WindowWidth = 768;
            Vector2Helper.WindowHeight = 648;

            IsMouseVisible = true;

            _world = World.Instance();
            _world.Create();

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _world.Load(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            _world.Update(gameTime);

            var mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                _leftClick = true;
            }

            if (mouseState.LeftButton == ButtonState.Released && _leftClick)
            {
                _leftClick = false;

                var seekMouse = new Vector2(mouseState.X, mouseState.Y);

                _world.RobotMoveTo(seekMouse);
            }

            var keyState = Keyboard.GetState();
            
            /**
             * Display or not the graph
             */
            if (keyState.IsKeyDown(Keys.G) && !_gKeyPressed)
            {
                _gKeyPressed = true;

                _world.DisplayGraph(_displayGraph = !_displayGraph);
            }

            if (keyState.IsKeyUp(Keys.G) && _gKeyPressed)
            {
                _gKeyPressed = false;
            }

            if (keyState.IsKeyDown(Keys.D0))
            {
                _brainStackLevel = 0;
                _world.Robot.ShowBrainStack(_displayRobotBrainStack, _brainStackLevel);
            }

            if (keyState.IsKeyDown(Keys.D1))
            {
                _brainStackLevel = 1;
                _world.Robot.ShowBrainStack(_displayRobotBrainStack, _brainStackLevel);
            }

            if (keyState.IsKeyDown(Keys.D2))
            {
                _brainStackLevel = 2;
                _world.Robot.ShowBrainStack(_displayRobotBrainStack, _brainStackLevel);
            }

            if (keyState.IsKeyDown(Keys.D3))
            {
                _brainStackLevel = 3;
                _world.Robot.ShowBrainStack(_displayRobotBrainStack, _brainStackLevel);
            }

            if (keyState.IsKeyDown(Keys.D4))
            {
                _brainStackLevel = 4;
                _world.Robot.ShowBrainStack(_displayRobotBrainStack, _brainStackLevel);
            }

            /**
             * Display robots' brain stack
             */
            if (keyState.IsKeyDown(Keys.B) && !_bKeyPressed)
            {
                _bKeyPressed = true;

                _world.Robot.ShowBrainStack(_displayRobotBrainStack = !_displayRobotBrainStack, _brainStackLevel);
            }

            if (keyState.IsKeyUp(Keys.B) && _bKeyPressed)
            {
                _bKeyPressed = false;
            }

            /**
             * Reset the game
             */
            if (keyState.IsKeyDown(Keys.R) && !_rKeyPressed)
            {
                _rKeyPressed = true;

                _world.Objects.Clear();
                _world.Create();
                _world.Load(Content);
            }

            if (keyState.IsKeyUp(Keys.R) && _rKeyPressed)
            {
                _rKeyPressed = false;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            _spriteBatch.Begin();
            _world.Draw(_spriteBatch);
            _world.DisplayInfo(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
