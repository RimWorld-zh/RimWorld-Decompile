using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A0 RID: 1952
	public class Alert_Exhaustion : Alert
	{
		// Token: 0x06002B34 RID: 11060 RVA: 0x0016D021 File Offset: 0x0016B421
		public Alert_Exhaustion()
		{
			this.defaultLabel = "Exhaustion".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x06002B35 RID: 11061 RVA: 0x0016D044 File Offset: 0x0016B444
		private IEnumerable<Pawn> ExhaustedColonists
		{
			get
			{
				return from p in PawnsFinder.AllMaps_FreeColonistsSpawned
				where p.needs.rest != null && p.needs.rest.CurCategory == RestCategory.Exhausted
				select p;
			}
		}

		// Token: 0x06002B36 RID: 11062 RVA: 0x0016D080 File Offset: 0x0016B480
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.ExhaustedColonists)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort);
			}
			return string.Format("ExhaustionDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x06002B37 RID: 11063 RVA: 0x0016D110 File Offset: 0x0016B510
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ExhaustedColonists);
		}
	}
}
