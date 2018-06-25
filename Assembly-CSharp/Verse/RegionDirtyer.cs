using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C8B RID: 3211
	public class RegionDirtyer
	{
		// Token: 0x04003002 RID: 12290
		private Map map;

		// Token: 0x04003003 RID: 12291
		private List<IntVec3> dirtyCells = new List<IntVec3>();

		// Token: 0x04003004 RID: 12292
		private List<Region> regionsToDirty = new List<Region>();

		// Token: 0x0600466E RID: 18030 RVA: 0x00252C96 File Offset: 0x00251096
		public RegionDirtyer(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000B17 RID: 2839
		// (get) Token: 0x0600466F RID: 18031 RVA: 0x00252CBC File Offset: 0x002510BC
		public bool AnyDirty
		{
			get
			{
				return this.dirtyCells.Count > 0;
			}
		}

		// Token: 0x17000B18 RID: 2840
		// (get) Token: 0x06004670 RID: 18032 RVA: 0x00252CE0 File Offset: 0x002510E0
		public List<IntVec3> DirtyCells
		{
			get
			{
				return this.dirtyCells;
			}
		}

		// Token: 0x06004671 RID: 18033 RVA: 0x00252CFC File Offset: 0x002510FC
		internal void Notify_WalkabilityChanged(IntVec3 c)
		{
			this.regionsToDirty.Clear();
			for (int i = 0; i < 9; i++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCellsAndInside[i];
				if (c2.InBounds(this.map))
				{
					Region regionAt_NoRebuild_InvalidAllowed = this.map.regionGrid.GetRegionAt_NoRebuild_InvalidAllowed(c2);
					if (regionAt_NoRebuild_InvalidAllowed != null && regionAt_NoRebuild_InvalidAllowed.valid)
					{
						this.map.temperatureCache.TryCacheRegionTempInfo(c, regionAt_NoRebuild_InvalidAllowed);
						this.regionsToDirty.Add(regionAt_NoRebuild_InvalidAllowed);
					}
				}
			}
			for (int j = 0; j < this.regionsToDirty.Count; j++)
			{
				this.SetRegionDirty(this.regionsToDirty[j], true);
			}
			this.regionsToDirty.Clear();
			if (c.Walkable(this.map) && !this.dirtyCells.Contains(c))
			{
				this.dirtyCells.Add(c);
			}
		}

		// Token: 0x06004672 RID: 18034 RVA: 0x00252E04 File Offset: 0x00251204
		internal void Notify_ThingAffectingRegionsSpawned(Thing b)
		{
			this.regionsToDirty.Clear();
			CellRect.CellRectIterator iterator = b.OccupiedRect().ExpandedBy(1).ClipInsideMap(b.Map).GetIterator();
			while (!iterator.Done())
			{
				IntVec3 c = iterator.Current;
				Region validRegionAt_NoRebuild = b.Map.regionGrid.GetValidRegionAt_NoRebuild(c);
				if (validRegionAt_NoRebuild != null)
				{
					b.Map.temperatureCache.TryCacheRegionTempInfo(c, validRegionAt_NoRebuild);
					this.regionsToDirty.Add(validRegionAt_NoRebuild);
				}
				iterator.MoveNext();
			}
			for (int i = 0; i < this.regionsToDirty.Count; i++)
			{
				this.SetRegionDirty(this.regionsToDirty[i], true);
			}
			this.regionsToDirty.Clear();
		}

		// Token: 0x06004673 RID: 18035 RVA: 0x00252EE8 File Offset: 0x002512E8
		internal void Notify_ThingAffectingRegionsDespawned(Thing b)
		{
			this.regionsToDirty.Clear();
			Region validRegionAt_NoRebuild = this.map.regionGrid.GetValidRegionAt_NoRebuild(b.Position);
			if (validRegionAt_NoRebuild != null)
			{
				this.map.temperatureCache.TryCacheRegionTempInfo(b.Position, validRegionAt_NoRebuild);
				this.regionsToDirty.Add(validRegionAt_NoRebuild);
			}
			foreach (IntVec3 c in GenAdj.CellsAdjacent8Way(b))
			{
				if (c.InBounds(this.map))
				{
					Region validRegionAt_NoRebuild2 = this.map.regionGrid.GetValidRegionAt_NoRebuild(c);
					if (validRegionAt_NoRebuild2 != null)
					{
						this.map.temperatureCache.TryCacheRegionTempInfo(c, validRegionAt_NoRebuild2);
						this.regionsToDirty.Add(validRegionAt_NoRebuild2);
					}
				}
			}
			for (int i = 0; i < this.regionsToDirty.Count; i++)
			{
				this.SetRegionDirty(this.regionsToDirty[i], true);
			}
			this.regionsToDirty.Clear();
			if (b.def.size.x == 1 && b.def.size.z == 1)
			{
				this.dirtyCells.Add(b.Position);
			}
			else
			{
				CellRect cellRect = b.OccupiedRect();
				for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
				{
					for (int k = cellRect.minX; k <= cellRect.maxX; k++)
					{
						IntVec3 item = new IntVec3(k, 0, j);
						this.dirtyCells.Add(item);
					}
				}
			}
		}

		// Token: 0x06004674 RID: 18036 RVA: 0x002530CC File Offset: 0x002514CC
		internal void SetAllClean()
		{
			for (int i = 0; i < this.dirtyCells.Count; i++)
			{
				this.map.temperatureCache.ResetCachedCellInfo(this.dirtyCells[i]);
			}
			this.dirtyCells.Clear();
		}

		// Token: 0x06004675 RID: 18037 RVA: 0x00253120 File Offset: 0x00251520
		private void SetRegionDirty(Region reg, bool addCellsToDirtyCells = true)
		{
			if (reg.valid)
			{
				reg.valid = false;
				reg.Room = null;
				for (int i = 0; i < reg.links.Count; i++)
				{
					reg.links[i].Deregister(reg);
				}
				reg.links.Clear();
				if (addCellsToDirtyCells)
				{
					foreach (IntVec3 intVec in reg.Cells)
					{
						this.dirtyCells.Add(intVec);
						if (DebugViewSettings.drawRegionDirties)
						{
							this.map.debugDrawer.FlashCell(intVec, 0f, null, 50);
						}
					}
				}
			}
		}

		// Token: 0x06004676 RID: 18038 RVA: 0x00253208 File Offset: 0x00251608
		internal void SetAllDirty()
		{
			this.dirtyCells.Clear();
			foreach (IntVec3 item in this.map)
			{
				this.dirtyCells.Add(item);
			}
			foreach (Region reg in this.map.regionGrid.AllRegions_NoRebuild_InvalidAllowed)
			{
				this.SetRegionDirty(reg, false);
			}
		}
	}
}
