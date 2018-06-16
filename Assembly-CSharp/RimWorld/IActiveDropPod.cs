using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006E2 RID: 1762
	public interface IActiveDropPod : IThingHolder
	{
		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x0600264D RID: 9805
		ActiveDropPodInfo Contents { get; }
	}
}
