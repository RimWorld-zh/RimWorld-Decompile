using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000881 RID: 2177
	public class PawnColumnWorker_CopyPasteTimetable : PawnColumnWorker_CopyPaste
	{
		// Token: 0x04001ACC RID: 6860
		private static List<TimeAssignmentDef> clipboard;

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x060031B5 RID: 12725 RVA: 0x001AE744 File Offset: 0x001ACB44
		protected override bool AnythingInClipboard
		{
			get
			{
				return PawnColumnWorker_CopyPasteTimetable.clipboard != null;
			}
		}

		// Token: 0x060031B6 RID: 12726 RVA: 0x001AE764 File Offset: 0x001ACB64
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.timetable != null)
			{
				base.DoCell(rect, pawn, table);
			}
		}

		// Token: 0x060031B7 RID: 12727 RVA: 0x001AE780 File Offset: 0x001ACB80
		protected override void CopyFrom(Pawn p)
		{
			PawnColumnWorker_CopyPasteTimetable.clipboard = p.timetable.times.ToList<TimeAssignmentDef>();
		}

		// Token: 0x060031B8 RID: 12728 RVA: 0x001AE798 File Offset: 0x001ACB98
		protected override void PasteTo(Pawn p)
		{
			for (int i = 0; i < 24; i++)
			{
				p.timetable.times[i] = PawnColumnWorker_CopyPasteTimetable.clipboard[i];
			}
		}
	}
}
