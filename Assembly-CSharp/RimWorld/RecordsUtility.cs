using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020004AC RID: 1196
	public static class RecordsUtility
	{
		// Token: 0x06001559 RID: 5465 RVA: 0x000BD7E0 File Offset: 0x000BBBE0
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

		// Token: 0x0600155A RID: 5466 RVA: 0x000BD858 File Offset: 0x000BBC58
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

		// Token: 0x0600155B RID: 5467 RVA: 0x000BD8D0 File Offset: 0x000BBCD0
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

		// Token: 0x0600155C RID: 5468 RVA: 0x000BD960 File Offset: 0x000BBD60
		private static bool ShouldIncrementThingsCrafted(Thing crafted)
		{
			return crafted.def.IsApparel || crafted.def.IsWeapon || crafted.def.HasComp(typeof(CompArt)) || crafted.def.HasComp(typeof(CompQuality));
		}
	}
}
