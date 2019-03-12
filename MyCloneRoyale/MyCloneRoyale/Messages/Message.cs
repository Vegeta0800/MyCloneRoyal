using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCloneRoyale.Messages
{
    public abstract class Message
    {
        public enum MessageTypes
        {
            TestMessage,
            ChatMessage,
            GameInputMessage,
            ReadyMessage,
            DamageMessage,
            TimeoutMessage,
            EndMessage,
            HeartBeat,
            ResetMessage,
            PauseMessage,
            SelectTypeMessage,
        }

        public int Size;
        public abstract MessageTypes Type { get; }

        public byte[] Encode()
        {
            MemoryStream memStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memStream);

            // header
            writer.Write(this.Size);        // always 0...
            writer.Write((int) this.Type);

            // body
            Write(writer);

            this.Size = (int) memStream.Length;
            memStream.Seek(0, SeekOrigin.Begin);
            writer.Write(this.Size);                // corrected size!

            // done!
            return memStream.ToArray();
        }

        public static Message Decode(byte[] b)
        {
            var memStream = new MemoryStream(b);
            var reader = new BinaryReader(memStream);

            int size = reader.ReadInt32();
            var type = (MessageTypes) reader.ReadInt32();

            Message m;
            switch (type)
            {
                case MessageTypes.TestMessage:
                    m = new TestMessage();
                    break;
                case MessageTypes.ChatMessage:
                    m = new ChatMessage();
                    break;
                case MessageTypes.GameInputMessage:
                    m = new GameInputMessage();
                    break;
                case MessageTypes.ReadyMessage:
                    m = new ReadyMessage();
                    break;
                case MessageTypes.DamageMessage:
                    m = new DamageMessage();
                    break;
                case MessageTypes.TimeoutMessage:
                    m = new TimeoutMessage();
                    break;
                case MessageTypes.EndMessage:
                    m = new EndMessage();
                    break;
                case MessageTypes.HeartBeat:
                    m = new HeartBeat();
                    break;
                case MessageTypes.ResetMessage:
                    m = new ResetMessage();
                    break;
                case MessageTypes.PauseMessage:
                    m = new PauseMessage();
                    break;
                case MessageTypes.SelectTypeMessage:
                    m = new SelectTypeMessage();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            m.Size = size;
            m.Read(reader);

            return m;
        }

        protected abstract void Write(BinaryWriter writer);
        protected abstract void Read(BinaryReader reader);

    }
}
