using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200015B RID: 347
	public class WorkGiver_RescueDowned : WorkGiver_TakeToBed
	{
		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000727 RID: 1831 RVA: 0x0004878C File Offset: 0x00046B8C
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x000487A4 File Offset: 0x00046BA4
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000729 RID: 1833 RVA: 0x000487BC File Offset: 0x00046BBC
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x000487D8 File Offset: 0x00046BD8
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 != null && pawn2.Downed && pawn2.Faction == pawn.Faction && !pawn2.InBed())
			{
				LocalTargetInfo target = pawn2;
				if (pawn.CanReserve(target, 1, -1, null, forced) && !GenAI.EnemyIsNear(pawn2, 40f))
				{
					Thing thing = base.FindBed(pawn, pawn2);
					return thing != null && pawn2.CanReserve(thing, 1, -1, null, false);
				}
			}
			return false;
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00048888 File Offset: 0x00046C88
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			Thing t2 = base.FindBed(pawn, pawn2);
			return new Job(JobDefOf.Rescue, pawn2, t2)
			{
				count = 1
			};
		}

		// Token: 0x04000330 RID: 816
		private const float MinDistFromEnemy = 40f;
	}
}
