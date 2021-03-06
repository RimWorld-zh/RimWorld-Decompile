﻿using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class WorkGiverUtility
	{
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
