using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_DropUnusedInventory : ThinkNode_JobGiver
	{
		private const int RawFoodDropDelay = 150000;

		public JobGiver_DropUnusedInventory()
		{
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (pawn.inventory == null)
			{
				result = null;
			}
			else if (!pawn.Map.areaManager.Home[pawn.Position])
			{
				result = null;
			}
			else if (pawn.Faction != Faction.OfPlayer)
			{
				result = null;
			}
			else
			{
				if (Find.TickManager.TicksGame > pawn.mindState.lastInventoryRawFoodUseTick + 150000)
				{
					for (int i = pawn.inventory.innerContainer.Count - 1; i >= 0; i--)
					{
						Thing thing = pawn.inventory.innerContainer[i];
						if (thing.def.IsIngestible && !thing.def.IsDrug && thing.def.ingestible.preferability <= FoodPreferability.RawTasty)
						{
							this.Drop(pawn, thing);
						}
					}
				}
				for (int j = pawn.inventory.innerContainer.Count - 1; j >= 0; j--)
				{
					Thing thing2 = pawn.inventory.innerContainer[j];
					if (thing2.def.IsDrug && pawn.drugs != null && !pawn.drugs.AllowedToTakeScheduledEver(thing2.def) && pawn.drugs.HasEverTaken(thing2.def) && !AddictionUtility.IsAddicted(pawn, thing2))
					{
						this.Drop(pawn, thing2);
					}
				}
				result = null;
			}
			return result;
		}

		private void Drop(Pawn pawn, Thing thing)
		{
			Thing thing2;
			pawn.inventory.innerContainer.TryDrop(thing, pawn.Position, pawn.Map, ThingPlaceMode.Near, out thing2, null, null);
		}
	}
}
