using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007E8 RID: 2024
	public abstract class Designator_ZoneAddStockpile : Designator_ZoneAdd
	{
		// Token: 0x040017B9 RID: 6073
		protected StorageSettingsPreset preset;

		// Token: 0x06002D00 RID: 11520 RVA: 0x0017ADCF File Offset: 0x001791CF
		public Designator_ZoneAddStockpile()
		{
			this.zoneTypeToPlace = typeof(Zone_Stockpile);
		}

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x06002D01 RID: 11521 RVA: 0x0017ADE8 File Offset: 0x001791E8
		protected override string NewZoneLabel
		{
			get
			{
				return this.preset.PresetName();
			}
		}

		// Token: 0x06002D02 RID: 11522 RVA: 0x0017AE08 File Offset: 0x00179208
		protected override Zone MakeNewZone()
		{
			return new Zone_Stockpile(this.preset, Find.CurrentMap.zoneManager);
		}

		// Token: 0x06002D03 RID: 11523 RVA: 0x0017AE34 File Offset: 0x00179234
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport acceptanceReport = base.CanDesignateCell(c);
			AcceptanceReport result;
			if (!acceptanceReport.Accepted)
			{
				result = acceptanceReport;
			}
			else
			{
				TerrainDef terrain = c.GetTerrain(base.Map);
				if (terrain.passability == Traversability.Impassable)
				{
					result = false;
				}
				else
				{
					List<Thing> list = base.Map.thingGrid.ThingsListAt(c);
					for (int i = 0; i < list.Count; i++)
					{
						if (!list[i].def.CanOverlapZones)
						{
							return false;
						}
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06002D04 RID: 11524 RVA: 0x0017AEE1 File Offset: 0x001792E1
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Stockpiles, KnowledgeAmount.Total);
		}
	}
}
