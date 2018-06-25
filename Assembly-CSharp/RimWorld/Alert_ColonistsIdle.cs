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
		// Token: 0x04001735 RID: 5941
		public const int MinDaysPassed = 1;

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06002B3A RID: 11066 RVA: 0x0016D828 File Offset: 0x0016BC28
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

		// Token: 0x06002B3B RID: 11067 RVA: 0x0016D84C File Offset: 0x0016BC4C
		public override string GetLabel()
		{
			return string.Format("ColonistsIdle".Translate(), this.IdleColonists.Count<Pawn>().ToStringCached());
		}

		// Token: 0x06002B3C RID: 11068 RVA: 0x0016D880 File Offset: 0x0016BC80
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.IdleColonists)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort.CapitalizeFirst());
			}
			return string.Format("ColonistsIdleDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x06002B3D RID: 11069 RVA: 0x0016D914 File Offset: 0x0016BD14
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
