using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	public class Alert_ColonistNeedsTend : Alert
	{
		private IEnumerable<Pawn> NeedingColonists
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
							if (p.health.HasHediffsNeedingTendByColony(true))
							{
								Building_Bed curBed = p.CurrentBed();
								if ((curBed == null || !curBed.Medical) && !Alert_ColonistNeedsRescuing.NeedsRescue(p))
									break;
							}
							continue;
						}
						yield break;
					}
					yield return p;
					/*Error: Unable to find new state assignment for yield return*/;
				}
				IL_0118:
				/*Error near IL_0119: Unexpected return in MoveNext()*/;
			}
		}

		public Alert_ColonistNeedsTend()
		{
			base.defaultLabel = "ColonistNeedsTreatment".Translate();
			base.defaultPriority = AlertPriority.High;
		}

		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn needingColonist in this.NeedingColonists)
			{
				stringBuilder.AppendLine("    " + needingColonist.NameStringShort);
			}
			return string.Format("ColonistNeedsTreatmentDesc".Translate(), stringBuilder.ToString());
		}

		public override AlertReport GetReport()
		{
			Pawn pawn = this.NeedingColonists.FirstOrDefault();
			return (pawn != null) ? AlertReport.CulpritIs((Thing)pawn) : false;
		}
	}
}
