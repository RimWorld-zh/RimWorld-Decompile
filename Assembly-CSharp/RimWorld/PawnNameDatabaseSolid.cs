using System;
using System.Collections;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020004E4 RID: 1252
	public static class PawnNameDatabaseSolid
	{
		// Token: 0x04000D04 RID: 3332
		private static Dictionary<GenderPossibility, List<NameTriple>> solidNames = new Dictionary<GenderPossibility, List<NameTriple>>();

		// Token: 0x04000D05 RID: 3333
		private const float PreferredNameChance = 0.5f;

		// Token: 0x06001653 RID: 5715 RVA: 0x000C63E0 File Offset: 0x000C47E0
		static PawnNameDatabaseSolid()
		{
			IEnumerator enumerator = Enum.GetValues(typeof(GenderPossibility)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					GenderPossibility key = (GenderPossibility)obj;
					PawnNameDatabaseSolid.solidNames.Add(key, new List<NameTriple>());
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x000C6468 File Offset: 0x000C4868
		public static void AddPlayerContentName(NameTriple newName, GenderPossibility genderPos)
		{
			PawnNameDatabaseSolid.solidNames[genderPos].Add(newName);
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x000C647C File Offset: 0x000C487C
		public static List<NameTriple> GetListForGender(GenderPossibility gp)
		{
			return PawnNameDatabaseSolid.solidNames[gp];
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x000C649C File Offset: 0x000C489C
		public static IEnumerable<NameTriple> AllNames()
		{
			foreach (KeyValuePair<GenderPossibility, List<NameTriple>> kvp in PawnNameDatabaseSolid.solidNames)
			{
				foreach (NameTriple name in kvp.Value)
				{
					yield return name;
				}
			}
			yield break;
		}
	}
}
