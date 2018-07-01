using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet
{
	public class CaravanArrivalAction_VisitSite : CaravanArrivalAction
	{
		private Site site;

		public CaravanArrivalAction_VisitSite()
		{
		}

		public CaravanArrivalAction_VisitSite(Site site)
		{
			this.site = site;
		}

		public override string Label
		{
			get
			{
				return this.site.ApproachOrderString;
			}
		}

		public override string ReportString
		{
			get
			{
				return this.site.ApproachingReportString;
			}
		}

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

		public override void Arrived(Caravan caravan)
		{
			this.site.core.def.Worker.VisitAction(caravan, this.site);
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Site>(ref this.site, "site", false);
		}

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

		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Site site)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitSite>(() => CaravanArrivalAction_VisitSite.CanVisit(caravan, site), () => new CaravanArrivalAction_VisitSite(site), site.ApproachOrderString, caravan, site.Tile, site);
		}

		[CompilerGenerated]
		private sealed class <GetFloatMenuOptions>c__AnonStorey0
		{
			internal Caravan caravan;

			internal Site site;

			public <GetFloatMenuOptions>c__AnonStorey0()
			{
			}

			internal FloatMenuAcceptanceReport <>m__0()
			{
				return CaravanArrivalAction_VisitSite.CanVisit(this.caravan, this.site);
			}

			internal CaravanArrivalAction_VisitSite <>m__1()
			{
				return new CaravanArrivalAction_VisitSite(this.site);
			}
		}
	}
}
