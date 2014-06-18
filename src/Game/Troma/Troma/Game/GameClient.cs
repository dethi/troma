using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Lidgren.Network;
using ClientServerExtension;

namespace Troma
{
    public class GameClient
    {
        #region Constants

        private const string APP_NAME = "TROMA";
        private const int PORT = 11420;
        private const int MAX_CLIENT = 20;
        private const int DT = 30; // ms

        #endregion

        #region Fields

        private NetClient Client;
        private NetPeerConfiguration Config;
        private NetIncomingMessage IncMsg;
        private NetOutgoingMessage OutMsg;

        public int ID;
        public STATE State;
        public Map Terrain;

        public List<OtherPlayer> Players;

        #endregion

        public GameClient(string host)
        {
            Initialize();
            SetupClient();

            Connect(host);
            WaitInitialData();
        }

        #region Initialization

        private void Initialize()
        {
            Players = new List<OtherPlayer>();
            Players.Capacity = MAX_CLIENT;
        }

        private void SetupClient()
        {
            Config = new NetPeerConfiguration(APP_NAME);
            Config.EnableMessageType(NetIncomingMessageType.Data);
            Config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

#if DEBUG
            Config.EnableMessageType(NetIncomingMessageType.WarningMessage);
            Config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            Config.EnableMessageType(NetIncomingMessageType.ErrorMessage);
            Config.EnableMessageType(NetIncomingMessageType.Error);
            Config.EnableMessageType(NetIncomingMessageType.DebugMessage);
#endif

            Client = new NetClient(Config);
            Client.Start();

#if DEBUG
            Console.WriteLine("Client start.");
#endif
        }

        private void Connect(string host)
        {
            OutMsg = Client.CreateMessage();
            OutMsg.Write((byte)PacketTypes.LOGIN);
            OutMsg.Write(Settings.Name);

            Client.Connect(host, PORT, OutMsg);

#if DEBUG
            Console.WriteLine("Send connection request to server.");
#endif
        }

        private void WaitInitialData()
        {
            bool canStart = false;

#if DEBUG
            Console.WriteLine("Waiting initial data...");
#endif

            while (!canStart)
            {
                if ((IncMsg = Client.ReadMessage()) != null)
                {
                    switch (IncMsg.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            #region Connection Approved

                            switch(IncMsg.ReadPacketType())
                            {
                                case PacketTypes.LOGIN:
                                    ID = IncMsg.ReadInt32();
                                    Terrain = (Map)IncMsg.ReadByte();
                                    break;
                                
                                case PacketTypes.SPAWN:
                                    if (IncMsg.ReadInt32() == ID)
                                    {
                                        State = IncMsg.ReadPlayerState();
                                        canStart = true;

#if DEBUG
                                        Console.WriteLine("Confirm initial data!");
#endif
                                    }

                                    break;
                                
                                default:
                                    break;
                            }

                            break;

                            #endregion

#if DEBUG
                        case NetIncomingMessageType.VerboseDebugMessage:
                            break;

                        case NetIncomingMessageType.DebugMessage:
                            break;

                        case NetIncomingMessageType.WarningMessage:
                            break;

                        case NetIncomingMessageType.ErrorMessage:
                            break;

                        case NetIncomingMessageType.Error:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Corrupted message!!!");
                            break;
#endif

                        default:
                            break;
                    }

#if DEBUG
                    Console.ResetColor();
#endif

                    Client.Recycle(IncMsg); // generate less garbage
                }

                System.Threading.Thread.Sleep(1);
            }
        }

        #endregion

        public void HandleMsg()
        {
            if ((IncMsg = Client.ReadMessage()) != null)
            {
                switch (IncMsg.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        // process data
                        break;

                    default:
                        break;
                }

                Client.Recycle(IncMsg); // generate less garbage
            }
        }

        public void SendState()
        {

        }

        public void SendInput()
        {
        }

        public void SendShoot()
        {
            OutMsg = Client.CreateMessage();
            OutMsg.Write((byte)PacketTypes.SHOOT);

            Client.SendMessage(OutMsg, NetDeliveryMethod.Unreliable, 0);
        }
    }

    public static class Extends
    {
        #region Extends Methods

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
        public static void WritePlayerState(this NetBuffer buffer, INPUT input)
        {
            buffer.Write(input.IsMove);
            buffer.Write(input.IsRun);
            buffer.Write(input.IsCrouch);

            buffer.Write(input.IsReload);
            buffer.Write(input.InSightPosition);
            buffer.Write((byte)input.Weapon);
        }

        #endregion
    }
}
