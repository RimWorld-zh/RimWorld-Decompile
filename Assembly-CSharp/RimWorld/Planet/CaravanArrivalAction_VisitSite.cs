using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005D3 RID: 1491
	public class CaravanArrivalAction_VisitSite : CaravanArrivalAction
	{
		// Token: 0x06001CFF RID: 7423 RVA: 0x000F80D3 File Offset: 0x000F64D3
		public CaravanArrivalAction_VisitSite()
		{
		}

		// Token: 0x06001D00 RID: 7424 RVA: 0x000F80DC File Offset: 0x000F64DC
		public CaravanArrivalAction_VisitSite(Site site)
		{
			this.site = site;
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06001D01 RID: 7425 RVA: 0x000F80EC File Offset: 0x000F64EC
		public override string Label
		{
			get
			{
				return this.site.ApproachOrderString;
			}
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06001D02 RID: 7426 RVA: 0x000F810C File Offset: 0x000F650C
		public override string ReportString
		{
			get
			{
				return this.site.ApproachingReportString;
			}
		}

		// Token: 0x06001D03 RID: 7427 RVA: 0x000F812C File Offset: 0x000F652C
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

		// Token: 0x06001D04 RID: 7428 RVA: 0x000F8190 File Offset: 0x000F6590
		public override void Arrived(Caravan caravan)
		{
			this.site.core.Worker.VisitAction(caravan, this.site);
		}

		// Token: 0x06001D05 RID: 7429 RVA: 0x000F81AF File Offset: 0x000F65AF
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Site>(ref this.site, "site", false);
		}

		// Token: 0x06001D06 RID: 7430 RVA: 0x000F81CC File Offset: 0x000F65CC
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

		// Token: 0x06001D07 RID: 7431 RVA: 0x000F8240 File Offset: 0x000F6640
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Site site)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitSite>(() => CaravanArrivalAction_VisitSite.CanVisit(caravan, site), () => new CaravanArrivalAction_VisitSite(site), site.ApproachOrderString, caravan, site.Tile, site);
		}

		// Token: 0x04001159 RID: 4441
		private Site site;
	}
}
