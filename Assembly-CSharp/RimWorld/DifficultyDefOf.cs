using System;

namespace RimWorld
{
	[DefOf]
	public static class DifficultyDefOf
	{
		public static DifficultyDef Easy;

		public static DifficultyDef Hard;

		public static DifficultyDef ExtraHard;

		static DifficultyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DifficultyDefOf));
		}
	}
}
