using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020002F7 RID: 759
	public interface IArchivable : IExposable
	{
		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000C9F RID: 3231
		Texture ArchivedIcon { get; }

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000CA0 RID: 3232
		Color ArchivedIconColor { get; }

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000CA1 RID: 3233
		string ArchivedLabel { get; }

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000CA2 RID: 3234
		string ArchivedTooltip { get; }

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000CA3 RID: 3235
		int CreatedTicksGame { get; }

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000CA4 RID: 3236
		bool CanCullArchivedNow { get; }

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000CA5 RID: 3237
		LookTargets LookTargets { get; }

		// Token: 0x06000CA6 RID: 3238
		void OpenArchived();
	}
}
