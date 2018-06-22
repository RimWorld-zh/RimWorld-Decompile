using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000454 RID: 1108
	public class Zone_Growing : Zone, IPlantToGrowSettable
	{
		// Token: 0x06001364 RID: 4964 RVA: 0x000A80BE File Offset: 0x000A64BE
		public Zone_Growing()
		{
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x000A80D9 File Offset: 0x000A64D9
		public Zone_Growing(ZoneManager zoneManager) : base("GrowingZone".Translate(), zoneManager)
		{
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06001366 RID: 4966 RVA: 0x000A8100 File Offset: 0x000A6500
		public override bool IsMultiselectable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06001367 RID: 4967 RVA: 0x000A8118 File Offset: 0x000A6518
		protected override Color NextZoneColor
		{
			get
			{
				return ZoneColorUtility.NextGrowingZoneColor();
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06001368 RID: 4968 RVA: 0x000A8134 File Offset: 0x000A6534
		IEnumerable<IntVec3> IPlantToGrowSettable.Cells
		{
			get
			{
				return base.Cells;
			}
		}

		// Token: 0x06001369 RID: 4969 RVA: 0x000A814F File Offset: 0x000A654F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.plantDefToGrow, "plantDefToGrow");
			Scribe_Values.Look<bool>(ref this.allowSow, "allowSow", true, false);
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x000A817C File Offset: 0x000A657C
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

		// Token: 0x0600136B RID: 4971 RVA: 0x000A8244 File Offset: 0x000A6644
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

		// Token: 0x0600136C RID: 4972 RVA: 0x000A82E8 File Offset: 0x000A66E8
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

		// Token: 0x0600136D RID: 4973 RVA: 0x000A8314 File Offset: 0x000A6714
		public override IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			yield return DesignatorUtility.FindAllowedDesignator<Designator_ZoneAdd_Growing_Expand>();
			yield break;
		}

		// Token: 0x0600136E RID: 4974 RVA: 0x000A8338 File Offset: 0x000A6738
		public ThingDef GetPlantDefToGrow()
		{
			return this.plantDefToGrow;
		}

		// Token: 0x0600136F RID: 4975 RVA: 0x000A8353 File Offset: 0x000A6753
		public void SetPlantDefToGrow(ThingDef plantDef)
		{
			this.plantDefToGrow = plantDef;
		}

		// Token: 0x06001370 RID: 4976 RVA: 0x000A8360 File Offset: 0x000A6760
		public bool CanAcceptSowNow()
		{
			return true;
		}

		// Token: 0x04000BD2 RID: 3026
		private ThingDef plantDefToGrow = ThingDefOf.Plant_Potato;

		// Token: 0x04000BD3 RID: 3027
		public bool allowSow = true;
	}
}
