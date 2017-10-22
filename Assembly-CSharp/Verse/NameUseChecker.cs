using RimWorld;
using System.Collections.Generic;

namespace Verse
{
	public static class NameUseChecker
	{
		public static IEnumerable<Name> AllPawnsNamesEverUsed
		{
			get
			{
				using (IEnumerator<Pawn> enumerator = PawnsFinder.AllMapsAndWorld_AliveOrDead.GetEnumerator())
				{
					Pawn p;
					while (true)
					{
						if (enumerator.MoveNext())
						{
							p = enumerator.Current;
							if (p.Name != null)
								break;
							continue;
						}
						yield break;
					}
					yield return p.Name;
					/*Error: Unable to find new state assignment for yield return*/;
				}
				IL_00cc:
				/*Error near IL_00cd: Unexpected return in MoveNext()*/;
			}
		}

		public static bool NameWordIsUsed(string singleName)
		{
			foreach (Name item in NameUseChecker.AllPawnsNamesEverUsed)
			{
				NameTriple nameTriple = item as NameTriple;
				if (nameTriple != null && (singleName == nameTriple.First || singleName == nameTriple.Nick || singleName == nameTriple.Last))
				{
					return true;
				}
				NameSingle nameSingle = item as NameSingle;
				if (nameSingle != null && nameSingle.Name == singleName)
				{
					return true;
				}
			}
			return false;
		}

		public static bool NameSingleIsUsed(string candidate)
		{
			foreach (Pawn item in PawnsFinder.AllMapsAndWorld_AliveOrDead)
			{
				NameSingle nameSingle = item.Name as NameSingle;
				if (nameSingle != null && nameSingle.Name == candidate)
				{
					return true;
				}
			}
			return false;
		}
	}
}
