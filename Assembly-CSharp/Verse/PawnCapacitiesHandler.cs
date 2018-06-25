using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D3E RID: 3390
	public class PawnCapacitiesHandler
	{
		// Token: 0x04003270 RID: 12912
		private Pawn pawn;

		// Token: 0x04003271 RID: 12913
		private DefMap<PawnCapacityDef, PawnCapacitiesHandler.CacheElement> cachedCapacityLevels = null;

		// Token: 0x06004AD2 RID: 19154 RVA: 0x00270E91 File Offset: 0x0026F291
		public PawnCapacitiesHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000BEE RID: 3054
		// (get) Token: 0x06004AD3 RID: 19155 RVA: 0x00270EA8 File Offset: 0x0026F2A8
		public bool CanBeAwake
		{
			get
			{
				return this.GetLevel(PawnCapacityDefOf.Consciousness) >= 0.3f;
			}
		}

		// Token: 0x06004AD4 RID: 19156 RVA: 0x00270ED2 File Offset: 0x0026F2D2
		public void Clear()
		{
			this.cachedCapacityLevels = null;
		}

		// Token: 0x06004AD5 RID: 19157 RVA: 0x00270EDC File Offset: 0x0026F2DC
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

		// Token: 0x06004AD6 RID: 19158 RVA: 0x00270FB0 File Offset: 0x0026F3B0
		public bool CapableOf(PawnCapacityDef capacity)
		{
			return this.GetLevel(capacity) > capacity.minForCapable;
		}

		// Token: 0x06004AD7 RID: 19159 RVA: 0x00270FD4 File Offset: 0x0026F3D4
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

		// Token: 0x02000D3F RID: 3391
		private enum CacheStatus
		{
			// Token: 0x04003273 RID: 12915
			Uncached,
			// Token: 0x04003274 RID: 12916
			Caching,
			// Token: 0x04003275 RID: 12917
			Cached
		}

		// Token: 0x02000D40 RID: 3392
		private class CacheElement
		{
			// Token: 0x04003276 RID: 12918
			public PawnCapacitiesHandler.CacheStatus status;

			// Token: 0x04003277 RID: 12919
			public float value;
		}
	}
}
