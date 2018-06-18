using System;

namespace Verse.AI
{
	// Token: 0x02000AE1 RID: 2785
	public interface IAttackTargetSearcher
	{
		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x06003DA6 RID: 15782
		Thing Thing { get; }

		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x06003DA7 RID: 15783
		Verb CurrentEffectiveVerb { get; }

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x06003DA8 RID: 15784
		LocalTargetInfo LastAttackedTarget { get; }

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x06003DA9 RID: 15785
		int LastAttackTargetTick { get; }
	}
}
