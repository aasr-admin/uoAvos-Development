using Server.Items;

using System;

namespace Server.Engines.Quests.Items
{
	public class HagStew : BaseAddon
	{

		[Constructable]
		public HagStew()
		{
			AddonComponent stew;
			stew = new AddonComponent(2416) {
				Name = "stew",
				Visible = true
			};
			AddComponent(stew, 0, 0, -7);      //stew
		}

		public override void OnComponentUsed(AddonComponent stew, Mobile from)
		{
			if (!from.InRange(GetWorldLocation(), 2))
			{
				from.SendMessage("You are too far away.");
			}
			else
			{
				{
					stew.Visible = false;

					var hagstew = new BreadLoaf();        //this decides your fillrate
					hagstew.Eat(from);

					Timer m_timer = new ShowStew(stew);
					m_timer.Start();
				}
			}
		}

		public class ShowStew : Timer
		{
			private readonly AddonComponent stew;

			public ShowStew(AddonComponent ac) : base(TimeSpan.FromSeconds(30))
			{
				stew = ac;
				Priority = TimerPriority.OneSecond;
			}

			protected override void OnTick()
			{
				if (stew.Visible == false)
				{
					Stop();
					stew.Visible = true;
					return;
				}
			}
		}

		public HagStew(Serial serial) : base(serial)
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