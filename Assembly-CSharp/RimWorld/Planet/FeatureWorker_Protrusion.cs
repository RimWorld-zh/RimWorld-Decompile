using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000571 RID: 1393
	public abstract class FeatureWorker_Protrusion : FeatureWorker
	{
		// Token: 0x04000F5E RID: 3934
		private List<int> roots = new List<int>();

		// Token: 0x04000F5F RID: 3935
		private HashSet<int> rootsSet = new HashSet<int>();

		// Token: 0x04000F60 RID: 3936
		private List<int> rootsWithoutSmallPassages = new List<int>();

		// Token: 0x04000F61 RID: 3937
		private HashSet<int> rootsWithoutSmallPassagesSet = new HashSet<int>();

		// Token: 0x04000F62 RID: 3938
		private List<int> currentGroup = new List<int>();

		// Token: 0x04000F63 RID: 3939
		private List<int> currentGroupMembers = new List<int>();

		// Token: 0x04000F64 RID: 3940
		private static List<int> tmpGroup = new List<int>();

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06001A5A RID: 6746 RVA: 0x000E4364 File Offset: 0x000E2764
		protected virtual int MinSize
		{
			get
			{
				return this.def.minSize;
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06001A5B RID: 6747 RVA: 0x000E4384 File Offset: 0x000E2784
		protected virtual int MaxSize
		{
			get
			{
				return this.def.maxSize;
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06001A5C RID: 6748 RVA: 0x000E43A4 File Offset: 0x000E27A4
		protected virtual int MaxPassageWidth
		{
			get
			{
				return this.def.maxPassageWidth;
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06001A5D RID: 6749 RVA: 0x000E43C4 File Offset: 0x000E27C4
		protected virtual float MaxPctOfWholeArea
		{
			get
			{
				return this.def.maxPctOfWholeArea;
			}
		}

		// Token: 0x06001A5E RID: 6750
		protected abstract bool IsRoot(int tile);

		// Token: 0x06001A5F RID: 6751 RVA: 0x000E43E4 File Offset: 0x000E27E4
		protected virtual bool IsMember(int tile)
		{
			return Find.WorldGrid[tile].feature == null;
		}

		// Token: 0x06001A60 RID: 6752 RVA: 0x000E440C File Offset: 0x000E280C
		public override void GenerateWhereAppropriate()
		{
			this.CalculateRoots();
			this.CalculateRootsWithoutSmallPassages();
			this.CalculateContiguousGroups();
		}

		// Token: 0x06001A61 RID: 6753 RVA: 0x000E4424 File Offset: 0x000E2824
		private void CalculateRoots()
		{
			this.roots.Clear();
			int tilesCount = Find.WorldGrid.TilesCount;
			for (int i = 0; i < tilesCount; i++)
			{
				if (this.IsRoot(i))
				{
					this.roots.Add(i);
				}
			}
			this.rootsSet.Clear();
			this.rootsSet.AddRange(this.roots);
		}

		// Token: 0x06001A62 RID: 6754 RVA: 0x000E4490 File Offset: 0x000E2890
		private void CalculateRootsWithoutSmallPassages()
		{
			this.rootsWithoutSmallPassages.Clear();
			this.rootsWithoutSmallPassages.AddRange(this.roots);
			GenPlanetMorphology.Open(this.rootsWithoutSmallPassages, this.MaxPassageWidth);
			this.rootsWithoutSmallPassagesSet.Clear();
			this.rootsWithoutSmallPassagesSet.AddRange(this.rootsWithoutSmallPassages);
		}

		// Token: 0x06001A63 RID: 6755 RVA: 0x000E44E8 File Offset: 0x000E28E8
		private void CalculateContiguousGroups()
		{
			WorldGrid worldGrid = Find.WorldGrid;
			WorldFloodFiller worldFloodFiller = Find.WorldFloodFiller;
			int minSize = this.MinSize;
			int maxSize = this.MaxSize;
			float maxPctOfWholeArea = this.MaxPctOfWholeArea;
			int maxPassageWidth = this.MaxPassageWidth;
			FeatureWorker.ClearVisited();
			FeatureWorker.ClearGroupSizes();
			for (int i = 0; i < this.roots.Count; i++)
			{
				int num = this.roots[i];
				if (!FeatureWorker.visited[num])
				{
					FeatureWorker_Protrusion.tmpGroup.Clear();
					worldFloodFiller.FloodFill(num, (int x) => this.rootsSet.Contains(x), delegate(int x)
					{
						FeatureWorker.visited[x] = true;
						FeatureWorker_Protrusion.tmpGroup.Add(x);
					}, int.MaxValue, null);
					for (int j = 0; j < FeatureWorker_Protrusion.tmpGroup.Count; j++)
					{
						FeatureWorker.groupSize[FeatureWorker_Protrusion.tmpGroup[j]] = FeatureWorker_Protrusion.tmpGroup.Count;
					}
				}
			}
			FeatureWorker.ClearVisited();
			for (int k = 0; k < this.rootsWithoutSmallPassages.Count; k++)
			{
				int num2 = this.rootsWithoutSmallPassages[k];
				if (!FeatureWorker.visited[num2])
				{
					this.currentGroup.Clear();
					worldFloodFiller.FloodFill(num2, (int x) => this.rootsWithoutSmallPassagesSet.Contains(x), delegate(int x)
					{
						FeatureWorker.visited[x] = true;
						this.currentGroup.Add(x);
					}, int.MaxValue, null);
					if (this.currentGroup.Count >= minSize)
					{
						GenPlanetMorphology.Dilate(this.currentGroup, maxPassageWidth * 2, (int x) => this.rootsSet.Contains(x));
						if (this.currentGroup.Count <= maxSize)
						{
							float num3 = (float)this.currentGroup.Count / (float)FeatureWorker.groupSize[num2];
							if (num3 <= maxPctOfWholeArea)
							{
								if (this.def.canTouchWorldEdge || !this.currentGroup.Any((int x) => worldGrid.IsOnEdge(x)))
								{
									this.currentGroupMembers.Clear();
									for (int l = 0; l < this.currentGroup.Count; l++)
									{
										if (this.IsMember(this.currentGroup[l]))
										{
											this.currentGroupMembers.Add(this.currentGroup[l]);
										}
									}
									if (this.currentGroupMembers.Count >= minSize)
									{
										if (this.currentGroup.Any((int x) => worldGrid[x].feature == null))
										{
											this.currentGroup.RemoveAll((int x) => worldGrid[x].feature != null);
										}
										base.AddFeature(this.currentGroupMembers, this.currentGroup);
									}
								}
							}
						}
					}
				}
			}
		}
	}
}
