using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D3F RID: 3391
	public class PawnCapacitiesHandler
	{
		// Token: 0x06004ABA RID: 19130 RVA: 0x0026F809 File Offset: 0x0026DC09
		public PawnCapacitiesHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000BED RID: 3053
		// (get) Token: 0x06004ABB RID: 19131 RVA: 0x0026F820 File Offset: 0x0026DC20
		public bool CanBeAwake
		{
			get
			{
				return this.GetLevel(PawnCapacityDefOf.Consciousness) >= 0.3f;
			}
		}

		// Token: 0x06004ABC RID: 19132 RVA: 0x0026F84A File Offset: 0x0026DC4A
		public void Clear()
		{
			this.cachedCapacityLevels = null;
		}

		// Token: 0x06004ABD RID: 19133 RVA: 0x0026F854 File Offset: 0x0026DC54
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

		// Token: 0x06004ABE RID: 19134 RVA: 0x0026F928 File Offset: 0x0026DD28
		public bool CapableOf(PawnCapacityDef capacity)
		{
			return this.GetLevel(capacity) > capacity.minForCapable;
		}

		// Token: 0x06004ABF RID: 19135 RVA: 0x0026F94C File Offset: 0x0026DD4C
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

		// Token: 0x04003265 RID: 12901
		private Pawn pawn;

		// Token: 0x04003266 RID: 12902
		private DefMap<PawnCapacityDef, PawnCapacitiesHandler.CacheElement> cachedCapacityLevels = null;

		// Token: 0x02000D40 RID: 3392
		private enum CacheStatus
		{
			// Token: 0x04003268 RID: 12904
			Uncached,
			// Token: 0x04003269 RID: 12905
			Caching,
			// Token: 0x0400326A RID: 12906
			Cached
		}

		// Token: 0x02000D41 RID: 3393
		private class CacheElement
		{
			// Token: 0x0400326B RID: 12907
			public PawnCapacitiesHandler.CacheStatus status;

			// Token: 0x0400326C RID: 12908
			public float value;
		}
	}
}
