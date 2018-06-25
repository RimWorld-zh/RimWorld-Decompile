using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000569 RID: 1385
	public abstract class FeatureWorker_Cluster : FeatureWorker
	{
		// Token: 0x04000F4C RID: 3916
		private List<int> roots = new List<int>();

		// Token: 0x04000F4D RID: 3917
		private HashSet<int> rootsSet = new HashSet<int>();

		// Token: 0x04000F4E RID: 3918
		private List<int> rootsWithAreaInBetween = new List<int>();

		// Token: 0x04000F4F RID: 3919
		private HashSet<int> rootsWithAreaInBetweenSet = new HashSet<int>();

		// Token: 0x04000F50 RID: 3920
		private List<int> currentGroup = new List<int>();

		// Token: 0x04000F51 RID: 3921
		private List<int> currentGroupMembers = new List<int>();

		// Token: 0x04000F52 RID: 3922
		private HashSet<int> visitedValidGroupIDs = new HashSet<int>();

		// Token: 0x04000F53 RID: 3923
		private static List<int> tmpGroup = new List<int>();

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06001A2D RID: 6701 RVA: 0x000E2F18 File Offset: 0x000E1318
		protected virtual int MinRootGroupsInCluster
		{
			get
			{
				return this.def.minRootGroupsInCluster;
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06001A2E RID: 6702 RVA: 0x000E2F38 File Offset: 0x000E1338
		protected virtual int MinRootGroupSize
		{
			get
			{
				return this.def.minRootGroupSize;
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06001A2F RID: 6703 RVA: 0x000E2F58 File Offset: 0x000E1358
		protected virtual int MaxRootGroupSize
		{
			get
			{
				return this.def.maxRootGroupSize;
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06001A30 RID: 6704 RVA: 0x000E2F78 File Offset: 0x000E1378
		protected virtual int MinOverallSize
		{
			get
			{
				return this.def.minSize;
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06001A31 RID: 6705 RVA: 0x000E2F98 File Offset: 0x000E1398
		protected virtual int MaxOverallSize
		{
			get
			{
				return this.def.maxSize;
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06001A32 RID: 6706 RVA: 0x000E2FB8 File Offset: 0x000E13B8
		protected virtual int MaxSpaceBetweenRootGroups
		{
			get
			{
				return this.def.maxSpaceBetweenRootGroups;
			}
		}

		// Token: 0x06001A33 RID: 6707
		protected abstract bool IsRoot(int tile);

		// Token: 0x06001A34 RID: 6708 RVA: 0x000E2FD8 File Offset: 0x000E13D8
		protected virtual bool CanTraverse(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = false;
			return true;
		}

		// Token: 0x06001A35 RID: 6709 RVA: 0x000E2FF4 File Offset: 0x000E13F4
		protected virtual bool IsMember(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = false;
			return Find.WorldGrid[tile].feature == null;
		}

		// Token: 0x06001A36 RID: 6710 RVA: 0x000E301F File Offset: 0x000E141F
		public override void GenerateWhereAppropriate()
		{
			this.CalculateRootTiles();
			this.CalculateRootsWithAreaInBetween();
			this.CalculateContiguousGroups();
		}

		// Token: 0x06001A37 RID: 6711 RVA: 0x000E3034 File Offset: 0x000E1434
		private void CalculateRootTiles()
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

		// Token: 0x06001A38 RID: 6712 RVA: 0x000E30A0 File Offset: 0x000E14A0
		private void CalculateRootsWithAreaInBetween()
		{
			this.rootsWithAreaInBetween.Clear();
			this.rootsWithAreaInBetween.AddRange(this.roots);
			GenPlanetMorphology.Close(this.rootsWithAreaInBetween, this.MaxSpaceBetweenRootGroups);
			this.rootsWithAreaInBetweenSet.Clear();
			this.rootsWithAreaInBetweenSet.AddRange(this.rootsWithAreaInBetween);
		}

		// Token: 0x06001A39 RID: 6713 RVA: 0x000E30F8 File Offset: 0x000E14F8
		private void CalculateContiguousGroups()
		{
			WorldFloodFiller worldFloodFiller = Find.WorldFloodFiller;
			WorldGrid worldGrid = Find.WorldGrid;
			int minRootGroupSize = this.MinRootGroupSize;
			int maxRootGroupSize = this.MaxRootGroupSize;
			int minOverallSize = this.MinOverallSize;
			int maxOverallSize = this.MaxOverallSize;
			int minRootGroupsInCluster = this.MinRootGroupsInCluster;
			FeatureWorker.ClearVisited();
			FeatureWorker.ClearGroupSizes();
			FeatureWorker.ClearGroupIDs();
			for (int i = 0; i < this.roots.Count; i++)
			{
				int num = this.roots[i];
				if (!FeatureWorker.visited[num])
				{
					bool anyMember = false;
					FeatureWorker_Cluster.tmpGroup.Clear();
					worldFloodFiller.FloodFill(num, (int x) => this.rootsSet.Contains(x), delegate(int x)
					{
						FeatureWorker.visited[x] = true;
						FeatureWorker_Cluster.tmpGroup.Add(x);
						bool flag2;
						if (!anyMember && this.IsMember(x, out flag2))
						{
							anyMember = true;
						}
					}, int.MaxValue, null);
					for (int j = 0; j < FeatureWorker_Cluster.tmpGroup.Count; j++)
					{
						FeatureWorker.groupSize[FeatureWorker_Cluster.tmpGroup[j]] = FeatureWorker_Cluster.tmpGroup.Count;
						if (anyMember)
						{
							FeatureWorker.groupID[FeatureWorker_Cluster.tmpGroup[j]] = i + 1;
						}
					}
				}
			}
			FeatureWorker.ClearVisited();
			for (int k = 0; k < this.roots.Count; k++)
			{
				int num2 = this.roots[k];
				if (!FeatureWorker.visited[num2])
				{
					if (FeatureWorker.groupSize[num2] >= minRootGroupSize && FeatureWorker.groupSize[num2] <= maxRootGroupSize)
					{
						if (FeatureWorker.groupSize[num2] <= maxOverallSize)
						{
							this.currentGroup.Clear();
							this.visitedValidGroupIDs.Clear();
							worldFloodFiller.FloodFill(num2, delegate(int x)
							{
								bool flag2;
								return this.rootsWithAreaInBetweenSet.Contains(x) && this.CanTraverse(x, out flag2) && (!flag2 || !this.rootsSet.Contains(x) || (FeatureWorker.groupSize[x] >= minRootGroupSize && FeatureWorker.groupSize[x] <= maxRootGroupSize));
							}, delegate(int x)
							{
								FeatureWorker.visited[x] = true;
								this.currentGroup.Add(x);
								if (FeatureWorker.groupID[x] != 0 && FeatureWorker.groupSize[x] >= minRootGroupSize && FeatureWorker.groupSize[x] <= maxRootGroupSize)
								{
									this.visitedValidGroupIDs.Add(FeatureWorker.groupID[x]);
								}
							}, int.MaxValue, null);
							if (this.currentGroup.Count >= minOverallSize && this.currentGroup.Count <= maxOverallSize)
							{
								if (this.visitedValidGroupIDs.Count >= minRootGroupsInCluster)
								{
									if (this.def.canTouchWorldEdge || !this.currentGroup.Any((int x) => worldGrid.IsOnEdge(x)))
									{
										this.currentGroupMembers.Clear();
										for (int l = 0; l < this.currentGroup.Count; l++)
										{
											int num3 = this.currentGroup[l];
											bool flag;
											if (this.IsMember(num3, out flag))
											{
												if (!flag || !this.rootsSet.Contains(num3) || (FeatureWorker.groupSize[num3] >= minRootGroupSize && FeatureWorker.groupSize[num3] <= maxRootGroupSize))
												{
													this.currentGroupMembers.Add(this.currentGroup[l]);
												}
											}
										}
										if (this.currentGroupMembers.Count >= minOverallSize)
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
}
