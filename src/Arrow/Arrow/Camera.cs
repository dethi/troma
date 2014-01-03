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


namespace Arrow
{
    public class Camera : Microsoft.Xna.Framework.GameComponent
    {
        public float moveSpeed = 0.01f;

        private Matrix m_projection;
        private Matrix m_view;

        private Vector3 v_camPosition;
        private Vector2 v_rotation;
        private Matrix m_rotation;
        private Vector3 v_translation;

        private Vector3 v_right;
        private Vector3 v_up;
        private Vector3 v_forward;

        private List<BasicEffect> effects;
        private MouseManager mouse;

        public Camera(Game game) : this(game, Vector3.Zero) { }

        public Camera(Game game, Vector3 v_position)
            : base(game)
        {
            effects = new List<BasicEffect>();

            v_camPosition = v_position;
            v_forward = Vector3.Forward;
            v_rotation = Vector2.Zero;
            m_rotation = new Matrix();
            v_translation = Vector3.Zero;
            v_right = Vector3.Right;

            m_projection =  Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 
                Game.GraphicsDevice.Viewport.AspectRatio, 0.01f, 10000.0f);
            m_view = Matrix.CreateLookAt(v_camPosition, v_forward, Vector3.UnitZ);

            mouse = new MouseManager();
            game.GraphicsDevice.RasterizerState = new RasterizerState()
            {
                CullMode = CullMode.None,
                FillMode = FillMode.Solid
            };
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            mouse.Update(gameTime);

            if (mouse.isMove)
            {
                v_rotation.X = mouse.deltaX / 5;
                v_rotation.Y = mouse.deltaY / 5;
                m_rotation = Matrix.CreateRotationX(MathHelper.ToRadians(v_rotation.Y)) *
                    Matrix.CreateRotationY(MathHelper.ToRadians(v_rotation.X));

                v_up = Vector3.Transform(Vector3.Up, m_rotation);
                v_forward = Vector3.Transform(Vector3.Forward, m_rotation);
                v_right = Vector3.Transform(Vector3.Right, m_rotation);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W))
                v_camPosition += v_forward * moveSpeed * gameTime.ElapsedGameTime.Milliseconds;
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                v_camPosition -= v_forward * moveSpeed * gameTime.ElapsedGameTime.Milliseconds;
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                v_camPosition -= v_right * moveSpeed * gameTime.ElapsedGameTime.Milliseconds;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                v_camPosition += v_right * moveSpeed * gameTime.ElapsedGameTime.Milliseconds;

            foreach (BasicEffect that in effects)
            {
                that.View = Matrix.CreateLookAt(v_camPosition, v_camPosition + v_forward, v_up);
            }

            base.Update(gameTime);
        }

        public void AddItems(BasicEffect itemEffect)
        {
            itemEffect.Projection = m_projection;
            itemEffect.View = m_view;

            if (itemEffect.World == null)
                itemEffect.World = Matrix.Identity;

            effects.Add(itemEffect);
        }
    }
}
