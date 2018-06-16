using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000691 RID: 1681
	public interface IHaulDestination : IStoreSettingsParent
	{
		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x0600239F RID: 9119
		IntVec3 Position { get; }

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x060023A0 RID: 9120
		Map Map { get; }

		// Token: 0x060023A1 RID: 9121
		bool Accepts(Thing t);
	}
}
