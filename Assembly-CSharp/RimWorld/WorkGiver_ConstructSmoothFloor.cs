using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000126 RID: 294
	public class WorkGiver_ConstructSmoothFloor : WorkGiver_ConstructAffectFloor
	{
		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000610 RID: 1552 RVA: 0x00040848 File Offset: 0x0003EC48
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.SmoothFloor;
			}
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00040864 File Offset: 0x0003EC64
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return new Job(JobDefOf.SmoothFloor, c);
		}
	}
}
