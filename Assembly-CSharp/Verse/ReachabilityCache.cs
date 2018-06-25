using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Verse
{
	// Token: 0x02000C81 RID: 3201
	internal class ReachabilityCache
	{
		// Token: 0x04002FC9 RID: 12233
		private Dictionary<ReachabilityCache.CachedEntry, bool> cacheDict = new Dictionary<ReachabilityCache.CachedEntry, bool>();

		// Token: 0x17000B06 RID: 2822
		// (get) Token: 0x0600460D RID: 17933 RVA: 0x0024EDBC File Offset: 0x0024D1BC
		public int Count
		{
			get
			{
				return this.cacheDict.Count;
			}
		}

		// Token: 0x0600460E RID: 17934 RVA: 0x0024EDDC File Offset: 0x0024D1DC
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

		// Token: 0x0600460F RID: 17935 RVA: 0x0024EE2C File Offset: 0x0024D22C
		public void AddCachedResult(Room A, Room B, TraverseParms traverseParams, bool reachable)
		{
			ReachabilityCache.CachedEntry key = new ReachabilityCache.CachedEntry(A.ID, B.ID, traverseParams);
			if (!this.cacheDict.ContainsKey(key))
			{
				this.cacheDict.Add(key, reachable);
			}
		}

		// Token: 0x06004610 RID: 17936 RVA: 0x0024EE6D File Offset: 0x0024D26D
		public void Clear()
		{
			this.cacheDict.Clear();
		}

		// Token: 0x02000C82 RID: 3202
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct CachedEntry : IEquatable<ReachabilityCache.CachedEntry>
		{
			// Token: 0x06004611 RID: 17937 RVA: 0x0024EE7B File Offset: 0x0024D27B
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
			// (get) Token: 0x06004612 RID: 17938 RVA: 0x0024EEB8 File Offset: 0x0024D2B8
			// (set) Token: 0x06004613 RID: 17939 RVA: 0x0024EED2 File Offset: 0x0024D2D2
			public int FirstRoomID { get; private set; }

			// Token: 0x17000B08 RID: 2824
			// (get) Token: 0x06004614 RID: 17940 RVA: 0x0024EEDC File Offset: 0x0024D2DC
			// (set) Token: 0x06004615 RID: 17941 RVA: 0x0024EEF6 File Offset: 0x0024D2F6
			public int SecondRoomID { get; private set; }

			// Token: 0x17000B09 RID: 2825
			// (get) Token: 0x06004616 RID: 17942 RVA: 0x0024EF00 File Offset: 0x0024D300
			// (set) Token: 0x06004617 RID: 17943 RVA: 0x0024EF1A File Offset: 0x0024D31A
			public TraverseParms TraverseParms { get; private set; }

			// Token: 0x06004618 RID: 17944 RVA: 0x0024EF24 File Offset: 0x0024D324
			public static bool operator ==(ReachabilityCache.CachedEntry lhs, ReachabilityCache.CachedEntry rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x06004619 RID: 17945 RVA: 0x0024EF44 File Offset: 0x0024D344
			public static bool operator !=(ReachabilityCache.CachedEntry lhs, ReachabilityCache.CachedEntry rhs)
			{
				return !lhs.Equals(rhs);
			}

			// Token: 0x0600461A RID: 17946 RVA: 0x0024EF64 File Offset: 0x0024D364
			public override bool Equals(object obj)
			{
				return obj is ReachabilityCache.CachedEntry && this.Equals((ReachabilityCache.CachedEntry)obj);
			}

			// Token: 0x0600461B RID: 17947 RVA: 0x0024EF98 File Offset: 0x0024D398
			public bool Equals(ReachabilityCache.CachedEntry other)
			{
				return this.FirstRoomID == other.FirstRoomID && this.SecondRoomID == other.SecondRoomID && this.TraverseParms == other.TraverseParms;
			}

			// Token: 0x0600461C RID: 17948 RVA: 0x0024EFE8 File Offset: 0x0024D3E8
			public override int GetHashCode()
			{
				int seed = Gen.HashCombineInt(this.FirstRoomID, this.SecondRoomID);
				return Gen.HashCombineStruct<TraverseParms>(seed, this.TraverseParms);
			}
		}
	}
}
