using Server.Items;
using Server.Network;

using System;

namespace Server.Engines.Quests.Items
{
	public class EnchantedSextant : Item
	{
		//TODO: Trammel/Haven
		private static readonly Point2D[] m_TrammelBanks = new Point2D[]
			{
				new Point2D( 652, 820 ),
				new Point2D( 1813, 2825 ),
				new Point2D( 3734, 2149 ),
				new Point2D( 2503, 552 ),
				new Point2D( 3764, 1317 ),
				new Point2D( 587, 2146 ),
				new Point2D( 1655, 1606 ),
				new Point2D( 1425, 1690 ),
				new Point2D( 4471, 1156 ),
				new Point2D( 1317, 3773 ),
				new Point2D( 2881, 684 ),
				new Point2D( 2731, 2192 ),
				new Point2D( 3620, 2617 ),
				new Point2D( 2880, 3472 ),
				new Point2D( 1897, 2684 ),
				new Point2D( 5346, 74 ),
				new Point2D( 5275, 3977 ),
				new Point2D( 5669, 3131 )
			};

		private static readonly Point2D[] m_FeluccaBanks = new Point2D[]
			{
				new Point2D( 652, 820 ),
				new Point2D( 1813, 2825 ),
				new Point2D( 3734, 2149 ),
				new Point2D( 2503, 552 ),
				new Point2D( 3764, 1317 ),
				new Point2D( 3695, 2511 ),
				new Point2D( 587, 2146 ),
				new Point2D( 1655, 1606 ),
				new Point2D( 1425, 1690 ),
				new Point2D( 4471, 1156 ),
				new Point2D( 1317, 3773 ),
				new Point2D( 2881, 684 ),
				new Point2D( 2731, 2192 ),
				new Point2D( 2880, 3472 ),
				new Point2D( 1897, 2684 ),
				new Point2D( 5346, 74 ),
				new Point2D( 5275, 3977 ),
				new Point2D( 5669, 3131 )
			};

		private static readonly Point2D[] m_IlshenarBanks = new Point2D[]
			{
				new Point2D( 854, 680 ),
				new Point2D( 855, 603 ),
				new Point2D( 1226, 554 ),
				new Point2D( 1610, 556 )
			};

		private static readonly Point2D[] m_MalasBanks = new Point2D[]
			{
				new Point2D( 996, 519 ),
				new Point2D( 2048, 1345 )
			};

		private const double m_LongDistance = 300.0;
		private const double m_ShortDistance = 5.0;

		public override int LabelNumber => 1046226;  // an enchanted sextant

		[Constructable]
		public EnchantedSextant() : base(0x1058)
		{
			Weight = 2.0;
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (!from.InRange(GetWorldLocation(), 2))
			{
				from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
				return;
			}

			Point2D[] banks;
			PMList moongates;
			if (from.Map == Map.Trammel)
			{
				banks = m_TrammelBanks;
				moongates = PMList.Trammel;
			}
			else if (from.Map == Map.Felucca)
			{
				banks = m_FeluccaBanks;
				moongates = PMList.Felucca;
			}
			else if (from.Map == Map.Ilshenar)
			{
#if false
				banks = m_IlshenarBanks;
				moongates = PMList.Ilshenar;
#else
				from.Send(new MessageLocalized(Serial, ItemID, MessageType.Label, 0x482, 3, 1061684, "", "")); // The magic of the sextant fails...
				return;
#endif
			}
			else if (from.Map == Map.Malas)
			{
				banks = m_MalasBanks;
				moongates = PMList.Malas;
			}
			else
			{
				banks = null;
				moongates = null;
			}

			var closestMoongate = Point3D.Zero;
			var moongateDistance = Double.MaxValue;
			if (moongates != null)
			{
				foreach (var entry in moongates.Entries)
				{
					var dist = from.GetDistanceToSqrt(entry.Location);
					if (moongateDistance > dist)
					{
						closestMoongate = entry.Location;
						moongateDistance = dist;
					}
				}
			}

			var closestBank = Point2D.Zero;
			var bankDistance = Double.MaxValue;
			if (banks != null)
			{
				foreach (var p in banks)
				{
					var dist = from.GetDistanceToSqrt(p);
					if (bankDistance > dist)
					{
						closestBank = p;
						bankDistance = dist;
					}
				}
			}

			int moonMsg;
			if (moongateDistance == Double.MaxValue)
			{
				moonMsg = 1048021; // The sextant fails to find a Moongate nearby.
			}
			else if (moongateDistance > m_LongDistance)
			{
				moonMsg = 1046449 + (int)from.GetDirectionTo(closestMoongate); // A moongate is * from here
			}
			else if (moongateDistance > m_ShortDistance)
			{
				moonMsg = 1048010 + (int)from.GetDirectionTo(closestMoongate); // There is a Moongate * of here.
			}
			else
			{
				moonMsg = 1048018; // You are next to a Moongate at the moment.
			}

			from.Send(new MessageLocalized(Serial, ItemID, MessageType.Label, 0x482, 3, moonMsg, "", ""));

			int bankMsg;
			if (bankDistance == Double.MaxValue)
			{
				bankMsg = 1048020; // The sextant fails to find a Bank nearby.
			}
			else if (bankDistance > m_LongDistance)
			{
				bankMsg = 1046462 + (int)from.GetDirectionTo(closestBank); // A town is * from here
			}
			else if (bankDistance > m_ShortDistance)
			{
				bankMsg = 1048002 + (int)from.GetDirectionTo(closestBank); // There is a city Bank * of here.
			}
			else
			{
				bankMsg = 1048019; // You are next to a Bank at the moment.
			}

			from.Send(new MessageLocalized(Serial, ItemID, MessageType.Label, 0x5AA, 3, bankMsg, "", ""));
		}

		public EnchantedSextant(Serial serial) : base(serial)
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