using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	public class Alert_ColonistNeedsRescuing : Alert_Critical
	{
		private IEnumerable<Pawn> ColonistsNeedingRescue
		{
			get
			{
				using (IEnumerator<Pawn> enumerator = PawnsFinder.AllMaps_FreeColonistsSpawned.GetEnumerator())
				{
					Pawn p;
					while (true)
					{
						if (enumerator.MoveNext())
						{
							p = enumerator.Current;
							if (Alert_ColonistNeedsRescuing.NeedsRescue(p))
								break;
							continue;
						}
						yield break;
					}
					yield return p;
					/*Error: Unable to find new state assignment for yield return*/;
				}
				IL_00c7:
				/*Error near IL_00c8: Unexpected return in MoveNext()*/;
			}
		}

		public static bool NeedsRescue(Pawn p)
		{
			return (byte)((p.Downed && !p.InBed() && !(p.ParentHolder is Pawn_CarryTracker)) ? ((p.jobs.jobQueue == null || p.jobs.jobQueue.Count <= 0 || !p.jobs.jobQueue.Peek().job.CanBeginNow(p)) ? 1 : 0) : 0) != 0;
		}

		public override string GetLabel()
		{
			return (this.ColonistsNeedingRescue.Count() != 1) ? "ColonistsNeedRescue".Translate() : "ColonistNeedsRescue".Translate();
		}

		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn item in this.ColonistsNeedingRescue)
			{
				stringBuilder.AppendLine("    " + item.NameStringShort);
			}
			return string.Format("ColonistsNeedRescueDesc".Translate(), stringBuilder.ToString());
		}

		public override AlertReport GetReport()
		{
			return AlertReport.CulpritIs((Thing)this.ColonistsNeedingRescue.FirstOrDefault());
		}
	}
}
