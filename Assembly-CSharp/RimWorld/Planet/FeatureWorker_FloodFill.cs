using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200056B RID: 1387
	public abstract class FeatureWorker_FloodFill : FeatureWorker
	{
		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06001A42 RID: 6722 RVA: 0x000E38D8 File Offset: 0x000E1CD8
		protected virtual int MinSize
		{
			get
			{
				return this.def.minSize;
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06001A43 RID: 6723 RVA: 0x000E38F8 File Offset: 0x000E1CF8
		protected virtual int MaxSize
		{
			get
			{
				return this.def.maxSize;
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06001A44 RID: 6724 RVA: 0x000E3918 File Offset: 0x000E1D18
		protected virtual int MaxPossiblyAllowedSizeToTake
		{
			get
			{
				return this.def.maxPossiblyAllowedSizeToTake;
			}
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06001A45 RID: 6725 RVA: 0x000E3938 File Offset: 0x000E1D38
		protected virtual float MaxPossiblyAllowedSizePctOfMeToTake
		{
			get
			{
				return this.def.maxPossiblyAllowedSizePctOfMeToTake;
			}
		}

		// Token: 0x06001A46 RID: 6726
		protected abstract bool IsRoot(int tile);

		// Token: 0x06001A47 RID: 6727 RVA: 0x000E3958 File Offset: 0x000E1D58
		protected virtual bool IsPossiblyAllowed(int tile)
		{
			return false;
		}

		// Token: 0x06001A48 RID: 6728 RVA: 0x000E3970 File Offset: 0x000E1D70
		protected virtual bool IsMember(int tile)
		{
			return Find.WorldGrid[tile].feature == null;
		}

		// Token: 0x06001A49 RID: 6729 RVA: 0x000E3998 File Offset: 0x000E1D98
		public override void GenerateWhereAppropriate()
		{
			this.CalculateRootsAndPossiblyAllowedTiles();
			this.CalculateContiguousGroups();
		}

		// Token: 0x06001A4A RID: 6730 RVA: 0x000E39A8 File Offset: 0x000E1DA8
		private void CalculateRootsAndPossiblyAllowedTiles()
		{
			this.roots.Clear();
			this.possiblyAllowed.Clear();
			int tilesCount = Find.WorldGrid.TilesCount;
			for (int i = 0; i < tilesCount; i++)
			{
				if (this.IsRoot(i))
				{
					this.roots.Add(i);
				}
				if (this.IsPossiblyAllowed(i))
				{
					this.possiblyAllowed.Add(i);
				}
			}
			this.rootsSet.Clear();
			this.rootsSet.AddRange(this.roots);
			this.possiblyAllowedSet.Clear();
			this.possiblyAllowedSet.AddRange(this.possiblyAllowed);
		}

		// Token: 0x06001A4B RID: 6731 RVA: 0x000E3A54 File Offset: 0x000E1E54
		private void CalculateContiguousGroups()
		{
			WorldFloodFiller worldFloodFiller = Find.WorldFloodFiller;
			WorldGrid worldGrid = Find.WorldGrid;
			int tilesCount = worldGrid.TilesCount;
			int minSize = this.MinSize;
			int maxSize = this.MaxSize;
			int maxPossiblyAllowedSizeToTake = this.MaxPossiblyAllowedSizeToTake;
			float maxPossiblyAllowedSizePctOfMeToTake = this.MaxPossiblyAllowedSizePctOfMeToTake;
			FeatureWorker.ClearVisited();
			FeatureWorker.ClearGroupSizes();
			for (int i = 0; i < this.possiblyAllowed.Count; i++)
			{
				int num = this.possiblyAllowed[i];
				if (!FeatureWorker.visited[num])
				{
					if (!this.rootsSet.Contains(num))
					{
						FeatureWorker_FloodFill.tmpGroup.Clear();
						worldFloodFiller.FloodFill(num, (int x) => this.possiblyAllowedSet.Contains(x) && !this.rootsSet.Contains(x), delegate(int x)
						{
							FeatureWorker.visited[x] = true;
							FeatureWorker_FloodFill.tmpGroup.Add(x);
						}, int.MaxValue, null);
						for (int j = 0; j < FeatureWorker_FloodFill.tmpGroup.Count; j++)
						{
							FeatureWorker.groupSize[FeatureWorker_FloodFill.tmpGroup[j]] = FeatureWorker_FloodFill.tmpGroup.Count;
						}
					}
				}
			}
			for (int k = 0; k < this.roots.Count; k++)
			{
				int num2 = this.roots[k];
				if (!FeatureWorker.visited[num2])
				{
					int initialMembersCountClamped = 0;
					worldFloodFiller.FloodFill(num2, (int x) => (this.rootsSet.Contains(x) || this.possiblyAllowedSet.Contains(x)) && this.IsMember(x), delegate(int x)
					{
						FeatureWorker.visited[x] = true;
						initialMembersCountClamped++;
						return initialMembersCountClamped >= minSize;
					}, int.MaxValue, null);
					if (initialMembersCountClamped >= minSize)
					{
						int initialRootsCount = 0;
						worldFloodFiller.FloodFill(num2, (int x) => this.rootsSet.Contains(x), delegate(int x)
						{
							FeatureWorker.visited[x] = true;
							initialRootsCount++;
						}, int.MaxValue, null);
						if (initialRootsCount >= minSize && initialRootsCount <= maxSize)
						{
							int traversedRootsCount = 0;
							this.currentGroup.Clear();
							worldFloodFiller.FloodFill(num2, (int x) => this.rootsSet.Contains(x) || (this.possiblyAllowedSet.Contains(x) && FeatureWorker.groupSize[x] <= maxPossiblyAllowedSizeToTake && (float)FeatureWorker.groupSize[x] <= maxPossiblyAllowedSizePctOfMeToTake * (float)Mathf.Max(traversedRootsCount, initialRootsCount) && FeatureWorker.groupSize[x] < maxSize), delegate(int x)
							{
								FeatureWorker.visited[x] = true;
								if (this.rootsSet.Contains(x))
								{
									traversedRootsCount++;
								}
								this.currentGroup.Add(x);
							}, int.MaxValue, null);
							if (this.currentGroup.Count >= minSize && this.currentGroup.Count <= maxSize)
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

		// Token: 0x04000F56 RID: 3926
		private List<int> roots = new List<int>();

		// Token: 0x04000F57 RID: 3927
		private HashSet<int> rootsSet = new HashSet<int>();

		// Token: 0x04000F58 RID: 3928
		private List<int> possiblyAllowed = new List<int>();

		// Token: 0x04000F59 RID: 3929
		private HashSet<int> possiblyAllowedSet = new HashSet<int>();

		// Token: 0x04000F5A RID: 3930
		private List<int> currentGroup = new List<int>();

		// Token: 0x04000F5B RID: 3931
		private List<int> currentGroupMembers = new List<int>();

		// Token: 0x04000F5C RID: 3932
		private static List<int> tmpGroup = new List<int>();
	}
}
