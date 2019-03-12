using System.IO;

namespace MyCloneRoyale.Messages
{
    public class GameInputMessage : Message
    {
        public int GameRound;
        public int UnitTypePlaced;
        public int X;
        public int Y;
        public int playerNumber;
        public int entityType;
        public int energy;
        public int health;
        public bool gameRunning;


        public override MessageTypes Type { get { return MessageTypes.GameInputMessage; } }

        protected override void Write(BinaryWriter writer)
        {
            writer.Write(this.GameRound);
            writer.Write(this.UnitTypePlaced);
            writer.Write(this.X);
            writer.Write(this.Y);
            writer.Write(this.playerNumber);
            writer.Write(this.entityType);
            writer.Write(this.energy);
            writer.Write(this.health);
            writer.Write(this.gameRunning);
        }

        protected override void Read(BinaryReader reader)
        {
            this.GameRound = reader.ReadInt32();
            this.UnitTypePlaced = reader.ReadInt32();
            this.X = reader.ReadInt32();
            this.Y = reader.ReadInt32();
            this.playerNumber = reader.ReadInt32();
            this.entityType = reader.ReadInt32();
            this.energy = reader.ReadInt32();
            this.health = reader.ReadInt32();
            this.gameRunning = reader.ReadBoolean();
        }
    }

    public class ReadyMessage : Message
    {
        public bool ready;
        public bool gameRunning;

        public override MessageTypes Type { get { return MessageTypes.ReadyMessage; } }

        protected override void Write(BinaryWriter writer)
        {
            writer.Write(this.ready);
            writer.Write(this.gameRunning);
        }

        protected override void Read(BinaryReader reader)
        {
            this.ready = reader.ReadBoolean();
            this.gameRunning = reader.ReadBoolean();
        }
    }

    public class DamageMessage : Message
    {
        public int health;
        public int targetPlayer;

        public override MessageTypes Type { get { return MessageTypes.DamageMessage; } }

        protected override void Write(BinaryWriter writer)
        {
            writer.Write(this.health);
            writer.Write(this.targetPlayer);
        }

        protected override void Read(BinaryReader reader)
        {
            this.health = reader.ReadInt32();
            this.targetPlayer = reader.ReadInt32();
        }
    }

    public class TimeoutMessage : Message
    {
        public bool gameRunning;

        public override MessageTypes Type { get { return MessageTypes.TimeoutMessage; } }

        protected override void Write(BinaryWriter writer)
        {
            writer.Write(this.gameRunning);
        }

        protected override void Read(BinaryReader reader)
        {
            this.gameRunning = reader.ReadBoolean();
        }
    }

    public class EndMessage : Message
    {
        public bool victory;

        public override MessageTypes Type { get { return MessageTypes.EndMessage; } }

        protected override void Write(BinaryWriter writer)
        {
            writer.Write(this.victory);
        }

        protected override void Read(BinaryReader reader)
        {
            this.victory = reader.ReadBoolean();
        }
    }

    public class HeartBeat : Message
    {
        public int heartbeat;

        public override MessageTypes Type { get { return MessageTypes.HeartBeat; } }

        protected override void Write(BinaryWriter writer)
        {
            writer.Write(this.heartbeat);
        }

        protected override void Read(BinaryReader reader)
        {
            this.heartbeat = reader.ReadInt32();
        }
    }

    public class ResetMessage : Message
    {
        public bool reset;

        public override MessageTypes Type { get { return MessageTypes.ResetMessage; } }

        protected override void Write(BinaryWriter writer)
        {
            writer.Write(this.reset);
        }

        protected override void Read(BinaryReader reader)
        {
            this.reset = reader.ReadBoolean();
        }
    }

    public class PauseMessage : Message
    {
        public bool pause;

        public override MessageTypes Type { get { return MessageTypes.PauseMessage; } }

        protected override void Write(BinaryWriter writer)
        {
            writer.Write(this.pause);
        }

        protected override void Read(BinaryReader reader)
        {
            this.pause = reader.ReadBoolean();
        }
    }

    public class SelectTypeMessage : Message
    {
        public int selectedType;

        public override MessageTypes Type { get { return MessageTypes.SelectTypeMessage; } }

        protected override void Write(BinaryWriter writer)
        {
            writer.Write(this.selectedType);
        }

        protected override void Read(BinaryReader reader)
        {
            this.selectedType = reader.ReadInt32();
        }
    }
}
