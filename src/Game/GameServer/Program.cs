using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Lidgren.Network;

namespace GameServer
{
    #region Enum

    public enum PacketTypes
    {
        LOGIN,
        INPUT,
        STATUS
    }

    #endregion

    public static class Program
    {
        #region Constants

        const string APP_NAME = "TROMA";
        const int PORT = 14242;
        const int MAX_CLIENT = 20;
        const int DT = 30; // ms

        #endregion

        #region Fields

        static NetServer Server;
        static NetPeerConfiguration Config;
        static NetIncomingMessage IncMsg;
        static NetOutgoingMessage OutMsg;

        static Player[] Clients;
        static Boolean[] OpenSlots;
        static DateTime Time;
        static TimeSpan TimeToPass;

        #endregion

        static void Main(string[] args)
        {
            Initialize();
            SetupServer();

#if DEBUG
            Console.WriteLine("Waiting messages...\n");
#endif

            while (true)
            {
                HandleMsg();
                Time = DateTime.Now;
                System.Threading.Thread.Sleep(1); // slow server, less CPU used
            }
        }

        static void Initialize()
        {
            Clients = new Player[MAX_CLIENT];
            OpenSlots = new Boolean[MAX_CLIENT];

            for (int i = 0; i < MAX_CLIENT; i++)
                OpenSlots[i] = true;

            Time = DateTime.Now;
            TimeToPass = new TimeSpan(0, 0, 0, 0, DT);
        }

        static void SetupServer()
        {
            Config = new NetPeerConfiguration(APP_NAME);
            Config.Port = PORT;
            Config.MaximumConnections = MAX_CLIENT;
            Config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

#if DEBUG
            Config.SimulatedMinimumLatency = 0.03f; // 30ms
            Config.SimulatedRandomLatency = 0.1f; // 100ms
#endif

            Server = new NetServer(Config);
            Server.Start();

#if DEBUG
            Console.WriteLine("Server started.");
#endif
        }

        static void HandleMsg()
        {
            if ((IncMsg = Server.ReadMessage()) != null)
            {
                switch (IncMsg.MessageType)
                {
                    case NetIncomingMessageType.ConnectionApproval:
                        if (IncMsg.ReadPacketType() == PacketTypes.LOGIN)
                        {
                            PlayerConnected(IncMsg);

#if DEBUG
                            Console.WriteLine("Incoming connection.");
                            Console.WriteLine("Approved new connection and sended initial data.\n");
#endif
                        }

                        break;

                    case NetIncomingMessageType.Data:
                        // process data
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        // NetConnectionStatus.Connected;
                        // NetConnectionStatus.Connecting;
                        // NetConnectionStatus.Disconnected;
                        // NetConnectionStatus.Disconnecting;
                        // NetConnectionStatus.None;

#if DEBUG
                        Console.WriteLine(IncMsg.SenderConnection.ToString() + " status changed. " +
                            (NetConnectionStatus)IncMsg.SenderConnection.Status + "\n");
#endif

                        if (IncMsg.SenderConnection.Status == NetConnectionStatus.Disconnected ||
                            IncMsg.SenderConnection.Status == NetConnectionStatus.Disconnecting)
                        {
                            // Find disconnected player and remove it
                            foreach (Player player in Clients)
                            {
                                if (player.Connection == IncMsg.SenderConnection)
                                {
                                    PlayerDisconnected(player);
                                    break;
                                }
                            }
                        }
                        break;

                    default:
                        Console.WriteLine("Receive a message that are not filtering.");
                        break;
                }

                Server.Recycle(IncMsg); // generate less garbage
            }
        }

        static void PlayerConnected(NetIncomingMessage msg)
        {
            msg.SenderConnection.Approve();
            // set initial data and send msg to all
        }

        static void PlayerDisconnected(Player player)
        {
            // send message to all and open the slot
        }

        /// <summary>
        /// Extend NetIncomingMessage.
        /// Return the type value (one byte) of the Incoming Message
        /// </summary>
        public static PacketTypes ReadPacketType(this NetIncomingMessage msg)
        {
            return (PacketTypes)msg.ReadByte();
        }
    }
}
