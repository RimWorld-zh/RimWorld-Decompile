using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200011C RID: 284
	public static class WorkGiverUtility
	{
		// Token: 0x060005E4 RID: 1508 RVA: 0x0003F3AC File Offset: 0x0003D7AC
		public static Job HaulStuffOffBillGiverJob(Pawn pawn, IBillGiver giver, Thing thingToIgnore)
		{
			foreach (IntVec3 c in giver.IngredientStackCells)
			{
				Thing thing = pawn.Map.thingGrid.ThingAt(c, ThingCategory.Item);
				if (thing != null)
				{
					if (thing != thingToIgnore)
					{
						return HaulAIUtility.HaulAsideJobFor(pawn, thing);
					}
				}
			}
			return null;
		}
	}
}
