using System;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001E6 RID: 486
	public class ThinkNode_ConditionalAnyUndownedColonistSpawnedNearby : ThinkNode_Conditional
	{
		// Token: 0x06000985 RID: 2437 RVA: 0x00056B28 File Offset: 0x00054F28
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
	}
}
