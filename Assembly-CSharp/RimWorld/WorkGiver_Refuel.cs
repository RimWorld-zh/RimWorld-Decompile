using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000157 RID: 343
	public class WorkGiver_Refuel : WorkGiver_Scanner
	{
		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600070E RID: 1806 RVA: 0x00047E9C File Offset: 0x0004629C
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Refuelable);
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x0600070F RID: 1807 RVA: 0x00047EB8 File Offset: 0x000462B8
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000710 RID: 1808 RVA: 0x00047ED0 File Offset: 0x000462D0
		public virtual JobDef JobStandard
		{
			get
			{
				return JobDefOf.Refuel;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x00047EEC File Offset: 0x000462EC
		public virtual JobDef JobAtomic
		{
			get
			{
				return JobDefOf.RefuelAtomic;
			}
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x00047F08 File Offset: 0x00046308
		public virtual bool CanRefuelThing(Thing t)
		{
			return !(t is Building_Turret);
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x00047F2C File Offset: 0x0004632C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return this.CanRefuelThing(t) && RefuelWorkGiverUtility.CanRefuel(pawn, t, forced);
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x00047F58 File Offset: 0x00046358
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return RefuelWorkGiverUtility.RefuelJob(pawn, t, forced, this.JobStandard, this.JobAtomic);
		}
	}
}
