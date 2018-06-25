using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D48 RID: 3400
	public class SummaryHealthHandler
	{
		// Token: 0x04003282 RID: 12930
		private Pawn pawn;

		// Token: 0x04003283 RID: 12931
		private float cachedSummaryHealthPercent = 1f;

		// Token: 0x04003284 RID: 12932
		private bool dirty = true;

		// Token: 0x06004AED RID: 19181 RVA: 0x00271DC2 File Offset: 0x002701C2
		public SummaryHealthHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x06004AEE RID: 19182 RVA: 0x00271DE4 File Offset: 0x002701E4
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

		// Token: 0x06004AEF RID: 19183 RVA: 0x00271F0E File Offset: 0x0027030E
		public void Notify_HealthChanged()
		{
			this.dirty = true;
		}
	}
}
