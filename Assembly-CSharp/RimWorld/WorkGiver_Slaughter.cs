using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000121 RID: 289
	public class WorkGiver_Slaughter : WorkGiver_Scanner
	{
		// Token: 0x060005F8 RID: 1528 RVA: 0x0003F9EC File Offset: 0x0003DDEC
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation des in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Slaughter))
			{
				yield return des.target.Thing;
			}
			yield break;
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060005F9 RID: 1529 RVA: 0x0003FA18 File Offset: 0x0003DE18
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x0003FA30 File Offset: 0x0003DE30
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			bool result;
			if (pawn2 == null || !pawn2.RaceProps.Animal)
			{
				result = false;
			}
			else if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Slaughter) == null)
			{
				result = false;
			}
			else if (pawn.Faction != t.Faction)
			{
				result = false;
			}
			else if (pawn2.InAggroMentalState)
			{
				result = false;
			}
			else
			{
				LocalTargetInfo target = t;
				if (!pawn.CanReserve(target, 1, -1, null, forced))
				{
					result = false;
				}
				else if (pawn.story != null && pawn.story.WorkTagIsDisabled(WorkTags.Violent))
				{
					JobFailReason.Is("IsIncapableOfViolenceShort".Translate(), null);
					result = false;
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0003FB10 File Offset: 0x0003DF10
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(JobDefOf.Slaughter, t);
		}
	}
}
