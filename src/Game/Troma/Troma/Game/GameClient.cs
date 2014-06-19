using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Lidgren.Network;
using ClientServerExtension;
using System.ComponentModel;

namespace Troma
{
    public class GameClient
    {
        #region Constants

        private const string APP_NAME = "TROMA";
        private const int PORT = 11420;
        private const int MAX_CLIENT = 10;
        private const int DT = 30; // ms

        #endregion

        #region Fields

        private NetClient Client;
        private NetPeerConfiguration Config;
        private NetIncomingMessage IncMsg;
        private NetOutgoingMessage OutMsg;

        public int ID;
        public STATE State;
        public INPUT Input;
        public bool Alive;
        public int Score;
        public Map Terrain;
        public int MaxScore;

        public event EventHandler ScoreChanged;
        public event EventHandler EndedGame;

        public List<OtherPlayer> Players;
        public Tuple<int, string, int>[] Scoring;

        private System.Timers.Timer timerUpdate;
        private BackgroundWorker backgroundUpdater;

        public bool Connected
        {
            get 
            { 
                return (Client.ConnectionStatus == NetConnectionStatus.Connected); 
            }
        }

        #endregion

        public GameClient()
        {
            Initialize();
            SetupClient();
        }

        #region Initialization

        private void Initialize()
        {
            Players = new List<OtherPlayer>();
            Players.Capacity = MAX_CLIENT;

            Score = 0;

            timerUpdate = new System.Timers.Timer(35); // 35ms
            timerUpdate.Elapsed += new System.Timers.ElapsedEventHandler(UpdateElapsed);

            backgroundUpdater = new BackgroundWorker();
            backgroundUpdater.DoWork += HandleMsg;
        }

        private void SetupClient()
        {
            Config = new NetPeerConfiguration(APP_NAME);
            Config.EnableUPnP = true;
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
            Client.UPnP.ForwardPort(Client.Port, "Troma client");

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
            DateTime expired = DateTime.Now + new TimeSpan(0, 0, 0, 30, 0);
            bool canStart = false;

#if DEBUG
            Console.WriteLine("Waiting initial data...");
#endif

            while (!canStart && expired > DateTime.Now)
            {
                if ((IncMsg = Client.ReadMessage()) != null)
                {
                    switch (IncMsg.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            #region Connection Approved

                            switch (IncMsg.ReadPacketType())
                            {
                                case PacketTypes.LOGIN:
                                    ID = IncMsg.ReadInt32();
                                    Terrain = (Map)IncMsg.ReadByte();
                                    MaxScore = IncMsg.ReadInt32();
                                    canStart = true;

#if DEBUG
                                    Console.WriteLine("Confirm initial data!");
#endif

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

        public void Join(string host)
        {
            Connect(host);
            WaitInitialData();
        }

        public void Shutdown()
        {
            Client.Shutdown("End");
        }

        public void Start()
        {
            timerUpdate.Start();
            backgroundUpdater.RunWorkerAsync();
        }

        private void HandleMsg(object sender, DoWorkEventArgs e)
        {
            OtherPlayer p;
            int id;
            string name;

            while (true)
            {
                if ((IncMsg = Client.ReadMessage()) != null)
                {
                    switch (IncMsg.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            #region Process Data

                            switch (IncMsg.ReadPacketType())
                            {
                                case PacketTypes.QUIT:
                                    p = FindPlayer(IncMsg.ReadInt32(), Players);

                                    if (p == null)
                                        break;

                                    Players.Remove(p);
                                    break;

                                case PacketTypes.KILL:
                                    Alive = false;
                                    break;

                                case PacketTypes.SPAWN:
                                    Alive = true;
                                    State = IncMsg.ReadPlayerState();
                                    break;

                                case PacketTypes.SHOOT:
                                    p = FindPlayer(IncMsg.ReadInt32(), Players);

                                    if (p == null)
                                        break;

                                    p.Shoot();
                                    break;

                                case PacketTypes.SCORE:
                                    Score = IncMsg.ReadInt32();
                                    ScoreChanged(null, null);
                                    break;

                                case PacketTypes.END:
                                    int size = IncMsg.ReadInt32();
                                    Scoring = new Tuple<int, string, int>[size];

                                    for (int i = 0; i < size; i++)
                                    {
                                        Scoring[i] = new Tuple<int, string, int>(
                                            IncMsg.ReadInt32(),
                                            IncMsg.ReadString(),
                                            IncMsg.ReadInt32());
                                    }

                                    EndedGame(null, null);
                                    break;

                                case PacketTypes.STATE:
                                    id = IncMsg.ReadInt32();
                                    name = IncMsg.ReadString();

                                    p = FindPlayer(id, Players);

                                    if (p == null)
                                    {
                                        p = new OtherPlayer(name, id);
                                        Players.Add(p);
                                    }

                                    p.State = IncMsg.ReadPlayerState();
                                    break;

                                case PacketTypes.INPUT:
                                    p = FindPlayer(IncMsg.ReadInt32(), Players);

                                    if (p == null)
                                        break;

                                    p.Input = IncMsg.ReadPlayerInput();
                                    break;

                                default:
                                    break;
                            }

                            break;

                            #endregion

                        default:
                            break;
                    }

                    Client.Recycle(IncMsg); // generate less garbage
                }

                System.Threading.Thread.Sleep(1);
            }
        }

        private void UpdateElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            SendState(State);
            SendInput(Input);
        }

        private void SendState(STATE s)
        {
            OutMsg = Client.CreateMessage();
            OutMsg.Write((byte)PacketTypes.STATE);
            OutMsg.WritePlayerState(s);

            Client.SendMessage(OutMsg, NetDeliveryMethod.UnreliableSequenced, 2);
        }

        private void SendInput(INPUT i)
        {
            OutMsg = Client.CreateMessage();
            OutMsg.Write((byte)PacketTypes.INPUT);
            OutMsg.WritePlayerInput(i);

            Client.SendMessage(OutMsg, NetDeliveryMethod.UnreliableSequenced, 2);
        }

        public void SendShoot()
        {
            OutMsg = Client.CreateMessage();
            OutMsg.Write((byte)PacketTypes.SHOOT);

            Client.SendMessage(OutMsg, NetDeliveryMethod.Unreliable, 0);
        }

        public void SetData(STATE s, INPUT i)
        {
            State = s;
            Input = i;
        }

        #region Help Methods

        public OtherPlayer FindPlayer(int id, List<OtherPlayer> list)
        {
            foreach (OtherPlayer player in list)
            {
                if (player.ID == id)
                    return player;
            }

            return null;
        }

        #endregion
    }
}
