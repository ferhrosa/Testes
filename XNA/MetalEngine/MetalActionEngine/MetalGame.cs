using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MetalActionEngine
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MetalGame : Game
    {
        GraphicsDeviceManager graphics;
        
        
        internal SpriteBatch SpriteBatch { get; private set; }


        #region MainMenu property
        internal MetalMenu MainMenu { get; set; }

        /// <summary>
        /// Name of the XAML file that contains the specifications for the main menu of the Game.
        /// </summary>
        public string MainMenuFile { get; set; }
        #endregion MainMenu property


        #region Character property
        internal MetalCharacter Character { get; set; }
        
        /// <summary>
        /// Defines if the character selection menu will be shown before the game starts.
        /// </summary>
        public bool ShowCharacterSelection { get; set; }

        /// <summary>
        /// Name of the XAML file for the default character.
        /// It will be used when the ShowCharacterSelection property is set to false.
        /// </summary>
        public string DefaultCharacterFile { get; set; }
        #endregion Character property


        #region Stage property
        internal MetalStage Stage { get; set; }
        #endregion Stage property



        #region MainMenuFont property
        private SpriteFont _mainMenuFont;

        internal SpriteFont MainMenuFont
        {
            get { return _mainMenuFont; }
            private set { _mainMenuFont = value; }
        }
        #endregion MainMenuFont property


        #region Construction
        public MetalGame()
        {
            graphics = new GraphicsDeviceManager(this);

#if !WINDOWSPHONE
            // Sets the default resolution.
            graphics.PreferredBackBufferWidth = 850;
            graphics.PreferredBackBufferHeight = 480;
#endif

            Content.RootDirectory = "Content";
        }

        public static MetalGame CreateFromXaml(string filePath)
        {
            return XamlLoader.LoadComponent<MetalGame>(filePath);
        }
        #endregion Construction


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

            #region Validation
            if ( !ShowCharacterSelection && String.IsNullOrWhiteSpace(DefaultCharacterFile) )
            {
                throw new Exception("The \"DefaultCharacterFile\" property must be set when the value of the property \"ShowCharacterSelection\" is \"false\".");
            }
            #endregion Validation

            MainMenuFont = Content.Load<SpriteFont>("Calibri_18");

            MainMenu = XamlLoader.LoadComponent<MetalMenu>(MainMenuFile);
            MainMenu.SetParent(this);
            //MainMenu.Initialize();
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
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
            if ( GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed )
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }


        protected override bool BeginDraw()
        {
            // Clears any sprite drawn in the screen an filld it with black.
            GraphicsDevice.Clear(Color.Black);
            //GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch.Begin();

            return base.BeginDraw();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        protected override void EndDraw()
        {
            SpriteBatch.End();
            
            base.EndDraw();
        }


        /// <summary>
        /// Starts a new game.
        /// </summary>
        internal void StartGame()
        {
            // TODO: Logic to start the game.

            //MainMenu.Dispose();
            //MainMenu = null;

            MainMenu.Deactivate();

            if ( ShowCharacterSelection )
            {
                // TODO: Show character selection screen.
            }
            else
            {
                LoadCharacter(DefaultCharacterFile);
                LoadStartingStage();
            }
        }


        /// <summary>
        /// Loads the player character, based on the given XAML file name.
        /// </summary>
        /// <param name="characterFile">Name of the XAML file to be loaded, conained in the Characters folder.</param>
        private void LoadCharacter(string characterFile)
        {
            Character = XamlLoader.LoadComponent<MetalCharacter>("Characters/" + characterFile);
            Character.SetParent(this, DefaultCharacterFile);
        }


        private void LoadStartingStage()
        {
            // TODO: Verify if is loading a saved game and load other stage.

            LoadStage(1);
        }

        private void LoadStage(byte stageNumber)
        {
            Stage = XamlLoader.LoadComponent<MetalStage>(String.Format("Stages/Stage{0}", stageNumber.ToString("000")));
            Stage.SetParent(this);
            //Stage.Initialize();
        }

    }
}
