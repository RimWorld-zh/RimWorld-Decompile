using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003E1 RID: 993
	public abstract class SymbolResolver
	{
		// Token: 0x04000A53 RID: 2643
		public IntVec2 minRectSize = IntVec2.One;

		// Token: 0x04000A54 RID: 2644
		public float selectionWeight = 1f;

		// Token: 0x060010FF RID: 4351 RVA: 0x00086A5C File Offset: 0x00084E5C
		public virtual bool CanResolve(ResolveParams rp)
		{
			return rp.rect.Width >= this.minRectSize.x && rp.rect.Height >= this.minRectSize.z;
		}

		// Token: 0x06001100 RID: 4352
		public abstract void Resolve(ResolveParams rp);
	}
}
