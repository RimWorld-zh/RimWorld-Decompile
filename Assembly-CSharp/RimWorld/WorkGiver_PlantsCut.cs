using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_PlantsCut : WorkGiver_Scanner
	{
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			List<Designation> desList = pawn.Map.designationManager.allDesignations;
			int i = 0;
			Designation des;
			while (true)
			{
				if (i < desList.Count)
				{
					des = desList[i];
					if (des.def != DesignationDefOf.CutPlant && des.def != DesignationDefOf.HarvestPlant)
					{
						i++;
						continue;
					}
					break;
				}
				yield break;
			}
			yield return des.target.Thing;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job result;
			if (t.def.category != ThingCategory.Plant)
			{
				result = null;
			}
			else
			{
				LocalTargetInfo target = t;
				if (!pawn.CanReserve(target, 1, -1, null, forced))
				{
					result = null;
				}
				else if (t.IsForbidden(pawn))
				{
					result = null;
				}
				else if (t.IsBurning())
				{
					result = null;
				}
				else
				{
					using (IEnumerator<Designation> enumerator = pawn.Map.designationManager.AllDesignationsOn(t).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Designation current = enumerator.Current;
							if (current.def == DesignationDefOf.HarvestPlant)
								goto IL_0099;
							if (current.def == DesignationDefOf.CutPlant)
							{
								return new Job(JobDefOf.CutPlant, t);
							}
						}
						goto end_IL_0079;
						IL_0099:
						if (!((Plant)t).HarvestableNow)
						{
							return null;
						}
						return new Job(JobDefOf.Harvest, t);
						end_IL_0079:;
					}
					result = null;
				}
			}
			return result;
		}
	}
}
