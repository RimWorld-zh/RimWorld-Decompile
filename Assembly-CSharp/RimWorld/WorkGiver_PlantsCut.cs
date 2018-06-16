using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000155 RID: 341
	public class WorkGiver_PlantsCut : WorkGiver_Scanner
	{
		// Token: 0x06000703 RID: 1795 RVA: 0x00047780 File Offset: 0x00045B80
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x00047798 File Offset: 0x00045B98
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			List<Designation> desList = pawn.Map.designationManager.allDesignations;
			for (int i = 0; i < desList.Count; i++)
			{
				Designation des = desList[i];
				if (des.def == DesignationDefOf.CutPlant || des.def == DesignationDefOf.HarvestPlant)
				{
					yield return des.target.Thing;
				}
			}
			yield break;
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000705 RID: 1797 RVA: 0x000477C4 File Offset: 0x00045BC4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x000477DC File Offset: 0x00045BDC
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
					foreach (Designation designation in pawn.Map.designationManager.AllDesignationsOn(t))
					{
						if (designation.def == DesignationDefOf.HarvestPlant)
						{
							if (!((Plant)t).HarvestableNow)
							{
								return null;
							}
							return new Job(JobDefOf.HarvestDesignated, t);
						}
						else if (designation.def == DesignationDefOf.CutPlant)
						{
							return new Job(JobDefOf.CutPlantDesignated, t);
						}
					}
					result = null;
				}
			}
			return result;
		}
	}
}
