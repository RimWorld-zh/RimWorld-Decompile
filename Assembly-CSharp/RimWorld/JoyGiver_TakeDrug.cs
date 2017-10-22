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
			Thing result;
			ThingOwner<Thing> innerContainer;
			int i;
			Thing thing;
			if (pawn.drugs == null)
			{
				result = null;
			}
			else
			{
				Predicate<Thing> predicate = (Predicate<Thing>)((Thing t) => (byte)(this.CanIngestForJoy(pawn, t) ? (((object)extraValidator == null || extraValidator(t)) ? 1 : 0) : 0) != 0);
				innerContainer = pawn.inventory.innerContainer;
				for (i = 0; i < innerContainer.Count; i++)
				{
					if (predicate(innerContainer[i]))
						goto IL_006d;
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
						IntVec3 position = pawn.Position;
						Map map = pawn.Map;
						List<Thing> searchSet = list;
						PathEndMode peMode = PathEndMode.OnCell;
						TraverseParms traverseParams = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
						Predicate<Thing> validator = predicate;
						thing = GenClosest.ClosestThing_Global_Reachable(position, map, searchSet, peMode, traverseParams, 9999f, validator, null);
						if (thing != null)
							goto IL_018c;
					}
				}
				result = null;
			}
			goto IL_01b4;
			IL_018c:
			result = thing;
			goto IL_01b4;
			IL_01b4:
			return result;
			IL_006d:
			result = innerContainer[i];
			goto IL_01b4;
		}

		public override float GetChance(Pawn pawn)
		{
			int num = 0;
			if (pawn.story != null)
			{
				num = pawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire);
			}
			float result;
			if (num < 0)
			{
				result = 0f;
			}
			else
			{
				float num2 = base.def.baseChance;
				if (num == 1)
				{
					num2 = (float)(num2 * 2.0);
				}
				if (num == 2)
				{
					num2 = (float)(num2 * 5.0);
				}
				result = num2;
			}
			return result;
		}

		protected override Job CreateIngestJob(Thing ingestible, Pawn pawn)
		{
			return DrugAIUtility.IngestAndTakeToInventoryJob(ingestible, pawn, 9999);
		}
	}
}
