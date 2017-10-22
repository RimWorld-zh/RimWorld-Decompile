using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JoyGiver_TakeDrug : JoyGiver_Ingest
	{
		private static List<ThingDef> takeableDrugs = new List<ThingDef>();

		protected override Thing BestIngestItem(Pawn pawn, Predicate<Thing> extraValidator)
		{
			Predicate<Thing> predicate = (Predicate<Thing>)delegate(Thing t)
			{
				if (!this.CanIngestForJoy(pawn, t))
				{
					return false;
				}
				if ((object)extraValidator != null && !extraValidator(t))
				{
					return false;
				}
				return true;
			};
			ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				if (predicate(innerContainer[i]))
				{
					return innerContainer[i];
				}
			}
			JoyGiver_TakeDrug.takeableDrugs.Clear();
			DrugPolicy currentPolicy = pawn.drugs.CurrentPolicy;
			for (int j = 0; j < currentPolicy.Count; j++)
			{
				if (currentPolicy[j].allowedForJoy)
				{
					JoyGiver_TakeDrug.takeableDrugs.Add(currentPolicy[j].drug);
				}
			}
			JoyGiver_TakeDrug.takeableDrugs.Shuffle();
			for (int k = 0; k < JoyGiver_TakeDrug.takeableDrugs.Count; k++)
			{
				List<Thing> list = pawn.Map.listerThings.ThingsOfDef(JoyGiver_TakeDrug.takeableDrugs[k]);
				if (list.Count > 0)
				{
					Predicate<Thing> validator = predicate;
					Thing thing = GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, list, PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, null);
					if (thing != null)
					{
						return thing;
					}
				}
			}
			return null;
		}

		public override float GetChance(Pawn pawn)
		{
			int num = 0;
			if (pawn.story != null)
			{
				num = pawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire);
			}
			if (num < 0)
			{
				return 0f;
			}
			float num2 = base.def.baseChance;
			if (num == 1)
			{
				num2 = (float)(num2 * 2.0);
			}
			if (num == 2)
			{
				num2 = (float)(num2 * 5.0);
			}
			return num2;
		}

		protected override Job CreateIngestJob(Thing ingestible, Pawn pawn)
		{
			return DrugAIUtility.IngestAndTakeToInventoryJob(ingestible, pawn, 9999);
		}
	}
}
