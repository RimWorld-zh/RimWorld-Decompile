using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet
{
	public class CaravanArrivalAction_VisitPeaceTalks : CaravanArrivalAction
	{
		private PeaceTalks peaceTalks;

		public CaravanArrivalAction_VisitPeaceTalks()
		{
		}

		public CaravanArrivalAction_VisitPeaceTalks(PeaceTalks peaceTalks)
		{
			this.peaceTalks = peaceTalks;
		}

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

		public override FloatMenuAcceptanceReport StillValid(Caravan caravan, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(caravan, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.peaceTalks != null && this.peaceTalks.Tile != destinationTile)
			{
				return false;
			}
			return CaravanArrivalAction_VisitPeaceTalks.CanVisit(caravan, this.peaceTalks);
		}

		public override void Arrived(Caravan caravan)
		{
			this.peaceTalks.Notify_CaravanArrived(caravan);
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<PeaceTalks>(ref this.peaceTalks, "peaceTalks", false);
		}

		public static FloatMenuAcceptanceReport CanVisit(Caravan caravan, PeaceTalks peaceTalks)
		{
			return peaceTalks != null && peaceTalks.Spawned;
		}

		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, PeaceTalks peaceTalks)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitPeaceTalks>(() => CaravanArrivalAction_VisitPeaceTalks.CanVisit(caravan, peaceTalks), () => new CaravanArrivalAction_VisitPeaceTalks(peaceTalks), "VisitPeaceTalks".Translate(new object[]
			{
				peaceTalks.Label
			}), caravan, peaceTalks.Tile, peaceTalks);
		}

		[CompilerGenerated]
		private sealed class <GetFloatMenuOptions>c__AnonStorey0
		{
			internal Caravan caravan;

			internal PeaceTalks peaceTalks;

			public <GetFloatMenuOptions>c__AnonStorey0()
			{
			}

			internal FloatMenuAcceptanceReport <>m__0()
			{
				return CaravanArrivalAction_VisitPeaceTalks.CanVisit(this.caravan, this.peaceTalks);
			}

			internal CaravanArrivalAction_VisitPeaceTalks <>m__1()
			{
				return new CaravanArrivalAction_VisitPeaceTalks(this.peaceTalks);
			}
		}
	}
}
