using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020003BC RID: 956
	public class SymbolResolver_AncientRuins : SymbolResolver
	{
		// Token: 0x06001094 RID: 4244 RVA: 0x0008C8C4 File Offset: 0x0008ACC4
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
