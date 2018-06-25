using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000102 RID: 258
	public class JoyGiver_TakeDrug : JoyGiver_Ingest
	{
		// Token: 0x040002E3 RID: 739
		private static List<ThingDef> takeableDrugs = new List<ThingDef>();

		// Token: 0x0600056C RID: 1388 RVA: 0x0003AF7C File Offset: 0x0003937C
		protected override Thing BestIngestItem(Pawn pawn, Predicate<Thing> extraValidator)
		{
			Thing result;
			if (pawn.drugs == null)
			{
				result = null;
			}
			else
			{
				Predicate<Thing> predicate = (Thing t) => this.CanIngestForJoy(pawn, t) && (extraValidator == null || extraValidator(t));
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
				JoyGiver_TakeDrug.takeableDrugs.Shuffle<ThingDef>();
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
						Thing thing = GenClosest.ClosestThing_Global_Reachable(position, map, searchSet, peMode, traverseParams, 9999f, validator, null);
						if (thing != null)
						{
							return thing;
						}
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x0003B140 File Offset: 0x00039540
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
				float num2 = this.def.baseChance;
				if (num == 1)
				{
					num2 *= 2f;
				}
				if (num == 2)
				{
					num2 *= 5f;
				}
				result = num2;
			}
			return result;
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0003B1B8 File Offset: 0x000395B8
		protected override Job CreateIngestJob(Thing ingestible, Pawn pawn)
		{
			return DrugAIUtility.IngestAndTakeToInventoryJob(ingestible, pawn, 9999);
		}
	}
}
