using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000386 RID: 902
	public sealed class FertilityGrid
	{
		// Token: 0x04000994 RID: 2452
		private Map map;

		// Token: 0x06000F9B RID: 3995 RVA: 0x00083D01 File Offset: 0x00082101
		public FertilityGrid(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x00083D14 File Offset: 0x00082114
		public float FertilityAt(IntVec3 loc)
		{
			return this.CalculateFertilityAt(loc);
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x00083D30 File Offset: 0x00082130
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
