using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003D5 RID: 981
	public class SymbolResolver_Interior_AncientTemple : SymbolResolver
	{
		// Token: 0x060010E5 RID: 4325 RVA: 0x000900F0 File Offset: 0x0008E4F0
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

		// Token: 0x04000A46 RID: 2630
		private const float MechanoidsChance = 0.5f;

		// Token: 0x04000A47 RID: 2631
		private static readonly IntRange MechanoidCountRange = new IntRange(1, 5);

		// Token: 0x04000A48 RID: 2632
		private const float HivesChance = 0.45f;

		// Token: 0x04000A49 RID: 2633
		private static readonly IntRange HivesCountRange = new IntRange(1, 2);
	}
}
