using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200039A RID: 922
	public class SymbolResolver_BasePart_Indoors : SymbolResolver
	{
		// Token: 0x0600100D RID: 4109 RVA: 0x0008715C File Offset: 0x0008555C
		public override void Resolve(ResolveParams rp)
		{
			bool flag = rp.rect.Width > 13 || rp.rect.Height > 13 || ((rp.rect.Width >= 9 || rp.rect.Height >= 9) && Rand.Chance(0.3f));
			if (flag)
			{
				BaseGen.symbolStack.Push("basePart_indoors_division", rp);
			}
			else
			{
				BaseGen.symbolStack.Push("basePart_indoors_leaf", rp);
			}
		}
	}
}
