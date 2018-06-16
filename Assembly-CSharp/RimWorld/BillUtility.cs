using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000027 RID: 39
	public static class BillUtility
	{
		// Token: 0x0600016A RID: 362 RVA: 0x0000E0C9 File Offset: 0x0000C4C9
		public static void TryDrawIngredientSearchRadiusOnMap(this Bill bill, IntVec3 center)
		{
			if (bill.ingredientSearchRadius < GenRadial.MaxRadialPatternRadius)
			{
				GenDraw.DrawRadiusRing(center, bill.ingredientSearchRadius);
			}
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000E0EC File Offset: 0x0000C4EC
		public static Bill MakeNewBill(this RecipeDef recipe)
		{
			Bill result;
			if (recipe.UsesUnfinishedThing)
			{
				result = new Bill_ProductionWithUft(recipe);
			}
			else
			{
				result = new Bill_Production(recipe);
			}
			return result;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000E120 File Offset: 0x0000C520
		public static IEnumerable<IBillGiver> GlobalBillGivers()
		{
			foreach (Map map in Find.Maps)
			{
				foreach (Thing thing in map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.PotentialBillGiver)))
				{
					IBillGiver billgiver = thing as IBillGiver;
					if (billgiver == null)
					{
						Log.ErrorOnce("Found non-bill-giver tagged as PotentialBillGiver", 13389774, false);
					}
					else
					{
						yield return billgiver;
					}
				}
				foreach (Thing thing2 in map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.MinifiedThing)))
				{
					IBillGiver billgiver2 = thing2.GetInnerIfMinified() as IBillGiver;
					if (billgiver2 != null)
					{
						yield return billgiver2;
					}
				}
			}
			foreach (Caravan caravan in Find.WorldObjects.Caravans)
			{
				foreach (Thing thing3 in caravan.AllThings)
				{
					IBillGiver billgiver3 = thing3.GetInnerIfMinified() as IBillGiver;
					if (billgiver3 != null)
					{
						yield return billgiver3;
					}
				}
			}
			yield break;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000E144 File Offset: 0x0000C544
		public static IEnumerable<Bill> GlobalBills()
		{
			foreach (IBillGiver billgiver in BillUtility.GlobalBillGivers())
			{
				foreach (Bill bill in billgiver.BillStack)
				{
					yield return bill;
				}
			}
			if (BillUtility.Clipboard != null)
			{
				yield return BillUtility.Clipboard;
			}
			yield break;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000E168 File Offset: 0x0000C568
		public static void Notify_ZoneStockpileRemoved(Zone_Stockpile stockpile)
		{
			foreach (Bill bill in BillUtility.GlobalBills())
			{
				bill.ValidateSettings();
			}
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000E1C4 File Offset: 0x0000C5C4
		public static void Notify_ColonistUnavailable(Pawn pawn)
		{
			foreach (Bill bill in BillUtility.GlobalBills())
			{
				bill.ValidateSettings();
			}
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000E220 File Offset: 0x0000C620
		public static WorkGiverDef GetWorkgiver(this IBillGiver billGiver)
		{
			Thing thing = billGiver as Thing;
			WorkGiverDef result;
			if (thing == null)
			{
				Log.ErrorOnce(string.Format("Attempting to get the workgiver for a non-Thing IBillGiver {0}", billGiver.ToString()), 96810282, false);
				result = null;
			}
			else
			{
				List<WorkGiverDef> allDefsListForReading = DefDatabase<WorkGiverDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					WorkGiverDef workGiverDef = allDefsListForReading[i];
					WorkGiver_DoBill workGiver_DoBill = workGiverDef.Worker as WorkGiver_DoBill;
					if (workGiver_DoBill != null)
					{
						if (workGiver_DoBill.ThingIsUsableBillGiver(thing))
						{
							return workGiverDef;
						}
					}
				}
				Log.ErrorOnce(string.Format("Can't find a WorkGiver for a BillGiver {0}", thing.ToString()), 57348705, false);
				result = null;
			}
			return result;
		}

		// Token: 0x0400019F RID: 415
		public static Bill Clipboard = null;
	}
}
