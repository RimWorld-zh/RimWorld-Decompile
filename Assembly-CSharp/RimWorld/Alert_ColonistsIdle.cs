using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200079E RID: 1950
	public class Alert_ColonistsIdle : Alert
	{
		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06002B36 RID: 11062 RVA: 0x0016D6D8 File Offset: 0x0016BAD8
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

		// Token: 0x06002B37 RID: 11063 RVA: 0x0016D6FC File Offset: 0x0016BAFC
		public override string GetLabel()
		{
			return string.Format("ColonistsIdle".Translate(), this.IdleColonists.Count<Pawn>().ToStringCached());
		}

		// Token: 0x06002B38 RID: 11064 RVA: 0x0016D730 File Offset: 0x0016BB30
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.IdleColonists)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort.CapitalizeFirst());
			}
			return string.Format("ColonistsIdleDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x06002B39 RID: 11065 RVA: 0x0016D7C4 File Offset: 0x0016BBC4
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

		// Token: 0x04001735 RID: 5941
		public const int MinDaysPassed = 1;
	}
}
