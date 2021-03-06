using Server.Items;

namespace Server.Engines.ChainQuests.Items
{
	public class SadrahSatchel : Backpack
	{
		[Constructable]
		public SadrahSatchel()
		{
			Hue = Utility.RandomBrightHue();
			DropItem(new Bloodmoss(10));
			DropItem(new MortarPestle());
		}

		public SadrahSatchel(Serial serial)
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