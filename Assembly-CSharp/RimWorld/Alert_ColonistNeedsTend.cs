using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200079C RID: 1948
	public class Alert_ColonistNeedsTend : Alert
	{
		// Token: 0x06002B29 RID: 11049 RVA: 0x0016CD5D File Offset: 0x0016B15D
		public Alert_ColonistNeedsTend()
		{
			this.defaultLabel = "ColonistNeedsTreatment".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x06002B2A RID: 11050 RVA: 0x0016CD80 File Offset: 0x0016B180
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

		// Token: 0x06002B2B RID: 11051 RVA: 0x0016CDA4 File Offset: 0x0016B1A4
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.NeedingColonists)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort);
			}
			return string.Format("ColonistNeedsTreatmentDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x06002B2C RID: 11052 RVA: 0x0016CE34 File Offset: 0x0016B234
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.NeedingColonists);
		}
	}
}
