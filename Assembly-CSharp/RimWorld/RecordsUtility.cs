using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020004B0 RID: 1200
	public static class RecordsUtility
	{
		// Token: 0x06001562 RID: 5474 RVA: 0x000BD7C8 File Offset: 0x000BBBC8
		public static void Notify_PawnKilled(Pawn killed, Pawn killer)
		{
			killer.records.Increment(RecordDefOf.Kills);
			RaceProperties raceProps = killed.RaceProps;
			if (raceProps.Humanlike)
			{
				killer.records.Increment(RecordDefOf.KillsHumanlikes);
			}
			if (raceProps.Animal)
			{
				killer.records.Increment(RecordDefOf.KillsAnimals);
			}
			if (raceProps.IsMechanoid)
			{
				killer.records.Increment(RecordDefOf.KillsMechanoids);
			}
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x000BD840 File Offset: 0x000BBC40
		public static void Notify_PawnDowned(Pawn downed, Pawn instigator)
		{
			instigator.records.Increment(RecordDefOf.PawnsDowned);
			RaceProperties raceProps = downed.RaceProps;
			if (raceProps.Humanlike)
			{
				instigator.records.Increment(RecordDefOf.PawnsDownedHumanlikes);
			}
			if (raceProps.Animal)
			{
				instigator.records.Increment(RecordDefOf.PawnsDownedAnimals);
			}
			if (raceProps.IsMechanoid)
			{
				instigator.records.Increment(RecordDefOf.PawnsDownedMechanoids);
			}
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x000BD8B8 File Offset: 0x000BBCB8
		public static void Notify_BillDone(Pawn billDoer, List<Thing> products)
		{
			for (int i = 0; i < products.Count; i++)
			{
				if (products[i].def.IsNutritionGivingIngestible && products[i].def.ingestible.preferability >= FoodPreferability.MealAwful)
				{
					billDoer.records.Increment(RecordDefOf.MealsCooked);
				}
				else if (RecordsUtility.ShouldIncrementThingsCrafted(products[i]))
				{
					billDoer.records.Increment(RecordDefOf.ThingsCrafted);
				}
			}
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x000BD948 File Offset: 0x000BBD48
		private static bool ShouldIncrementThingsCrafted(Thing crafted)
		{
			return crafted.def.IsApparel || crafted.def.IsWeapon || crafted.def.HasComp(typeof(CompArt)) || crafted.def.HasComp(typeof(CompQuality));
		}
	}
}
