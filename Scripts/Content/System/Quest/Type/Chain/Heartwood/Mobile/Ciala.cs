using Server.Items;
using Server.Mobiles;

namespace Server.Engines.ChainQuests.Mobiles
{
	[QuesterName("Ciala (The Heartwood)")]
	public class Ciala : BaseCreature
	{
		public override bool IsInvulnerable => true;
		public override bool CanTeach => true;

		public override bool CanShout => true;
		public override void Shout(PlayerMobile pm)
		{
			ChainQuestSystem.Tell(this, pm, Utility.RandomList(
				1074206, // Excuse me please traveler, might I have a little of your time?
				1074186 // Come here, I have a task.
			));
		}

		[Constructable]
		public Ciala()
			: base(AIType.AI_Vendor, FightMode.None, 2, 1, 0.5, 2)
		{
			Name = "Ciala";
			Title = "the arborist";
			Race = Race.Elf;
			BodyValue = 0x25E;
			Female = true;
			Hue = Race.RandomSkinHue();
			InitStats(100, 100, 25);

			Utility.AssignRandomHair(this, true);

			SetSkill(SkillName.Meditation, 60.0, 80.0);
			SetSkill(SkillName.Focus, 60.0, 80.0);

			AddItem(new Skirt(Utility.RandomBlueHue()));
			AddItem(new ElvenShirt(Utility.RandomYellowHue()));
			AddItem(new RoyalCirclet());

			if (Utility.RandomBool())
			{
				AddItem(new Boots(Utility.RandomYellowHue()));
			}
			else
			{
				AddItem(new ThighBoots(Utility.RandomYellowHue()));
			}
		}

		public Ciala(Serial serial)
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