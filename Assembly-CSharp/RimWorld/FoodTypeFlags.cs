using System;

namespace RimWorld
{
	// Token: 0x02000268 RID: 616
	[Flags]
	public enum FoodTypeFlags
	{
		// Token: 0x040004DD RID: 1245
		None = 0,
		// Token: 0x040004DE RID: 1246
		VegetableOrFruit = 1,
		// Token: 0x040004DF RID: 1247
		Meat = 2,
		// Token: 0x040004E0 RID: 1248
		Fluid = 4,
		// Token: 0x040004E1 RID: 1249
		Corpse = 8,
		// Token: 0x040004E2 RID: 1250
		Seed = 16,
		// Token: 0x040004E3 RID: 1251
		AnimalProduct = 32,
		// Token: 0x040004E4 RID: 1252
		Plant = 64,
		// Token: 0x040004E5 RID: 1253
		Tree = 128,
		// Token: 0x040004E6 RID: 1254
		Meal = 256,
		// Token: 0x040004E7 RID: 1255
		Processed = 512,
		// Token: 0x040004E8 RID: 1256
		Liquor = 1024,
		// Token: 0x040004E9 RID: 1257
		Kibble = 2048,
		// Token: 0x040004EA RID: 1258
		VegetarianAnimal = 3857,
		// Token: 0x040004EB RID: 1259
		VegetarianRoughAnimal = 3921,
		// Token: 0x040004EC RID: 1260
		CarnivoreAnimal = 2826,
		// Token: 0x040004ED RID: 1261
		CarnivoreAnimalStrict = 10,
		// Token: 0x040004EE RID: 1262
		OmnivoreAnimal = 3867,
		// Token: 0x040004EF RID: 1263
		OmnivoreRoughAnimal = 3931,
		// Token: 0x040004F0 RID: 1264
		DendrovoreAnimal = 2705,
		// Token: 0x040004F1 RID: 1265
		OvivoreAnimal = 2848,
		// Token: 0x040004F2 RID: 1266
		OmnivoreHuman = 3903
	}
}
