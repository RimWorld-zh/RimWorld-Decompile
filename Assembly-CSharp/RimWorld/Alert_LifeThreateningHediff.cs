using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	public class Alert_LifeThreateningHediff : Alert_Critical
	{
		private IEnumerable<Pawn> SickPawns
		{
			get
			{
				foreach (Pawn item in PawnsFinder.AllMaps_FreeColonistsAndPrisonersSpawned)
				{
					List<Hediff>.Enumerator enumerator2 = item.health.hediffSet.hediffs.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							Hediff diff = enumerator2.Current;
							if (diff.CurStage != null && diff.CurStage.lifeThreatening && !diff.FullyImmune())
							{
								yield return item;
								break;
							}
						}
					}
					finally
					{
						((IDisposable)(object)enumerator2).Dispose();
					}
				}
			}
		}

		public override string GetLabel()
		{
			return "PawnsWithLifeThreateningDisease".Translate();
		}

		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			foreach (Pawn sickPawn in this.SickPawns)
			{
				stringBuilder.AppendLine("    " + sickPawn.NameStringShort);
				List<Hediff>.Enumerator enumerator2 = sickPawn.health.hediffSet.hediffs.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						Hediff current2 = enumerator2.Current;
						if (current2.CurStage != null && current2.CurStage.lifeThreatening && current2.Part != null && current2.Part != sickPawn.RaceProps.body.corePart)
						{
							flag = true;
							break;
						}
					}
				}
				finally
				{
					((IDisposable)(object)enumerator2).Dispose();
				}
			}
			if (flag)
			{
				return string.Format("PawnsWithLifeThreateningDiseaseAmputationDesc".Translate(), stringBuilder.ToString());
			}
			return string.Format("PawnsWithLifeThreateningDiseaseDesc".Translate(), stringBuilder.ToString());
		}

		public override AlertReport GetReport()
		{
			return AlertReport.CulpritIs((Thing)this.SickPawns.FirstOrDefault());
		}
	}
}
