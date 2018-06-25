using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200039F RID: 927
	public class SymbolResolver_BasePart_Outdoors_LeafDecorated_RandomInnerRect : SymbolResolver
	{
		// Token: 0x04000A0B RID: 2571
		private const int MinLength = 5;

		// Token: 0x04000A0C RID: 2572
		private const int MaxRectSize = 15;

		// Token: 0x06001025 RID: 4133 RVA: 0x0008814C File Offset: 0x0008654C
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && rp.rect.Width <= 15 && rp.rect.Height <= 15 && rp.rect.Width > 5 && rp.rect.Height > 5;
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x000881B8 File Offset: 0x000865B8
		public override void Resolve(ResolveParams rp)
		{
			int num = Rand.RangeInclusive(5, rp.rect.Width - 1);
			int num2 = Rand.RangeInclusive(5, rp.rect.Height - 1);
			int num3 = Rand.RangeInclusive(0, rp.rect.Width - num);
			int num4 = Rand.RangeInclusive(0, rp.rect.Height - num2);
			ResolveParams resolveParams = rp;
			resolveParams.rect = new CellRect(rp.rect.minX + num3, rp.rect.minZ + num4, num, num2);
			BaseGen.symbolStack.Push("basePart_outdoors_leaf", resolveParams);
		}
	}
}
