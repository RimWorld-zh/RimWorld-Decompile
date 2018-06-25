using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003E3 RID: 995
	public abstract class SymbolResolver
	{
		// Token: 0x04000A56 RID: 2646
		public IntVec2 minRectSize = IntVec2.One;

		// Token: 0x04000A57 RID: 2647
		public float selectionWeight = 1f;

		// Token: 0x06001102 RID: 4354 RVA: 0x00086BBC File Offset: 0x00084FBC
		public virtual bool CanResolve(ResolveParams rp)
		{
			return rp.rect.Width >= this.minRectSize.x && rp.rect.Height >= this.minRectSize.z;
		}

		// Token: 0x06001103 RID: 4355
		public abstract void Resolve(ResolveParams rp);
	}
}
