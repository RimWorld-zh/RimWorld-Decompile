using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public class GenStep_DownedRefugee : GenStep_Scatterer
	{
		public GenStep_DownedRefugee()
		{
		}

		public override int SeedPart
		{
			get
			{
				return 931842770;
			}
		}

		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			return base.CanScatterAt(c, map) && c.Standable(map);
		}

		protected override void ScatterAt(IntVec3 loc, Map map, int count = 1)
		{
			DownedRefugeeComp component = map.Parent.GetComponent<DownedRefugeeComp>();
			Pawn pawn;
			if (component != null && component.pawn.Any)
			{
				pawn = component.pawn.Take(component.pawn[0]);
			}
			else
			{
				pawn = DownedRefugeeQuestUtility.GenerateRefugee(map.Tile);
			}
			HealthUtility.DamageUntilDowned(pawn, false);
			HealthUtility.DamageLegsUntilIncapableOfMoving(pawn, false);
			GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
			pawn.mindState.willJoinColonyIfRescued = true;
			MapGenerator.rootsToUnfog.Add(loc);
		}
	}
}
