using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000409 RID: 1033
	public class GenStep_DownedRefugee : GenStep_Scatterer
	{
		// Token: 0x1700025E RID: 606
		// (get) Token: 0x060011C4 RID: 4548 RVA: 0x0009A71C File Offset: 0x00098B1C
		public override int SeedPart
		{
			get
			{
				return 931842770;
			}
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x0009A738 File Offset: 0x00098B38
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			return base.CanScatterAt(c, map) && c.Standable(map);
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x0009A764 File Offset: 0x00098B64
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
			GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
			pawn.mindState.willJoinColonyIfRescued = true;
			MapGenerator.rootsToUnfog.Add(loc);
		}
	}
}
