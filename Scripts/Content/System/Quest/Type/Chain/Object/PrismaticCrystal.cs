using Server.Engines.ChainQuests.Definitions;
using Server.Mobiles;
using Server.Network;

namespace Server.Engines.ChainQuests.Items
{
	public class PrismaticCrystal : Item
	{
		public override int LabelNumber => 1074269;  // prismatic crystal

		[Constructable]
		public PrismaticCrystal() : base(0x2DA)
		{
			Movable = false;
			Hue = 0x32;
		}

		public PrismaticCrystal(Serial serial) : base(serial)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			var pm = from as PlayerMobile;

			if (pm == null || pm.Backpack == null)
			{
				return;
			}

			if (pm.InRange(GetWorldLocation(), 2))
			{
				var context = ChainQuestSystem.GetContext(pm);

				if (context != null && context.IsDoingQuest(typeof(UnfadingMemoriesPartOne)) && pm.Backpack.FindItemByType(typeof(PrismaticAmber), false) == null)
				{
					Item amber = new PrismaticAmber();

					if (pm.PlaceInBackpack(amber))
					{
						ChainQuestSystem.MarkQuestItem(pm, amber);
						Delete();
					}
					else
					{
						pm.SendLocalizedMessage(502385); // Your pack cannot hold this item.
						amber.Delete();
					}
				}
				else
				{
					pm.SendLocalizedMessage(1075464); // You already have as many of those as you need.
				}
			}
			else
			{
				pm.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
			}
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