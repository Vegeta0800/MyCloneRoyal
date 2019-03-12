using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MyCloneRoyale.Messages;

namespace MyCloneRoyale
{
    public class OpenConnection : IDisposable
    {
        public event Action<Message> MessageComplete;

        private TcpClient client;
        private NetworkStream stream;
        public bool recievedHeartbeat = true;

        private Thread thread;
        private volatile bool threadStopped = false;

        private MessageProtocol protocol;


        public OpenConnection(TcpClient client)
        {
            this.client = client;
            this.stream = this.client.GetStream();

            this.protocol = new MessageProtocol();
            this.protocol.MessageComplete += OnProtocolMessageComplete;

            this.thread = new Thread(this.ThreadProc);
            this.thread.Start();
        }

        public void Dispose()
        {
            this.threadStopped = true;
            this.thread.Join();
            this.client.Dispose();
        }

        private void OnProtocolMessageComplete(byte[] b)
        {
            if (this.MessageComplete != null)
            {
                var message = Message.Decode(b);

                if(message is HeartBeat)
                {
                    HeartBeat e = (HeartBeat)message;
                    if (e.heartbeat == 1)
                        this.recievedHeartbeat = true;
                    else
                        this.recievedHeartbeat = false;
                }
                else
                    this.MessageComplete(message);
            }
        }

        public void Send(Message m)
        {
            var bytes = m.Encode();
            try
            {
                this.stream.Write(bytes, 0, bytes.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void ThreadProc()
        {
            byte[] receiveBuffer = new byte[1024];

            while (!threadStopped)
            {
                while (this.stream.DataAvailable)
                {
                    int bytesRead = this.stream.Read(receiveBuffer, 0, receiveBuffer.Length);
                    var b = new byte[bytesRead];
                    Array.Copy(receiveBuffer, b, bytesRead);
                    protocol.Receive(b);

                    Send(new HeartBeat()
                    {
                        heartbeat = 1
                    });
                }


                Thread.Sleep(1);
            }

            Send(new HeartBeat()
            {
                heartbeat = 0
            });
        }

    }
}
