using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	public class Alert_ColonistsIdle : Alert
	{
		public const int MinDaysPassed = 1;

		private IEnumerable<Pawn> IdleColonists
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].IsPlayerHome)
					{
						foreach (Pawn item in maps[i].mapPawns.FreeColonistsSpawned)
						{
							if (item.mindState.IsIdle)
							{
								yield return item;
								/*Error: Unable to find new state assignment for yield return*/;
							}
						}
					}
				}
				yield break;
				IL_013c:
				/*Error near IL_013d: Unexpected return in MoveNext()*/;
			}
		}

		public override string GetLabel()
		{
			return string.Format("ColonistsIdle".Translate(), this.IdleColonists.Count().ToStringCached());
		}

		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn idleColonist in this.IdleColonists)
			{
				stringBuilder.AppendLine("    " + idleColonist.NameStringShort);
			}
			return string.Format("ColonistsIdleDesc".Translate(), stringBuilder.ToString());
		}

		public override AlertReport GetReport()
		{
			return (GenDate.DaysPassed >= 1) ? ((Thing)this.IdleColonists.FirstOrDefault()) : AlertReport.Inactive;
		}
	}
}
