using System;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_Interior_Barracks : SymbolResolver
	{
		public SymbolResolver_Interior_Barracks()
		{
		}

		public override void Resolve(ResolveParams rp)
		{
			InteriorSymbolResolverUtility.PushBedroomHeatersCoolersAndLightSourcesSymbols(rp, true);
			BaseGen.symbolStack.Push("fillWithBeds", rp);
		}
	}
}
