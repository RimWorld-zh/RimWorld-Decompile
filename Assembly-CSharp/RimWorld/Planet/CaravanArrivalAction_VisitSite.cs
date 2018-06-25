using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005D1 RID: 1489
	public class CaravanArrivalAction_VisitSite : CaravanArrivalAction
	{
		// Token: 0x04001156 RID: 4438
		private Site site;

		// Token: 0x06001CFC RID: 7420 RVA: 0x000F82EF File Offset: 0x000F66EF
		public CaravanArrivalAction_VisitSite()
		{
		}

		// Token: 0x06001CFD RID: 7421 RVA: 0x000F82F8 File Offset: 0x000F66F8
		public CaravanArrivalAction_VisitSite(Site site)
		{
			this.site = site;
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06001CFE RID: 7422 RVA: 0x000F8308 File Offset: 0x000F6708
		public override string Label
		{
			get
			{
				return this.site.ApproachOrderString;
			}
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06001CFF RID: 7423 RVA: 0x000F8328 File Offset: 0x000F6728
		public override string ReportString
		{
			get
			{
				return this.site.ApproachingReportString;
			}
		}

		// Token: 0x06001D00 RID: 7424 RVA: 0x000F8348 File Offset: 0x000F6748
		public override FloatMenuAcceptanceReport StillValid(Caravan caravan, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(caravan, destinationTile);
			FloatMenuAcceptanceReport result;
			if (!floatMenuAcceptanceReport)
			{
				result = floatMenuAcceptanceReport;
			}
			else if (this.site != null && this.site.Tile != destinationTile)
			{
				result = false;
			}
			else
			{
				result = CaravanArrivalAction_VisitSite.CanVisit(caravan, this.site);
			}
			return result;
		}

		// Token: 0x06001D01 RID: 7425 RVA: 0x000F83AC File Offset: 0x000F67AC
		public override void Arrived(Caravan caravan)
		{
			this.site.core.Worker.VisitAction(caravan, this.site);
		}

		// Token: 0x06001D02 RID: 7426 RVA: 0x000F83CB File Offset: 0x000F67CB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Site>(ref this.site, "site", false);
		}

		// Token: 0x06001D03 RID: 7427 RVA: 0x000F83E8 File Offset: 0x000F67E8
		public static FloatMenuAcceptanceReport CanVisit(Caravan caravan, Site site)
		{
			FloatMenuAcceptanceReport result;
			if (site == null || !site.Spawned)
			{
				result = false;
			}
			else if (site.EnterCooldownBlocksEntering())
			{
				result = FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(new object[]
				{
					site.EnterCooldownDaysLeft().ToString("0.#")
				}));
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06001D04 RID: 7428 RVA: 0x000F845C File Offset: 0x000F685C
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Site site)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitSite>(() => CaravanArrivalAction_VisitSite.CanVisit(caravan, site), () => new CaravanArrivalAction_VisitSite(site), site.ApproachOrderString, caravan, site.Tile, site);
		}
	}
}
