using System;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001E6 RID: 486
	public class ThinkNode_ConditionalAnyUndownedColonistSpawnedNearby : ThinkNode_Conditional
	{
		// Token: 0x06000986 RID: 2438 RVA: 0x00056B2C File Offset: 0x00054F2C
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
