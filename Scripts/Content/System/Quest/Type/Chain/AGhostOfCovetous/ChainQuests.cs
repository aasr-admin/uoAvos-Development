using Server.Engines.ChainQuests.Items;
using Server.Engines.ChainQuests.Mobiles;
using Server.Mobiles;

using System;

namespace Server.Engines.ChainQuests.Definitions
{
	public class AGhostOfCovetous : ChainQuest
	{
		public override Type NextQuest => typeof(SaveHisDad);

		public AGhostOfCovetous()
		{
			Activated = true;
			Title = 1075287; // A Ghost of Covetous
			Description = 1075286; // What? Oh, you startled me! Sorry, I'm a little jumpy. My master Griswolt learned that a ghost has recently taken up residence in the Covetous dungeon. He sent me to capture it, but I . . . well, it terrified me, to be perfectly honest. If you think yourself courageous enough, I'll give you my Spirit Bottle, and you can try to capture it yourself. I'm certain my master would reward you richly for such service.
			RefusalMessage = 1075288; // That's okay, I'm sure someone with more courage than either of us will come along eventually.
			InProgressMessage = 1075290; // You'll find that ghost in the mountain pass above the Covetous dungeon.
			CompletionMessage = 1075291; // (As you try to use the Spirit Bottle, the ghost snatches it out of your hand and smashes it on the rocks) Please, don't be frightened. I need your help!
			CompletionNotice = CompletionNoticeShort;

			Objectives.Add(new DeliverObjective(typeof(SpiritBottle), 1, "Spirit Bottle", typeof(Frederic)));

			Rewards.Add(new DummyReward(1075284)); // Return the filled Spirit Bottle to Griswolt the Master Necromancer to receive a reward.
		}

		public override void Generate()
		{
			base.Generate();

			PutSpawner(new Spawner(1, 5, 10, 0, 3, "Ben"), new Point3D(2467, 402, 15), Map.Trammel);
			PutSpawner(new Spawner(1, 5, 10, 0, 3, "Ben"), new Point3D(2467, 402, 15), Map.Felucca);
		}
	}

	public class SaveHisDad : ChainQuest
	{
		public override Type NextQuest => typeof(AFathersGratitude);
		public override bool IsChainTriggered => true;

		public SaveHisDad()
		{
			Activated = true;
			Title = 1075337; // Save His Dad
			Description = 1075338; // My father, Andros, is a smith in Minoc. Last week his forge overturned and he was splashed by molten steel. He was horribly burned, and we feared he would die. An alchemist in Vesper promised to make a bandage that could heal him, but he needed the silk of a dread spider. I came here to get some, but I was careless, and succumbed to their poison. Please, won’t you help my father?
			RefusalMessage = 1075340; // Oh . . . that’s your decision . . . OooOoooOOoo . . .
			InProgressMessage = 1075341; // Thank you! Deliver it to Leon the Alchemist in Vesper. The silk crumbles easily, and much time has already passed since I died. Please! Hurry!
			CompletionMessage = 1075342; // How may I help thee? You have the silk of a dread spider? Of course I can make you a bandage, but what happened to Frederic?
			CompletionNotice = CompletionNoticeShort;

			Objectives.Add(new TimedDeliverObjective(TimeSpan.FromSeconds(600), typeof(DreadSpiderSilk), 1, "Dread Spider Silk", typeof(Leon)));

			Rewards.Add(new DummyReward(1075339)); // Hurry! You must get the silk to Leon the Alchemist quickly, or it will crumble and become useless!
		}

		public override void Generate()
		{
			base.Generate();

			PutSpawner(new Spawner(1, 5, 10, 0, 0, "Frederic"), new Point3D(2415, 887, 0), Map.Trammel);
			PutSpawner(new Spawner(1, 5, 10, 0, 0, "Frederic"), new Point3D(2415, 887, 0), Map.Felucca);
		}
	}

	public class AFathersGratitude : ChainQuest
	{
		public override bool IsChainTriggered => true;

		public AFathersGratitude()
		{
			Activated = true;
			OneTimeOnly = true;
			Title = 1075343; // A Father’s Gratitude
			Description = 1075344; // That is simply terrible. First Andros, and now his son. Well, let’s make sure Frederic’s sacrifice wasn’t in vain. Will you take the bandages to his father? You can probably deliver them faster than I can, can’t you?
			RefusalMessage = 1075346; // Well I’m sorry to hear you say that. Without your help, I don’t know if I can get these to Andros quickly enough to help him.
			InProgressMessage = 1075347; // I don’t know how much longer Andros will survive. You’d better get this to him as quick as you can. Every second counts!
			CompletionMessage = 1075348; // Sorry, I’m not accepting commissions at the moment. What? You have the bandage I need from Leon? Thank you so much! But why didn’t my son bring this to me himself? . . . Oh, no! You can't be serious! *sag* My Freddie, my son! Thank you for carrying out his last wish. Here -- I made this for my son, to give to him when he became a journeyman. I want you to have it.
			CompletionNotice = CompletionNoticeShort;

			Objectives.Add(new DeliverObjective(typeof(AlchemistsBandage), 1, "Alchemist's Bandage", typeof(Andros)));

			Rewards.Add(new ItemReward(1075345, typeof(AndrosGratitude))); // Andros’ Gratitude
		}

		public override void Generate()
		{
			base.Generate();

			PutSpawner(new Spawner(1, 5, 10, 0, 3, "Leon"), new Point3D(2918, 851, 0), Map.Trammel);
			PutSpawner(new Spawner(1, 5, 10, 0, 3, "Leon"), new Point3D(2918, 851, 0), Map.Felucca);
			PutSpawner(new Spawner(1, 5, 10, 0, 3, "Andros"), new Point3D(2531, 581, 0), Map.Trammel);
			PutSpawner(new Spawner(1, 5, 10, 0, 3, "Andros"), new Point3D(2531, 581, 0), Map.Felucca);
		}
	}
}