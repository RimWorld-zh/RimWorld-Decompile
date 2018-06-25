using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02000395 RID: 917
	public class SymbolResolver_BasePart_Indoors_Leaf_Barracks : SymbolResolver
	{
		// Token: 0x06000FFF RID: 4095 RVA: 0x00086E64 File Offset: 0x00085264
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("barracks", rp);
			BaseGen.globalSettings.basePart_barracksResolved++;
		}
	}
}
