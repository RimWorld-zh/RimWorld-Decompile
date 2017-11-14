using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_ConditionalAnyUndownedColonistSpawnedNearby : ThinkNode_Conditional
	{
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Spawned && pawn.Map.mapPawns.FreeColonistsSpawned.Any((Pawn x) => !x.Downed);
		}
	}
}
