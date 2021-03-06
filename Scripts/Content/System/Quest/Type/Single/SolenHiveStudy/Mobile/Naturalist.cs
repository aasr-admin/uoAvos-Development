using Server.Engines.Plants;
using Server.Engines.Quests.Definitions;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests.Mobiles
{
	public class Naturalist : BaseQuester
	{
		[Constructable]
		public Naturalist() : base("the Naturalist")
		{
		}

		public override void InitBody()
		{
			InitStats(100, 100, 25);

			Hue = Utility.RandomSkinHue();

			Female = false;
			Body = 0x190;
			Name = NameList.RandomName("male");
		}

		public override void InitOutfit()
		{
			AddItem(new Tunic(0x598));
			AddItem(new LongPants(0x59B));
			AddItem(new Boots());


			Utility.AssignRandomHair(this);
			Utility.AssignRandomFacialHair(this, HairHue);
		}

		public override void OnTalk(PlayerMobile player, bool contextMenu)
		{
			var qs = player.Quest as StudyOfSolenQuest;

			if (qs != null && qs.Naturalist == this)
			{
				var study = qs.FindObjective(typeof(StudyNestsObjective_StudyOfSolenQuest)) as StudyNestsObjective_StudyOfSolenQuest;

				if (study != null)
				{
					if (!study.Completed)
					{
						PlaySound(0x41F);
						qs.AddConversation(new NaturalistDuringStudyConversation_StudyOfSolenQuest());
					}
					else
					{
						var obj = qs.FindObjective(typeof(ReturnToNaturalistObjective_StudyOfSolenQuest));

						if (obj != null && !obj.Completed)
						{
							Seed reward;

							PlantType type;
							switch (Utility.Random(17))
							{
								case 0: type = PlantType.CampionFlowers; break;
								case 1: type = PlantType.Poppies; break;
								case 2: type = PlantType.Snowdrops; break;
								case 3: type = PlantType.Bulrushes; break;
								case 4: type = PlantType.Lilies; break;
								case 5: type = PlantType.PampasGrass; break;
								case 6: type = PlantType.Rushes; break;
								case 7: type = PlantType.ElephantEarPlant; break;
								case 8: type = PlantType.Fern; break;
								case 9: type = PlantType.PonytailPalm; break;
								case 10: type = PlantType.SmallPalm; break;
								case 11: type = PlantType.CenturyPlant; break;
								case 12: type = PlantType.WaterPlant; break;
								case 13: type = PlantType.SnakePlant; break;
								case 14: type = PlantType.PricklyPearCactus; break;
								case 15: type = PlantType.BarrelCactus; break;
								default: type = PlantType.TribarrelCactus; break;
							}

							if (study.StudiedSpecialNest)
							{
								reward = new Seed(type, PlantHue.FireRed, false);
							}
							else
							{
								PlantHue hue;
								switch (Utility.Random(3))
								{
									case 0: hue = PlantHue.Pink; break;
									case 1: hue = PlantHue.Magenta; break;
									default: hue = PlantHue.Aqua; break;
								}

								reward = new Seed(type, hue, false);
							}

							if (player.PlaceInBackpack(reward))
							{
								obj.Complete();

								PlaySound(0x449);
								PlaySound(0x41B);

								if (study.StudiedSpecialNest)
								{
									qs.AddConversation(new SpecialEndConversation_StudyOfSolenQuest());
								}
								else
								{
									qs.AddConversation(new EndConversation_StudyOfSolenQuest());
								}
							}
							else
							{
								reward.Delete();

								qs.AddConversation(new FullBackpackConversation_StudyOfSolenQuest());
							}
						}
					}
				}
			}
			else
			{
				QuestSystem newQuest = new StudyOfSolenQuest(player, this);

				if (player.Quest == null && QuestSystem.CanOfferQuest(player, typeof(StudyOfSolenQuest)))
				{
					PlaySound(0x42F);
					newQuest.SendOffer();
				}
				else
				{
					PlaySound(0x448);
					newQuest.AddConversation(new DontOfferConversation_StudyOfSolenQuest());
				}
			}
		}

		public Naturalist(Serial serial) : base(serial)
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