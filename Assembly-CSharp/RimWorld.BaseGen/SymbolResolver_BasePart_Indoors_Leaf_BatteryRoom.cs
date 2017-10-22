namespace RimWorld.BaseGen
{
	public class SymbolResolver_BasePart_Indoors_Leaf_BatteryRoom : SymbolResolver
	{
		private const float MaxCoverage = 0.06f;

		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_barracksResolved >= BaseGen.globalSettings.minBarracks && !(BaseGen.globalSettings.basePart_batteriesCoverage + (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area >= 0.059999998658895493) && (rp.faction == null || (int)rp.faction.def.techLevel >= 4);
		}

		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("batteryRoom", rp);
			BaseGen.globalSettings.basePart_batteriesCoverage += (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area;
		}
	}
}
