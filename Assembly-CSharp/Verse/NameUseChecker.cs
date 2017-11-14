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
				foreach (Pawn item in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
				{
					if (item.Name != null)
					{
						yield return item.Name;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				yield break;
				IL_00c8:
				/*Error near IL_00c9: Unexpected return in MoveNext()*/;
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
			foreach (Pawn item in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
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
