using Server.Engines.Quests.Definitions;
using Server.Items;
using Server.Mobiles;

using System;

namespace Server.Engines.Quests.Items
{
	public class HonorCandle : CandleLong
	{
		private static readonly TimeSpan LitDuration = TimeSpan.FromSeconds(20.0);

		public override int LitSound => 0;
		public override int UnlitSound => 0;

		[Constructable]
		public HonorCandle()
		{
			Movable = false;
			Duration = LitDuration;
		}

		public HonorCandle(Serial serial) : base(serial)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			var wasBurning = Burning;

			base.OnDoubleClick(from);

			if (!wasBurning && Burning)
			{
				var player = from as PlayerMobile;

				if (player == null)
				{
					return;
				}

				var qs = player.Quest;

				if (qs != null && qs is HaochisTrialsQuest)
				{
					var obj = qs.FindObjective(typeof(SixthTrialIntroObjective_HaochisTrialsQuest));

					if (obj != null && !obj.Completed)
					{
						obj.Complete();
					}

					SendLocalizedMessageTo(from, 1063251); // You light a candle in honor.
				}
			}
		}

		public override void Burn()
		{
			Douse();
		}

		public override void Douse()
		{
			base.Douse();

			Duration = LitDuration;
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