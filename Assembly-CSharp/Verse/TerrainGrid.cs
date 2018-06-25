using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000C26 RID: 3110
	public sealed class TerrainGrid : IExposable
	{
		// Token: 0x04002E6B RID: 11883
		private Map map;

		// Token: 0x04002E6C RID: 11884
		public TerrainDef[] topGrid;

		// Token: 0x04002E6D RID: 11885
		private TerrainDef[] underGrid;

		// Token: 0x06004446 RID: 17478 RVA: 0x0023ED40 File Offset: 0x0023D140
		public TerrainGrid(Map map)
		{
			this.map = map;
			this.ResetGrids();
		}

		// Token: 0x06004447 RID: 17479 RVA: 0x0023ED56 File Offset: 0x0023D156
		public void ResetGrids()
		{
			this.topGrid = new TerrainDef[this.map.cellIndices.NumGridCells];
			this.underGrid = new TerrainDef[this.map.cellIndices.NumGridCells];
		}

		// Token: 0x06004448 RID: 17480 RVA: 0x0023ED90 File Offset: 0x0023D190
		public TerrainDef TerrainAt(int ind)
		{
			return this.topGrid[ind];
		}

		// Token: 0x06004449 RID: 17481 RVA: 0x0023EDB0 File Offset: 0x0023D1B0
		public TerrainDef TerrainAt(IntVec3 c)
		{
			return this.topGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x0600444A RID: 17482 RVA: 0x0023EDE0 File Offset: 0x0023D1E0
		public TerrainDef UnderTerrainAt(int ind)
		{
			return this.underGrid[ind];
		}

		// Token: 0x0600444B RID: 17483 RVA: 0x0023EE00 File Offset: 0x0023D200
		public TerrainDef UnderTerrainAt(IntVec3 c)
		{
			return this.underGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x0600444C RID: 17484 RVA: 0x0023EE30 File Offset: 0x0023D230
		public void SetTerrain(IntVec3 c, TerrainDef newTerr)
		{
			if (newTerr == null)
			{
				Log.Error("Tried to set terrain at " + c + " to null.", false);
			}
			else
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					Designation designation = this.map.designationManager.DesignationAt(c, DesignationDefOf.SmoothFloor);
					if (designation != null)
					{
						designation.Delete();
					}
				}
				int num = this.map.cellIndices.CellToIndex(c);
				if (newTerr.layerable)
				{
					if (this.underGrid[num] == null)
					{
						if (this.topGrid[num].passability != Traversability.Impassable)
						{
							this.underGrid[num] = this.topGrid[num];
						}
						else
						{
							this.underGrid[num] = TerrainDefOf.Sand;
						}
					}
				}
				else
				{
					this.underGrid[num] = null;
				}
				this.topGrid[num] = newTerr;
				this.DoTerrainChangedEffects(c);
			}
		}

		// Token: 0x0600444D RID: 17485 RVA: 0x0023EF1C File Offset: 0x0023D31C
		public void SetUnderTerrain(IntVec3 c, TerrainDef newTerr)
		{
			if (!c.InBounds(this.map))
			{
				Log.Error("Tried to set terrain out of bounds at " + c, false);
			}
			else
			{
				int num = this.map.cellIndices.CellToIndex(c);
				this.underGrid[num] = newTerr;
			}
		}

		// Token: 0x0600444E RID: 17486 RVA: 0x0023EF74 File Offset: 0x0023D374
		public void RemoveTopLayer(IntVec3 c, bool doLeavings = true)
		{
			int num = this.map.cellIndices.CellToIndex(c);
			if (doLeavings)
			{
				GenLeaving.DoLeavingsFor(this.topGrid[num], c, this.map);
			}
			if (this.underGrid[num] != null)
			{
				this.topGrid[num] = this.underGrid[num];
				this.underGrid[num] = null;
				this.DoTerrainChangedEffects(c);
			}
		}

		// Token: 0x0600444F RID: 17487 RVA: 0x0023EFE0 File Offset: 0x0023D3E0
		public bool CanRemoveTopLayerAt(IntVec3 c)
		{
			int num = this.map.cellIndices.CellToIndex(c);
			return this.topGrid[num].Removable && this.underGrid[num] != null;
		}

		// Token: 0x06004450 RID: 17488 RVA: 0x0023F02C File Offset: 0x0023D42C
		private void DoTerrainChangedEffects(IntVec3 c)
		{
			this.map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Terrain, true, false);
			List<Thing> thingList = c.GetThingList(this.map);
			for (int i = thingList.Count - 1; i >= 0; i--)
			{
				if (thingList[i].def.category == ThingCategory.Plant && this.map.fertilityGrid.FertilityAt(c) < thingList[i].def.plant.fertilityMin)
				{
					thingList[i].Destroy(DestroyMode.Vanish);
				}
				else if (thingList[i].def.category == ThingCategory.Filth && !this.TerrainAt(c).acceptFilth)
				{
					thingList[i].Destroy(DestroyMode.Vanish);
				}
			}
			this.map.pathGrid.RecalculatePerceivedPathCostAt(c);
			Region regionAt_NoRebuild_InvalidAllowed = this.map.regionGrid.GetRegionAt_NoRebuild_InvalidAllowed(c);
			if (regionAt_NoRebuild_InvalidAllowed != null && regionAt_NoRebuild_InvalidAllowed.Room != null)
			{
				regionAt_NoRebuild_InvalidAllowed.Room.Notify_TerrainChanged();
			}
		}

		// Token: 0x06004451 RID: 17489 RVA: 0x0023F147 File Offset: 0x0023D547
		public void ExposeData()
		{
			this.ExposeTerrainGrid(this.topGrid, "topGrid");
			this.ExposeTerrainGrid(this.underGrid, "underGrid");
		}

		// Token: 0x06004452 RID: 17490 RVA: 0x0023F16C File Offset: 0x0023D56C
		public void Notify_TerrainBurned(IntVec3 c)
		{
			TerrainDef terrain = c.GetTerrain(this.map);
			this.Notify_TerrainDestroyed(c);
			if (terrain.burnedDef != null)
			{
				this.SetTerrain(c, terrain.burnedDef);
			}
		}

		// Token: 0x06004453 RID: 17491 RVA: 0x0023F1A8 File Offset: 0x0023D5A8
		public void Notify_TerrainDestroyed(IntVec3 c)
		{
			TerrainDef terrainDef = this.TerrainAt(c);
			this.RemoveTopLayer(c, false);
			if (terrainDef.destroyBuildingsOnDestroyed)
			{
				Building firstBuilding = c.GetFirstBuilding(this.map);
				if (firstBuilding != null)
				{
					firstBuilding.Kill(null, null);
				}
			}
			if (terrainDef.destroyEffectWater != null && this.TerrainAt(c) != null && this.TerrainAt(c).IsWater)
			{
				Effecter effecter = terrainDef.destroyEffectWater.Spawn();
				effecter.Trigger(new TargetInfo(c, this.map, false), new TargetInfo(c, this.map, false));
				effecter.Cleanup();
			}
			else if (terrainDef.destroyEffect != null)
			{
				Effecter effecter2 = terrainDef.destroyEffect.Spawn();
				effecter2.Trigger(new TargetInfo(c, this.map, false), new TargetInfo(c, this.map, false));
				effecter2.Cleanup();
			}
		}

		// Token: 0x06004454 RID: 17492 RVA: 0x0023F29C File Offset: 0x0023D69C
		private void ExposeTerrainGrid(TerrainDef[] grid, string label)
		{
			Dictionary<ushort, TerrainDef> terrainDefsByShortHash = new Dictionary<ushort, TerrainDef>();
			foreach (TerrainDef terrainDef in DefDatabase<TerrainDef>.AllDefs)
			{
				terrainDefsByShortHash.Add(terrainDef.shortHash, terrainDef);
			}
			Func<IntVec3, ushort> shortReader = delegate(IntVec3 c)
			{
				TerrainDef terrainDef2 = grid[this.map.cellIndices.CellToIndex(c)];
				return (terrainDef2 == null) ? 0 : terrainDef2.shortHash;
			};
			Action<IntVec3, ushort> shortWriter = delegate(IntVec3 c, ushort val)
			{
				TerrainDef terrainDef2 = terrainDefsByShortHash.TryGetValue(val, null);
				if (terrainDef2 == null && val != 0)
				{
					TerrainDef terrainDef3 = BackCompatibility.BackCompatibleTerrainWithShortHash(val);
					if (terrainDef3 == null)
					{
						Log.Error(string.Concat(new object[]
						{
							"Did not find terrain def with short hash ",
							val,
							" for cell ",
							c,
							"."
						}), false);
						terrainDef3 = TerrainDefOf.Soil;
					}
					terrainDef2 = terrainDef3;
					terrainDefsByShortHash.Add(val, terrainDef3);
				}
				grid[this.map.cellIndices.CellToIndex(c)] = terrainDef2;
			};
			MapExposeUtility.ExposeUshort(this.map, shortReader, shortWriter, label);
		}

		// Token: 0x06004455 RID: 17493 RVA: 0x0023F34C File Offset: 0x0023D74C
		public string DebugStringAt(IntVec3 c)
		{
			string result;
			if (c.InBounds(this.map))
			{
				TerrainDef terrain = c.GetTerrain(this.map);
				TerrainDef terrainDef = this.underGrid[this.map.cellIndices.CellToIndex(c)];
				result = "top: " + ((terrain == null) ? "null" : terrain.defName) + ", under=" + ((terrainDef == null) ? "null" : terrainDef.defName);
			}
			else
			{
				result = "out of bounds";
			}
			return result;
		}
	}
}
