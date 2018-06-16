using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000884 RID: 2180
	public abstract class PawnColumnWorker_CopyPaste : PawnColumnWorker
	{
		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x060031B3 RID: 12723
		protected abstract bool AnythingInClipboard { get; }

		// Token: 0x060031B4 RID: 12724 RVA: 0x001AE3A0 File Offset: 0x001AC7A0
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Action pasteAction = null;
			if (this.AnythingInClipboard)
			{
				pasteAction = delegate()
				{
					this.PasteTo(pawn);
				};
			}
			Rect rect2 = new Rect(rect.x, rect.y, 36f, 30f);
			CopyPasteUI.DoCopyPasteButtons(rect2, delegate
			{
				this.CopyFrom(pawn);
			}, pasteAction);
		}

		// Token: 0x060031B5 RID: 12725 RVA: 0x001AE410 File Offset: 0x001AC810
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 36);
		}

		// Token: 0x060031B6 RID: 12726 RVA: 0x001AE434 File Offset: 0x001AC834
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x060031B7 RID: 12727
		protected abstract void CopyFrom(Pawn p);

		// Token: 0x060031B8 RID: 12728
		protected abstract void PasteTo(Pawn p);
	}
}
