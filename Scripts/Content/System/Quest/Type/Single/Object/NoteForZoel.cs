using Server.Engines.Quests.Definitions;
using Server.Mobiles;

namespace Server.Engines.Quests.Items
{
	public class NoteForZoel : QuestItem
	{
		public override int LabelNumber => 1063186;  // A Note for Zoel

		[Constructable]
		public NoteForZoel() : base(0x14EF)
		{
			Weight = 1.0;
			Hue = 0x6B9;
		}

		public NoteForZoel(Serial serial) : base(serial)
		{
		}

		public override bool CanDrop(PlayerMobile player)
		{
			var qs = player.Quest as EminosUndertakingQuest;

			if (qs == null)
			{
				return true;
			}

			//return !qs.IsObjectiveInProgress( typeof( GiveZoelNoteObjective_EminosUndertakingQuest ) );
			return false;
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