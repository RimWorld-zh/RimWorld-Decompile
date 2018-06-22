using System;
using System.Collections;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020004E2 RID: 1250
	public static class PawnNameDatabaseSolid
	{
		// Token: 0x0600164F RID: 5711 RVA: 0x000C6290 File Offset: 0x000C4690
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

		// Token: 0x06001650 RID: 5712 RVA: 0x000C6318 File Offset: 0x000C4718
		public static void AddPlayerContentName(NameTriple newName, GenderPossibility genderPos)
		{
			PawnNameDatabaseSolid.solidNames[genderPos].Add(newName);
		}

		// Token: 0x06001651 RID: 5713 RVA: 0x000C632C File Offset: 0x000C472C
		public static List<NameTriple> GetListForGender(GenderPossibility gp)
		{
			return PawnNameDatabaseSolid.solidNames[gp];
		}

		// Token: 0x06001652 RID: 5714 RVA: 0x000C634C File Offset: 0x000C474C
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

		// Token: 0x04000D04 RID: 3332
		private static Dictionary<GenderPossibility, List<NameTriple>> solidNames = new Dictionary<GenderPossibility, List<NameTriple>>();

		// Token: 0x04000D05 RID: 3333
		private const float PreferredNameChance = 0.5f;
	}
}
