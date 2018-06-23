using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D3C RID: 3388
	public class PawnCapacitiesHandler
	{
		// Token: 0x04003270 RID: 12912
		private Pawn pawn;

		// Token: 0x04003271 RID: 12913
		private DefMap<PawnCapacityDef, PawnCapacitiesHandler.CacheElement> cachedCapacityLevels = null;

		// Token: 0x06004ACE RID: 19150 RVA: 0x00270D65 File Offset: 0x0026F165
		public PawnCapacitiesHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000BEF RID: 3055
		// (get) Token: 0x06004ACF RID: 19151 RVA: 0x00270D7C File Offset: 0x0026F17C
		public bool CanBeAwake
		{
			get
			{
				return this.GetLevel(PawnCapacityDefOf.Consciousness) >= 0.3f;
			}
		}

		// Token: 0x06004AD0 RID: 19152 RVA: 0x00270DA6 File Offset: 0x0026F1A6
		public void Clear()
		{
			this.cachedCapacityLevels = null;
		}

		// Token: 0x06004AD1 RID: 19153 RVA: 0x00270DB0 File Offset: 0x0026F1B0
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

		// Token: 0x06004AD2 RID: 19154 RVA: 0x00270E84 File Offset: 0x0026F284
		public bool CapableOf(PawnCapacityDef capacity)
		{
			return this.GetLevel(capacity) > capacity.minForCapable;
		}

		// Token: 0x06004AD3 RID: 19155 RVA: 0x00270EA8 File Offset: 0x0026F2A8
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

		// Token: 0x02000D3D RID: 3389
		private enum CacheStatus
		{
			// Token: 0x04003273 RID: 12915
			Uncached,
			// Token: 0x04003274 RID: 12916
			Caching,
			// Token: 0x04003275 RID: 12917
			Cached
		}

		// Token: 0x02000D3E RID: 3390
		private class CacheElement
		{
			// Token: 0x04003276 RID: 12918
			public PawnCapacitiesHandler.CacheStatus status;

			// Token: 0x04003277 RID: 12919
			public float value;
		}
	}
}
