using Server.Items;

namespace Server.Engines.ChainQuests.Items
{
	public class BagOfSmokeBombs : Bag
	{
		[Constructable]
		public BagOfSmokeBombs() : this(20)
		{
		}

		[Constructable]
		public BagOfSmokeBombs(int amount)
		{
			for (var i = 0; i < amount; ++i)
			{
				DropItem(new SmokeBomb());
			}
		}

		public BagOfSmokeBombs(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.WriteEncodedInt(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			var version = reader.ReadEncodedInt();
		}
	}
}