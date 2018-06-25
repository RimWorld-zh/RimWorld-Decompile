using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public abstract class FeatureWorker_FloodFill : FeatureWorker
	{
		private List<int> roots = new List<int>();

		private HashSet<int> rootsSet = new HashSet<int>();

		private List<int> possiblyAllowed = new List<int>();

		private HashSet<int> possiblyAllowedSet = new HashSet<int>();

		private List<int> currentGroup = new List<int>();

		private List<int> currentGroupMembers = new List<int>();

		private static List<int> tmpGroup = new List<int>();

		[CompilerGenerated]
		private static Action<int> <>f__am$cache0;

		protected FeatureWorker_FloodFill()
		{
		}

		protected virtual int MinSize
		{
			get
			{
				return this.def.minSize;
			}
		}

		protected virtual int MaxSize
		{
			get
			{
				return this.def.maxSize;
			}
		}

		protected virtual int MaxPossiblyAllowedSizeToTake
		{
			get
			{
				return this.def.maxPossiblyAllowedSizeToTake;
			}
		}

		protected virtual float MaxPossiblyAllowedSizePctOfMeToTake
		{
			get
			{
				return this.def.maxPossiblyAllowedSizePctOfMeToTake;
			}
		}

		protected abstract bool IsRoot(int tile);

		protected virtual bool IsPossiblyAllowed(int tile)
		{
			return false;
		}

		protected virtual bool IsMember(int tile)
		{
			return Find.WorldGrid[tile].feature == null;
		}

		public override void GenerateWhereAppropriate()
		{
			this.CalculateRootsAndPossiblyAllowedTiles();
			this.CalculateContiguousGroups();
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static FeatureWorker_FloodFill()
		{
		}

		[CompilerGenerated]
		private static void <CalculateContiguousGroups>m__0(int x)
		{
			FeatureWorker.visited[x] = true;
			FeatureWorker_FloodFill.tmpGroup.Add(x);
		}

		[CompilerGenerated]
		private sealed class <CalculateContiguousGroups>c__AnonStorey1
		{
			internal int minSize;

			internal int maxPossiblyAllowedSizeToTake;

			internal float maxPossiblyAllowedSizePctOfMeToTake;

			internal int maxSize;

			internal WorldGrid worldGrid;

			internal FeatureWorker_FloodFill $this;

			public <CalculateContiguousGroups>c__AnonStorey1()
			{
			}

			internal bool <>m__0(int x)
			{
				return this.$this.possiblyAllowedSet.Contains(x) && !this.$this.rootsSet.Contains(x);
			}
		}

		[CompilerGenerated]
		private sealed class <CalculateContiguousGroups>c__AnonStorey0
		{
			internal int initialMembersCountClamped;

			internal int initialRootsCount;

			internal int traversedRootsCount;

			internal FeatureWorker_FloodFill.<CalculateContiguousGroups>c__AnonStorey1 <>f__ref$1;

			public <CalculateContiguousGroups>c__AnonStorey0()
			{
			}

			internal bool <>m__0(int x)
			{
				return (this.<>f__ref$1.$this.rootsSet.Contains(x) || this.<>f__ref$1.$this.possiblyAllowedSet.Contains(x)) && this.<>f__ref$1.$this.IsMember(x);
			}

			internal bool <>m__1(int x)
			{
				FeatureWorker.visited[x] = true;
				this.initialMembersCountClamped++;
				return this.initialMembersCountClamped >= this.<>f__ref$1.minSize;
			}

			internal bool <>m__2(int x)
			{
				return this.<>f__ref$1.$this.rootsSet.Contains(x);
			}

			internal void <>m__3(int x)
			{
				FeatureWorker.visited[x] = true;
				this.initialRootsCount++;
			}

			internal bool <>m__4(int x)
			{
				return this.<>f__ref$1.$this.rootsSet.Contains(x) || (this.<>f__ref$1.$this.possiblyAllowedSet.Contains(x) && FeatureWorker.groupSize[x] <= this.<>f__ref$1.maxPossiblyAllowedSizeToTake && (float)FeatureWorker.groupSize[x] <= this.<>f__ref$1.maxPossiblyAllowedSizePctOfMeToTake * (float)Mathf.Max(this.traversedRootsCount, this.initialRootsCount) && FeatureWorker.groupSize[x] < this.<>f__ref$1.maxSize);
			}

			internal void <>m__5(int x)
			{
				FeatureWorker.visited[x] = true;
				if (this.<>f__ref$1.$this.rootsSet.Contains(x))
				{
					this.traversedRootsCount++;
				}
				this.<>f__ref$1.$this.currentGroup.Add(x);
			}

			internal bool <>m__6(int x)
			{
				return this.<>f__ref$1.worldGrid.IsOnEdge(x);
			}

			internal bool <>m__7(int x)
			{
				return this.<>f__ref$1.worldGrid[x].feature == null;
			}

			internal bool <>m__8(int x)
			{
				return this.<>f__ref$1.worldGrid[x].feature != null;
			}
		}
	}
}
