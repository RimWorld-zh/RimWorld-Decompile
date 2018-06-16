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
		// (get) Token: 0x060031BA RID: 12730 RVA: 0x001AE494 File Offset: 0x001AC894
		protected override bool AnythingInClipboard
		{
			get
			{
				return PawnColumnWorker_CopyPasteTimetable.clipboard != null;
			}
		}

		// Token: 0x060031BB RID: 12731 RVA: 0x001AE4B4 File Offset: 0x001AC8B4
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.timetable != null)
			{
				base.DoCell(rect, pawn, table);
			}
		}

		// Token: 0x060031BC RID: 12732 RVA: 0x001AE4D0 File Offset: 0x001AC8D0
		protected override void CopyFrom(Pawn p)
		{
			PawnColumnWorker_CopyPasteTimetable.clipboard = p.timetable.times.ToList<TimeAssignmentDef>();
		}

		// Token: 0x060031BD RID: 12733 RVA: 0x001AE4E8 File Offset: 0x001AC8E8
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
