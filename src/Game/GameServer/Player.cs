using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Lidgren.Network;
using ClientServerExtension;

namespace GameServer
{
    public struct RayCollision
    {
        public float Distance;
        public Player CollisionWith;
    }

    public class Player
    {
        public string Name;
        public int ID;
        public NetConnection Connection;

        public STATE State;
        public INPUT Input;

        public bool Alive { get; private set; }

        public float Height
        {
            get { return (Input.IsCrouch) ? 4.8f : 7f; }
        }

        public Vector3 ViewPosition
        {
            get
            {
                return State.Position + new Vector3(0, Height, 0);
            }
        }

        public Vector3 LookAt
        {
            get
            {
                Matrix rotationMatrix = Matrix.CreateRotationX(State.Rotation.X) *
                    Matrix.CreateRotationY(State.Rotation.Y);
                Vector3 lookAtOffset = Vector3.Transform(Vector3.UnitZ, rotationMatrix);
                return ViewPosition + lookAtOffset;
            }
        }

        public Matrix World
        {
            get
            {
                return Matrix.CreateFromYawPitchRoll(State.Rotation.Y, State.Rotation.X,
                    State.Rotation.Z) * Matrix.CreateTranslation(State.Position);
            }
        }

        public Player(string name, int id, NetConnection co)
        {
            Name = name;
            ID = id;
            Connection = co;

            State = new STATE();
            Input = new INPUT();

            Alive = false;
        }

        public void Spawn(STATE state)
        {
            Alive = true;

            State = state;
            Input = new INPUT();
        }

        public void Killed()
        {
            Alive = false;
        }
    }
}
