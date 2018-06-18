using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D48 RID: 3400
	public class SummaryHealthHandler
	{
		// Token: 0x06004AD5 RID: 19157 RVA: 0x0027045A File Offset: 0x0026E85A
		public SummaryHealthHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000BF1 RID: 3057
		// (get) Token: 0x06004AD6 RID: 19158 RVA: 0x0027047C File Offset: 0x0026E87C
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

		// Token: 0x06004AD7 RID: 19159 RVA: 0x002705A6 File Offset: 0x0026E9A6
		public void Notify_HealthChanged()
		{
			this.dirty = true;
		}

		// Token: 0x04003270 RID: 12912
		private Pawn pawn;

		// Token: 0x04003271 RID: 12913
		private float cachedSummaryHealthPercent = 1f;

		// Token: 0x04003272 RID: 12914
		private bool dirty = true;
	}
}
