using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Verse
{
	// Token: 0x02000C7E RID: 3198
	internal class ReachabilityCache
	{
		// Token: 0x04002FC2 RID: 12226
		private Dictionary<ReachabilityCache.CachedEntry, bool> cacheDict = new Dictionary<ReachabilityCache.CachedEntry, bool>();

		// Token: 0x17000B07 RID: 2823
		// (get) Token: 0x0600460A RID: 17930 RVA: 0x0024EA00 File Offset: 0x0024CE00
		public int Count
		{
			get
			{
				return this.cacheDict.Count;
			}
		}

		// Token: 0x0600460B RID: 17931 RVA: 0x0024EA20 File Offset: 0x0024CE20
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

		// Token: 0x0600460C RID: 17932 RVA: 0x0024EA70 File Offset: 0x0024CE70
		public void AddCachedResult(Room A, Room B, TraverseParms traverseParams, bool reachable)
		{
			ReachabilityCache.CachedEntry key = new ReachabilityCache.CachedEntry(A.ID, B.ID, traverseParams);
			if (!this.cacheDict.ContainsKey(key))
			{
				this.cacheDict.Add(key, reachable);
			}
		}

		// Token: 0x0600460D RID: 17933 RVA: 0x0024EAB1 File Offset: 0x0024CEB1
		public void Clear()
		{
			this.cacheDict.Clear();
		}

		// Token: 0x02000C7F RID: 3199
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct CachedEntry : IEquatable<ReachabilityCache.CachedEntry>
		{
			// Token: 0x0600460E RID: 17934 RVA: 0x0024EABF File Offset: 0x0024CEBF
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

			// Token: 0x17000B08 RID: 2824
			// (get) Token: 0x0600460F RID: 17935 RVA: 0x0024EAFC File Offset: 0x0024CEFC
			// (set) Token: 0x06004610 RID: 17936 RVA: 0x0024EB16 File Offset: 0x0024CF16
			public int FirstRoomID { get; private set; }

			// Token: 0x17000B09 RID: 2825
			// (get) Token: 0x06004611 RID: 17937 RVA: 0x0024EB20 File Offset: 0x0024CF20
			// (set) Token: 0x06004612 RID: 17938 RVA: 0x0024EB3A File Offset: 0x0024CF3A
			public int SecondRoomID { get; private set; }

			// Token: 0x17000B0A RID: 2826
			// (get) Token: 0x06004613 RID: 17939 RVA: 0x0024EB44 File Offset: 0x0024CF44
			// (set) Token: 0x06004614 RID: 17940 RVA: 0x0024EB5E File Offset: 0x0024CF5E
			public TraverseParms TraverseParms { get; private set; }

			// Token: 0x06004615 RID: 17941 RVA: 0x0024EB68 File Offset: 0x0024CF68
			public static bool operator ==(ReachabilityCache.CachedEntry lhs, ReachabilityCache.CachedEntry rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x06004616 RID: 17942 RVA: 0x0024EB88 File Offset: 0x0024CF88
			public static bool operator !=(ReachabilityCache.CachedEntry lhs, ReachabilityCache.CachedEntry rhs)
			{
				return !lhs.Equals(rhs);
			}

			// Token: 0x06004617 RID: 17943 RVA: 0x0024EBA8 File Offset: 0x0024CFA8
			public override bool Equals(object obj)
			{
				return obj is ReachabilityCache.CachedEntry && this.Equals((ReachabilityCache.CachedEntry)obj);
			}

			// Token: 0x06004618 RID: 17944 RVA: 0x0024EBDC File Offset: 0x0024CFDC
			public bool Equals(ReachabilityCache.CachedEntry other)
			{
				return this.FirstRoomID == other.FirstRoomID && this.SecondRoomID == other.SecondRoomID && this.TraverseParms == other.TraverseParms;
			}

			// Token: 0x06004619 RID: 17945 RVA: 0x0024EC2C File Offset: 0x0024D02C
			public override int GetHashCode()
			{
				int seed = Gen.HashCombineInt(this.FirstRoomID, this.SecondRoomID);
				return Gen.HashCombineStruct<TraverseParms>(seed, this.TraverseParms);
			}
		}
	}
}
