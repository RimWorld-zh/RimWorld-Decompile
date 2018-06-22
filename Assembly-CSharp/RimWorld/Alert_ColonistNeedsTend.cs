using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200079A RID: 1946
	public class Alert_ColonistNeedsTend : Alert
	{
		// Token: 0x06002B25 RID: 11045 RVA: 0x0016CC0D File Offset: 0x0016B00D
		public Alert_ColonistNeedsTend()
		{
			this.defaultLabel = "ColonistNeedsTreatment".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x06002B26 RID: 11046 RVA: 0x0016CC30 File Offset: 0x0016B030
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

		// Token: 0x06002B27 RID: 11047 RVA: 0x0016CC54 File Offset: 0x0016B054
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.NeedingColonists)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort);
			}
			return string.Format("ColonistNeedsTreatmentDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x06002B28 RID: 11048 RVA: 0x0016CCE4 File Offset: 0x0016B0E4
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.NeedingColonists);
		}
	}
}
