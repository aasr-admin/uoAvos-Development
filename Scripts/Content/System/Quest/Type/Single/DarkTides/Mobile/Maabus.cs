using Server.Mobiles;

namespace Server.Engines.Quests.Mobiles
{
	public class Maabus : BaseQuester
	{
		public Maabus()
		{
		}

		public override void InitBody()
		{
			Body = 0x94;
			Name = "Maabus";
		}

		public Maabus(Serial serial) : base(serial)
		{
		}

		public override bool CanTalkTo(PlayerMobile to)
		{
			return false;
		}

		public override void OnTalk(PlayerMobile player, bool contextMenu)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			var version = reader.ReadInt();
		}
	}
}