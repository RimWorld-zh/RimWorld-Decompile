using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200014D RID: 333
	public class WorkGiver_LoadTransporters : WorkGiver_Scanner
	{
		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060006E2 RID: 1762 RVA: 0x00046788 File Offset: 0x00044B88
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Transporter);
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060006E3 RID: 1763 RVA: 0x000467A4 File Offset: 0x00044BA4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x000467BC File Offset: 0x00044BBC
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x000467D4 File Offset: 0x00044BD4
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompTransporter transporter = t.TryGetComp<CompTransporter>();
			return LoadTransportersJobUtility.HasJobOnTransporter(pawn, transporter);
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x000467F8 File Offset: 0x00044BF8
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompTransporter transporter = t.TryGetComp<CompTransporter>();
			return LoadTransportersJobUtility.JobOnTransporter(pawn, transporter);
		}
	}
}
