using System;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_BasePart_Indoors_Leaf_Barracks : SymbolResolver
	{
		public SymbolResolver_BasePart_Indoors_Leaf_Barracks()
		{
		}

		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("barracks", rp);
			BaseGen.globalSettings.basePart_barracksResolved++;
		}
	}
}
