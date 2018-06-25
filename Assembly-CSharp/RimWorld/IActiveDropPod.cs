using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006E0 RID: 1760
	public interface IActiveDropPod : IThingHolder
	{
		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x0600264B RID: 9803
		ActiveDropPodInfo Contents { get; }
	}
}
