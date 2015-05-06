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

namespace GraphicsPractical1
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private FrameRateCounter frameRateCounter;
        private BasicEffect effect;
        private Camera camera;
        private Terrain terrain;

        //Load variables for the XNA framework
        public Game1()
        {
            
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.frameRateCounter = new FrameRateCounter(this);
            this.Components.Add(this.frameRateCounter);
        }

        //Initialize the configuration
        protected override void Initialize()
        {
            this.graphics.PreferredBackBufferWidth = 800;
            this.graphics.PreferredBackBufferHeight = 600;
            this.graphics.IsFullScreen = false;
            this.graphics.SynchronizeWithVerticalRetrace = false;
            this.graphics.ApplyChanges();
            this.IsFixedTimeStep = false;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Create the effect
            this.effect = new BasicEffect(this.GraphicsDevice);
            this.effect.VertexColorEnabled = true;
            this.effect.LightingEnabled = true;
            this.effect.DirectionalLight0.Enabled = true;
            this.effect.DirectionalLight0.DiffuseColor = Color.White.ToVector3();
            this.effect.DirectionalLight0.Direction = new Vector3(0, -1, 0);
            this.effect.AmbientLightColor = new Vector3(0.3f);

            //Create the camera
            this.camera = new Camera(new Vector3(60, 80, -80), new Vector3(0, 0, 0), new Vector3(0, 1, 0));

            //Load the map and the terrain that's calculated from it
            Texture2D map = Content.Load<Texture2D>("heightmap");
            this.terrain = new Terrain(new HeightMap(map), 0.2f, GraphicsDevice);
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            base.Update(gameTime);

            //Show the framerate in the title of the window
            this.Window.Title = "Graphics Tutorial | FPS: " + this.frameRateCounter.FrameRate;

            //Set the timestep, which is used to calculate how much the terrain must be rotated
            float timeStep = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Rotate the terrain if left or right is pressed
            float deltaAngle = 0;
            KeyboardState kbState = Keyboard.GetState();
            if (kbState.IsKeyDown(Keys.Left))
                deltaAngle += -3 * timeStep;
            if (kbState.IsKeyDown(Keys.Right))
                deltaAngle += 3 * timeStep;
            if (deltaAngle != 0)
                this.camera.Eye = Vector3.Transform(this.camera.Eye, Matrix.CreateRotationY(deltaAngle));
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.RasterizerState = new RasterizerState
            {
                //Culling off for designing
                CullMode = CullMode.None,
                //Fill the triangles solid so they don't look like just triangles
                FillMode = FillMode.Solid
            };

            //Clear the window so the background is CornflowerBlue
            this.GraphicsDevice.Clear(Color.CornflowerBlue);

            //Change the effect according to the properties of the camera object
            this.effect.Projection = this.camera.ProjectionMatrix;
            this.effect.View = this.camera.ViewMatrix;

            //Move the terrain to the origin
            Matrix translation = Matrix.CreateTranslation(-0.5f * this.terrain.Width, 0, 0.5f * this.terrain.Width);
            this.effect.World = translation;

            //Apply the effects
            foreach (EffectPass pass in this.effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                this.terrain.Draw(this.GraphicsDevice);
            }

            base.Draw(gameTime);
        }
    }
}
