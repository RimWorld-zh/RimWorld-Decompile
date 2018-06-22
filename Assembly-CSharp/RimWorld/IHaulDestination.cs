using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200068D RID: 1677
	public interface IHaulDestination : IStoreSettingsParent
	{
		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06002399 RID: 9113
		IntVec3 Position { get; }

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x0600239A RID: 9114
		Map Map { get; }

		// Token: 0x0600239B RID: 9115
		bool Accepts(Thing t);
	}
}
