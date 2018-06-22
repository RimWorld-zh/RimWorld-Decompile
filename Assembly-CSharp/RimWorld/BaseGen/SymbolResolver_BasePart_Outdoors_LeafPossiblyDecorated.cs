using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003A2 RID: 930
	public class SymbolResolver_BasePart_Outdoors_LeafPossiblyDecorated : SymbolResolver
	{
		// Token: 0x06001032 RID: 4146 RVA: 0x0008869C File Offset: 0x00086A9C
		public override void Resolve(ResolveParams rp)
		{
			if (rp.rect.Width >= 10 && rp.rect.Height >= 10 && Rand.Chance(0.25f))
			{
				BaseGen.symbolStack.Push("basePart_outdoors_leafDecorated", rp);
			}
			else
			{
				BaseGen.symbolStack.Push("basePart_outdoors_leaf", rp);
			}
		}
	}
}
