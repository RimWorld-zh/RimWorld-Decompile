using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D47 RID: 3399
	public class SummaryHealthHandler
	{
		// Token: 0x0400327B RID: 12923
		private Pawn pawn;

		// Token: 0x0400327C RID: 12924
		private float cachedSummaryHealthPercent = 1f;

		// Token: 0x0400327D RID: 12925
		private bool dirty = true;

		// Token: 0x06004AED RID: 19181 RVA: 0x00271AE2 File Offset: 0x0026FEE2
		public SummaryHealthHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x06004AEE RID: 19182 RVA: 0x00271B04 File Offset: 0x0026FF04
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

		// Token: 0x06004AEF RID: 19183 RVA: 0x00271C2E File Offset: 0x0027002E
		public void Notify_HealthChanged()
		{
			this.dirty = true;
		}
	}
}
