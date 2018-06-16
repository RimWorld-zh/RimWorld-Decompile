using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Verse
{
	// Token: 0x02000C82 RID: 3202
	internal class ReachabilityCache
	{
		// Token: 0x17000B06 RID: 2822
		// (get) Token: 0x06004603 RID: 17923 RVA: 0x0024D658 File Offset: 0x0024BA58
		public int Count
		{
			get
			{
				return this.cacheDict.Count;
			}
		}

		// Token: 0x06004604 RID: 17924 RVA: 0x0024D678 File Offset: 0x0024BA78
		public BoolUnknown CachedResultFor(Room A, Room B, TraverseParms traverseParams)
		{
			bool flag;
			BoolUnknown result;
			if (this.cacheDict.TryGetValue(new ReachabilityCache.CachedEntry(A.ID, B.ID, traverseParams), out flag))
			{
				result = ((!flag) ? BoolUnknown.False : BoolUnknown.True);
			}
			else
			{
				result = BoolUnknown.Unknown;
			}
			return result;
		}

		// Token: 0x06004605 RID: 17925 RVA: 0x0024D6C8 File Offset: 0x0024BAC8
		public void AddCachedResult(Room A, Room B, TraverseParms traverseParams, bool reachable)
		{
			ReachabilityCache.CachedEntry key = new ReachabilityCache.CachedEntry(A.ID, B.ID, traverseParams);
			if (!this.cacheDict.ContainsKey(key))
			{
				this.cacheDict.Add(key, reachable);
			}
		}

		// Token: 0x06004606 RID: 17926 RVA: 0x0024D709 File Offset: 0x0024BB09
		public void Clear()
		{
			this.cacheDict.Clear();
		}

		// Token: 0x04002FBA RID: 12218
		private Dictionary<ReachabilityCache.CachedEntry, bool> cacheDict = new Dictionary<ReachabilityCache.CachedEntry, bool>();

		// Token: 0x02000C83 RID: 3203
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct CachedEntry : IEquatable<ReachabilityCache.CachedEntry>
		{
			// Token: 0x06004607 RID: 17927 RVA: 0x0024D717 File Offset: 0x0024BB17
			public CachedEntry(int firstRoomID, int secondRoomID, TraverseParms traverseParms)
			{
				this = default(ReachabilityCache.CachedEntry);
				if (firstRoomID < secondRoomID)
				{
					this.FirstRoomID = firstRoomID;
					this.SecondRoomID = secondRoomID;
				}
				else
				{
					this.FirstRoomID = secondRoomID;
					this.SecondRoomID = firstRoomID;
				}
				this.TraverseParms = traverseParms;
			}

			// Token: 0x17000B07 RID: 2823
			// (get) Token: 0x06004608 RID: 17928 RVA: 0x0024D754 File Offset: 0x0024BB54
			// (set) Token: 0x06004609 RID: 17929 RVA: 0x0024D76E File Offset: 0x0024BB6E
			public int FirstRoomID { get; private set; }

			// Token: 0x17000B08 RID: 2824
			// (get) Token: 0x0600460A RID: 17930 RVA: 0x0024D778 File Offset: 0x0024BB78
			// (set) Token: 0x0600460B RID: 17931 RVA: 0x0024D792 File Offset: 0x0024BB92
			public int SecondRoomID { get; private set; }

			// Token: 0x17000B09 RID: 2825
			// (get) Token: 0x0600460C RID: 17932 RVA: 0x0024D79C File Offset: 0x0024BB9C
			// (set) Token: 0x0600460D RID: 17933 RVA: 0x0024D7B6 File Offset: 0x0024BBB6
			public TraverseParms TraverseParms { get; private set; }

			// Token: 0x0600460E RID: 17934 RVA: 0x0024D7C0 File Offset: 0x0024BBC0
			public static bool operator ==(ReachabilityCache.CachedEntry lhs, ReachabilityCache.CachedEntry rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x0600460F RID: 17935 RVA: 0x0024D7E0 File Offset: 0x0024BBE0
			public static bool operator !=(ReachabilityCache.CachedEntry lhs, ReachabilityCache.CachedEntry rhs)
			{
				return !lhs.Equals(rhs);
			}

			// Token: 0x06004610 RID: 17936 RVA: 0x0024D800 File Offset: 0x0024BC00
			public override bool Equals(object obj)
			{
				return obj is ReachabilityCache.CachedEntry && this.Equals((ReachabilityCache.CachedEntry)obj);
			}

			// Token: 0x06004611 RID: 17937 RVA: 0x0024D834 File Offset: 0x0024BC34
			public bool Equals(ReachabilityCache.CachedEntry other)
			{
				return this.FirstRoomID == other.FirstRoomID && this.SecondRoomID == other.SecondRoomID && this.TraverseParms == other.TraverseParms;
			}

			// Token: 0x06004612 RID: 17938 RVA: 0x0024D884 File Offset: 0x0024BC84
			public override int GetHashCode()
			{
				int seed = Gen.HashCombineInt(this.FirstRoomID, this.SecondRoomID);
				return Gen.HashCombineStruct<TraverseParms>(seed, this.TraverseParms);
			}
		}
	}
}
