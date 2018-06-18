using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Verse
{
	// Token: 0x02000C81 RID: 3201
	internal class ReachabilityCache
	{
		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x06004601 RID: 17921 RVA: 0x0024D630 File Offset: 0x0024BA30
		public int Count
		{
			get
			{
				return this.cacheDict.Count;
			}
		}

		// Token: 0x06004602 RID: 17922 RVA: 0x0024D650 File Offset: 0x0024BA50
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

		// Token: 0x06004603 RID: 17923 RVA: 0x0024D6A0 File Offset: 0x0024BAA0
		public void AddCachedResult(Room A, Room B, TraverseParms traverseParams, bool reachable)
		{
			ReachabilityCache.CachedEntry key = new ReachabilityCache.CachedEntry(A.ID, B.ID, traverseParams);
			if (!this.cacheDict.ContainsKey(key))
			{
				this.cacheDict.Add(key, reachable);
			}
		}

		// Token: 0x06004604 RID: 17924 RVA: 0x0024D6E1 File Offset: 0x0024BAE1
		public void Clear()
		{
			this.cacheDict.Clear();
		}

		// Token: 0x04002FB8 RID: 12216
		private Dictionary<ReachabilityCache.CachedEntry, bool> cacheDict = new Dictionary<ReachabilityCache.CachedEntry, bool>();

		// Token: 0x02000C82 RID: 3202
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct CachedEntry : IEquatable<ReachabilityCache.CachedEntry>
		{
			// Token: 0x06004605 RID: 17925 RVA: 0x0024D6EF File Offset: 0x0024BAEF
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

			// Token: 0x17000B06 RID: 2822
			// (get) Token: 0x06004606 RID: 17926 RVA: 0x0024D72C File Offset: 0x0024BB2C
			// (set) Token: 0x06004607 RID: 17927 RVA: 0x0024D746 File Offset: 0x0024BB46
			public int FirstRoomID { get; private set; }

			// Token: 0x17000B07 RID: 2823
			// (get) Token: 0x06004608 RID: 17928 RVA: 0x0024D750 File Offset: 0x0024BB50
			// (set) Token: 0x06004609 RID: 17929 RVA: 0x0024D76A File Offset: 0x0024BB6A
			public int SecondRoomID { get; private set; }

			// Token: 0x17000B08 RID: 2824
			// (get) Token: 0x0600460A RID: 17930 RVA: 0x0024D774 File Offset: 0x0024BB74
			// (set) Token: 0x0600460B RID: 17931 RVA: 0x0024D78E File Offset: 0x0024BB8E
			public TraverseParms TraverseParms { get; private set; }

			// Token: 0x0600460C RID: 17932 RVA: 0x0024D798 File Offset: 0x0024BB98
			public static bool operator ==(ReachabilityCache.CachedEntry lhs, ReachabilityCache.CachedEntry rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x0600460D RID: 17933 RVA: 0x0024D7B8 File Offset: 0x0024BBB8
			public static bool operator !=(ReachabilityCache.CachedEntry lhs, ReachabilityCache.CachedEntry rhs)
			{
				return !lhs.Equals(rhs);
			}

			// Token: 0x0600460E RID: 17934 RVA: 0x0024D7D8 File Offset: 0x0024BBD8
			public override bool Equals(object obj)
			{
				return obj is ReachabilityCache.CachedEntry && this.Equals((ReachabilityCache.CachedEntry)obj);
			}

			// Token: 0x0600460F RID: 17935 RVA: 0x0024D80C File Offset: 0x0024BC0C
			public bool Equals(ReachabilityCache.CachedEntry other)
			{
				return this.FirstRoomID == other.FirstRoomID && this.SecondRoomID == other.SecondRoomID && this.TraverseParms == other.TraverseParms;
			}

			// Token: 0x06004610 RID: 17936 RVA: 0x0024D85C File Offset: 0x0024BC5C
			public override int GetHashCode()
			{
				int seed = Gen.HashCombineInt(this.FirstRoomID, this.SecondRoomID);
				return Gen.HashCombineStruct<TraverseParms>(seed, this.TraverseParms);
			}
		}
	}
}
