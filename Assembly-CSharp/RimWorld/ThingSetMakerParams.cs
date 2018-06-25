using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002E3 RID: 739
	public struct ThingSetMakerParams
	{
		// Token: 0x04000799 RID: 1945
		public TechLevel? techLevel;

		// Token: 0x0400079A RID: 1946
		public IntRange? countRange;

		// Token: 0x0400079B RID: 1947
		public ThingFilter filter;

		// Token: 0x0400079C RID: 1948
		public Predicate<ThingDef> validator;

		// Token: 0x0400079D RID: 1949
		public QualityGenerator? qualityGenerator;

		// Token: 0x0400079E RID: 1950
		public float? maxTotalMass;

		// Token: 0x0400079F RID: 1951
		public float? maxThingMarketValue;

		// Token: 0x040007A0 RID: 1952
		public FloatRange? totalMarketValueRange;

		// Token: 0x040007A1 RID: 1953
		public FloatRange? totalNutritionRange;

		// Token: 0x040007A2 RID: 1954
		public PodContentsType? podContentsType;

		// Token: 0x040007A3 RID: 1955
		public TraderKindDef traderDef;

		// Token: 0x040007A4 RID: 1956
		public int? tile;

		// Token: 0x040007A5 RID: 1957
		public Faction traderFaction;

		// Token: 0x040007A6 RID: 1958
		public Dictionary<string, object> custom;
	}
}
