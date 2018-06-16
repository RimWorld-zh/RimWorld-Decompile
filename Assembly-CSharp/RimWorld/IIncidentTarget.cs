using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000316 RID: 790
	public interface IIncidentTarget : ILoadReferenceable
	{
		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000D63 RID: 3427
		int Tile { get; }

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000D64 RID: 3428
		StoryState StoryState { get; }

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000D65 RID: 3429
		GameConditionManager GameConditionManager { get; }

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000D66 RID: 3430
		float PlayerWealthForStoryteller { get; }

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000D67 RID: 3431
		IEnumerable<Pawn> PlayerPawnsForStoryteller { get; }

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000D68 RID: 3432
		FloatRange IncidentPointsRandomFactorRange { get; }

		// Token: 0x06000D69 RID: 3433
		IEnumerable<IncidentTargetTypeDef> AcceptedTypes();
	}
}
