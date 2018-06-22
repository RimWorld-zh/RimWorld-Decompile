using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006DE RID: 1758
	public interface IActiveDropPod : IThingHolder
	{
		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06002647 RID: 9799
		ActiveDropPodInfo Contents { get; }
	}
}
