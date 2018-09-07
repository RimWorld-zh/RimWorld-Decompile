using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_ConditionalAnyUndownedColonistSpawnedNearby : ThinkNode_Conditional
	{
		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache0;

		public ThinkNode_ConditionalAnyUndownedColonistSpawnedNearby()
		{
		}

		protected override bool Satisfied(Pawn pawn)
		{
			bool result;
			if (pawn.Spawned)
			{
				result = pawn.Map.mapPawns.FreeColonistsSpawned.Any((Pawn x) => !x.Downed);
			}
			else
			{
				result = false;
			}
			return result;
		}

		[CompilerGenerated]
		private static bool <Satisfied>m__0(Pawn x)
		{
			return !x.Downed;
		}
	}
}
