using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000159 RID: 345
	public class WorkGiver_Refuel_Turret : WorkGiver_Refuel
	{
		// Token: 0x17000114 RID: 276
		// (get) Token: 0x0600071B RID: 1819 RVA: 0x000484C4 File Offset: 0x000468C4
		public override JobDef JobStandard
		{
			get
			{
				return JobDefOf.RearmTurret;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x0600071C RID: 1820 RVA: 0x000484E0 File Offset: 0x000468E0
		public override JobDef JobAtomic
		{
			get
			{
				return JobDefOf.RearmTurretAtomic;
			}
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x000484FC File Offset: 0x000468FC
		public override bool CanRefuelThing(Thing t)
		{
			return t is Building_Turret;
		}
	}
}
