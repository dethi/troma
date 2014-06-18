using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Lidgren.Network;

namespace ClientServerExtension
{
    public static class Extends
    {
        /// <summary>
        /// Extend NetIncomingMessage.
        /// Return the type value (one byte) of the Incoming Message
        /// </summary>
        public static PacketTypes ReadPacketType(this NetIncomingMessage msg)
        {
            return (PacketTypes)msg.ReadByte();
        }

        /// <summary>
        /// Extend NetIncomingMessage.
        /// Read a Player State
        /// </summary>
        public static STATE ReadPlayerState(this NetIncomingMessage msg)
        {
            return new STATE()
            {
                Position = new Vector3(
                    msg.ReadFloat(),
                    msg.ReadFloat(),
                    msg.ReadFloat()),

                Rotation = new Vector3(
                    msg.ReadFloat(),
                    msg.ReadFloat(),
                    msg.ReadFloat())
            };
        }

        /// <summary>
        /// Extend NetBuffer.
        /// Write the Player State in the buffer
        /// </summary>
        public static void WritePlayerState(this NetBuffer buffer, STATE state)
        {
            buffer.Write(state.Position.X);
            buffer.Write(state.Position.Y);
            buffer.Write(state.Position.Z);

            buffer.Write(state.Rotation.X);
            buffer.Write(state.Rotation.Y);
            buffer.Write(state.Rotation.Z);
        }

        /// <summary>
        /// Extend NetIncomingMessage.
        /// Read a Player Input
        /// </summary>
        public static INPUT ReadPlayerInput(this NetIncomingMessage msg)
        {
            return new INPUT()
            {
                IsMove = msg.ReadBoolean(),
                IsRun = msg.ReadBoolean(),
                IsCrouch = msg.ReadBoolean(),

                IsReload = msg.ReadBoolean(),
                InSightPosition = msg.ReadBoolean(),
                Weapon = (Weapons)msg.ReadByte()
            };
        }

        /// <summary>
        /// Extend NetBuffer.
        /// Write the Player Input in the buffer
        /// </summary>
        public static void WritePlayerInput(this NetBuffer buffer, INPUT input)
        {
            buffer.Write(input.IsMove);
            buffer.Write(input.IsRun);
            buffer.Write(input.IsCrouch);

            buffer.Write(input.IsReload);
            buffer.Write(input.InSightPosition);
            buffer.Write((byte)input.Weapon);
        }
    }
}
