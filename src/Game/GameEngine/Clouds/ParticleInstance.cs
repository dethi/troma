using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameEngine
{
    public class ParticleInstance
    {
        #region Fields

        private static int nextID = 1;
        private int _id;

        public int ID
        {
            get { return _id; }
        }

        private Vector3 _position;
        private Vector3 _scale;
        private Quaternion _rotation;
        private Vector3 _mods;

        public Matrix World;
        public Matrix AAWorld;

        private Random _rand;
        private ParticleInstancer _instancer;

        #endregion

        #region Initialization

        public ParticleInstance(ParticleInstancer instancer)
            : this(instancer, Vector3.Zero, 0.5f * Vector3.One, Vector3.Zero, Quaternion.Identity)
        { }

        public ParticleInstance(ParticleInstancer instancer, Vector3 pos, Vector3 scale)
            : this(instancer, pos, scale, Vector3.Zero, Quaternion.Identity)
        { }

        public ParticleInstance(ParticleInstancer instancer, Vector3 pos, Vector3 scale, Vector3 mods)
            :this(instancer, pos, scale, mods, Quaternion.Identity)
        { }

        public ParticleInstance(ParticleInstancer instancer, Vector3 pos, Vector3 scale, Quaternion rot)
            : this(instancer, pos, scale, Vector3.Zero, rot)
        { }

        public ParticleInstance(ParticleInstancer instancer, Vector3 pos, Vector3 scale, Vector3 mods, Quaternion rot)
        {
            _id = nextID;
            nextID++;

            _instancer = instancer;
            _position = pos;
            _scale = scale;
            _mods = mods;
            _rotation = rot;

            _instancer.InstancesTransformMatrices.Add(this, World);
            _instancer.Instances.Add(ID, this);

            _rand = new Random(DateTime.Now.Millisecond);

            // Forces the first calculation
            Update(null);
        }

        #endregion

        public void Update(GameTime gameTime)
        {
            World = Matrix.CreateScale(_scale) * 
                Matrix.CreateFromQuaternion(_rotation) * 
                Matrix.CreateTranslation(_position);

            // Set the scale
            World.M13 = _scale.X;
            World.M24 = _scale.Y;

            // Set the image, alpha and color mod
            World.M12 = _mods.X;
            World.M23 = _mods.Y;
            World.M34 = _mods.Z;

            _instancer.InstancesTransformMatrices[this] = World;
        }

        public void TranslateAA(Vector3 distance)
        {
            _position += Vector3.Transform(distance, Matrix.Identity);
        }

        private bool Moved(Vector3 distance)
        {
            return distance != Vector3.Zero;
        }
    }
}
