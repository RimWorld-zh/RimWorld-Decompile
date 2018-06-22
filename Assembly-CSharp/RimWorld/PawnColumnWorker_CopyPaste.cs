using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000880 RID: 2176
	public abstract class PawnColumnWorker_CopyPaste : PawnColumnWorker
	{
		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x060031AE RID: 12718
		protected abstract bool AnythingInClipboard { get; }

		// Token: 0x060031AF RID: 12719 RVA: 0x001AE650 File Offset: 0x001ACA50
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

		// Token: 0x060031B0 RID: 12720 RVA: 0x001AE6C0 File Offset: 0x001ACAC0
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 36);
		}

		// Token: 0x060031B1 RID: 12721 RVA: 0x001AE6E4 File Offset: 0x001ACAE4
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x060031B2 RID: 12722
		protected abstract void CopyFrom(Pawn p);

		// Token: 0x060031B3 RID: 12723
		protected abstract void PasteTo(Pawn p);
	}
}
