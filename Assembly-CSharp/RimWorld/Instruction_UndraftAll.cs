using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C8 RID: 2248
	public class Instruction_UndraftAll : Lesson_Instruction
	{
		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x06003370 RID: 13168 RVA: 0x001B958C File Offset: 0x001B798C
		protected override float ProgressPercent
		{
			get
			{
				return 1f - (float)this.DraftedPawns().Count<Pawn>() / (float)base.Map.mapPawns.FreeColonistsSpawnedCount;
			}
		}

		// Token: 0x06003371 RID: 13169 RVA: 0x001B95C8 File Offset: 0x001B79C8
		private IEnumerable<Pawn> DraftedPawns()
		{
			return from p in base.Map.mapPawns.FreeColonistsSpawned
			where p.Drafted
			select p;
		}

		// Token: 0x06003372 RID: 13170 RVA: 0x001B9610 File Offset: 0x001B7A10
		public override void LessonUpdate()
		{
			foreach (Pawn pawn in this.DraftedPawns())
			{
				GenDraw.DrawArrowPointingAt(pawn.DrawPos, false);
			}
			if (this.ProgressPercent > 0.9999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
