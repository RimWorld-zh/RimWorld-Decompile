using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000386 RID: 902
	public sealed class FertilityGrid
	{
		// Token: 0x04000991 RID: 2449
		private Map map;

		// Token: 0x06000F9C RID: 3996 RVA: 0x00083CF1 File Offset: 0x000820F1
		public FertilityGrid(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x00083D04 File Offset: 0x00082104
		public float FertilityAt(IntVec3 loc)
		{
			return this.CalculateFertilityAt(loc);
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x00083D20 File Offset: 0x00082120
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
