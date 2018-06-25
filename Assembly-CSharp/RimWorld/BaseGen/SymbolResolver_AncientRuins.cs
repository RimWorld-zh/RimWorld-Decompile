using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020003BE RID: 958
	public class SymbolResolver_AncientRuins : SymbolResolver
	{
		// Token: 0x06001098 RID: 4248 RVA: 0x0008CA14 File Offset: 0x0008AE14
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			resolveParams.wallStuff = (rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, true));
			float? chanceToSkipWallBlock = rp.chanceToSkipWallBlock;
			resolveParams.chanceToSkipWallBlock = new float?((chanceToSkipWallBlock == null) ? 0.1f : chanceToSkipWallBlock.Value);
			bool? clearEdificeOnly = rp.clearEdificeOnly;
			resolveParams.clearEdificeOnly = new bool?(clearEdificeOnly == null || clearEdificeOnly.Value);
			bool? noRoof = rp.noRoof;
			resolveParams.noRoof = new bool?(noRoof == null || noRoof.Value);
			BaseGen.symbolStack.Push("emptyRoom", resolveParams);
		}
	}
}
