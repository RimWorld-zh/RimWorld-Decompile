using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000384 RID: 900
	public sealed class FertilityGrid
	{
		// Token: 0x06000F98 RID: 3992 RVA: 0x000839B5 File Offset: 0x00081DB5
		public FertilityGrid(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x000839C8 File Offset: 0x00081DC8
		public float FertilityAt(IntVec3 loc)
		{
			return this.CalculateFertilityAt(loc);
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x000839E4 File Offset: 0x00081DE4
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

		// Token: 0x0400098F RID: 2447
		private Map map;
	}
}
