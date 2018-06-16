using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200039D RID: 925
	public class SymbolResolver_BasePart_Outdoors_LeafDecorated_RandomInnerRect : SymbolResolver
	{
		// Token: 0x06001021 RID: 4129 RVA: 0x00087E10 File Offset: 0x00086210
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && rp.rect.Width <= 15 && rp.rect.Height <= 15 && rp.rect.Width > 5 && rp.rect.Height > 5;
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x00087E7C File Offset: 0x0008627C
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

		// Token: 0x04000A09 RID: 2569
		private const int MinLength = 5;

		// Token: 0x04000A0A RID: 2570
		private const int MaxRectSize = 15;
	}
}
