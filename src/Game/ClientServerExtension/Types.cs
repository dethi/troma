using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ClientServerExtension
{
    public enum PacketTypes
    {
        LOGIN,
        NEW,
        QUIT,
        STATE,
        INPUT,
        SHOOT,
        KILL,
        SPAWN
    }

    public struct STATE
    {
        public Vector3 Position;
        public Vector3 Rotation;
    }

    public struct INPUT
    {
        public bool IsMove;
        public bool IsRun;
        public bool IsCrouch;

        public bool IsReload;
        public bool InSightPosition;
        public Weapons Weapon;
    }

    public enum Weapons
    {
        M1,
        M1911
    }

    public enum Map
    {
        Town,
        Cracovie,
    }
}
