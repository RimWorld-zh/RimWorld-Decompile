namespace RimWorld.BaseGen
{
	public class SymbolResolver_AncientRuins : SymbolResolver
	{
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			resolveParams.wallStuff = (rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, true));
			float? chanceToSkipWallBlock = rp.chanceToSkipWallBlock;
			resolveParams.chanceToSkipWallBlock = new float?((float)((!chanceToSkipWallBlock.HasValue) ? 0.10000000149011612 : chanceToSkipWallBlock.Value));
			bool? clearEdificeOnly = rp.clearEdificeOnly;
			resolveParams.clearEdificeOnly = new bool?(!clearEdificeOnly.HasValue || clearEdificeOnly.Value);
			bool? noRoof = rp.noRoof;
			resolveParams.noRoof = new bool?(!noRoof.HasValue || noRoof.Value);
			BaseGen.symbolStack.Push("emptyRoom", resolveParams);
		}
	}
}
