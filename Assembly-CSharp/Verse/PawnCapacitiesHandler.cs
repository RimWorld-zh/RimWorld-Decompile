using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D3F RID: 3391
	public class PawnCapacitiesHandler
	{
		// Token: 0x04003277 RID: 12919
		private Pawn pawn;

		// Token: 0x04003278 RID: 12920
		private DefMap<PawnCapacityDef, PawnCapacitiesHandler.CacheElement> cachedCapacityLevels = null;

		// Token: 0x06004AD2 RID: 19154 RVA: 0x00271171 File Offset: 0x0026F571
		public PawnCapacitiesHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000BEE RID: 3054
		// (get) Token: 0x06004AD3 RID: 19155 RVA: 0x00271188 File Offset: 0x0026F588
		public bool CanBeAwake
		{
			get
			{
				return this.GetLevel(PawnCapacityDefOf.Consciousness) >= 0.3f;
			}
		}

		// Token: 0x06004AD4 RID: 19156 RVA: 0x002711B2 File Offset: 0x0026F5B2
		public void Clear()
		{
			this.cachedCapacityLevels = null;
		}

		// Token: 0x06004AD5 RID: 19157 RVA: 0x002711BC File Offset: 0x0026F5BC
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

		// Token: 0x06004AD6 RID: 19158 RVA: 0x00271290 File Offset: 0x0026F690
		public bool CapableOf(PawnCapacityDef capacity)
		{
			return this.GetLevel(capacity) > capacity.minForCapable;
		}

		// Token: 0x06004AD7 RID: 19159 RVA: 0x002712B4 File Offset: 0x0026F6B4
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

		// Token: 0x02000D40 RID: 3392
		private enum CacheStatus
		{
			// Token: 0x0400327A RID: 12922
			Uncached,
			// Token: 0x0400327B RID: 12923
			Caching,
			// Token: 0x0400327C RID: 12924
			Cached
		}

		// Token: 0x02000D41 RID: 3393
		private class CacheElement
		{
			// Token: 0x0400327D RID: 12925
			public PawnCapacitiesHandler.CacheStatus status;

			// Token: 0x0400327E RID: 12926
			public float value;
		}
	}
}
