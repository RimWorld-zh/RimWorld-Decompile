using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200079E RID: 1950
	public class Alert_ColonistNeedsTend : Alert
	{
		// Token: 0x06002B2A RID: 11050 RVA: 0x0016C9A1 File Offset: 0x0016ADA1
		public Alert_ColonistNeedsTend()
		{
			this.defaultLabel = "ColonistNeedsTreatment".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x06002B2B RID: 11051 RVA: 0x0016C9C4 File Offset: 0x0016ADC4
		private IEnumerable<Pawn> NeedingColonists
		{
			get
			{
				foreach (Pawn p in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if (p.health.HasHediffsNeedingTendByPlayer(true))
					{
						Building_Bed curBed = p.CurrentBed();
						if (curBed == null || !curBed.Medical)
						{
							if (!Alert_ColonistNeedsRescuing.NeedsRescue(p))
							{
								yield return p;
							}
						}
					}
				}
				yield break;
			}
		}

		// Token: 0x06002B2C RID: 11052 RVA: 0x0016C9E8 File Offset: 0x0016ADE8
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.NeedingColonists)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort);
			}
			return string.Format("ColonistNeedsTreatmentDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x06002B2D RID: 11053 RVA: 0x0016CA78 File Offset: 0x0016AE78
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.NeedingColonists);
		}
	}
}
