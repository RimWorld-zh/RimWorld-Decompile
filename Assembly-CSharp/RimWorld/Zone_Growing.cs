using System;
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
			string text = string.Empty;
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
			if (list.NullOrEmpty())
			{
				return "NoGrowingPeriod".Translate();
			}
			if (list.Count == 12)
			{
				return "GrowYearRound".Translate();
			}
			return "PeriodDays".Translate(list.Count * 5) + " (" + QuadrumUtility.QuadrumsRangeLabel(list) + ")";
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			yield return (Gizmo)PlantToGrowSettableUtility.SetPlantToGrowCommand(this);
			yield return (Gizmo)new Command_Toggle
			{
				defaultLabel = "CommandAllowSow".Translate(),
				defaultDesc = "CommandAllowSowDesc".Translate(),
				hotKey = KeyBindingDefOf.CommandItemForbid,
				icon = TexCommand.Forbidden,
				isActive = (Func<bool>)(() => ((_003CGetGizmos_003Ec__IteratorBB)/*Error near IL_0126: stateMachine*/)._003C_003Ef__this.allowSow),
				toggleAction = (Action)delegate
				{
					((_003CGetGizmos_003Ec__IteratorBB)/*Error near IL_013d: stateMachine*/)._003C_003Ef__this.allowSow = !((_003CGetGizmos_003Ec__IteratorBB)/*Error near IL_013d: stateMachine*/)._003C_003Ef__this.allowSow;
				}
			};
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

		virtual Map get_Map()
		{
			return base.Map;
		}

		Map IPlantToGrowSettable.get_Map()
		{
			//ILSpy generated this explicit interface implementation from .override directive in get_Map
			return this.get_Map();
		}
	}
}
