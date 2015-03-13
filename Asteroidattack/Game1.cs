using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Asteroidattack
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //these imaginatively named variables are for graphics, the last 2 are for planets
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont Scorefont;
        Rectangle World;
        Camera2d Camera;
        const int bodycount=8,astcount=5; 
        planet[] planets = new planet[bodycount];
        List<asteroid> asteroids = new List<asteroid>();
        body sun;
        Texture2D suntext;
        Vector2 sunvect = Vector2.Zero;
        //this is the game!!!
        public Game1()
        {
            //here we tell it where all the pictures are and set screen size
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 480;
            graphics.PreferredBackBufferWidth = 800;
            Content.RootDirectory = "Content";
            // then put the screen size into a rectangle
            World =new Rectangle(
            0,
            0,
            1000,
            1000);
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
            this.IsMouseVisible = true;
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

            
            Camera = new Camera2d(GraphicsDevice.Viewport, World.Width, World.Height, 1.0f);
            Scorefont = Content.Load<SpriteFont>("Scorefont");
            // body needs a sprite texture thing
            Texture2D tempTexture =Content.Load<Texture2D>("planet1");
            planet mer = new planet(0.00017385, 0.00, 0.4471, -0.02737, 0.00, tempTexture);
            tempTexture = Content.Load<Texture2D>("planet2");
            planet ven= new planet(0.0026, -0.7257, 0.00, 0.00, -0.0202,tempTexture);
            tempTexture = Content.Load<Texture2D>("planet3");
            planet earth= new planet(0.0031, 1.00, 0.00, 0.00, 0.0172,tempTexture);
            tempTexture = Content.Load<Texture2D>("planet4");
            planet mars= new planet(0.00033795, 0.00, -1.417, 0.0139, 0.00,tempTexture);
            tempTexture = Content.Load<Texture2D>("planet5");
            planet jup = new planet(1.0, 0.00, 5.306, -0.00754, 0.00,tempTexture);
            tempTexture = Content.Load<Texture2D>("planet6");
            planet sat= new planet(0.2993, 0.00, 9.964, -0.00557, 0.00,tempTexture);
            tempTexture = Content.Load<Texture2D>("planet7");
            planet ura= new planet(0.045719, -20.0, 0.00, 0.00, -0.00392,tempTexture);
            tempTexture = Content.Load<Texture2D>("planet10");
            planet nep= new planet(0.053934, -29.97, 0.0, 0.00, -0.00314,tempTexture);
            planets[0] = mer;
            planets[1] = ven;
            planets[2] = earth;
            planets[3] = mars;
            planets[4] = jup;
            planets[5] = sat;
            planets[6] = ura;
            planets[7] = nep;


            planets[2].inhabit=true;

            //asteroids
            asteroid.LoadContent(this.Content);

            for (int i = 0; i < astcount; i++)
            {
                asteroids.Add(new asteroid(World, planets, Camera));
            }

            //The SUN!
            suntext = Content.Load<Texture2D>("sun");
            sun = new body(1000.0, 0.0, 0.0, 0.0, 0.0, suntext);
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
            KeyboardState keyboardState = Keyboard.GetState();
            Vector2 movement = Vector2.Zero;
            // Allows the game to exit
            if (keyboardState.IsKeyDown(Keys.Escape) == true)
                this.Exit();

            // runs planet update 200 times per refresh. this is so planet gravity timesteps are tiny but planets move quickly on screen
            for (int t = 0; t < 300; t++)
            {
                for (int i = 0; i < bodycount-1; i++)
                {
                    planets[i].update(planets,Camera);
                }
                foreach (asteroid ast in asteroids)
                {
                    ast.update(planets);
                }
                
            }

            Camera.update(keyboardState);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Plum);
            spriteBatch.Begin(SpriteSortMode.BackToFront,null,null,null,null,null,Camera.GetTransformation());
           for (int i = 0; i<bodycount-1; i++)
           {
              planets[i].Draw(spriteBatch,Scorefont);
           }

           foreach (asteroid ast in asteroids)
           {
               ast.Draw(spriteBatch);
           }

           sun.Draw(spriteBatch);
            spriteBatch.End();


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
