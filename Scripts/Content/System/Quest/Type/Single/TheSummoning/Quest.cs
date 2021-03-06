using Server.Engines.Quests.Mobiles;
using Server.Items;
using Server.Mobiles;

using System;

namespace Server.Engines.Quests.Definitions
{
	public class TheSummoningQuest : QuestSystem
	{
		private static readonly Type[] m_TypeReferenceTable = new Type[]
			{
				typeof( AcceptConversation_TheSummoningQuest ),
				typeof( CollectBonesObjective_TheSummoningQuest ),
				typeof( VanquishDaemonConversation_TheSummoningQuest ),
				typeof( VanquishDaemonObjective_TheSummoningQuest )
			};

		public override Type[] TypeReferenceTable => m_TypeReferenceTable;

		private Victoria m_Victoria;
		private bool m_WaitForSummon;

		public Victoria Victoria => m_Victoria;

		public bool WaitForSummon
		{
			get => m_WaitForSummon;
			set => m_WaitForSummon = value;
		}

		public override object Name =>
				// The Summoning
				1050025;

		public override object OfferMessage =>
				/* <I>Victoria turns to you and smiles...</I><BR><BR>
* 
* Chyloth, eh?  He is the ferry man of lake <I>Mortis</I>, beyond which lies
* the nest of the <I>The Dark Father</I> - the fountainhead of all the evil
* that you see around you here.<BR><BR>
* 
* 800 and some years ago, my sisters and I persuaded the ferry man Chyloth
* to take us across the lake to battle the <I>The Dark Father</I>.
* My party was utterly destroyed, except for me.  For my insolence, I was
* cursed by the <I>The Dark Father</I> to wander these halls for eternity,
* unable to die - unable to leave.<BR><BR>
* 
* Chyloth usually only crosses over the souls of the undead, but he can be
* persuaded otherwise...with a token of gold, in the form of a human skull.
* Such a gem can be found only in the belly of the hellspawn known as
* <I>the devourer</I>.<BR><BR>
* 
* I can help you summon the beast from the depths of the abyss, but I require
* 1000 Daemon bones to do so.  If you accept my help, I will store the Daemon
* bones for you until you have collected all 1000 of them.  Once the bones
* are collected in full, I will summon the beast for you, which you must
* slay to claim your prize.<BR><BR>
* 
* Do you accept?
*/
				1050020;

		public override bool IsTutorial => false;
		public override TimeSpan RestartDelay => TimeSpan.Zero;
		public override int Picture => 0x15B5;

		// NOTE: Quest not entirely OSI-accurate: some changes made to prevent numerous OSI bugs

		public override void Slice()
		{
			if (m_WaitForSummon && m_Victoria != null)
			{
				var altar = m_Victoria.Altar;

				if (altar != null && (altar.Daemon == null || !altar.Daemon.Alive))
				{
					if (From.Map == m_Victoria.Map && From.InRange(m_Victoria, 8))
					{
						m_WaitForSummon = false;

						AddConversation(new VanquishDaemonConversation_TheSummoningQuest());
					}
				}
			}

			base.Slice();
		}

		public static int GetDaemonBonesFor(BaseCreature creature)
		{
			if (creature == null || creature.Controlled || creature.Summoned)
			{
				return 0;
			}

			var fame = creature.Fame;

			if (fame < 1500)
			{
				return Utility.Dice(2, 5, -1);
			}
			else if (fame < 20000)
			{
				return Utility.Dice(2, 4, 8);
			}
			else
			{
				return 50;
			}
		}

		public TheSummoningQuest(Victoria victoria, PlayerMobile from) : base(from)
		{
			m_Victoria = victoria;
		}

		public TheSummoningQuest()
		{
		}

		public override void Cancel()
		{
			base.Cancel();

			var obj = FindObjective(typeof(CollectBonesObjective_TheSummoningQuest));

			if (obj != null && obj.CurProgress > 0)
			{
				From.BankBox.DropItem(new DaemonBone(obj.CurProgress));

				From.SendLocalizedMessage(1050030); // The Daemon bones that you have thus far given to Victoria have been returned to you.
			}
		}

