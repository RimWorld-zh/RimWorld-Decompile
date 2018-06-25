using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000883 RID: 2179
	public class PawnColumnWorker_CopyPasteTimetable : PawnColumnWorker_CopyPaste
	{
		// Token: 0x04001ACC RID: 6860
		private static List<TimeAssignmentDef> clipboard;

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x060031B9 RID: 12729 RVA: 0x001AE884 File Offset: 0x001ACC84
		protected override bool AnythingInClipboard
		{
			get
			{
				return PawnColumnWorker_CopyPasteTimetable.clipboard != null;
			}
		}

		// Token: 0x060031BA RID: 12730 RVA: 0x001AE8A4 File Offset: 0x001ACCA4
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.timetable != null)
			{
				base.DoCell(rect, pawn, table);
			}
		}

		// Token: 0x060031BB RID: 12731 RVA: 0x001AE8C0 File Offset: 0x001ACCC0
		protected override void CopyFrom(Pawn p)
		{
			PawnColumnWorker_CopyPasteTimetable.clipboard = p.timetable.times.ToList<TimeAssignmentDef>();
		}

		// Token: 0x060031BC RID: 12732 RVA: 0x001AE8D8 File Offset: 0x001ACCD8
		protected override void PasteTo(Pawn p)
		{
			for (int i = 0; i < 24; i++)
			{
				p.timetable.times[i] = PawnColumnWorker_CopyPasteTimetable.clipboard[i];
			}
		}
	}
}
