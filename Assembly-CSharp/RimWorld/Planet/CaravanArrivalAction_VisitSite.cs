using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005CF RID: 1487
	public class CaravanArrivalAction_VisitSite : CaravanArrivalAction
	{
		// Token: 0x04001156 RID: 4438
		private Site site;

		// Token: 0x06001CF8 RID: 7416 RVA: 0x000F819F File Offset: 0x000F659F
		public CaravanArrivalAction_VisitSite()
		{
		}

		// Token: 0x06001CF9 RID: 7417 RVA: 0x000F81A8 File Offset: 0x000F65A8
		public CaravanArrivalAction_VisitSite(Site site)
		{
			this.site = site;
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06001CFA RID: 7418 RVA: 0x000F81B8 File Offset: 0x000F65B8
		public override string Label
		{
			get
			{
				return this.site.ApproachOrderString;
			}
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06001CFB RID: 7419 RVA: 0x000F81D8 File Offset: 0x000F65D8
		public override string ReportString
		{
			get
			{
				return this.site.ApproachingReportString;
			}
		}

		// Token: 0x06001CFC RID: 7420 RVA: 0x000F81F8 File Offset: 0x000F65F8
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

		// Token: 0x06001CFD RID: 7421 RVA: 0x000F825C File Offset: 0x000F665C
		public override void Arrived(Caravan caravan)
		{
			this.site.core.Worker.VisitAction(caravan, this.site);
		}

		// Token: 0x06001CFE RID: 7422 RVA: 0x000F827B File Offset: 0x000F667B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Site>(ref this.site, "site", false);
		}

		// Token: 0x06001CFF RID: 7423 RVA: 0x000F8298 File Offset: 0x000F6698
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

		// Token: 0x06001D00 RID: 7424 RVA: 0x000F830C File Offset: 0x000F670C
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Site site)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitSite>(() => CaravanArrivalAction_VisitSite.CanVisit(caravan, site), () => new CaravanArrivalAction_VisitSite(site), site.ApproachOrderString, caravan, site.Tile, site);
		}
	}
}
