using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003D7 RID: 983
	public class SymbolResolver_Interior_AncientTemple : SymbolResolver
	{
		// Token: 0x04000A4B RID: 2635
		private const float MechanoidsChance = 0.5f;

		// Token: 0x04000A4C RID: 2636
		private static readonly IntRange MechanoidCountRange = new IntRange(1, 5);

		// Token: 0x04000A4D RID: 2637
		private const float HivesChance = 0.45f;

		// Token: 0x04000A4E RID: 2638
		private static readonly IntRange HivesCountRange = new IntRange(1, 2);

		// Token: 0x060010E8 RID: 4328 RVA: 0x0009043C File Offset: 0x0008E83C
		public override void Resolve(ResolveParams rp)
		{
			List<Thing> list = ThingSetMakerDefOf.MapGen_AncientTempleContents.root.Generate();
			for (int i = 0; i < list.Count; i++)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singleThingToSpawn = list[i];
				BaseGen.symbolStack.Push("thing", resolveParams);
			}
			if (!Find.Storyteller.difficulty.peacefulTemples)
			{
				if (Rand.Chance(0.5f))
				{
					ResolveParams resolveParams2 = rp;
					int? mechanoidsCount = rp.mechanoidsCount;
					resolveParams2.mechanoidsCount = new int?((mechanoidsCount == null) ? SymbolResolver_Interior_AncientTemple.MechanoidCountRange.RandomInRange : mechanoidsCount.Value);
					BaseGen.symbolStack.Push("randomMechanoidGroup", resolveParams2);
				}
				else if (Rand.Chance(0.45f))
				{
					ResolveParams resolveParams3 = rp;
					int? hivesCount = rp.hivesCount;
					resolveParams3.hivesCount = new int?((hivesCount == null) ? SymbolResolver_Interior_AncientTemple.HivesCountRange.RandomInRange : hivesCount.Value);
					BaseGen.symbolStack.Push("hives", resolveParams3);
				}
			}
			int? ancientTempleEntranceHeight = rp.ancientTempleEntranceHeight;
			int num = (ancientTempleEntranceHeight == null) ? 0 : ancientTempleEntranceHeight.Value;
			ResolveParams resolveParams4 = rp;
			resolveParams4.rect.minZ = resolveParams4.rect.minZ + num;
			BaseGen.symbolStack.Push("ancientShrinesGroup", resolveParams4);
		}
	}
}