		public override void Accept()
		{
			base.Accept();

			AddConversation(new AcceptConversation_TheSummoningQuest());
		}

		public override void ChildDeserialize(GenericReader reader)
		{
			var version = reader.ReadEncodedInt();

			m_Victoria = reader.ReadMobile() as Victoria;
			m_WaitForSummon = reader.ReadBool();
		}

		public override void ChildSerialize(GenericWriter writer)
		{
			writer.WriteEncodedInt(0); // version

			writer.Write(m_Victoria);
			writer.Write(m_WaitForSummon);
		}
	}

	#region Conversation

	public class AcceptConversation_TheSummoningQuest : QuestConversation
	{
		public override object Message =>
				/* You have accepted Victoria's help.  She requires 1000 Daemon
* bones to summon the devourer.<BR><BR>
* 
* You may hand Victoria the bones as you collect them and she
* will keep count of how many you have brought her.<BR><BR>
* 
* Daemon bones can be collected via various means throughout
* Dungeon Doom.<BR><BR>
* 
* Good luck.
*/
				1050027;

		public AcceptConversation_TheSummoningQuest()
		{
		}

		public override void OnRead()
		{
			System.AddObjective(new CollectBonesObjective_TheSummoningQuest());
		}
	}

	public class VanquishDaemonConversation_TheSummoningQuest : QuestConversation
	{
		public override object Message =>
				/* Well done brave soul.   I shall summon the beast to the circle
* of stones just South-East of here.  Take great care - the beast
* takes many forms.  Now hurry...
*/
				1050021;

		public VanquishDaemonConversation_TheSummoningQuest()
		{
		}

		public override void OnRead()
		{
			var victoria = ((TheSummoningQuest)System).Victoria;

			if (victoria == null)
			{
				System.From.SendMessage("Internal error: unable to find Victoria. Quest unable to continue.");
				System.Cancel();
			}
			else
			{
				var altar = victoria.Altar;

				if (altar == null)
				{
					System.From.SendMessage("Internal error: unable to find summoning altar. Quest unable to continue.");
					System.Cancel();
				}
				else if (altar.Daemon == null || !altar.Daemon.Alive)
				{
					var daemon = new BoneDemon();

					daemon.MoveToWorld(altar.Location, altar.Map);
					altar.Daemon = daemon;

					System.AddObjective(new VanquishDaemonObjective_TheSummoningQuest(daemon));
				}
				else
				{
					victoria.SayTo(System.From, "The devourer has already been summoned.");

					((TheSummoningQuest)System).WaitForSummon = true;
				}
			}
		}
	}

	#endregion

	#region Objective

	public class CollectBonesObjective_TheSummoningQuest : QuestObjective
	{
		public override object Message =>
				/* Find 1000 Daemon bones and hand them
* to Victoria as you find them.
*/
				1050026;

		public override int MaxProgress => 1000;

		public CollectBonesObjective_TheSummoningQuest()
		{
		}

		public override void OnComplete()
		{
			var victoria = ((TheSummoningQuest)System).Victoria;

			if (victoria == null)
			{
				System.From.SendMessage("Internal error: unable to find Victoria. Quest unable to continue.");
				System.Cancel();
			}
			else
			{
				var altar = victoria.Altar;

				if (altar == null)
				{
					System.From.SendMessage("Internal error: unable to find summoning altar. Quest unable to continue.");
					System.Cancel();
				}
				else if (altar.Daemon == null || !altar.Daemon.Alive)
				{
					System.AddConversation(new VanquishDaemonConversation_TheSummoningQuest());
				}
				else
				{
					victoria.SayTo(System.From, "The devourer has already been summoned. Return when the devourer has been slain and I will summon it for you.");
					((TheSummoningQuest)System).WaitForSummon = true;
				}
			}
		}

