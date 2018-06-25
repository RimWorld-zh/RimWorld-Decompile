using System;
using System.Collections;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020004E4 RID: 1252
	public static class PawnNameDatabaseSolid
	{
		// Token: 0x04000D07 RID: 3335
		private static Dictionary<GenderPossibility, List<NameTriple>> solidNames = new Dictionary<GenderPossibility, List<NameTriple>>();

		// Token: 0x04000D08 RID: 3336
		private const float PreferredNameChance = 0.5f;

		// Token: 0x06001652 RID: 5714 RVA: 0x000C65E0 File Offset: 0x000C49E0
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

		// Token: 0x06001653 RID: 5715 RVA: 0x000C6668 File Offset: 0x000C4A68
		public static void AddPlayerContentName(NameTriple newName, GenderPossibility genderPos)
		{
			PawnNameDatabaseSolid.solidNames[genderPos].Add(newName);
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x000C667C File Offset: 0x000C4A7C
		public static List<NameTriple> GetListForGender(GenderPossibility gp)
		{
			return PawnNameDatabaseSolid.solidNames[gp];
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x000C669C File Offset: 0x000C4A9C
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
