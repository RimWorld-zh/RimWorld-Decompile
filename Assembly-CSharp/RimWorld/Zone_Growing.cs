using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Zone_Growing : Zone, IPlantToGrowSettable
	{
		private ThingDef plantDefToGrow = ThingDefOf.PlantPotato;

		public bool allowSow = true;

		IEnumerable<IntVec3> IPlantToGrowSettable.Cells
		{
			get
			{
				return base.Cells;
			}
		}

		public override bool IsMultiselectable
		{
			get
			{
				return true;
			}
		}

		protected override Color NextZoneColor
		{
			get
			{
				return ZoneColorUtility.NextGrowingZoneColor();
			}
		}

		public Zone_Growing()
		{
		}

		public Zone_Growing(ZoneManager zoneManager) : base("GrowingZone".Translate(), zoneManager)
		{
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.plantDefToGrow, "plantDefToGrow");
			Scribe_Values.Look<bool>(ref this.allowSow, "allowSow", true, false);
		}

		public override string GetInspectString()
		{
			string text = "";
			if (!base.Cells.NullOrEmpty())
			{
				IntVec3 c = base.Cells.First();
				if (c.UsesOutdoorTemperature(base.Map))
				{
					string text2 = text;
					text = text2 + "OutdoorGrowingPeriod".Translate() + ": " + Zone_Growing.GrowingQuadrumsDescription(base.Map.Tile) + "\n";
				}
				text = ((!GenPlant.GrowthSeasonNow(c, base.Map)) ? (text + "CannotGrowBadSeasonTemperature".Translate()) : (text + "GrowSeasonHereNow".Translate()));
			}
			return text;
		}

		public static string GrowingQuadrumsDescription(int tile)
		{
			List<Twelfth> list = GenTemperature.TwelfthsInAverageTemperatureRange(tile, 10f, 42f);
			return (!list.NullOrEmpty()) ? ((list.Count != 12) ? ("PeriodDays".Translate(list.Count * 5 + "/" + 60) + " (" + QuadrumUtility.QuadrumsRangeLabel(list) + ")") : "GrowYearRound".Translate()) : "NoGrowingPeriod".Translate();
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			using (IEnumerator<Gizmo> enumerator = this._003CGetGizmos_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Gizmo g = enumerator.Current;
					yield return g;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield return (Gizmo)PlantToGrowSettableUtility.SetPlantToGrowCommand(this);
			/*Error: Unable to find new state assignment for yield return*/;
			IL_018f:
			/*Error near IL_0190: Unexpected return in MoveNext()*/;
		}

		public ThingDef GetPlantDefToGrow()
		{
			return this.plantDefToGrow;
		}

		public void SetPlantDefToGrow(ThingDef plantDef)
		{
			this.plantDefToGrow = plantDef;
		}

		public bool CanAcceptSowNow()
		{
			return true;
		}
	}
}
