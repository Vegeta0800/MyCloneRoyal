using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCloneRoyale.Messages
{
    public class TestMessage : Message
    {
        public string Text;

        public override MessageTypes Type { get { return MessageTypes.TestMessage; } }

        protected override void Write(BinaryWriter writer)
        {
            writer.Write(this.Text);
        }

        protected override void Read(BinaryReader reader)
        {
            this.Text = reader.ReadString();
        }
    }
}
