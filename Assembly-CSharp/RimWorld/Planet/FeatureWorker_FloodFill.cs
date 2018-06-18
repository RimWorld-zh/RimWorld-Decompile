using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200056F RID: 1391
	public abstract class FeatureWorker_FloodFill : FeatureWorker
	{
		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06001A4B RID: 6731 RVA: 0x000E3884 File Offset: 0x000E1C84
		protected virtual int MinSize
		{
			get
			{
				return this.def.minSize;
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06001A4C RID: 6732 RVA: 0x000E38A4 File Offset: 0x000E1CA4
		protected virtual int MaxSize
		{
			get
			{
				return this.def.maxSize;
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06001A4D RID: 6733 RVA: 0x000E38C4 File Offset: 0x000E1CC4
		protected virtual int MaxPossiblyAllowedSizeToTake
		{
			get
			{
				return this.def.maxPossiblyAllowedSizeToTake;
			}
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06001A4E RID: 6734 RVA: 0x000E38E4 File Offset: 0x000E1CE4
		protected virtual float MaxPossiblyAllowedSizePctOfMeToTake
		{
			get
			{
				return this.def.maxPossiblyAllowedSizePctOfMeToTake;
			}
		}

		// Token: 0x06001A4F RID: 6735
		protected abstract bool IsRoot(int tile);

		// Token: 0x06001A50 RID: 6736 RVA: 0x000E3904 File Offset: 0x000E1D04
		protected virtual bool IsPossiblyAllowed(int tile)
		{
			return false;
		}

		// Token: 0x06001A51 RID: 6737 RVA: 0x000E391C File Offset: 0x000E1D1C
		protected virtual bool IsMember(int tile)
		{
			return Find.WorldGrid[tile].feature == null;
		}

		// Token: 0x06001A52 RID: 6738 RVA: 0x000E3944 File Offset: 0x000E1D44
		public override void GenerateWhereAppropriate()
		{
			this.CalculateRootsAndPossiblyAllowedTiles();
			this.CalculateContiguousGroups();
		}

		// Token: 0x06001A53 RID: 6739 RVA: 0x000E3954 File Offset: 0x000E1D54
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

		// Token: 0x06001A54 RID: 6740 RVA: 0x000E3A00 File Offset: 0x000E1E00
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

		// Token: 0x04000F59 RID: 3929
		private List<int> roots = new List<int>();

		// Token: 0x04000F5A RID: 3930
		private HashSet<int> rootsSet = new HashSet<int>();

		// Token: 0x04000F5B RID: 3931
		private List<int> possiblyAllowed = new List<int>();

		// Token: 0x04000F5C RID: 3932
		private HashSet<int> possiblyAllowedSet = new HashSet<int>();

		// Token: 0x04000F5D RID: 3933
		private List<int> currentGroup = new List<int>();

		// Token: 0x04000F5E RID: 3934
		private List<int> currentGroupMembers = new List<int>();

		// Token: 0x04000F5F RID: 3935
		private static List<int> tmpGroup = new List<int>();
	}
}
