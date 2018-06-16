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
		// Token: 0x06002B32 RID: 11058 RVA: 0x0016CF8D File Offset: 0x0016B38D
		public Alert_Exhaustion()
		{
			this.defaultLabel = "Exhaustion".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x06002B33 RID: 11059 RVA: 0x0016CFB0 File Offset: 0x0016B3B0
		private IEnumerable<Pawn> ExhaustedColonists
		{
			get
			{
				return from p in PawnsFinder.AllMaps_FreeColonistsSpawned
				where p.needs.rest != null && p.needs.rest.CurCategory == RestCategory.Exhausted
				select p;
			}
		}

		// Token: 0x06002B34 RID: 11060 RVA: 0x0016CFEC File Offset: 0x0016B3EC
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.ExhaustedColonists)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort);
			}
			return string.Format("ExhaustionDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x06002B35 RID: 11061 RVA: 0x0016D07C File Offset: 0x0016B47C
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ExhaustedColonists);
		}
	}
}
