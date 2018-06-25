using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200079E RID: 1950
	public class Alert_Exhaustion : Alert
	{
		// Token: 0x06002B31 RID: 11057 RVA: 0x0016D349 File Offset: 0x0016B749
		public Alert_Exhaustion()
		{
			this.defaultLabel = "Exhaustion".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x06002B32 RID: 11058 RVA: 0x0016D36C File Offset: 0x0016B76C
		private IEnumerable<Pawn> ExhaustedColonists
		{
			get
			{
				return from p in PawnsFinder.AllMaps_FreeColonistsSpawned
				where p.needs.rest != null && p.needs.rest.CurCategory == RestCategory.Exhausted
				select p;
			}
		}

		// Token: 0x06002B33 RID: 11059 RVA: 0x0016D3A8 File Offset: 0x0016B7A8
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.ExhaustedColonists)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort);
			}
			return string.Format("ExhaustionDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x06002B34 RID: 11060 RVA: 0x0016D438 File Offset: 0x0016B838
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ExhaustedColonists);
		}
	}
}
