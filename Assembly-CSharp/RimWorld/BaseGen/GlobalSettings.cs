using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02000390 RID: 912
	public class GlobalSettings
	{
		// Token: 0x06000FF2 RID: 4082 RVA: 0x00085AE4 File Offset: 0x00083EE4
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

		// Token: 0x040009AD RID: 2477
		public Map map;

		// Token: 0x040009AE RID: 2478
		public int minBuildings;

		// Token: 0x040009AF RID: 2479
		public int minEmptyNodes;

		// Token: 0x040009B0 RID: 2480
		public int minBarracks;

		// Token: 0x040009B1 RID: 2481
		public CellRect mainRect;

		// Token: 0x040009B2 RID: 2482
		public int basePart_buildingsResolved;

		// Token: 0x040009B3 RID: 2483
		public int basePart_emptyNodesResolved;

		// Token: 0x040009B4 RID: 2484
		public int basePart_barracksResolved;

		// Token: 0x040009B5 RID: 2485
		public float basePart_batteriesCoverage;

		// Token: 0x040009B6 RID: 2486
		public float basePart_farmsCoverage;

		// Token: 0x040009B7 RID: 2487
		public float basePart_powerPlantsCoverage;

		// Token: 0x040009B8 RID: 2488
		public float basePart_breweriesCoverage;
	}
}
