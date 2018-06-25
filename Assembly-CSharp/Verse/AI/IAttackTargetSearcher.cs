using System;

namespace Verse.AI
{
	// Token: 0x02000ADF RID: 2783
	public interface IAttackTargetSearcher
	{
		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x06003DA5 RID: 15781
		Thing Thing { get; }

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x06003DA6 RID: 15782
		Verb CurrentEffectiveVerb { get; }

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x06003DA7 RID: 15783
		LocalTargetInfo LastAttackedTarget { get; }

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x06003DA8 RID: 15784
		int LastAttackTargetTick { get; }
	}
}
