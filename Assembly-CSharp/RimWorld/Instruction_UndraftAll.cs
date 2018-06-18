using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008CC RID: 2252
	public class Instruction_UndraftAll : Lesson_Instruction
	{
		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x06003377 RID: 13175 RVA: 0x001B93A4 File Offset: 0x001B77A4
		protected override float ProgressPercent
		{
			get
			{
				return 1f - (float)this.DraftedPawns().Count<Pawn>() / (float)base.Map.mapPawns.FreeColonistsSpawnedCount;
			}
		}

		// Token: 0x06003378 RID: 13176 RVA: 0x001B93E0 File Offset: 0x001B77E0
		private IEnumerable<Pawn> DraftedPawns()
		{
			return from p in base.Map.mapPawns.FreeColonistsSpawned
			where p.Drafted
			select p;
		}

		// Token: 0x06003379 RID: 13177 RVA: 0x001B9428 File Offset: 0x001B7828
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
