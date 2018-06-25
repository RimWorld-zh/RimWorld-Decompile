using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200079C RID: 1948
	public class Alert_ColonistNeedsTend : Alert
	{
		// Token: 0x06002B28 RID: 11048 RVA: 0x0016CFC1 File Offset: 0x0016B3C1
		public Alert_ColonistNeedsTend()
		{
			this.defaultLabel = "ColonistNeedsTreatment".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x06002B29 RID: 11049 RVA: 0x0016CFE4 File Offset: 0x0016B3E4
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

		// Token: 0x06002B2A RID: 11050 RVA: 0x0016D008 File Offset: 0x0016B408
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.NeedingColonists)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort);
			}
			return string.Format("ColonistNeedsTreatmentDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x06002B2B RID: 11051 RVA: 0x0016D098 File Offset: 0x0016B498
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.NeedingColonists);
		}
	}
}
