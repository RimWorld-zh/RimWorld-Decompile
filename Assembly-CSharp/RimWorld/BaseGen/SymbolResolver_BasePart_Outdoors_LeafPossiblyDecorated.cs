using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003A4 RID: 932
	public class SymbolResolver_BasePart_Outdoors_LeafPossiblyDecorated : SymbolResolver
	{
		// Token: 0x06001036 RID: 4150 RVA: 0x000887EC File Offset: 0x00086BEC
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
