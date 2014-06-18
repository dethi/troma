using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Lidgren.Network;

namespace Troma
{
    #region Enum

    public enum PacketTypes
    {
        LOGIN,
        INPUT,
        STATUS,
        QUIT,
        NEW
    }

    #endregion

    public class GameClient
    {
        #region Constants

        const string APP_NAME = "TROMA";
        const int PORT = 11420;
        const int MAX_CLIENT = 20;
        const int DT = 30; // ms

        #endregion

        #region Fields

        static NetClient Client;
        static NetPeerConfiguration Config;
        static NetIncomingMessage IncMsg;
        static NetOutgoingMessage OutMsg;

        static int ID;
        static STATE State;
        static Map Terrain;

        static List<OtherPlayer> Players;

        static DateTime Time;
        static TimeSpan TimeToPass;

        #endregion

        public GameClient(string host)
        {
            Initialize();
            SetupClient();

            Connect(host);
            WaitInitialData();
        }

        #region Initialization

        static void Initialize()
        {
            Players = new List<OtherPlayer>();
            Players.Capacity = MAX_CLIENT;

            Time = DateTime.Now;
            TimeToPass = new TimeSpan(0, 0, 0, 0, DT);
        }

        static void SetupClient()
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

        static void Connect(string host)
        {
            OutMsg = Client.CreateMessage();
            OutMsg.Write((byte)PacketTypes.LOGIN);
            OutMsg.Write(Settings.Name);

            Client.Connect(host, PORT, OutMsg);

#if DEBUG
            Console.WriteLine("Send connection request to server.");
#endif
        }

        static void WaitInitialData()
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

                            if (IncMsg.ReadPacketType() == PacketTypes.LOGIN)
                            {
                                ID = IncMsg.ReadInt32();
                                State = IncMsg.ReadPlayerState();
                                Terrain = (Map)IncMsg.ReadByte();

                                canStart = true;

#if DEBUG
                                Console.WriteLine("Confirm initial data!");
#endif
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
                            //Console.WriteLine("Receive a message that are not filtering.");
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
        /// Write the Player Status in the buffer
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

        #endregion
    }
}
