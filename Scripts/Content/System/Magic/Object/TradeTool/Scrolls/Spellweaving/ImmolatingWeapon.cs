namespace Server.Items
{
	public class ImmolatingWeaponScroll : SpellScroll
	{
		[Constructable]
		public ImmolatingWeaponScroll()
			: this(1)
		{
		}

		[Constructable]
		public ImmolatingWeaponScroll(int amount)
			: base(602, 0x2D53, amount)
		{
			Hue = 0x8FD;
		}

		public ImmolatingWeaponScroll(Serial serial)
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