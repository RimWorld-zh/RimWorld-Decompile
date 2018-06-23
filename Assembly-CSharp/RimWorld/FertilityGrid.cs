using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000384 RID: 900
	public sealed class FertilityGrid
	{
		// Token: 0x04000991 RID: 2449
		private Map map;

		// Token: 0x06000F98 RID: 3992 RVA: 0x00083BA1 File Offset: 0x00081FA1
		public FertilityGrid(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x00083BB4 File Offset: 0x00081FB4
		public float FertilityAt(IntVec3 loc)
		{
			return this.CalculateFertilityAt(loc);
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x00083BD0 File Offset: 0x00081FD0
		private float CalculateFertilityAt(IntVec3 loc)
		{
			Thing edifice = loc.GetEdifice(this.map);
			float fertility;
			if (edifice != null && edifice.def.fertility >= 0f)
			{
				fertility = edifice.def.fertility;
			}
			else
			{
				fertility = this.map.terrainGrid.TerrainAt(loc).fertility;
			}
			return fertility;
		}
	}
}
