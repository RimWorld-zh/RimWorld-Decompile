using RimWorld;

namespace Verse
{
	public class PawnCapacitiesHandler
	{
		private enum CacheStatus
		{
			Uncached = 0,
			Caching = 1,
			Cached = 2
		}

		private class CacheElement
		{
			public CacheStatus status;

			public float value;
		}

		private Pawn pawn;

		private DefMap<PawnCapacityDef, CacheElement> cachedCapacityLevels = null;

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
			float result;
			if (this.pawn.health.Dead)
			{
				result = 0f;
			}
			else
			{
				if (this.cachedCapacityLevels == null)
				{
					this.Notify_CapacityLevelsDirty();
				}
				CacheElement cacheElement = this.cachedCapacityLevels[capacity];
				if (cacheElement.status == CacheStatus.Caching)
				{
					Log.Error(string.Format("Detected infinite stat recursion when evaluating {0}", capacity));
					result = 0f;
				}
				else
				{
					if (cacheElement.status == CacheStatus.Uncached)
					{
						cacheElement.status = CacheStatus.Caching;
						cacheElement.value = PawnCapacityUtility.CalculateCapacityLevel(this.pawn.health.hediffSet, capacity, null);
						cacheElement.status = CacheStatus.Cached;
					}
					result = cacheElement.value;
				}
			}
			return result;
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
