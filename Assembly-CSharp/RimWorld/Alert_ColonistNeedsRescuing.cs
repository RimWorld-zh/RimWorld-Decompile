using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200078D RID: 1933
	public class Alert_ColonistNeedsRescuing : Alert_Critical
	{
		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x06002AE4 RID: 10980 RVA: 0x0016A81C File Offset: 0x00168C1C
		private IEnumerable<Pawn> ColonistsNeedingRescue
		{
			get
			{
				foreach (Pawn p in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if (Alert_ColonistNeedsRescuing.NeedsRescue(p))
					{
						yield return p;
					}
				}
				yield break;
			}
		}

		// Token: 0x06002AE5 RID: 10981 RVA: 0x0016A840 File Offset: 0x00168C40
		public static bool NeedsRescue(Pawn p)
		{
			return p.Downed && !p.InBed() && !(p.ParentHolder is Pawn_CarryTracker) && (p.jobs.jobQueue == null || p.jobs.jobQueue.Count <= 0 || !p.jobs.jobQueue.Peek().job.CanBeginNow(p, false));
		}

		// Token: 0x06002AE6 RID: 10982 RVA: 0x0016A8D4 File Offset: 0x00168CD4
		public override string GetLabel()
		{
			string result;
			if (this.ColonistsNeedingRescue.Count<Pawn>() == 1)
			{
				result = "ColonistNeedsRescue".Translate();
			}
			else
			{
				result = "ColonistsNeedRescue".Translate();
			}
			return result;
		}

		// Token: 0x06002AE7 RID: 10983 RVA: 0x0016A914 File Offset: 0x00168D14
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.ColonistsNeedingRescue)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort);
			}
			return string.Format("ColonistsNeedRescueDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x06002AE8 RID: 10984 RVA: 0x0016A9A4 File Offset: 0x00168DA4
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ColonistsNeedingRescue);
		}
	}
}
