using RimWorld;
using System;
using System.Collections.Generic;

namespace Verse
{
	public static class NameUseChecker
	{
		public static IEnumerable<Name> AllPawnsNamesEverUsed
		{
			get
			{
				NameUseChecker.<>c__Iterator1F5 <>c__Iterator1F = new NameUseChecker.<>c__Iterator1F5();
				NameUseChecker.<>c__Iterator1F5 expr_07 = <>c__Iterator1F;
				expr_07.$PC = -2;
				return expr_07;
			}
		}

		public static bool NameWordIsUsed(string singleName)
		{
			foreach (Name current in NameUseChecker.AllPawnsNamesEverUsed)
			{
				NameTriple nameTriple = current as NameTriple;
				if (nameTriple != null && (singleName == nameTriple.First || singleName == nameTriple.Nick || singleName == nameTriple.Last))
				{
					bool result = true;
					return result;
				}
				NameSingle nameSingle = current as NameSingle;
				if (nameSingle != null && nameSingle.Name == singleName)
				{
					bool result = true;
					return result;
				}
			}
			return false;
		}

		public static bool NameSingleIsUsed(string candidate)
		{
			foreach (Pawn current in PawnsFinder.AllMapsAndWorld_AliveOrDead)
			{
				NameSingle nameSingle = current.Name as NameSingle;
				if (nameSingle != null && nameSingle.Name == candidate)
				{
					return true;
				}
			}
			return false;
		}
	}
}
