using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public abstract class Designator_ZoneAddStockpile : Designator_ZoneAdd
	{
		protected StorageSettingsPreset preset;

		protected override string NewZoneLabel
		{
			get
			{
				return this.preset.PresetName();
			}
		}

		public Designator_ZoneAddStockpile()
		{
			base.zoneTypeToPlace = typeof(Zone_Stockpile);
		}

		protected override Zone MakeNewZone()
		{
			return new Zone_Stockpile(this.preset, Find.VisibleMap.zoneManager);
		}

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
							goto IL_0073;
					}
					result = true;
				}
			}
			goto IL_009f;
			IL_009f:
			return result;
			IL_0073:
			result = false;
			goto IL_009f;
		}

		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Stockpiles, KnowledgeAmount.Total);
		}
	}
}
