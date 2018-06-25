using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A0 RID: 1952
	public class Alert_ColonistsIdle : Alert
	{
		// Token: 0x04001739 RID: 5945
		public const int MinDaysPassed = 1;

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06002B39 RID: 11065 RVA: 0x0016DA8C File Offset: 0x0016BE8C
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

		// Token: 0x06002B3A RID: 11066 RVA: 0x0016DAB0 File Offset: 0x0016BEB0
		public override string GetLabel()
		{
			return string.Format("ColonistsIdle".Translate(), this.IdleColonists.Count<Pawn>().ToStringCached());
		}

		// Token: 0x06002B3B RID: 11067 RVA: 0x0016DAE4 File Offset: 0x0016BEE4
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.IdleColonists)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort.CapitalizeFirst());
			}
			return string.Format("ColonistsIdleDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x06002B3C RID: 11068 RVA: 0x0016DB78 File Offset: 0x0016BF78
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
	}
}
