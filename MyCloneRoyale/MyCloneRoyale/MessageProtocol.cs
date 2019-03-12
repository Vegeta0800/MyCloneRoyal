using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCloneRoyale.Messages;

namespace MyCloneRoyale
{
    class MessageProtocol
    {
        public event Action<byte[]> MessageComplete;

        private byte[] temp = new byte[1024*64];
        private int usedBytes = 0;
        private int neededBytes = 0;

        public void Receive(byte[] data)
        {
            foreach (byte b in data)
            {
                temp[this.usedBytes] = b;
                this.usedBytes++;

                if (this.usedBytes == 4)
                {
                    var reader = new BinaryReader(new MemoryStream(this.temp));
                    this.neededBytes = reader.ReadInt32();
                }

                if (this.usedBytes == this.neededBytes)
                {
                    MessageComplete(this.temp);
                    this.usedBytes = 0;
                    this.neededBytes = 0;
                }
            }
        }

        public static void Test()
        {
            int iMessagesComplete = 0;

            var p = new MessageProtocol();
            p.MessageComplete += bytes =>
            {
                var messageReceived = Message.Decode(bytes);
                System.Diagnostics.Debug.Assert(messageReceived is TestMessage);
                System.Diagnostics.Debug.Assert(((TestMessage) messageReceived).Text == "Hello World!");
                iMessagesComplete++;
            };

            var m = new TestMessage() {Text = "Hello World!"};
            byte[] b = m.Encode();
            p.Receive(b);
            System.Diagnostics.Debug.Assert(iMessagesComplete == 1);

            byte[] firstHalf = new byte[10];
            byte[] secondHalf = new byte[b.Length - firstHalf.Length];
            Array.Copy(b, firstHalf, firstHalf.Length);
            Array.Copy(b, firstHalf.Length, secondHalf, 0, secondHalf.Length);

            p.Receive(firstHalf);
            System.Diagnostics.Debug.Assert(iMessagesComplete == 1);
            p.Receive(secondHalf);
            System.Diagnostics.Debug.Assert(iMessagesComplete == 2);

            byte[] twoMessages = new byte[b.Length * 2];
            b.CopyTo(twoMessages, 0);
            b.CopyTo(twoMessages, b.Length);

            p.Receive(twoMessages);
            System.Diagnostics.Debug.Assert(iMessagesComplete == 4);
        }

        private static void OnMessageComplete(byte[] bytes)
        {
            // look at my message!
        }
    }



}
