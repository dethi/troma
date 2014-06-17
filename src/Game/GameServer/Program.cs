using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Lidgren.Network;

namespace GameServer
{
    class Program
    {
        public const string APP_NAME = "TROMA";
        public const int PORT = 14242;
        public const int MAX_CLIENT = 20;

        // Server object
        static NetServer Server;
        // Configuration object
        static NetPeerConfiguration Config;

        static void Main(string[] args)
        {
            Config = new NetPeerConfiguration(APP_NAME);
            Config.Port = PORT;
            Config.MaximumConnections = MAX_CLIENT;
            Config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            Server = new NetServer(Config);
            Server.Start();

            Console.WriteLine("Server started.");

            #region inutile
            // Create list of "Characters" ( defined later in code ). This list holds the world state. Character positions
            List<Character> GameWorldState = new List<Character>();
            #endregion

            // Object that can be used to store and read messages
            NetIncomingMessage inc;

            DateTime time = DateTime.Now;
            TimeSpan timetopass = new TimeSpan(0, 0, 0, 0, 30); // 30ms

            Console.WriteLine("Waiting for new connections and updateing world state to current ones");

            while (true)
            {
                if ((inc = Server.ReadMessage()) != null)
                {
                    switch (inc.MessageType)
                    {
                        case NetIncomingMessageType.ConnectionApproval:
                            if (inc.ReadByte() == (byte)PacketTypes.LOGIN)
                            {
                                Console.WriteLine("Incoming LOGIN");
                                inc.SenderConnection.Approve();

                                #region inutile
                                Random r = new Random();

                                // Add new character to the game.
                                // It adds new player to the list and stores name, ( that was sent from the client )
                                // Random x, y and stores client IP+Port
                                GameWorldState.Add(new Character(inc.ReadString(), r.Next(1, 40), r.Next(1, 20), inc.SenderConnection));
                                #endregion

                                NetOutgoingMessage outmsg = Server.CreateMessage();

                                // first we write byte
                                outmsg.Write((byte)PacketTypes.WORLDSTATE);

                                // then int
                                outmsg.Write(GameWorldState.Count);

                                foreach (Character ch in GameWorldState)
                                {
                                    outmsg.WriteAllProperties(ch);
                                }

                                // Now, packet contains:
                                // Byte = packet type
                                // Int = how many players there is in game
                                // character object * how many players is in game

                                Server.SendMessage(outmsg, inc.SenderConnection, NetDeliveryMethod.UnreliableSequenced, 0);

                                Console.WriteLine("Approved new connection and updated the world status");
                            }

                            break;

                        case NetIncomingMessageType.Data:
                            if (inc.ReadByte() == (byte)PacketTypes.MOVE)
                            {
                                // Check who sent the message
                                // This way we know, what character belongs to message sender
                                foreach (Character ch in GameWorldState)
                                {
                                    // If stored connection ( check approved message. We stored ip+port there, to character obj )
                                    // Find the correct character
                                    if (ch.Connection != inc.SenderConnection)
                                        continue;

                                    // Read next byte
                                    byte b = inc.ReadByte();

                                    // Handle movement. This byte should correspond to some direction
                                    if ((byte)MoveDirection.UP == b)
                                        ch.Y--;
                                    if ((byte)MoveDirection.DOWN == b)
                                        ch.Y++;
                                    if ((byte)MoveDirection.LEFT == b)
                                        ch.X--;
                                    if ((byte)MoveDirection.RIGHT == b)
                                        ch.X++;

                                    // Create new message
                                    NetOutgoingMessage outmsg = Server.CreateMessage();

                                    // Write byte, that is type of world state
                                    outmsg.Write((byte)PacketTypes.WORLDSTATE);

                                    // Write int, "how many players in game?"
                                    outmsg.Write(GameWorldState.Count);

                                    // Iterate throught all the players in game
                                    foreach (Character ch2 in GameWorldState)
                                    {
                                        // Write all the properties of object to message
                                        outmsg.WriteAllProperties(ch2);
                                    }

                                    // Message contains
                                    // Byte = PacketType
                                    // Int = Player count
                                    // Character obj * Player count

                                    // Send messsage to clients ( All connections, in reliable order, channel 0)
                                    Server.SendMessage(outmsg, Server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
                                    break;
                                }

                            }
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            // In case status changed
                            // It can be one of these
                            // NetConnectionStatus.Connected;
                            // NetConnectionStatus.Connecting;
                            // NetConnectionStatus.Disconnected;
                            // NetConnectionStatus.Disconnecting;
                            // NetConnectionStatus.None;

                            // NOTE: Disconnecting and Disconnected are not instant unless client is shutdown with disconnect()
                            Console.WriteLine(inc.SenderConnection.ToString() + " status changed. " + (NetConnectionStatus)inc.SenderConnection.Status);
                            if (inc.SenderConnection.Status == NetConnectionStatus.Disconnected || inc.SenderConnection.Status == NetConnectionStatus.Disconnecting)
                            {
                                // Find disconnected character and remove it
                                foreach (Character cha in GameWorldState)
                                {
                                    if (cha.Connection == inc.SenderConnection)
                                    {
                                        GameWorldState.Remove(cha);
                                        break;
                                    }
                                }
                            }
                            break;
                        default:
                            Console.WriteLine("Not Important Message");
                            break;
                    }
                }

                // if 30ms has passed
                if ((time + timetopass) < DateTime.Now)
                {
                    if (Server.ConnectionsCount != 0)
                    {
                        NetOutgoingMessage outmsg = Server.CreateMessage();

                        outmsg.Write((byte)PacketTypes.WORLDSTATE);
                        outmsg.Write(GameWorldState.Count);

                        // Iterate throught all the players in game
                        foreach (Character ch2 in GameWorldState)
                        {
                            // Write all properties of character, to the message
                            outmsg.WriteAllProperties(ch2);
                        }

                        // Message contains
                        // byte = Type
                        // Int = Player count
                        // Character obj * Player count

                        Server.SendMessage(outmsg, Server.Connections, NetDeliveryMethod.UnreliableSequenced, 0);
                    }

                    time = DateTime.Now;
                }

                System.Threading.Thread.Sleep(1);
            }
        }
    }

    class Character
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; set; }
        public NetConnection Connection { get; set; }
        public Character(string name, int x, int y, NetConnection conn)
        {
            Name = name;
            X = x;
            Y = y;
            Connection = conn;
        }
    }

    enum PacketTypes
    {
        LOGIN,
        MOVE,
        WORLDSTATE
    }

    enum MoveDirection
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        NONE
    }
}
