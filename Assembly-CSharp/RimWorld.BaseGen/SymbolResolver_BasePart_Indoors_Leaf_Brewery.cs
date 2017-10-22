namespace RimWorld.BaseGen
{
	public class SymbolResolver_BasePart_Indoors_Leaf_Brewery : SymbolResolver
	{
		private const float MaxCoverage = 0.08f;

		public override bool CanResolve(ResolveParams rp)
		{
			if (!base.CanResolve(rp))
			{
				return false;
			}
			if (BaseGen.globalSettings.basePart_barracksResolved < BaseGen.globalSettings.minBarracks)
			{
				return false;
			}
			if (BaseGen.globalSettings.basePart_breweriesCoverage + (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area >= 0.079999998211860657)
			{
				return false;
			}
			return rp.faction == null || (int)rp.faction.def.techLevel >= 3;
		}

		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("brewery", rp);
			BaseGen.globalSettings.basePart_breweriesCoverage += (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area;
		}
	}
}
