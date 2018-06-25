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
		// Token: 0x04001AD0 RID: 6864
		private static List<TimeAssignmentDef> clipboard;

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x060031B8 RID: 12728 RVA: 0x001AEAEC File Offset: 0x001ACEEC
		protected override bool AnythingInClipboard
		{
			get
			{
				return PawnColumnWorker_CopyPasteTimetable.clipboard != null;
			}
		}

		// Token: 0x060031B9 RID: 12729 RVA: 0x001AEB0C File Offset: 0x001ACF0C
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.timetable != null)
			{
				base.DoCell(rect, pawn, table);
			}
		}

		// Token: 0x060031BA RID: 12730 RVA: 0x001AEB28 File Offset: 0x001ACF28
		protected override void CopyFrom(Pawn p)
		{
			PawnColumnWorker_CopyPasteTimetable.clipboard = p.timetable.times.ToList<TimeAssignmentDef>();
		}

		// Token: 0x060031BB RID: 12731 RVA: 0x001AEB40 File Offset: 0x001ACF40
		protected override void PasteTo(Pawn p)
		{
			for (int i = 0; i < 24; i++)
			{
				p.timetable.times[i] = PawnColumnWorker_CopyPasteTimetable.clipboard[i];
			}
		}
	}
}
