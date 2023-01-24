using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cotf.Network
{
    public class NetMessage
    {
        internal static NetworkStream Local;
        internal static NetworkStream[] stream = new NetworkStream[256];
        private static BinaryWriter writer;
        private static BinaryReader reader;
        protected BinaryReader Reader(int whoAmI) => new BinaryReader(stream[whoAmI]);
        protected BinaryWriter Writer(int whoAmI) => new BinaryWriter(stream[whoAmI]);
        internal static void Initialize(IPAddress ip, int port = 8000, byte whoAmI = 0)
        {
            if (Main.netMode == NetModeID.Singleplayer)
                return;
            if (Main.netMode == NetModeID.MultiplayerClient)
            {
                var end = new IPEndPoint(ip, port);
                //  Primary network handling
                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                do
                {
                    try
                    {
                        Thread.Sleep(1000);
                        sock.Connect(end);
                    }
                    catch { continue; }
                } while (!sock.Connected);
                Local = new NetworkStream(sock);
                writer = new BinaryWriter(Local);
                reader = new BinaryReader(Local);
            }
            if (Main.netMode == NetModeID.Server)
            {
                //  Begin listen
                Socket listen = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                listen.Bind(new IPEndPoint(IPAddress.Any, port));
                listen.Listen(256);
                Socket sock = listen.Accept();
                //  Get whoAmI
                byte[] buffer = new byte[1];
                sock.Receive(buffer);
                byte whoAmi = buffer[0];
                //  Init stream
                stream[whoAmi] = new NetworkStream(sock);
            }
        }
        public static void SendData(byte packet, int toWhom = -1, int fromWhom = -1, NetInfo info = null, int num = 0, int num2 = 0, int num3 = 0)
        {
            if (Main.netMode == NetModeID.Singleplayer)
                return;
            if (Main.netMode == NetModeID.MultiplayerClient)
            {
                switch (packet)
                {

                }
            }
            if (Main.netMode == NetModeID.Server)
            {

            }
        }
    }
    public class Packet
    {
        public Packet()
        {
        }
        private BinaryWriter write;
        public void Send(byte packet, int toWhom = 255, NetInfo info = null)
        {
            if (NetMessage.stream[toWhom] == null)
                return;
            write = new BinaryWriter(NetMessage.stream[toWhom]);
            write.Write(packet);
        }
    }

    public class NetInfo
    {
    }
    public sealed class NetMessageID
    {
        public const byte
            SyncPlayer = 0;
    }
    public sealed class NetModeID
    {
        public const int
            Singleplayer = 0,
            MultiplayerClient = 1,
            Server = 2;
    }
}
