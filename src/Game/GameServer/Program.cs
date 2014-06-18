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
        STATUS,
        QUIT,
        NEW
    }

    public enum Map
    {
        Town,
        Cracovie,
    }

    #endregion

    public static class Program
    {
        #region Constants

        const string APP_NAME = "TROMA";
        const int PORT = 11420;
        const int MAX_CLIENT = 20;
        const int DT = 30; // ms

        const Map TERRAIN = Map.Town;
        static Vector3 INITIAL_POS = Vector3.One * 15f;
        static Vector3 INITIAL_ROT = Vector3.Zero;

        #endregion

        #region Fields

        static NetServer Server;
        static NetPeerConfiguration Config;
        static NetIncomingMessage IncMsg;
        static NetOutgoingMessage OutMsg;

        static int NextID;
        static List<Player> Clients;
        static List<Player> WaitingQueue;

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
                //System.Threading.Thread.Sleep(1); // slow server, less CPU used
            }
        }

        #region Initialization

        static void Initialize()
        {
            Clients = new List<Player>();
            Clients.Capacity = MAX_CLIENT;
            NextID = 0;

            WaitingQueue = new List<Player>();
            WaitingQueue.Capacity = MAX_CLIENT / 4;

            Time = DateTime.Now;
            TimeToPass = new TimeSpan(0, 0, 0, 0, DT);
        }

        static void SetupServer()
        {
            Config = new NetPeerConfiguration(APP_NAME);
            Config.Port = PORT;
            Config.MaximumConnections = MAX_CLIENT;
            Config.EnableMessageType(NetIncomingMessageType.Data);
            Config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

#if DEBUG
            Config.EnableMessageType(NetIncomingMessageType.WarningMessage);
            Config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            Config.EnableMessageType(NetIncomingMessageType.ErrorMessage);
            Config.EnableMessageType(NetIncomingMessageType.Error);
            Config.EnableMessageType(NetIncomingMessageType.DebugMessage);

            Config.SimulatedMinimumLatency = 0.03f; // 30ms
            Config.SimulatedRandomLatency = 0.1f; // 100ms
#endif

            Server = new NetServer(Config);
            Server.Start();

#if DEBUG
            Console.WriteLine("Server started.");
#endif
        }

        #endregion

        static void HandleMsg()
        {
            if ((IncMsg = Server.ReadMessage()) != null)
            {
                switch (IncMsg.MessageType)
                {
                    case NetIncomingMessageType.ConnectionApproval:
                        #region New Connection

                        if (IncMsg.ReadPacketType() == PacketTypes.LOGIN)
                        {
#if DEBUG
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Incoming connection.");
#endif

                            PlayerConnected();
                        }

                        break;

                        #endregion

                    case NetIncomingMessageType.Data:
                        // process data
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        // NetConnectionStatus.Connected;
                        // NetConnectionStatus.Connecting;
                        // NetConnectionStatus.Disconnected;
                        // NetConnectionStatus.Disconnecting;
                        // NetConnectionStatus.None;

                        #region Status Changed

#if DEBUG
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine(IncMsg.SenderConnection.ToString() + " status changed. " +
                            (NetConnectionStatus)IncMsg.SenderConnection.Status + "\n");
#endif

                        if (IncMsg.SenderConnection.Status == NetConnectionStatus.Disconnected ||
                            IncMsg.SenderConnection.Status == NetConnectionStatus.Disconnecting)
                        {
                            // Find disconnected player and remove it
                            Player p = FindPlayer(IncMsg.SenderConnection, Clients);

                            if (p != null)
                                PlayerDisconnected(p);
                        }
                        else if (IncMsg.SenderConnection.Status == NetConnectionStatus.Connected)
                        {
                            Player p = FindPlayer(IncMsg.SenderConnection, WaitingQueue);

                            if (p != null)
                                SendInitialData(p);
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

                Server.Recycle(IncMsg); // generate less garbage
            }
        }

        static void PlayerConnected()
        {
            if (Clients.Count < MAX_CLIENT)
            {
                IncMsg.SenderConnection.Approve();

                string name = IncMsg.ReadString();

                Player player = new Player(name, NextID, IncMsg.SenderConnection);
                player.Reset(INITIAL_POS, INITIAL_ROT);

                WaitingQueue.Add(player);
                NextID++;

                // Send msg to all except player
                OutMsg = Server.CreateMessage();
                OutMsg.Write((byte)PacketTypes.NEW);
                OutMsg.Write(player.ID);
                OutMsg.Write(player.Name);
                OutMsg.WritePlayerState(player.State);

                // type = NEW
                // int = ID
                // string = Name
                // State = Player state

                Server.SendToAll(OutMsg, player.Connection, NetDeliveryMethod.Unreliable, 0);

#if DEBUG
                Console.WriteLine(String.Format("Player {0} connected (slot {1})",
                    player.Name, player.ID));
#endif
            }
            else
                IncMsg.SenderConnection.Deny();
        }

        static void SendInitialData(Player player)
        {
            WaitingQueue.Remove(player);
            Clients.Add(player);

            OutMsg = Server.CreateMessage();
            OutMsg.Write((byte)PacketTypes.LOGIN);
            OutMsg.Write(player.ID);
            OutMsg.Write(player.Name);
            OutMsg.WritePlayerState(player.State);
            OutMsg.Write((byte)TERRAIN);

            // type = LOGIN
            // int = ID
            // string = Name
            // State = Player state
            // Map = terrain

            Server.SendMessage(OutMsg, player.Connection, NetDeliveryMethod.Unreliable, 0);
        }

        static void PlayerDisconnected(Player player)
        {
            // remove and disconnect
            Clients.Remove(player);
            player.Connection.Disconnect("END");

            //Server.Connections.Remove(player.Connection);

            // send msg to all
            OutMsg = Server.CreateMessage();
            OutMsg.Write((byte)PacketTypes.QUIT);
            OutMsg.Write(player.ID);

            // type = QUIT
            // int = ID

            Server.SendMessage(OutMsg, Server.Connections, NetDeliveryMethod.Unreliable, 0);
        }

        #region Help Methods

        static Player FindPlayer(NetConnection co, List<Player> list)
        {
            foreach (Player player in list)
            {
                if (player.Connection == co)
                    return player;
            }

            return null;
        }

        #endregion

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
