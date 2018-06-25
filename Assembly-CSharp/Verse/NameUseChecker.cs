using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000C35 RID: 3125
	public static class NameUseChecker
	{
		// Token: 0x17000AD2 RID: 2770
		// (get) Token: 0x060044C5 RID: 17605 RVA: 0x00242D08 File Offset: 0x00241108
		public static IEnumerable<Name> AllPawnsNamesEverUsed
		{
			get
			{
				foreach (Pawn p in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
				{
					if (p.Name != null)
					{
						yield return p.Name;
					}
				}
				yield break;
			}
		}

		// Token: 0x060044C6 RID: 17606 RVA: 0x00242D2C File Offset: 0x0024112C
		public static bool NameWordIsUsed(string singleName)
		{
			foreach (Name name in NameUseChecker.AllPawnsNamesEverUsed)
			{
				NameTriple nameTriple = name as NameTriple;
				if (nameTriple != null && (singleName == nameTriple.First || singleName == nameTriple.Nick || singleName == nameTriple.Last))
				{
					return true;
				}
				NameSingle nameSingle = name as NameSingle;
				if (nameSingle != null && nameSingle.Name == singleName)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060044C7 RID: 17607 RVA: 0x00242DF8 File Offset: 0x002411F8
		public static bool NameSingleIsUsed(string candidate)
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
			{
				NameSingle nameSingle = pawn.Name as NameSingle;
				if (nameSingle != null && nameSingle.Name == candidate)
				{
					return true;
				}
			}
			return false;
		}
	}
}
