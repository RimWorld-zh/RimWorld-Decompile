using System;
using RimWorld;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x02000C41 RID: 3137
	public sealed class MapDrawer
	{
		// Token: 0x0600450A RID: 17674 RVA: 0x00244938 File Offset: 0x00242D38
		public MapDrawer(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000AE5 RID: 2789
		// (get) Token: 0x0600450B RID: 17675 RVA: 0x00244948 File Offset: 0x00242D48
		private IntVec2 SectionCount
		{
			get
			{
				return new IntVec2
				{
					x = Mathf.CeilToInt((float)this.map.Size.x / 17f),
					z = Mathf.CeilToInt((float)this.map.Size.z / 17f)
				};
			}
		}

		// Token: 0x17000AE6 RID: 2790
		// (get) Token: 0x0600450C RID: 17676 RVA: 0x002449B4 File Offset: 0x00242DB4
		private CellRect VisibleSections
		{
			get
			{
				CellRect currentViewRect = Find.CameraDriver.CurrentViewRect;
				CellRect sunShadowsViewRect = this.GetSunShadowsViewRect(currentViewRect);
				sunShadowsViewRect.ClipInsideMap(this.map);
				IntVec2 intVec = this.SectionCoordsAt(sunShadowsViewRect.BottomLeft);
				IntVec2 intVec2 = this.SectionCoordsAt(sunShadowsViewRect.TopRight);
				CellRect result;
				if (intVec2.x < intVec.x || intVec2.z < intVec.z)
				{
					result = CellRect.Empty;
				}
				else
				{
					result = CellRect.FromLimits(intVec.x, intVec.z, intVec2.x, intVec2.z);
				}
				return result;
			}
		}

		// Token: 0x0600450D RID: 17677 RVA: 0x00244A5C File Offset: 0x00242E5C
		public void MapMeshDirty(IntVec3 loc, MapMeshFlag dirtyFlags)
		{
			bool regenAdjacentCells = (dirtyFlags & (MapMeshFlag.FogOfWar | MapMeshFlag.Buildings)) != MapMeshFlag.None;
			bool regenAdjacentSections = (dirtyFlags & MapMeshFlag.GroundGlow) != MapMeshFlag.None;
			this.MapMeshDirty(loc, dirtyFlags, regenAdjacentCells, regenAdjacentSections);
		}

		// Token: 0x0600450E RID: 17678 RVA: 0x00244A88 File Offset: 0x00242E88
		public void MapMeshDirty(IntVec3 loc, MapMeshFlag dirtyFlags, bool regenAdjacentCells, bool regenAdjacentSections)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				Section section = this.SectionAt(loc);
				section.dirtyFlags |= dirtyFlags;
				if (regenAdjacentCells)
				{
					for (int i = 0; i < 8; i++)
					{
						IntVec3 intVec = loc + GenAdj.AdjacentCells[i];
						if (intVec.InBounds(this.map))
						{
							this.SectionAt(intVec).dirtyFlags |= dirtyFlags;
						}
					}
				}
				if (regenAdjacentSections)
				{
					IntVec2 a = this.SectionCoordsAt(loc);
					for (int j = 0; j < 8; j++)
					{
						IntVec3 intVec2 = GenAdj.AdjacentCells[j];
						IntVec2 intVec3 = a + new IntVec2(intVec2.x, intVec2.z);
						IntVec2 sectionCount = this.SectionCount;
						if (intVec3.x >= 0 && intVec3.z >= 0 && intVec3.x <= sectionCount.x - 1 && intVec3.z <= sectionCount.z - 1)
						{
							Section section2 = this.sections[intVec3.x, intVec3.z];
							section2.dirtyFlags |= dirtyFlags;
						}
					}
				}
			}
		}

		// Token: 0x0600450F RID: 17679 RVA: 0x00244BEC File Offset: 0x00242FEC
		public void MapMeshDrawerUpdate_First()
		{
			CellRect visibleSections = this.VisibleSections;
			bool flag = false;
			CellRect.CellRectIterator iterator = visibleSections.GetIterator();
			while (!iterator.Done())
			{
				IntVec3 intVec = iterator.Current;
				Section sect = this.sections[intVec.x, intVec.z];
				if (this.TryUpdateSection(sect))
				{
					flag = true;
				}
				iterator.MoveNext();
			}
			if (!flag)
			{
				for (int i = 0; i < this.SectionCount.x; i++)
				{
					for (int j = 0; j < this.SectionCount.z; j++)
					{
						if (this.TryUpdateSection(this.sections[i, j]))
						{
							return;
						}
					}
				}
			}
		}

		// Token: 0x06004510 RID: 17680 RVA: 0x00244CCC File Offset: 0x002430CC
		private bool TryUpdateSection(Section sect)
		{
			bool result;
			if (sect.dirtyFlags == MapMeshFlag.None)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < MapMeshFlagUtility.allFlags.Count; i++)
				{
					MapMeshFlag mapMeshFlag = MapMeshFlagUtility.allFlags[i];
					if ((sect.dirtyFlags & mapMeshFlag) != MapMeshFlag.None)
					{
						sect.RegenerateLayers(mapMeshFlag);
					}
				}
				sect.dirtyFlags = MapMeshFlag.None;
				result = true;
			}
			return result;
		}

		// Token: 0x06004511 RID: 17681 RVA: 0x00244D38 File Offset: 0x00243138
		public void DrawMapMesh()
		{
			CellRect currentViewRect = Find.CameraDriver.CurrentViewRect;
			currentViewRect.minX -= 17;
			currentViewRect.minZ -= 17;
			CellRect visibleSections = this.VisibleSections;
			Profiler.BeginSample("Draw sections");
			CellRect.CellRectIterator iterator = visibleSections.GetIterator();
			while (!iterator.Done())
			{
				IntVec3 intVec = iterator.Current;
				Section section = this.sections[intVec.x, intVec.z];
				section.DrawSection(!currentViewRect.Contains(section.botLeft));
				iterator.MoveNext();
			}
			Profiler.EndSample();
		}

		// Token: 0x06004512 RID: 17682 RVA: 0x00244DE4 File Offset: 0x002431E4
		private IntVec2 SectionCoordsAt(IntVec3 loc)
		{
			return new IntVec2(Mathf.FloorToInt((float)(loc.x / 17)), Mathf.FloorToInt((float)(loc.z / 17)));
		}

		// Token: 0x06004513 RID: 17683 RVA: 0x00244E20 File Offset: 0x00243220
		public Section SectionAt(IntVec3 loc)
		{
			IntVec2 intVec = this.SectionCoordsAt(loc);
			return this.sections[intVec.x, intVec.z];
		}

		// Token: 0x06004514 RID: 17684 RVA: 0x00244E58 File Offset: 0x00243258
		public void RegenerateEverythingNow()
		{
			if (this.sections == null)
			{
				this.sections = new Section[this.SectionCount.x, this.SectionCount.z];
			}
			for (int i = 0; i < this.SectionCount.x; i++)
			{
				for (int j = 0; j < this.SectionCount.z; j++)
				{
					if (this.sections[i, j] == null)
					{
						this.sections[i, j] = new Section(new IntVec3(i, 0, j), this.map);
					}
					this.sections[i, j].RegenerateAllLayers();
				}
			}
		}

		// Token: 0x06004515 RID: 17685 RVA: 0x00244F20 File Offset: 0x00243320
		public void WholeMapChanged(MapMeshFlag change)
		{
			for (int i = 0; i < this.SectionCount.x; i++)
			{
				for (int j = 0; j < this.SectionCount.z; j++)
				{
					this.sections[i, j].dirtyFlags |= change;
				}
			}
		}

		// Token: 0x06004516 RID: 17686 RVA: 0x00244F8C File Offset: 0x0024338C
		private CellRect GetSunShadowsViewRect(CellRect rect)
		{
			GenCelestial.LightInfo lightSourceInfo = GenCelestial.GetLightSourceInfo(this.map, GenCelestial.LightType.Shadow);
			if (lightSourceInfo.vector.x < 0f)
			{
				rect.maxX -= Mathf.FloorToInt(lightSourceInfo.vector.x);
			}
			else
			{
				rect.minX -= Mathf.CeilToInt(lightSourceInfo.vector.x);
			}
			if (lightSourceInfo.vector.y < 0f)
			{
				rect.maxZ -= Mathf.FloorToInt(lightSourceInfo.vector.y);
			}
			else
			{
				rect.minZ -= Mathf.CeilToInt(lightSourceInfo.vector.y);
			}
			return rect;
		}

		// Token: 0x04002F38 RID: 12088
		private Map map;

		// Token: 0x04002F39 RID: 12089
		private Section[,] sections;
	}
}
