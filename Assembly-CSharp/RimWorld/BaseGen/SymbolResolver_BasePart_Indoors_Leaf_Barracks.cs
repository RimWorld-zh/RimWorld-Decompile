using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02000393 RID: 915
	public class SymbolResolver_BasePart_Indoors_Leaf_Barracks : SymbolResolver
	{
		// Token: 0x06000FFC RID: 4092 RVA: 0x00086B18 File Offset: 0x00084F18
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("barracks", rp);
			BaseGen.globalSettings.basePart_barracksResolved++;
		}
	}
}
