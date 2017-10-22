namespace RimWorld.BaseGen
{
	public class SymbolResolver_BasePart_Indoors_Leaf_DiningRoom : SymbolResolver
	{
		public override bool CanResolve(ResolveParams rp)
		{
			return (byte)(base.CanResolve(rp) ? ((BaseGen.globalSettings.basePart_barracksResolved >= BaseGen.globalSettings.minBarracks) ? 1 : 0) : 0) != 0;
		}

		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("diningRoom", rp);
		}
	}
}
