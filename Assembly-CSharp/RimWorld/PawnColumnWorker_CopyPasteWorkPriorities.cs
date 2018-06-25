using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000884 RID: 2180
	public class PawnColumnWorker_CopyPasteWorkPriorities : PawnColumnWorker_CopyPaste
	{
		// Token: 0x04001AD1 RID: 6865
		private static DefMap<WorkTypeDef, int> clipboard;

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x060031BD RID: 12733 RVA: 0x001AEB88 File Offset: 0x001ACF88
		protected override bool AnythingInClipboard
		{
			get
			{
				return PawnColumnWorker_CopyPasteWorkPriorities.clipboard != null;
			}
		}

		// Token: 0x060031BE RID: 12734 RVA: 0x001AEBA8 File Offset: 0x001ACFA8
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (!pawn.Dead && pawn.workSettings != null && pawn.workSettings.EverWork)
			{
				base.DoCell(rect, pawn, table);
			}
		}

		// Token: 0x060031BF RID: 12735 RVA: 0x001AEBE0 File Offset: 0x001ACFE0
		protected override void CopyFrom(Pawn p)
		{
			if (PawnColumnWorker_CopyPasteWorkPriorities.clipboard == null)
			{
				PawnColumnWorker_CopyPasteWorkPriorities.clipboard = new DefMap<WorkTypeDef, int>();
			}
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkTypeDef workTypeDef = allDefsListForReading[i];
				PawnColumnWorker_CopyPasteWorkPriorities.clipboard[workTypeDef] = (p.story.WorkTypeIsDisabled(workTypeDef) ? 3 : p.workSettings.GetPriority(workTypeDef));
			}
		}

		// Token: 0x060031C0 RID: 12736 RVA: 0x001AEC58 File Offset: 0x001AD058
		protected override void PasteTo(Pawn p)
		{
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkTypeDef workTypeDef = allDefsListForReading[i];
				if (!p.story.WorkTypeIsDisabled(workTypeDef))
				{
					p.workSettings.SetPriority(workTypeDef, PawnColumnWorker_CopyPasteWorkPriorities.clipboard[workTypeDef]);
				}
			}
		}
	}
}
