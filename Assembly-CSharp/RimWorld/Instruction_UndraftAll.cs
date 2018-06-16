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
		// (get) Token: 0x06003375 RID: 13173 RVA: 0x001B92DC File Offset: 0x001B76DC
		protected override float ProgressPercent
		{
			get
			{
				return 1f - (float)this.DraftedPawns().Count<Pawn>() / (float)base.Map.mapPawns.FreeColonistsSpawnedCount;
			}
		}

		// Token: 0x06003376 RID: 13174 RVA: 0x001B9318 File Offset: 0x001B7718
		private IEnumerable<Pawn> DraftedPawns()
		{
			return from p in base.Map.mapPawns.FreeColonistsSpawned
			where p.Drafted
			select p;
		}

		// Token: 0x06003377 RID: 13175 RVA: 0x001B9360 File Offset: 0x001B7760
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
