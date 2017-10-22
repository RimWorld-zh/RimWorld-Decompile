#define ENABLE_PROFILER
using RimWorld;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	public sealed class MapDrawer
	{
		private Map map;

		private Section[,] sections;

		private IntVec2 SectionCount
		{
			get
			{
				IntVec2 result = default(IntVec2);
				IntVec3 size = this.map.Size;
				result.x = Mathf.CeilToInt((float)((float)size.x / 17.0));
				IntVec3 size2 = this.map.Size;
				result.z = Mathf.CeilToInt((float)((float)size2.z / 17.0));
				return result;
			}
		}

		private CellRect VisibleSections
		{
			get
			{
				CellRect currentViewRect = Find.CameraDriver.CurrentViewRect;
				CellRect sunShadowsViewRect = this.GetSunShadowsViewRect(currentViewRect);
				sunShadowsViewRect.ClipInsideMap(this.map);
				IntVec2 intVec = this.SectionCoordsAt(sunShadowsViewRect.BottomLeft);
				IntVec2 intVec2 = this.SectionCoordsAt(sunShadowsViewRect.TopRight);
				return (intVec2.x >= intVec.x && intVec2.z >= intVec.z) ? CellRect.FromLimits(intVec.x, intVec.z, intVec2.x, intVec2.z) : CellRect.Empty;
			}
		}

		public MapDrawer(Map map)
		{
			this.map = map;
		}

		public void MapMeshDirty(IntVec3 loc, MapMeshFlag dirtyFlags)
		{
			bool regenAdjacentCells = ((int)dirtyFlags & 6) != 0;
			bool regenAdjacentSections = ((int)dirtyFlags & 8) != 0;
			this.MapMeshDirty(loc, dirtyFlags, regenAdjacentCells, regenAdjacentSections);
		}

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

		public void MapMeshDrawerUpdate_First()
		{
			CellRect visibleSections = this.VisibleSections;
			bool flag = false;
			CellRect.CellRectIterator iterator = visibleSections.GetIterator();
			while (!iterator.Done())
			{
				IntVec3 current = iterator.Current;
				Section sect = this.sections[current.x, current.z];
				if (this.TryUpdateSection(sect))
				{
					flag = true;
				}
				iterator.MoveNext();
			}
			if (!flag)
			{
				int num = 0;
				while (true)
				{
					int num2 = num;
					IntVec2 sectionCount = this.SectionCount;
					if (num2 < sectionCount.x)
					{
						int num3 = 0;
						while (true)
						{
							int num4 = num3;
							IntVec2 sectionCount2 = this.SectionCount;
							if (num4 < sectionCount2.z)
							{
								if (!this.TryUpdateSection(this.sections[num, num3]))
								{
									num3++;
									continue;
								}
								return;
							}
							break;
						}
						num++;
						continue;
					}
					break;
				}
			}
		}

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
					if ((sect.dirtyFlags & mapMeshFlag) != 0)
					{
						sect.RegenerateLayers(mapMeshFlag);
					}
				}
				sect.dirtyFlags = MapMeshFlag.None;
				result = true;
			}
			return result;
		}

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
				IntVec3 current = iterator.Current;
				Section section = this.sections[current.x, current.z];
				section.DrawSection(!currentViewRect.Contains(section.botLeft));
				iterator.MoveNext();
			}
			Profiler.EndSample();
		}

		private IntVec2 SectionCoordsAt(IntVec3 loc)
		{
			return new IntVec2(Mathf.FloorToInt((float)(loc.x / 17)), Mathf.FloorToInt((float)(loc.z / 17)));
		}

		public Section SectionAt(IntVec3 loc)
		{
			IntVec2 intVec = this.SectionCoordsAt(loc);
			return this.sections[intVec.x, intVec.z];
		}

		public void RegenerateEverythingNow()
		{
			if (this.sections == null)
			{
				IntVec2 sectionCount = this.SectionCount;
				int x = sectionCount.x;
				IntVec2 sectionCount2 = this.SectionCount;
				this.sections = new Section[x, sectionCount2.z];
			}
			int num = 0;
			while (true)
			{
				int num2 = num;
				IntVec2 sectionCount3 = this.SectionCount;
				if (num2 < sectionCount3.x)
				{
					int num3 = 0;
					while (true)
					{
						int num4 = num3;
						IntVec2 sectionCount4 = this.SectionCount;
						if (num4 < sectionCount4.z)
						{
							if (this.sections[num, num3] == null)
							{
								this.sections[num, num3] = new Section(new IntVec3(num, 0, num3), this.map);
							}
							this.sections[num, num3].RegenerateAllLayers();
							num3++;
							continue;
						}
						break;
					}
					num++;
					continue;
				}
				break;
			}
		}

		public void WholeMapChanged(MapMeshFlag change)
		{
			int num = 0;
			while (true)
			{
				int num2 = num;
				IntVec2 sectionCount = this.SectionCount;
				if (num2 < sectionCount.x)
				{
					int num3 = 0;
					while (true)
					{
						int num4 = num3;
						IntVec2 sectionCount2 = this.SectionCount;
						if (num4 < sectionCount2.z)
						{
							this.sections[num, num3].dirtyFlags |= change;
							num3++;
							continue;
						}
						break;
					}
					num++;
					continue;
				}
				break;
			}
		}

		private CellRect GetSunShadowsViewRect(CellRect rect)
		{
			GenCelestial.LightInfo lightSourceInfo = GenCelestial.GetLightSourceInfo(this.map, GenCelestial.LightType.Shadow);
			if (lightSourceInfo.vector.x < 0.0)
			{
				rect.maxX -= Mathf.FloorToInt(lightSourceInfo.vector.x);
			}
			else
			{
				rect.minX -= Mathf.CeilToInt(lightSourceInfo.vector.x);
			}
			if (lightSourceInfo.vector.y < 0.0)
			{
				rect.maxZ -= Mathf.FloorToInt(lightSourceInfo.vector.y);
			}
			else
			{
				rect.minZ -= Mathf.CeilToInt(lightSourceInfo.vector.y);
			}
			return rect;
		}
	}
}
