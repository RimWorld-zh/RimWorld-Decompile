using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000458 RID: 1112
	public class Zone_Growing : Zone, IPlantToGrowSettable
	{
		// Token: 0x0600136D RID: 4973 RVA: 0x000A80A2 File Offset: 0x000A64A2
		public Zone_Growing()
		{
		}

		// Token: 0x0600136E RID: 4974 RVA: 0x000A80BD File Offset: 0x000A64BD
		public Zone_Growing(ZoneManager zoneManager) : base("GrowingZone".Translate(), zoneManager)
		{
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x0600136F RID: 4975 RVA: 0x000A80E4 File Offset: 0x000A64E4
		public override bool IsMultiselectable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06001370 RID: 4976 RVA: 0x000A80FC File Offset: 0x000A64FC
		protected override Color NextZoneColor
		{
			get
			{
				return ZoneColorUtility.NextGrowingZoneColor();
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06001371 RID: 4977 RVA: 0x000A8118 File Offset: 0x000A6518
		IEnumerable<IntVec3> IPlantToGrowSettable.Cells
		{
			get
			{
				return base.Cells;
			}
		}

		// Token: 0x06001372 RID: 4978 RVA: 0x000A8133 File Offset: 0x000A6533
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.plantDefToGrow, "plantDefToGrow");
			Scribe_Values.Look<bool>(ref this.allowSow, "allowSow", true, false);
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x000A8160 File Offset: 0x000A6560
		public override string GetInspectString()
		{
			string text = "";
			if (!base.Cells.NullOrEmpty<IntVec3>())
			{
				IntVec3 c = base.Cells.First<IntVec3>();
				if (c.UsesOutdoorTemperature(base.Map))
				{
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						"OutdoorGrowingPeriod".Translate(),
						": ",
						Zone_Growing.GrowingQuadrumsDescription(base.Map.Tile),
						"\n"
					});
				}
				if (GenPlant.GrowthSeasonNow(c, base.Map, true))
				{
					text += "GrowSeasonHereNow".Translate();
				}
				else
				{
					text += "CannotGrowBadSeasonTemperature".Translate();
				}
			}
			return text;
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x000A8228 File Offset: 0x000A6628
		public static string GrowingQuadrumsDescription(int tile)
		{
			List<Twelfth> list = GenTemperature.TwelfthsInAverageTemperatureRange(tile, 10f, 42f);
			string result;
			if (list.NullOrEmpty<Twelfth>())
			{
				result = "NoGrowingPeriod".Translate();
			}
			else if (list.Count == 12)
			{
				result = "GrowYearRound".Translate();
			}
			else
			{
				result = "PeriodDays".Translate(new object[]
				{
					list.Count * 5 + "/" + 60
				}) + " (" + QuadrumUtility.QuadrumsRangeLabel(list) + ")";
			}
			return result;
		}

		// Token: 0x06001375 RID: 4981 RVA: 0x000A82CC File Offset: 0x000A66CC
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo g in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return g;
			}
			yield return PlantToGrowSettableUtility.SetPlantToGrowCommand(this);
			yield return new Command_Toggle
			{
				defaultLabel = "CommandAllowSow".Translate(),
				defaultDesc = "CommandAllowSowDesc".Translate(),
				hotKey = KeyBindingDefOf.Command_ItemForbid,
				icon = TexCommand.Forbidden,
				isActive = (() => this.allowSow),
				toggleAction = delegate()
				{
					this.allowSow = !this.allowSow;
				}
			};
			yield break;
		}

		// Token: 0x06001376 RID: 4982 RVA: 0x000A82F8 File Offset: 0x000A66F8
		public override IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			yield return DesignatorUtility.FindAllowedDesignator<Designator_ZoneAdd_Growing_Expand>();
			yield break;
		}

		// Token: 0x06001377 RID: 4983 RVA: 0x000A831C File Offset: 0x000A671C
		public ThingDef GetPlantDefToGrow()
		{
			return this.plantDefToGrow;
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x000A8337 File Offset: 0x000A6737
		public void SetPlantDefToGrow(ThingDef plantDef)
		{
			this.plantDefToGrow = plantDef;
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x000A8344 File Offset: 0x000A6744
		public bool CanAcceptSowNow()
		{
			return true;
		}

		// Token: 0x04000BD5 RID: 3029
		private ThingDef plantDefToGrow = ThingDefOf.Plant_Potato;

		// Token: 0x04000BD6 RID: 3030
		public bool allowSow = true;
	}
}
