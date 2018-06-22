using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000125 RID: 293
	public abstract class WorkGiver_ConstructAffectFloor : WorkGiver_Scanner
	{
		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600060B RID: 1547
		protected abstract DesignationDef DesDef { get; }

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600060C RID: 1548 RVA: 0x00040554 File Offset: 0x0003E954
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x0004056C File Offset: 0x0003E96C
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			if (pawn.Faction != Faction.OfPlayer)
			{
				yield break;
			}
			foreach (Designation des in pawn.Map.designationManager.SpawnedDesignationsOfDef(this.DesDef))
			{
				yield return des.target.Cell;
			}
			yield break;
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x000405A0 File Offset: 0x0003E9A0
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			if (!c.IsForbidden(pawn) && pawn.Map.designationManager.DesignationAt(c, this.DesDef) != null)
			{
				LocalTargetInfo target = c;
				ReservationLayerDef floor = ReservationLayerDefOf.Floor;
				if (pawn.CanReserve(target, 1, -1, floor, forced))
				{
					return true;
				}
			}
			return false;
		}
	}
}
