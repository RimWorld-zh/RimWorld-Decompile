using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D49 RID: 3401
	public class SummaryHealthHandler
	{
		// Token: 0x06004AD7 RID: 19159 RVA: 0x00270482 File Offset: 0x0026E882
		public SummaryHealthHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x06004AD8 RID: 19160 RVA: 0x002704A4 File Offset: 0x0026E8A4
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

		// Token: 0x06004AD9 RID: 19161 RVA: 0x002705CE File Offset: 0x0026E9CE
		public void Notify_HealthChanged()
		{
			this.dirty = true;
		}

		// Token: 0x04003272 RID: 12914
		private Pawn pawn;

		// Token: 0x04003273 RID: 12915
		private float cachedSummaryHealthPercent = 1f;

		// Token: 0x04003274 RID: 12916
		private bool dirty = true;
	}
}