		public override void RenderMessage(BaseQuestGump gump)
		{
			if (CurProgress > 0 && CurProgress < MaxProgress)
			{
				gump.AddHtmlObject(70, 130, 300, 100, 1050028, BaseQuestGump.Blue, false, false); // Victoria has accepted the Daemon bones, but the requirement is not yet met.
			}
			else
			{
				base.RenderMessage(gump);
			}
		}

		public override void RenderProgress(BaseQuestGump gump)
		{
			if (CurProgress > 0 && CurProgress < MaxProgress)
			{
				gump.AddHtmlObject(70, 260, 270, 100, 1050019, BaseQuestGump.Blue, false, false); // Number of bones collected:

				gump.AddLabel(70, 280, 100, CurProgress.ToString());
				gump.AddLabel(100, 280, 100, "/");
				gump.AddLabel(130, 280, 100, MaxProgress.ToString());
			}
			else
			{
				base.RenderProgress(gump);
			}
		}
	}

	public class VanquishDaemonObjective_TheSummoningQuest : QuestObjective
	{
		private BoneDemon m_Daemon;
		private Corpse m_CorpseWithSkull;

		public Corpse CorpseWithSkull
		{
			get => m_CorpseWithSkull;
			set => m_CorpseWithSkull = value;
		}

		public override object Message =>
				/* Go forth and vanquish the devourer that has been summoned!
*/
				1050037;

		public VanquishDaemonObjective_TheSummoningQuest(BoneDemon daemon)
		{
			m_Daemon = daemon;
		}

		// Serialization
		public VanquishDaemonObjective_TheSummoningQuest()
		{
		}

		public override void CheckProgress()
		{
			if (m_Daemon == null || !m_Daemon.Alive)
			{
				Complete();
			}
		}

		public override void OnComplete()
		{
			var victoria = ((TheSummoningQuest)System).Victoria;

			if (victoria != null)
			{
				var altar = victoria.Altar;

				if (altar != null)
				{
					altar.CheckDaemon();
				}
			}

			var from = System.From;

			if (!from.Alive)
			{
				from.SendLocalizedMessage(1050033); // The devourer lies dead, unfortunately so do you.  You cannot claim your reward while dead.  You will need to face him again.
				((TheSummoningQuest)System).WaitForSummon = true;
			}
			else
			{
				var hasRights = true;

				if (m_Daemon != null)
				{
					var lootingRights = BaseCreature.GetLootingRights(m_Daemon.DamageEntries, m_Daemon.HitsMax);

					for (var i = 0; i < lootingRights.Count; ++i)
					{
						var ds = lootingRights[i];

						if (ds.m_HasRight && ds.m_Mobile == from)
						{
							hasRights = true;
							break;
						}
					}
				}

				if (!hasRights)
				{
					from.SendLocalizedMessage(1050034); // The devourer lies dead.  Unfortunately you did not sufficiently prove your worth in combating the devourer.  Victoria shall summon another incarnation of the devourer to the circle of stones.  Try again noble adventurer.
					((TheSummoningQuest)System).WaitForSummon = true;
				}
				else
				{
					from.SendLocalizedMessage(1050035); // The devourer lies dead.  Search his corpse to claim your prize!

					if (m_Daemon != null)
					{
						m_CorpseWithSkull = m_Daemon.Corpse as Corpse;
					}
				}
			}
		}

		public override void ChildDeserialize(GenericReader reader)
		{
			var version = reader.ReadEncodedInt();

			m_Daemon = reader.ReadMobile() as BoneDemon;
			m_CorpseWithSkull = reader.ReadItem() as Corpse;
		}

		public override void ChildSerialize(GenericWriter writer)
		{
			writer.WriteEncodedInt(0); // version

			writer.Write(m_Daemon);
			writer.Write(m_CorpseWithSkull);
		}
	}

	#endregion
}