using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007EA RID: 2026
	public abstract class Designator_ZoneAddStockpile : Designator_ZoneAdd
	{
		// Token: 0x06002D02 RID: 11522 RVA: 0x0017A7AF File Offset: 0x00178BAF
		public Designator_ZoneAddStockpile()
		{
			this.zoneTypeToPlace = typeof(Zone_Stockpile);
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x06002D03 RID: 11523 RVA: 0x0017A7C8 File Offset: 0x00178BC8
		protected override string NewZoneLabel
		{
			get
			{
				return this.preset.PresetName();
			}
		}

		// Token: 0x06002D04 RID: 11524 RVA: 0x0017A7E8 File Offset: 0x00178BE8
		protected override Zone MakeNewZone()
		{
			return new Zone_Stockpile(this.preset, Find.CurrentMap.zoneManager);
		}

		// Token: 0x06002D05 RID: 11525 RVA: 0x0017A814 File Offset: 0x00178C14
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

		// Token: 0x06002D06 RID: 11526 RVA: 0x0017A8C1 File Offset: 0x00178CC1
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Stockpiles, KnowledgeAmount.Total);
		}

		// Token: 0x040017B7 RID: 6071
		protected StorageSettingsPreset preset;
	}
}
