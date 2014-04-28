using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public class Model3D : DrawableEntityComponent
    {
        public Model Model;
        private Effect _effect;

        public Model3D(Entity aParent, string model, Effect effect)
            : base(aParent)
        {
            Name = "Model3D";
            _requiredComponents.Add("Transform");

            _effect = effect;
            Model = FileManager.Load<Model>("Models/" + model);
        }

        public override void Draw(GameTime gameTime, ICamera camera)
        {
            throw new NotImplementedException();
        }
    }
}
