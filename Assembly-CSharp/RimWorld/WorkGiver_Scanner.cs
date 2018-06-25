using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200011B RID: 283
	public abstract class WorkGiver_Scanner : WorkGiver
	{
		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060005D5 RID: 1493 RVA: 0x0003F178 File Offset: 0x0003D578
		public virtual ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Undefined);
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060005D6 RID: 1494 RVA: 0x0003F194 File Offset: 0x0003D594
		public virtual int LocalRegionsToScanFirst
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060005D7 RID: 1495 RVA: 0x0003F1AC File Offset: 0x0003D5AC
		public virtual bool Prioritized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x0003F1C4 File Offset: 0x0003D5C4
		public virtual IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			yield break;
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x0003F1E8 File Offset: 0x0003D5E8
		public virtual IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return null;
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x0003F200 File Offset: 0x0003D600
		public virtual bool AllowUnreachable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060005DB RID: 1499 RVA: 0x0003F218 File Offset: 0x0003D618
		public virtual PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x0003F230 File Offset: 0x0003D630
		public virtual Danger MaxPathDanger(Pawn pawn)
		{
			return pawn.NormalMaxDanger();
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x0003F24C File Offset: 0x0003D64C
		public virtual bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return this.JobOnThing(pawn, t, forced) != null;
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x0003F270 File Offset: 0x0003D670
		public virtual Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return null;
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0003F288 File Offset: 0x0003D688
		public virtual bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return this.JobOnCell(pawn, c, forced) != null;
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0003F2AC File Offset: 0x0003D6AC
		public virtual Job JobOnCell(Pawn pawn, IntVec3 cell, bool forced = false)
		{
			return null;
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0003F2C4 File Offset: 0x0003D6C4
		public virtual float GetPriority(Pawn pawn, TargetInfo t)
		{
			return 0f;
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x0003F2E0 File Offset: 0x0003D6E0
		public float GetPriority(Pawn pawn, IntVec3 cell)
		{
			return this.GetPriority(pawn, new TargetInfo(cell, pawn.Map, false));
		}
	}
}
