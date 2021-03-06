using Server.Mobiles;

namespace Server.Engines.ChainQuests.Mobiles
{
	public class Bravehorn : BaseEscortable
	{
		public override bool StaticChainQuester => true;
		public override bool InitialInnocent => true;

		[Constructable]
		public Bravehorn()
		{
		}

		public override void InitBody()
		{
			Name = "Bravehorn";
			Body = 0xEA;

			SetStr(41, 71);
			SetDex(47, 77);
			SetInt(27, 57);

			SetHits(27, 41);
			SetMana(0);

			SetDamage(5, 9);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 20, 25);
			SetResistance(ResistanceType.Cold, 5, 10);

			SetSkill(SkillName.MagicResist, 26.8, 44.5);
			SetSkill(SkillName.Tactics, 29.8, 47.5);
			SetSkill(SkillName.Wrestling, 29.8, 47.5);

			Fame = 300;
			Karma = 0;

			VirtualArmor = 24;
		}

		public override void InitOutfit()
		{
		}

		public override int GetAttackSound()
		{
			return 0x82;
		}

		public override int GetHurtSound()
		{
			return 0x83;
		}

		public override int GetDeathSound()
		{
			return 0x84;
		}

		public Bravehorn(Serial serial)
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