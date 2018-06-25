using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020004AE RID: 1198
	public static class RecordsUtility
	{
		// Token: 0x0600155C RID: 5468 RVA: 0x000BDB30 File Offset: 0x000BBF30
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

		// Token: 0x0600155D RID: 5469 RVA: 0x000BDBA8 File Offset: 0x000BBFA8
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

		// Token: 0x0600155E RID: 5470 RVA: 0x000BDC20 File Offset: 0x000BC020
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

		// Token: 0x0600155F RID: 5471 RVA: 0x000BDCB0 File Offset: 0x000BC0B0
		private static bool ShouldIncrementThingsCrafted(Thing crafted)
		{
			return crafted.def.IsApparel || crafted.def.IsWeapon || crafted.def.HasComp(typeof(CompArt)) || crafted.def.HasComp(typeof(CompQuality));
		}
	}
}
