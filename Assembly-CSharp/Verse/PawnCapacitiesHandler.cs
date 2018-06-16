using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D40 RID: 3392
	public class PawnCapacitiesHandler
	{
		// Token: 0x06004ABC RID: 19132 RVA: 0x0026F831 File Offset: 0x0026DC31
		public PawnCapacitiesHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000BEE RID: 3054
		// (get) Token: 0x06004ABD RID: 19133 RVA: 0x0026F848 File Offset: 0x0026DC48
		public bool CanBeAwake
		{
			get
			{
				return this.GetLevel(PawnCapacityDefOf.Consciousness) >= 0.3f;
			}
		}

		// Token: 0x06004ABE RID: 19134 RVA: 0x0026F872 File Offset: 0x0026DC72
		public void Clear()
		{
			this.cachedCapacityLevels = null;
		}

		// Token: 0x06004ABF RID: 19135 RVA: 0x0026F87C File Offset: 0x0026DC7C
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
				PawnCapacitiesHandler.CacheElement cacheElement = this.cachedCapacityLevels[capacity];
				if (cacheElement.status == PawnCapacitiesHandler.CacheStatus.Caching)
				{
					Log.Error(string.Format("Detected infinite stat recursion when evaluating {0}", capacity), false);
					result = 0f;
				}
				else
				{
					if (cacheElement.status == PawnCapacitiesHandler.CacheStatus.Uncached)
					{
						cacheElement.status = PawnCapacitiesHandler.CacheStatus.Caching;
						try
						{
							cacheElement.value = PawnCapacityUtility.CalculateCapacityLevel(this.pawn.health.hediffSet, capacity, null);
						}
						finally
						{
							cacheElement.status = PawnCapacitiesHandler.CacheStatus.Cached;
						}
					}
					result = cacheElement.value;
				}
			}
			return result;
		}

		// Token: 0x06004AC0 RID: 19136 RVA: 0x0026F950 File Offset: 0x0026DD50
		public bool CapableOf(PawnCapacityDef capacity)
		{
			return this.GetLevel(capacity) > capacity.minForCapable;
		}

		// Token: 0x06004AC1 RID: 19137 RVA: 0x0026F974 File Offset: 0x0026DD74
		public void Notify_CapacityLevelsDirty()
		{
			if (this.cachedCapacityLevels == null)
			{
				this.cachedCapacityLevels = new DefMap<PawnCapacityDef, PawnCapacitiesHandler.CacheElement>();
			}
			for (int i = 0; i < this.cachedCapacityLevels.Count; i++)
			{
				this.cachedCapacityLevels[i].status = PawnCapacitiesHandler.CacheStatus.Uncached;
			}
		}

		// Token: 0x04003267 RID: 12903
		private Pawn pawn;

		// Token: 0x04003268 RID: 12904
		private DefMap<PawnCapacityDef, PawnCapacitiesHandler.CacheElement> cachedCapacityLevels = null;

		// Token: 0x02000D41 RID: 3393
		private enum CacheStatus
		{
			// Token: 0x0400326A RID: 12906
			Uncached,
			// Token: 0x0400326B RID: 12907
			Caching,
			// Token: 0x0400326C RID: 12908
			Cached
		}

		// Token: 0x02000D42 RID: 3394
		private class CacheElement
		{
			// Token: 0x0400326D RID: 12909
			public PawnCapacitiesHandler.CacheStatus status;

			// Token: 0x0400326E RID: 12910
			public float value;
		}
	}
}
