using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003E3 RID: 995
	public abstract class SymbolResolver
	{
		// Token: 0x04000A53 RID: 2643
		public IntVec2 minRectSize = IntVec2.One;

		// Token: 0x04000A54 RID: 2644
		public float selectionWeight = 1f;

		// Token: 0x06001103 RID: 4355 RVA: 0x00086BAC File Offset: 0x00084FAC
		public virtual bool CanResolve(ResolveParams rp)
		{
			return rp.rect.Width >= this.minRectSize.x && rp.rect.Height >= this.minRectSize.z;
		}

		// Token: 0x06001104 RID: 4356
		public abstract void Resolve(ResolveParams rp);
	}
}
