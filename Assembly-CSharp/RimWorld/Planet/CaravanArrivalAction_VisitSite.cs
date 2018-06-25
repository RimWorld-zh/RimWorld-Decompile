using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005D1 RID: 1489
	public class CaravanArrivalAction_VisitSite : CaravanArrivalAction
	{
		// Token: 0x0400115A RID: 4442
		private Site site;

		// Token: 0x06001CFB RID: 7419 RVA: 0x000F8557 File Offset: 0x000F6957
		public CaravanArrivalAction_VisitSite()
		{
		}

		// Token: 0x06001CFC RID: 7420 RVA: 0x000F8560 File Offset: 0x000F6960
		public CaravanArrivalAction_VisitSite(Site site)
		{
			this.site = site;
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06001CFD RID: 7421 RVA: 0x000F8570 File Offset: 0x000F6970
		public override string Label
		{
			get
			{
				return this.site.ApproachOrderString;
			}
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06001CFE RID: 7422 RVA: 0x000F8590 File Offset: 0x000F6990
		public override string ReportString
		{
			get
			{
				return this.site.ApproachingReportString;
			}
		}

		// Token: 0x06001CFF RID: 7423 RVA: 0x000F85B0 File Offset: 0x000F69B0
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

		// Token: 0x06001D00 RID: 7424 RVA: 0x000F8614 File Offset: 0x000F6A14
		public override void Arrived(Caravan caravan)
		{
			this.site.core.Worker.VisitAction(caravan, this.site);
		}

		// Token: 0x06001D01 RID: 7425 RVA: 0x000F8633 File Offset: 0x000F6A33
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Site>(ref this.site, "site", false);
		}

		// Token: 0x06001D02 RID: 7426 RVA: 0x000F8650 File Offset: 0x000F6A50
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

		// Token: 0x06001D03 RID: 7427 RVA: 0x000F86C4 File Offset: 0x000F6AC4
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Site site)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitSite>(() => CaravanArrivalAction_VisitSite.CanVisit(caravan, site), () => new CaravanArrivalAction_VisitSite(site), site.ApproachOrderString, caravan, site.Tile, site);
		}
	}
}
