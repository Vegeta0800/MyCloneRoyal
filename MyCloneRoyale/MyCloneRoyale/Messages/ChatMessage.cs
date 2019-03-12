using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCloneRoyale.Messages
{
    public class ChatMessage : Message
    {
        public string Name;
        public string Text;

        public override MessageTypes Type { get { return MessageTypes.ChatMessage; } }

        protected override void Write(BinaryWriter writer)
        {
            writer.Write(this.Name);
            writer.Write(this.Text);
        }

        protected override void Read(BinaryReader reader)
        {
            this.Name = reader.ReadString();
            this.Text = reader.ReadString();
        }
    }
}
