using RimWorld;

namespace Verse
{
	public class PawnCapacitiesHandler
	{
		private enum CacheStatus
		{
			Uncached,
			Caching,
			Cached
		}

		private class CacheElement
		{
			public CacheStatus status;

			public float value;
		}

		private Pawn pawn;

		private DefMap<PawnCapacityDef, CacheElement> cachedCapacityLevels;

		public bool CanBeAwake
		{
			get
			{
				return this.GetLevel(PawnCapacityDefOf.Consciousness) >= 0.30000001192092896;
			}
		}

		public PawnCapacitiesHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		public void Clear()
		{
			this.cachedCapacityLevels = null;
		}

		public float GetLevel(PawnCapacityDef capacity)
		{
			if (this.pawn.health.Dead)
			{
				return 0f;
			}
			if (this.cachedCapacityLevels == null)
			{
				this.Notify_CapacityLevelsDirty();
			}
			CacheElement cacheElement = this.cachedCapacityLevels[capacity];
			if (cacheElement.status == CacheStatus.Caching)
			{
				Log.Error(string.Format("Detected infinite stat recursion when evaluating {0}", capacity));
				return 0f;
			}
			if (cacheElement.status == CacheStatus.Uncached)
			{
				cacheElement.status = CacheStatus.Caching;
				cacheElement.value = PawnCapacityUtility.CalculateCapacityLevel(this.pawn.health.hediffSet, capacity, null);
				cacheElement.status = CacheStatus.Cached;
			}
			return cacheElement.value;
		}

		public bool CapableOf(PawnCapacityDef capacity)
		{
			return this.GetLevel(capacity) > capacity.minForCapable;
		}

		public void Notify_CapacityLevelsDirty()
		{
			if (this.cachedCapacityLevels == null)
			{
				this.cachedCapacityLevels = new DefMap<PawnCapacityDef, CacheElement>();
			}
			for (int i = 0; i < this.cachedCapacityLevels.Count; i++)
			{
				this.cachedCapacityLevels[i].status = CacheStatus.Uncached;
			}
		}
	}
}
