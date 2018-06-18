using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200078F RID: 1935
	public class Alert_ColonistNeedsRescuing : Alert_Critical
	{
		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x06002AE7 RID: 10983 RVA: 0x0016A4F4 File Offset: 0x001688F4
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

		// Token: 0x06002AE8 RID: 10984 RVA: 0x0016A518 File Offset: 0x00168918
		public static bool NeedsRescue(Pawn p)
		{
			return p.Downed && !p.InBed() && !(p.ParentHolder is Pawn_CarryTracker) && (p.jobs.jobQueue == null || p.jobs.jobQueue.Count <= 0 || !p.jobs.jobQueue.Peek().job.CanBeginNow(p, false));
		}

		// Token: 0x06002AE9 RID: 10985 RVA: 0x0016A5AC File Offset: 0x001689AC
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

		// Token: 0x06002AEA RID: 10986 RVA: 0x0016A5EC File Offset: 0x001689EC
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.ColonistsNeedingRescue)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort);
			}
			return string.Format("ColonistsNeedRescueDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x06002AEB RID: 10987 RVA: 0x0016A67C File Offset: 0x00168A7C
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ColonistsNeedingRescue);
		}
	}
}
