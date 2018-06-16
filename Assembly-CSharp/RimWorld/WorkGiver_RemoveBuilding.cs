using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200012F RID: 303
	public abstract class WorkGiver_RemoveBuilding : WorkGiver_Scanner
	{
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600063C RID: 1596
		protected abstract DesignationDef Designation { get; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x0600063D RID: 1597
		protected abstract JobDef RemoveBuildingJob { get; }

		// Token: 0x0600063E RID: 1598 RVA: 0x00041AB0 File Offset: 0x0003FEB0
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation des in pawn.Map.designationManager.SpawnedDesignationsOfDef(this.Designation))
			{
				yield return des.target.Thing;
			}
			yield break;
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600063F RID: 1599 RVA: 0x00041AE4 File Offset: 0x0003FEE4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x00041AFC File Offset: 0x0003FEFC
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			LocalTargetInfo target = t;
			return pawn.CanReserve(target, 1, -1, null, forced) && pawn.Map.designationManager.DesignationOn(t, this.Designation) != null;
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x00041B58 File Offset: 0x0003FF58
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(this.RemoveBuildingJob, t);
		}
	}
}
