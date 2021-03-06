using Server.Items;
using Server.Mobiles;

namespace Server.Engines.ChainQuests.Mobiles
{
	[QuesterName("Jusae (The Heartwood)")]
	public class Jusae : BaseCreature
	{
		public override bool IsInvulnerable => true;
		public override bool CanTeach => true;

		public override bool CanShout => true;
		public override void Shout(PlayerMobile pm)
		{
			ChainQuestSystem.Tell(this, pm, Utility.RandomList(
				1074210, // Hi.  Looking for something to do?
				1074213 // Hey buddy.  Looking for work?
			));
		}

		[Constructable]
		public Jusae()
			: base(AIType.AI_Vendor, FightMode.None, 2, 1, 0.5, 2)
		{
			Name = "Jusae";
			Title = "the bowcrafter";
			Race = Race.Elf;
			BodyValue = 0x25D;
			Female = false;
			Hue = Race.RandomSkinHue();
			InitStats(100, 100, 25);

			Utility.AssignRandomHair(this, true);

			SetSkill(SkillName.Meditation, 60.0, 80.0);
			SetSkill(SkillName.Focus, 60.0, 80.0);

			AddItem(new Sandals(0x901));
			AddItem(new ShortPants(0x661));
			AddItem(new MagicalShortbow());

			Item item;

			item = new HideChest {
				Hue = 0x27B
			};
			AddItem(item);

			item = new HidePauldrons {
				Hue = 0x27E
			};
			AddItem(item);
		}

		public Jusae(Serial serial)
			: base(serial)
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