using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	public static class BreakRiskAlertUtility
	{
		public static IEnumerable<Pawn> PawnsAtRiskExtreme
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
							if (!p.Downed && p.mindState.mentalBreaker.BreakExtremeIsImminent)
								break;
							continue;
						}
						yield break;
					}
					yield return p;
					/*Error: Unable to find new state assignment for yield return*/;
				}
				IL_00e1:
				/*Error near IL_00e2: Unexpected return in MoveNext()*/;
			}
		}

		public static IEnumerable<Pawn> PawnsAtRiskMajor
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
							if (!p.Downed && p.mindState.mentalBreaker.BreakMajorIsImminent)
								break;
							continue;
						}
						yield break;
					}
					yield return p;
					/*Error: Unable to find new state assignment for yield return*/;
				}
				IL_00e1:
				/*Error near IL_00e2: Unexpected return in MoveNext()*/;
			}
		}

		public static IEnumerable<Pawn> PawnsAtRiskMinor
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
							if (!p.Downed && p.mindState.mentalBreaker.BreakMinorIsImminent)
								break;
							continue;
						}
						yield break;
					}
					yield return p;
					/*Error: Unable to find new state assignment for yield return*/;
				}
				IL_00e1:
				/*Error near IL_00e2: Unexpected return in MoveNext()*/;
			}
		}

		public static string AlertLabel
		{
			get
			{
				int num = BreakRiskAlertUtility.PawnsAtRiskExtreme.Count();
				string text;
				if (num > 0)
				{
					text = "BreakRiskExtreme".Translate();
				}
				else
				{
					num = BreakRiskAlertUtility.PawnsAtRiskMajor.Count();
					if (num > 0)
					{
						text = "BreakRiskMajor".Translate();
					}
					else
					{
						num = BreakRiskAlertUtility.PawnsAtRiskMinor.Count();
						text = "BreakRiskMinor".Translate();
					}
				}
				if (num > 1)
				{
					text = text + " x" + num.ToStringCached();
				}
				return text;
			}
		}

		public static string AlertExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (BreakRiskAlertUtility.PawnsAtRiskExtreme.Any())
				{
					StringBuilder stringBuilder2 = new StringBuilder();
					foreach (Pawn item in BreakRiskAlertUtility.PawnsAtRiskExtreme)
					{
						stringBuilder2.AppendLine("    " + item.NameStringShort);
					}
					stringBuilder.Append("BreakRiskExtremeDesc".Translate(stringBuilder2));
				}
				if (BreakRiskAlertUtility.PawnsAtRiskMajor.Any())
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					StringBuilder stringBuilder3 = new StringBuilder();
					foreach (Pawn item2 in BreakRiskAlertUtility.PawnsAtRiskMajor)
					{
						stringBuilder3.AppendLine("    " + item2.NameStringShort);
					}
					stringBuilder.Append("BreakRiskMajorDesc".Translate(stringBuilder3));
				}
				if (BreakRiskAlertUtility.PawnsAtRiskMinor.Any())
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					StringBuilder stringBuilder4 = new StringBuilder();
					foreach (Pawn item3 in BreakRiskAlertUtility.PawnsAtRiskMinor)
					{
						stringBuilder4.AppendLine("    " + item3.NameStringShort);
					}
					stringBuilder.Append("BreakRiskMinorDesc".Translate(stringBuilder4));
				}
				stringBuilder.AppendLine();
				stringBuilder.Append("BreakRiskDescEnding".Translate());
				return stringBuilder.ToString();
			}
		}
	}
}
