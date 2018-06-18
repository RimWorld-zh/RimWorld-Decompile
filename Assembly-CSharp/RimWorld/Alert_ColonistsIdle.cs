using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A2 RID: 1954
	public class Alert_ColonistsIdle : Alert
	{
		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06002B3D RID: 11069 RVA: 0x0016D500 File Offset: 0x0016B900
		private IEnumerable<Pawn> IdleColonists
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].IsPlayerHome)
					{
						foreach (Pawn p in maps[i].mapPawns.FreeColonistsSpawned)
						{
							if (p.mindState.IsIdle)
							{
								yield return p;
							}
						}
					}
				}
				yield break;
			}
		}

		// Token: 0x06002B3E RID: 11070 RVA: 0x0016D524 File Offset: 0x0016B924
		public override string GetLabel()
		{
			return string.Format("ColonistsIdle".Translate(), this.IdleColonists.Count<Pawn>().ToStringCached());
		}

		// Token: 0x06002B3F RID: 11071 RVA: 0x0016D558 File Offset: 0x0016B958
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.IdleColonists)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort.CapitalizeFirst());
			}
			return string.Format("ColonistsIdleDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x06002B40 RID: 11072 RVA: 0x0016D5EC File Offset: 0x0016B9EC
		public override AlertReport GetReport()
		{
			AlertReport result;
			if (GenDate.DaysPassed < 1)
			{
				result = false;
			}
			else
			{
				result = AlertReport.CulpritsAre(this.IdleColonists);
			}
			return result;
		}

		// Token: 0x04001737 RID: 5943
		public const int MinDaysPassed = 1;
	}
}
