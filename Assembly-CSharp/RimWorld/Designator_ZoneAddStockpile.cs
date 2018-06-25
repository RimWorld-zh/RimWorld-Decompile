using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007E8 RID: 2024
	public abstract class Designator_ZoneAddStockpile : Designator_ZoneAdd
	{
		// Token: 0x040017B5 RID: 6069
		protected StorageSettingsPreset preset;

		// Token: 0x06002D01 RID: 11521 RVA: 0x0017AB6B File Offset: 0x00178F6B
		public Designator_ZoneAddStockpile()
		{
			this.zoneTypeToPlace = typeof(Zone_Stockpile);
		}

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x06002D02 RID: 11522 RVA: 0x0017AB84 File Offset: 0x00178F84
		protected override string NewZoneLabel
		{
			get
			{
				return this.preset.PresetName();
			}
		}

		// Token: 0x06002D03 RID: 11523 RVA: 0x0017ABA4 File Offset: 0x00178FA4
		protected override Zone MakeNewZone()
		{
			return new Zone_Stockpile(this.preset, Find.CurrentMap.zoneManager);
		}

		// Token: 0x06002D04 RID: 11524 RVA: 0x0017ABD0 File Offset: 0x00178FD0
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

		// Token: 0x06002D05 RID: 11525 RVA: 0x0017AC7D File Offset: 0x0017907D
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Stockpiles, KnowledgeAmount.Total);
		}
	}
}
