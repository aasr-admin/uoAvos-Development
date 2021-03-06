using Server.Items;

namespace Server.Engines.ChainQuests.Items
{
	public class GervisSatchel : Backpack
	{
		[Constructable]
		public GervisSatchel()
		{
			Hue = Utility.RandomBrightHue();
			DropItem(new IronIngot(10));
			DropItem(new SmithHammer());
		}

		public GervisSatchel(Serial serial)
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