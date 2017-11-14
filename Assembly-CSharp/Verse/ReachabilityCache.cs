using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Verse
{
	internal class ReachabilityCache
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct CachedEntry : IEquatable<CachedEntry>
		{
			public int FirstRoomID
			{
				get;
				private set;
			}

			public int SecondRoomID
			{
				get;
				private set;
			}

			public TraverseParms TraverseParms
			{
				get;
				private set;
			}

			public CachedEntry(int firstRoomID, int secondRoomID, TraverseParms traverseParms)
			{
				this = default(CachedEntry);
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

			public static bool operator ==(CachedEntry lhs, CachedEntry rhs)
			{
				return lhs.Equals(rhs);
			}

			public static bool operator !=(CachedEntry lhs, CachedEntry rhs)
			{
				return !lhs.Equals(rhs);
			}

			public override bool Equals(object obj)
			{
				if (!(obj is CachedEntry))
				{
					return false;
				}
				return this.Equals((CachedEntry)obj);
			}

			public bool Equals(CachedEntry other)
			{
				return this.FirstRoomID == other.FirstRoomID && this.SecondRoomID == other.SecondRoomID && this.TraverseParms == other.TraverseParms;
			}

			public override int GetHashCode()
			{
				int seed = Gen.HashCombineInt(this.FirstRoomID, this.SecondRoomID);
				return Gen.HashCombineStruct(seed, this.TraverseParms);
			}
		}

		private Dictionary<CachedEntry, bool> cacheDict = new Dictionary<CachedEntry, bool>();

		public int Count
		{
			get
			{
				return this.cacheDict.Count;
			}
		}

		public BoolUnknown CachedResultFor(Room A, Room B, TraverseParms traverseParams)
		{
			bool flag = default(bool);
			if (this.cacheDict.TryGetValue(new CachedEntry(A.ID, B.ID, traverseParams), out flag))
			{
				return (BoolUnknown)((!flag) ? 1 : 0);
			}
			return BoolUnknown.Unknown;
		}

		public void AddCachedResult(Room A, Room B, TraverseParms traverseParams, bool reachable)
		{
			CachedEntry key = new CachedEntry(A.ID, B.ID, traverseParams);
			if (!this.cacheDict.ContainsKey(key))
			{
				this.cacheDict.Add(key, reachable);
			}
		}

		public void Clear()
		{
			this.cacheDict.Clear();
		}
	}
}
