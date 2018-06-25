using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005CF RID: 1487
	public class CaravanArrivalAction_VisitPeaceTalks : CaravanArrivalAction
	{
		// Token: 0x04001154 RID: 4436
		private PeaceTalks peaceTalks;

		// Token: 0x06001CEA RID: 7402 RVA: 0x000F7E90 File Offset: 0x000F6290
		public CaravanArrivalAction_VisitPeaceTalks()
		{
		}

		// Token: 0x06001CEB RID: 7403 RVA: 0x000F7E99 File Offset: 0x000F6299
		public CaravanArrivalAction_VisitPeaceTalks(PeaceTalks peaceTalks)
		{
			this.peaceTalks = peaceTalks;
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06001CEC RID: 7404 RVA: 0x000F7EAC File Offset: 0x000F62AC
		public override string Label
		{
			get
			{
				return "VisitPeaceTalks".Translate(new object[]
				{
					this.peaceTalks.Label
				});
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06001CED RID: 7405 RVA: 0x000F7EE0 File Offset: 0x000F62E0
		public override string ReportString
		{
			get
			{
				return "CaravanVisiting".Translate(new object[]
				{
					this.peaceTalks.Label
				});
			}
		}

		// Token: 0x06001CEE RID: 7406 RVA: 0x000F7F14 File Offset: 0x000F6314
		public override FloatMenuAcceptanceReport StillValid(Caravan caravan, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(caravan, destinationTile);
			FloatMenuAcceptanceReport result;
			if (!floatMenuAcceptanceReport)
			{
				result = floatMenuAcceptanceReport;
			}
			else if (this.peaceTalks != null && this.peaceTalks.Tile != destinationTile)
			{
				result = false;
			}
			else
			{
				result = CaravanArrivalAction_VisitPeaceTalks.CanVisit(caravan, this.peaceTalks);
			}
			return result;
		}

		// Token: 0x06001CEF RID: 7407 RVA: 0x000F7F78 File Offset: 0x000F6378
		public override void Arrived(Caravan caravan)
		{
			this.peaceTalks.Notify_CaravanArrived(caravan);
		}

		// Token: 0x06001CF0 RID: 7408 RVA: 0x000F7F87 File Offset: 0x000F6387
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<PeaceTalks>(ref this.peaceTalks, "peaceTalks", false);
		}

		// Token: 0x06001CF1 RID: 7409 RVA: 0x000F7FA4 File Offset: 0x000F63A4
		public static FloatMenuAcceptanceReport CanVisit(Caravan caravan, PeaceTalks peaceTalks)
		{
			return peaceTalks != null && peaceTalks.Spawned;
		}

		// Token: 0x06001CF2 RID: 7410 RVA: 0x000F7FD0 File Offset: 0x000F63D0
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, PeaceTalks peaceTalks)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitPeaceTalks>(() => CaravanArrivalAction_VisitPeaceTalks.CanVisit(caravan, peaceTalks), () => new CaravanArrivalAction_VisitPeaceTalks(peaceTalks), "VisitPeaceTalks".Translate(new object[]
			{
				peaceTalks.Label
			}), caravan, peaceTalks.Tile, peaceTalks);
		}
	}
}
