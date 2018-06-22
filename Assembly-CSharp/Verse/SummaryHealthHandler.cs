using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D45 RID: 3397
	public class SummaryHealthHandler
	{
		// Token: 0x06004AE9 RID: 19177 RVA: 0x002719B6 File Offset: 0x0026FDB6
		public SummaryHealthHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x06004AEA RID: 19178 RVA: 0x002719D8 File Offset: 0x0026FDD8
		public float SummaryHealthPercent
		{
			get
			{
				float result;
				if (this.pawn.Dead)
				{
					result = 0f;
				}
				else
				{
					if (this.dirty)
					{
						List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
						float num = 1f;
						for (int i = 0; i < hediffs.Count; i++)
						{
							if (!(hediffs[i] is Hediff_MissingPart))
							{
								float num2 = Mathf.Min(hediffs[i].SummaryHealthPercentImpact, 0.95f);
								num *= 1f - num2;
							}
						}
						List<Hediff_MissingPart> missingPartsCommonAncestors = this.pawn.health.hediffSet.GetMissingPartsCommonAncestors();
						for (int j = 0; j < missingPartsCommonAncestors.Count; j++)
						{
							float num3 = Mathf.Min(missingPartsCommonAncestors[j].SummaryHealthPercentImpact, 0.95f);
							num *= 1f - num3;
						}
						this.cachedSummaryHealthPercent = Mathf.Clamp(num, 0.05f, 1f);
						this.dirty = false;
					}
					result = this.cachedSummaryHealthPercent;
				}
				return result;
			}
		}

		// Token: 0x06004AEB RID: 19179 RVA: 0x00271B02 File Offset: 0x0026FF02
		public void Notify_HealthChanged()
		{
			this.dirty = true;
		}

		// Token: 0x0400327B RID: 12923
		private Pawn pawn;

		// Token: 0x0400327C RID: 12924
		private float cachedSummaryHealthPercent = 1f;

		// Token: 0x0400327D RID: 12925
		private bool dirty = true;
	}
}
