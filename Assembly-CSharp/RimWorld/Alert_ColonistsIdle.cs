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
							}
						}
					}
				}
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
			if (GenDate.DaysPassed < 1)
			{
				return AlertReport.Inactive;
			}
			return (Thing)this.IdleColonists.FirstOrDefault();
		}
	}
}
