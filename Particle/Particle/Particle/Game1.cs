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

namespace Particle
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private Renderer renderer;
        private Sound sound;
        private ParticleControl particleControl;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = Parameter.ScreenWidth;
            graphics.PreferredBackBufferHeight = Parameter.ScreenHeight;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// 初期化
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            renderer = new Renderer(Content,GraphicsDevice);
            sound = new Sound(Content);
            particleControl = new ParticleControl(sound);

            base.Initialize();
        }

        /// <summary>
        /// 素材ロード
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            renderer.LoadTexture("firework");
            sound.LoadSE("fireburn");
            sound.LoadSE("fireup");
        }

        /// <summary>
        /// 素材アンロード
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            renderer.Unload();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed||
                Keyboard.GetState().IsKeyDown(Keys.Escape)) this.Exit();

            // TODO: Add your update logic here
            particleControl.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            renderer.Begin();

            particleControl.Draw(renderer);

            renderer.End();

            base.Draw(gameTime);
        }
    }
}
