using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using EquinoxEngine;
using EquinoxEngine.Terrain;

namespace Arrow
{
    public class TestScreen : GameScreen
    {
        #region Fields

        GraphicsDeviceManager _graphics;
        GraphicsDevice _device;

        ContentManager content;

        TestCamera _camera;
        QuadTree _quadTree;
        BackgroundWorker _terrainWorker;

        bool _isWire;

        #endregion

        #region Initialization

        public TestScreen(Game game)
            : base(game)
        {
            _graphics = game.graphics;
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            _device = _graphics.GraphicsDevice;
            game.IsMouseVisible = true;
            game.Window.AllowUserResizing = true;

            Texture2D heightmap = content.Load<Texture2D>("hmSmall");

            _camera = new TestCamera(new Vector3(300, 50, 300), 
                new Vector3(300, 0, 100), _device, 7000f);

            _quadTree = new QuadTree(Vector3.Zero, heightmap, 
                _camera.View, _camera.Projection, _device, 1);

            _quadTree.MinimumDepth = 0;
            _quadTree.Cull = true;
            _quadTree.Effect.Texture = content.Load<Texture2D>("jigsaw");

            _isWire = false;

            ScreenManager.Game.ResetElapsedTime();
        }

        /// <summary>
        /// Unload graphics content used by the game
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }

        #endregion

        public override void Update(GameTime gameTime, bool hasFocus, bool isVisible)
        {
            if (_isWire)
                game.ChangeRasterizerState(CullMode.CullCounterClockwiseFace, FillMode.WireFrame);
            else
                game.ChangeRasterizerState(CullMode.CullCounterClockwiseFace, FillMode.Solid);

            _quadTree.View = _camera.View;
            _quadTree.Projection = _camera.Projection;
            _quadTree.CameraPosition = _camera.Position;
            _quadTree.Update(gameTime);

            game.Window.Title = String.Format("Triangles Rendered: {0} - Culling Enabled: {1}", 
                _quadTree.IndexCount / 3, _quadTree.Cull);
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input.IsPressed(Keys.C))
                _quadTree.Cull = !_quadTree.Cull;

            if (input.IsPressed(Keys.W))
                _isWire = !_isWire;

            if (input.IsDown(Keys.Up))
                _camera.Move(new Vector3(0, 0, -0.5f));
            else if (input.IsDown(Keys.Down))
                _camera.Move(new Vector3(0, 0, 0.5f));
        }

        public override void Draw(GameTime gameTime)
        {
            _quadTree.Draw(gameTime);
        }
    }
}
