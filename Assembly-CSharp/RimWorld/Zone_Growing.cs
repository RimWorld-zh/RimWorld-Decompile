using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000456 RID: 1110
	public class Zone_Growing : Zone, IPlantToGrowSettable
	{
		// Token: 0x04000BD5 RID: 3029
		private ThingDef plantDefToGrow = ThingDefOf.Plant_Potato;

		// Token: 0x04000BD6 RID: 3030
		public bool allowSow = true;

		// Token: 0x06001367 RID: 4967 RVA: 0x000A840E File Offset: 0x000A680E
		public Zone_Growing()
		{
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x000A8429 File Offset: 0x000A6829
		public Zone_Growing(ZoneManager zoneManager) : base("GrowingZone".Translate(), zoneManager)
		{
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06001369 RID: 4969 RVA: 0x000A8450 File Offset: 0x000A6850
		public override bool IsMultiselectable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x0600136A RID: 4970 RVA: 0x000A8468 File Offset: 0x000A6868
		protected override Color NextZoneColor
		{
			get
			{
				return ZoneColorUtility.NextGrowingZoneColor();
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x0600136B RID: 4971 RVA: 0x000A8484 File Offset: 0x000A6884
		IEnumerable<IntVec3> IPlantToGrowSettable.Cells
		{
			get
			{
				return base.Cells;
			}
		}

		// Token: 0x0600136C RID: 4972 RVA: 0x000A849F File Offset: 0x000A689F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.plantDefToGrow, "plantDefToGrow");
			Scribe_Values.Look<bool>(ref this.allowSow, "allowSow", true, false);
		}

		// Token: 0x0600136D RID: 4973 RVA: 0x000A84CC File Offset: 0x000A68CC
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

		// Token: 0x0600136E RID: 4974 RVA: 0x000A8594 File Offset: 0x000A6994
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

		// Token: 0x0600136F RID: 4975 RVA: 0x000A8638 File Offset: 0x000A6A38
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

		// Token: 0x06001370 RID: 4976 RVA: 0x000A8664 File Offset: 0x000A6A64
		public override IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			yield return DesignatorUtility.FindAllowedDesignator<Designator_ZoneAdd_Growing_Expand>();
			yield break;
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x000A8688 File Offset: 0x000A6A88
		public ThingDef GetPlantDefToGrow()
		{
			return this.plantDefToGrow;
		}

		// Token: 0x06001372 RID: 4978 RVA: 0x000A86A3 File Offset: 0x000A6AA3
		public void SetPlantDefToGrow(ThingDef plantDef)
		{
			this.plantDefToGrow = plantDef;
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x000A86B0 File Offset: 0x000A6AB0
		public bool CanAcceptSowNow()
		{
			return true;
		}
	}
}
