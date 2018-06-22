using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005CD RID: 1485
	public class CaravanArrivalAction_VisitPeaceTalks : CaravanArrivalAction
	{
		// Token: 0x06001CE6 RID: 7398 RVA: 0x000F7D40 File Offset: 0x000F6140
		public CaravanArrivalAction_VisitPeaceTalks()
		{
		}

		// Token: 0x06001CE7 RID: 7399 RVA: 0x000F7D49 File Offset: 0x000F6149
		public CaravanArrivalAction_VisitPeaceTalks(PeaceTalks peaceTalks)
		{
			this.peaceTalks = peaceTalks;
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06001CE8 RID: 7400 RVA: 0x000F7D5C File Offset: 0x000F615C
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
		// (get) Token: 0x06001CE9 RID: 7401 RVA: 0x000F7D90 File Offset: 0x000F6190
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

		// Token: 0x06001CEA RID: 7402 RVA: 0x000F7DC4 File Offset: 0x000F61C4
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

		// Token: 0x06001CEB RID: 7403 RVA: 0x000F7E28 File Offset: 0x000F6228
		public override void Arrived(Caravan caravan)
		{
			this.peaceTalks.Notify_CaravanArrived(caravan);
		}

		// Token: 0x06001CEC RID: 7404 RVA: 0x000F7E37 File Offset: 0x000F6237
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<PeaceTalks>(ref this.peaceTalks, "peaceTalks", false);
		}

		// Token: 0x06001CED RID: 7405 RVA: 0x000F7E54 File Offset: 0x000F6254
		public static FloatMenuAcceptanceReport CanVisit(Caravan caravan, PeaceTalks peaceTalks)
		{
			return peaceTalks != null && peaceTalks.Spawned;
		}

		// Token: 0x06001CEE RID: 7406 RVA: 0x000F7E80 File Offset: 0x000F6280
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, PeaceTalks peaceTalks)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitPeaceTalks>(() => CaravanArrivalAction_VisitPeaceTalks.CanVisit(caravan, peaceTalks), () => new CaravanArrivalAction_VisitPeaceTalks(peaceTalks), "VisitPeaceTalks".Translate(new object[]
			{
				peaceTalks.Label
			}), caravan, peaceTalks.Tile, peaceTalks);
		}

		// Token: 0x04001154 RID: 4436
		private PeaceTalks peaceTalks;
	}
}
