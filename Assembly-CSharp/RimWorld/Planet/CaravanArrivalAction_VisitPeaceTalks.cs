using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005CF RID: 1487
	public class CaravanArrivalAction_VisitPeaceTalks : CaravanArrivalAction
	{
		// Token: 0x04001158 RID: 4440
		private PeaceTalks peaceTalks;

		// Token: 0x06001CE9 RID: 7401 RVA: 0x000F80F8 File Offset: 0x000F64F8
		public CaravanArrivalAction_VisitPeaceTalks()
		{
		}

		// Token: 0x06001CEA RID: 7402 RVA: 0x000F8101 File Offset: 0x000F6501
		public CaravanArrivalAction_VisitPeaceTalks(PeaceTalks peaceTalks)
		{
			this.peaceTalks = peaceTalks;
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06001CEB RID: 7403 RVA: 0x000F8114 File Offset: 0x000F6514
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
		// (get) Token: 0x06001CEC RID: 7404 RVA: 0x000F8148 File Offset: 0x000F6548
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

		// Token: 0x06001CED RID: 7405 RVA: 0x000F817C File Offset: 0x000F657C
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

		// Token: 0x06001CEE RID: 7406 RVA: 0x000F81E0 File Offset: 0x000F65E0
		public override void Arrived(Caravan caravan)
		{
			this.peaceTalks.Notify_CaravanArrived(caravan);
		}

		// Token: 0x06001CEF RID: 7407 RVA: 0x000F81EF File Offset: 0x000F65EF
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<PeaceTalks>(ref this.peaceTalks, "peaceTalks", false);
		}

		// Token: 0x06001CF0 RID: 7408 RVA: 0x000F820C File Offset: 0x000F660C
		public static FloatMenuAcceptanceReport CanVisit(Caravan caravan, PeaceTalks peaceTalks)
		{
			return peaceTalks != null && peaceTalks.Spawned;
		}

		// Token: 0x06001CF1 RID: 7409 RVA: 0x000F8238 File Offset: 0x000F6638
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, PeaceTalks peaceTalks)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitPeaceTalks>(() => CaravanArrivalAction_VisitPeaceTalks.CanVisit(caravan, peaceTalks), () => new CaravanArrivalAction_VisitPeaceTalks(peaceTalks), "VisitPeaceTalks".Translate(new object[]
			{
				peaceTalks.Label
			}), caravan, peaceTalks.Tile, peaceTalks);
		}
	}
}
