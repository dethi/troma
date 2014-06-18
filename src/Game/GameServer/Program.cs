﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Lidgren.Network;
using ClientServerExtension;

namespace GameServer
{
    #region Protocol Documenation

    //=========================
    // Client connect to server
    //=========================

    // type = LOGIN
    // string = client name

    // 1) Send initial data to the new client
    //
    // ReliableOrdered (ch. 1)
    // type = LOGIN
    // int = new client ID
    // Map = terrain

    // 2) Send spawn


    //==========================
    // Client disconnect (ch. 0)
    //==========================
    //
    // Unreliable
    // type = QUIT
    // int = client ID


    //==============
    // State (ch. 2)
    //==============
    //
    // UnreliableSequenced
    // type = STATE
    // int = client ID
    // string = client name
    // State = client state


    //==============
    // Spawn (ch. 1)
    //==============
    //
    // ReliableOrdered
    // type = SPAWN
    // State = client state


    //=============
    // Kill (ch. 1)
    //=============
    //
    // ReliableOrdered
    // type = KILL


    //==============
    // Shoot (ch. 0)
    //==============
    //
    // Unreliable
    // type = SHOOT
    // int = client ID


    //==============
    // Input (ch. 3)
    //==============
    //
    // UnreliableSequenced
    // type = INPUT
    // int = client ID
    // Input = client input

    #endregion

    public static class Program
    {
        #region Constants

        const string APP_NAME = "TROMA";
        const int PORT = 11420;
        const int MAX_CLIENT = 20;
        const int DT = 30; // ms

        const Map TERRAIN = Map.Town;
        static Vector3 INITIAL_POS = Vector3.One * 30f;
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
                System.Threading.Thread.Sleep(1); // slow server, less CPU used
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
                        #region Process Data

                        Player p;

                        switch (IncMsg.ReadPacketType())
                        {
                            case PacketTypes.STATE:
                                p = FindPlayer(IncMsg.SenderConnection, Clients);

                                if (p == null)
                                    break;

                                p.State = IncMsg.ReadPlayerState();

                                OutMsg = Server.CreateMessage();
                                OutMsg.Write((byte)PacketTypes.STATE);
                                OutMsg.Write(p.ID);
                                OutMsg.Write(p.Name);
                                OutMsg.WritePlayerState(p.State);

                                Server.SendToAll(OutMsg, p.Connection,
                                    NetDeliveryMethod.UnreliableSequenced, 2);

                                break;

                            case PacketTypes.INPUT:
                                p = FindPlayer(IncMsg.SenderConnection, Clients);

                                if (p == null)
                                    break;

                                p.Input = IncMsg.ReadPlayerInput();

                                OutMsg = Server.CreateMessage();
                                OutMsg.Write((byte)PacketTypes.INPUT);
                                OutMsg.Write(p.ID);
                                OutMsg.WritePlayerInput(p.Input);

                                Server.SendToAll(OutMsg, p.Connection,
                                    NetDeliveryMethod.UnreliableSequenced, 3);

                                break;

                            case PacketTypes.SHOOT:
                                p = FindPlayer(IncMsg.SenderConnection, Clients);

                                if (p == null)
                                    break;

                                OutMsg = Server.CreateMessage();
                                OutMsg.Write((byte)PacketTypes.SHOOT);
                                OutMsg.Write(p.ID);

                                Server.SendToAll(OutMsg, p.Connection,
                                    NetDeliveryMethod.Unreliable, 0);

#if DEBUG
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.WriteLine("Player {0} have shooted.", p.ID);
#endif

                                ComputeShoot(p);

                                break;

                            default:
                                break;
                        }

                        break;

                        #endregion

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
                            Player pDeco = FindPlayer(IncMsg.SenderConnection, Clients);

                            if (pDeco != null)
                                PlayerDisconnected(pDeco);
                        }
                        else if (IncMsg.SenderConnection.Status == NetConnectionStatus.Connected)
                        {
                            Player pCo = FindPlayer(IncMsg.SenderConnection, WaitingQueue);

                            if (pCo != null)
                                SendInitialData(pCo);
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

                WaitingQueue.Add(player);
                NextID++;

#if DEBUG
                Console.WriteLine(String.Format("Player {0} connected (id {1})",
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
            OutMsg.Write((byte)TERRAIN);

            Server.SendMessage(OutMsg, player.Connection, NetDeliveryMethod.ReliableOrdered, 1);

            STATE state = new STATE()
            {
                Position = INITIAL_POS,
                Rotation = INITIAL_ROT
            };

            OutMsg = Server.CreateMessage();
            OutMsg.Write((byte)PacketTypes.SPAWN);
            OutMsg.WritePlayerState(state);

            Server.SendMessage(OutMsg, player.Connection, NetDeliveryMethod.ReliableOrdered, 1);
        }

        static void PlayerDisconnected(Player player)
        {
            Clients.Remove(player);

            // send msg to all
            OutMsg = Server.CreateMessage();
            OutMsg.Write((byte)PacketTypes.QUIT);
            OutMsg.Write(player.ID);

            // type = QUIT
            // int = ID

            Server.SendToAll(OutMsg, null, NetDeliveryMethod.Unreliable, 0);
        }

        static void ComputeShoot(Player player)
        {
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

        static Player FindPlayer(int id, List<Player> list)
        {
            foreach (Player player in list)
            {
                if (player.ID == id)
                    return player;
            }

            return null;
        }

        #endregion
    }
}
