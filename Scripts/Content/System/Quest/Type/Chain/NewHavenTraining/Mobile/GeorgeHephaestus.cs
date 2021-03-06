using Server.Items;
using Server.Mobiles;

namespace Server.Engines.ChainQuests.Mobiles
{
	public class GeorgeHephaestus : Blacksmith
	{
		public override bool CanShout => true;
		public override void Shout(PlayerMobile pm)
		{
			ChainQuestSystem.Tell(this, pm, 1078122); // Wanna learn how to make powerful weapons and armor? Talk to me.
		}

		[Constructable]
		public GeorgeHephaestus()
		{
			Name = "George Hephaestus";
			Title = "the Blacksmith Instructor";
			BodyValue = 0x190;
			Hue = 0x83EA;
			HairItemID = 0x203B;
			HairHue = 0x47B;

			InitStats(100, 100, 25);

			SetSkill(SkillName.ArmsLore, 120.0);
			SetSkill(SkillName.Blacksmith, 120.0);
			SetSkill(SkillName.Magery, 120.0);
			SetSkill(SkillName.Tactics, 120.0);
			SetSkill(SkillName.Tinkering, 120.0);
			SetSkill(SkillName.Swords, 120.0);
			SetSkill(SkillName.Mining, 120.0);

		}

		public override void InitOutfit()
		{
			AddItem(new Backpack());
			AddItem(new Boots(0x973));
			AddItem(new LongPants());
			AddItem(new Bascinet());
			AddItem(new FullApron(0x8AB));

			Item item;

			item = new SmithHammer {
				Hue = 0x8AB
			};
			AddItem(item);
		}

		public GeorgeHephaestus(Serial serial)
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