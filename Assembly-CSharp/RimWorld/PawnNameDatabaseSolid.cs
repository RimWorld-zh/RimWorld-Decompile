using System;
using System.Collections;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020004E6 RID: 1254
	public static class PawnNameDatabaseSolid
	{
		// Token: 0x06001657 RID: 5719 RVA: 0x000C6248 File Offset: 0x000C4648
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

		// Token: 0x06001658 RID: 5720 RVA: 0x000C62D0 File Offset: 0x000C46D0
		public static void AddPlayerContentName(NameTriple newName, GenderPossibility genderPos)
		{
			PawnNameDatabaseSolid.solidNames[genderPos].Add(newName);
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x000C62E4 File Offset: 0x000C46E4
		public static List<NameTriple> GetListForGender(GenderPossibility gp)
		{
			return PawnNameDatabaseSolid.solidNames[gp];
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x000C6304 File Offset: 0x000C4704
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

		// Token: 0x04000D07 RID: 3335
		private static Dictionary<GenderPossibility, List<NameTriple>> solidNames = new Dictionary<GenderPossibility, List<NameTriple>>();

		// Token: 0x04000D08 RID: 3336
		private const float PreferredNameChance = 0.5f;
	}
}
