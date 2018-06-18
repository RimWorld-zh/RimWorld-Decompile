using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005D1 RID: 1489
	public class CaravanArrivalAction_VisitPeaceTalks : CaravanArrivalAction
	{
		// Token: 0x06001CEF RID: 7407 RVA: 0x000F7CEC File Offset: 0x000F60EC
		public CaravanArrivalAction_VisitPeaceTalks()
		{
		}

		// Token: 0x06001CF0 RID: 7408 RVA: 0x000F7CF5 File Offset: 0x000F60F5
		public CaravanArrivalAction_VisitPeaceTalks(PeaceTalks peaceTalks)
		{
			this.peaceTalks = peaceTalks;
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06001CF1 RID: 7409 RVA: 0x000F7D08 File Offset: 0x000F6108
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
		// (get) Token: 0x06001CF2 RID: 7410 RVA: 0x000F7D3C File Offset: 0x000F613C
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

		// Token: 0x06001CF3 RID: 7411 RVA: 0x000F7D70 File Offset: 0x000F6170
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

		// Token: 0x06001CF4 RID: 7412 RVA: 0x000F7DD4 File Offset: 0x000F61D4
		public override void Arrived(Caravan caravan)
		{
			this.peaceTalks.Notify_CaravanArrived(caravan);
		}

		// Token: 0x06001CF5 RID: 7413 RVA: 0x000F7DE3 File Offset: 0x000F61E3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<PeaceTalks>(ref this.peaceTalks, "peaceTalks", false);
		}

		// Token: 0x06001CF6 RID: 7414 RVA: 0x000F7E00 File Offset: 0x000F6200
		public static FloatMenuAcceptanceReport CanVisit(Caravan caravan, PeaceTalks peaceTalks)
		{
			return peaceTalks != null && peaceTalks.Spawned;
		}

		// Token: 0x06001CF7 RID: 7415 RVA: 0x000F7E2C File Offset: 0x000F622C
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, PeaceTalks peaceTalks)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitPeaceTalks>(() => CaravanArrivalAction_VisitPeaceTalks.CanVisit(caravan, peaceTalks), () => new CaravanArrivalAction_VisitPeaceTalks(peaceTalks), "VisitPeaceTalks".Translate(new object[]
			{
				peaceTalks.Label
			}), caravan, peaceTalks.Tile, peaceTalks);
		}

		// Token: 0x04001157 RID: 4439
		private PeaceTalks peaceTalks;
	}
}
