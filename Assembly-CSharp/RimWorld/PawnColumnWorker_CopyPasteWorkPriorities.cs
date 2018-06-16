using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000886 RID: 2182
	public class PawnColumnWorker_CopyPasteWorkPriorities : PawnColumnWorker_CopyPaste
	{
		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x060031BF RID: 12735 RVA: 0x001AE530 File Offset: 0x001AC930
		protected override bool AnythingInClipboard
		{
			get
			{
				return PawnColumnWorker_CopyPasteWorkPriorities.clipboard != null;
			}
		}

		// Token: 0x060031C0 RID: 12736 RVA: 0x001AE550 File Offset: 0x001AC950
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (!pawn.Dead && pawn.workSettings != null && pawn.workSettings.EverWork)
			{
				base.DoCell(rect, pawn, table);
			}
		}

		// Token: 0x060031C1 RID: 12737 RVA: 0x001AE588 File Offset: 0x001AC988
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

		// Token: 0x060031C2 RID: 12738 RVA: 0x001AE600 File Offset: 0x001ACA00
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

		// Token: 0x04001ACF RID: 6863
		private static DefMap<WorkTypeDef, int> clipboard;
	}
}
