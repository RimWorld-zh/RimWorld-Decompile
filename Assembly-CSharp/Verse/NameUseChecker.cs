using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000C32 RID: 3122
	public static class NameUseChecker
	{
		// Token: 0x17000AD3 RID: 2771
		// (get) Token: 0x060044C2 RID: 17602 RVA: 0x0024294C File Offset: 0x00240D4C
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

		// Token: 0x060044C3 RID: 17603 RVA: 0x00242970 File Offset: 0x00240D70
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

		// Token: 0x060044C4 RID: 17604 RVA: 0x00242A3C File Offset: 0x00240E3C
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
