using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public static class PawnNameDatabaseSolid
	{
		private const float PreferredNameChance = 0.5f;

		private static Dictionary<GenderPossibility, List<NameTriple>> solidNames;

		static PawnNameDatabaseSolid()
		{
			PawnNameDatabaseSolid.solidNames = new Dictionary<GenderPossibility, List<NameTriple>>();
			foreach (byte value in Enum.GetValues(typeof(GenderPossibility)))
			{
				PawnNameDatabaseSolid.solidNames.Add((GenderPossibility)value, new List<NameTriple>());
			}
		}

		public static void AddPlayerContentName(NameTriple newName, GenderPossibility genderPos)
		{
			PawnNameDatabaseSolid.solidNames[genderPos].Add(newName);
		}

		public static List<NameTriple> GetListForGender(GenderPossibility gp)
		{
			return PawnNameDatabaseSolid.solidNames[gp];
		}

		public static IEnumerable<NameTriple> AllNames()
		{
			Dictionary<GenderPossibility, List<NameTriple>>.Enumerator enumerator = PawnNameDatabaseSolid.solidNames.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					List<NameTriple>.Enumerator enumerator2 = enumerator.Current.Value.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							NameTriple name = enumerator2.Current;
							yield return name;
						}
					}
					finally
					{
						((IDisposable)(object)enumerator2).Dispose();
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
		}
	}
}
