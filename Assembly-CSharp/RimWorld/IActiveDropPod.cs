using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006E0 RID: 1760
	public interface IActiveDropPod : IThingHolder
	{
		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x0600264A RID: 9802
		ActiveDropPodInfo Contents { get; }
	}
}
