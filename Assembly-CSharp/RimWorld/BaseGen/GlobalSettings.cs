using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02000392 RID: 914
	public class GlobalSettings
	{
		// Token: 0x040009B2 RID: 2482
		public Map map;

		// Token: 0x040009B3 RID: 2483
		public int minBuildings;

		// Token: 0x040009B4 RID: 2484
		public int minEmptyNodes;

		// Token: 0x040009B5 RID: 2485
		public int minBarracks;

		// Token: 0x040009B6 RID: 2486
		public CellRect mainRect;

		// Token: 0x040009B7 RID: 2487
		public int basePart_buildingsResolved;

		// Token: 0x040009B8 RID: 2488
		public int basePart_emptyNodesResolved;

		// Token: 0x040009B9 RID: 2489
		public int basePart_barracksResolved;

		// Token: 0x040009BA RID: 2490
		public float basePart_batteriesCoverage;

		// Token: 0x040009BB RID: 2491
		public float basePart_farmsCoverage;

		// Token: 0x040009BC RID: 2492
		public float basePart_powerPlantsCoverage;

		// Token: 0x040009BD RID: 2493
		public float basePart_breweriesCoverage;

		// Token: 0x06000FF5 RID: 4085 RVA: 0x00085E30 File Offset: 0x00084230
		public void Clear()
		{
			this.map = null;
			this.minBuildings = 0;
			this.minBarracks = 0;
			this.minEmptyNodes = 0;
			this.mainRect = CellRect.Empty;
			this.basePart_buildingsResolved = 0;
			this.basePart_emptyNodesResolved = 0;
			this.basePart_barracksResolved = 0;
			this.basePart_batteriesCoverage = 0f;
			this.basePart_farmsCoverage = 0f;
			this.basePart_powerPlantsCoverage = 0f;
			this.basePart_breweriesCoverage = 0f;
		}
	}
}
