using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000882 RID: 2178
	public abstract class PawnColumnWorker_CopyPaste : PawnColumnWorker
	{
		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x060031B1 RID: 12721
		protected abstract bool AnythingInClipboard { get; }

		// Token: 0x060031B2 RID: 12722 RVA: 0x001AE9F8 File Offset: 0x001ACDF8
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

		// Token: 0x060031B3 RID: 12723 RVA: 0x001AEA68 File Offset: 0x001ACE68
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 36);
		}

		// Token: 0x060031B4 RID: 12724 RVA: 0x001AEA8C File Offset: 0x001ACE8C
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x060031B5 RID: 12725
		protected abstract void CopyFrom(Pawn p);

		// Token: 0x060031B6 RID: 12726
		protected abstract void PasteTo(Pawn p);
	}
}
