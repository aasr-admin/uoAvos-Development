namespace Server.Engines.ChainQuests.Items
{
	public class DreadSpiderSilk : Item
	{
		public override int LabelNumber => 1075319;  // Dread Spider Silk

		public override bool Nontransferable => true;

		public override void AddNameProperties(ObjectPropertyList list)
		{
			base.AddNameProperties(list);
			AddQuestItemProperty(list);
		}

		[Constructable]
		public DreadSpiderSilk() : base(0xDF8)
		{
			LootType = LootType.Blessed;
			Hue = 0x481;
		}

		public DreadSpiderSilk(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // Version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			var version = reader.ReadInt();
		}
	}
}