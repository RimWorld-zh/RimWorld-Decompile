using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200068F RID: 1679
	public interface IHaulDestination : IStoreSettingsParent
	{
		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x0600239D RID: 9117
		IntVec3 Position { get; }

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x0600239E RID: 9118
		Map Map { get; }

		// Token: 0x0600239F RID: 9119
		bool Accepts(Thing t);
	}
}
