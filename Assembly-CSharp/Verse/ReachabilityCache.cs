using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Verse
{
	internal class ReachabilityCache
	{
		private Dictionary<ReachabilityCache.CachedEntry, bool> cacheDict = new Dictionary<ReachabilityCache.CachedEntry, bool>();

		public ReachabilityCache()
		{
		}

		public int Count
		{
			get
			{
				return this.cacheDict.Count;
			}
		}

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

		public void AddCachedResult(Room A, Room B, TraverseParms traverseParams, bool reachable)
		{
			ReachabilityCache.CachedEntry key = new ReachabilityCache.CachedEntry(A.ID, B.ID, traverseParams);
			if (!this.cacheDict.ContainsKey(key))
			{
				this.cacheDict.Add(key, reachable);
			}
		}

		public void Clear()
		{
			this.cacheDict.Clear();
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct CachedEntry : IEquatable<ReachabilityCache.CachedEntry>
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			private int <FirstRoomID>k__BackingField;

			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			private int <SecondRoomID>k__BackingField;

			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			private TraverseParms <TraverseParms>k__BackingField;

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

			public int FirstRoomID
			{
				[CompilerGenerated]
				get
				{
					return this.<FirstRoomID>k__BackingField;
				}
				[CompilerGenerated]
				private set
				{
					this.<FirstRoomID>k__BackingField = value;
				}
			}

			public int SecondRoomID
			{
				[CompilerGenerated]
				get
				{
					return this.<SecondRoomID>k__BackingField;
				}
				[CompilerGenerated]
				private set
				{
					this.<SecondRoomID>k__BackingField = value;
				}
			}

			public TraverseParms TraverseParms
			{
				[CompilerGenerated]
				get
				{
					return this.<TraverseParms>k__BackingField;
				}
				[CompilerGenerated]
				private set
				{
					this.<TraverseParms>k__BackingField = value;
				}
			}

			public static bool operator ==(ReachabilityCache.CachedEntry lhs, ReachabilityCache.CachedEntry rhs)
			{
				return lhs.Equals(rhs);
			}

			public static bool operator !=(ReachabilityCache.CachedEntry lhs, ReachabilityCache.CachedEntry rhs)
			{
				return !lhs.Equals(rhs);
			}

			public override bool Equals(object obj)
			{
				return obj is ReachabilityCache.CachedEntry && this.Equals((ReachabilityCache.CachedEntry)obj);
			}

			public bool Equals(ReachabilityCache.CachedEntry other)
			{
				return this.FirstRoomID == other.FirstRoomID && this.SecondRoomID == other.SecondRoomID && this.TraverseParms == other.TraverseParms;
			}

			public override int GetHashCode()
			{
				int seed = Gen.HashCombineInt(this.FirstRoomID, this.SecondRoomID);
				return Gen.HashCombineStruct<TraverseParms>(seed, this.TraverseParms);
			}
		}
	}
}
