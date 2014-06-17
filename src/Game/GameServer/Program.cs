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
        DISCONNECTION
    }

    public enum Map
    {
        Town,
    }

    #endregion

    public static class Program
    {
        #region Constants

        const string APP_NAME = "TROMA";
        const int PORT = 14242;
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

        #region Initialization

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
                            Console.WriteLine("Incoming connection.");
#endif

                            PlayerConnected();

#if DEBUG
                            Console.WriteLine("Approved new connection and sended initial data.\n");
#endif
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
                        Console.WriteLine(IncMsg.SenderConnection.ToString() + " status changed. " +
                            (NetConnectionStatus)IncMsg.SenderConnection.Status + "\n");
#endif

                        if (IncMsg.SenderConnection.Status == NetConnectionStatus.Disconnected ||
                            IncMsg.SenderConnection.Status == NetConnectionStatus.Disconnecting)
                        {
                            for (int i = 0; i < MAX_CLIENT; i++)
                            {
                                if (!OpenSlots[i] && 
                                    Clients[i].Connection == IncMsg.SenderConnection)
                                {
                                    PlayerDisconnected(Clients[i]);
                                    break;
                                }
                            }

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

                        #endregion

                    default:
                        Console.WriteLine("Receive a message that are not filtering.");
                        break;
                }

                Server.Recycle(IncMsg); // generate less garbage
            }
        }

        static void PlayerConnected()
        {
            int slot = FindOpenSlot(OpenSlots);

            if (slot >= 0)
            {
                OpenSlots[slot] = false;
                IncMsg.SenderConnection.Approve();

                string name = IncMsg.ReadString();

                Player player = new Player(name, slot, IncMsg.SenderConnection);
                player.Reset(INITIAL_POS, INITIAL_ROT);
                Clients[slot] = player;

                OutMsg = Server.CreateMessage();
                OutMsg.Write((byte)PacketTypes.LOGIN);
                OutMsg.Write(player.Name);
                OutMsg.Write(player.Slot);
                OutMsg.WritePlayerState(player.State);
                OutMsg.Write((byte)TERRAIN);

                // Message contains
                // byte = Type
                // string = Player name
                // int = Player ID
                // sizeof(STATE) =  Player State
                // byte = Map

                // Send data to all player
                Server.SendToAll(OutMsg, NetDeliveryMethod.UnreliableSequenced);

#if DEBUG
                Console.WriteLine(String.Format("Player {0} connected (slot {1})",
                    player.Name, player.Slot));
#endif
            }
            else
                IncMsg.SenderConnection.Deny();
        }

        static void PlayerDisconnected(Player player)
        {
            // send message to all and open the slot
            OpenSlots[player.Slot] = true;
            Clients[player.Slot] = null;
            
            //Server.Connections.Remove(player.Connection);

            OutMsg = Server.CreateMessage();
            OutMsg.Write((byte)PacketTypes.DISCONNECTION);
            OutMsg.Write(player.Slot);

            Server.SendToAll(OutMsg, NetDeliveryMethod.UnreliableSequenced);
        }

        #region Help Methods

        static int FindOpenSlot(Boolean[] slots)
        {
            int i = 0;

            while (i < slots.Length && slots[i] == false)
                i++;

            return ((i < slots.Length) ? i : -1);
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
