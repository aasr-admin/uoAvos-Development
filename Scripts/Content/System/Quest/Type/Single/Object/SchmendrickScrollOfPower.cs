using Server.Engines.Quests.Definitions;
using Server.Mobiles;

namespace Server.Engines.Quests.Items
{
	public class SchmendrickScrollOfPower : QuestItem
	{
		public override int LabelNumber => 1049118;  // a scroll with ancient markings

		public SchmendrickScrollOfPower() : base(0xE34)
		{
			Weight = 1.0;
			Hue = 0x34D;
		}

		public SchmendrickScrollOfPower(Serial serial) : base(serial)
		{
		}

		public override bool CanDrop(PlayerMobile player)
		{
			var qs = player.Quest as UzeraanTurmoilQuest;

			if (qs == null)
			{
				return true;
			}

			return !qs.IsObjectiveInProgress(typeof(ReturnScrollOfPowerObjective_UzeraanTurmoilQuest));
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