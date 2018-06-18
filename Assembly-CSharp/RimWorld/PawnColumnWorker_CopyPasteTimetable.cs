using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000885 RID: 2181
	public class PawnColumnWorker_CopyPasteTimetable : PawnColumnWorker_CopyPaste
	{
		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x060031BC RID: 12732 RVA: 0x001AE55C File Offset: 0x001AC95C
		protected override bool AnythingInClipboard
		{
			get
			{
				return PawnColumnWorker_CopyPasteTimetable.clipboard != null;
			}
		}

		// Token: 0x060031BD RID: 12733 RVA: 0x001AE57C File Offset: 0x001AC97C
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.timetable != null)
			{
				base.DoCell(rect, pawn, table);
			}
		}

		// Token: 0x060031BE RID: 12734 RVA: 0x001AE598 File Offset: 0x001AC998
		protected override void CopyFrom(Pawn p)
		{
			PawnColumnWorker_CopyPasteTimetable.clipboard = p.timetable.times.ToList<TimeAssignmentDef>();
		}

		// Token: 0x060031BF RID: 12735 RVA: 0x001AE5B0 File Offset: 0x001AC9B0
		protected override void PasteTo(Pawn p)
		{
			for (int i = 0; i < 24; i++)
			{
				p.timetable.times[i] = PawnColumnWorker_CopyPasteTimetable.clipboard[i];
			}
		}

		// Token: 0x04001ACE RID: 6862
		private static List<TimeAssignmentDef> clipboard;
	}
}
