using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002E1 RID: 737
	public struct ThingSetMakerParams
	{
		// Token: 0x04000797 RID: 1943
		public TechLevel? techLevel;

		// Token: 0x04000798 RID: 1944
		public IntRange? countRange;

		// Token: 0x04000799 RID: 1945
		public ThingFilter filter;

		// Token: 0x0400079A RID: 1946
		public Predicate<ThingDef> validator;

		// Token: 0x0400079B RID: 1947
		public QualityGenerator? qualityGenerator;

		// Token: 0x0400079C RID: 1948
		public float? maxTotalMass;

		// Token: 0x0400079D RID: 1949
		public float? maxThingMarketValue;

		// Token: 0x0400079E RID: 1950
		public FloatRange? totalMarketValueRange;

		// Token: 0x0400079F RID: 1951
		public FloatRange? totalNutritionRange;

		// Token: 0x040007A0 RID: 1952
		public PodContentsType? podContentsType;

		// Token: 0x040007A1 RID: 1953
		public TraderKindDef traderDef;

		// Token: 0x040007A2 RID: 1954
		public int? tile;

		// Token: 0x040007A3 RID: 1955
		public Faction traderFaction;

		// Token: 0x040007A4 RID: 1956
		public Dictionary<string, object> custom;
	}
}
