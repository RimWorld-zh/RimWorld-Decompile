using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008CA RID: 2250
	public class Instruction_UndraftAll : Lesson_Instruction
	{
		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x06003374 RID: 13172 RVA: 0x001B99A0 File Offset: 0x001B7DA0
		protected override float ProgressPercent
		{
			get
			{
				return 1f - (float)this.DraftedPawns().Count<Pawn>() / (float)base.Map.mapPawns.FreeColonistsSpawnedCount;
			}
		}

		// Token: 0x06003375 RID: 13173 RVA: 0x001B99DC File Offset: 0x001B7DDC
		private IEnumerable<Pawn> DraftedPawns()
		{
			return from p in base.Map.mapPawns.FreeColonistsSpawned
			where p.Drafted
			select p;
		}

		// Token: 0x06003376 RID: 13174 RVA: 0x001B9A24 File Offset: 0x001B7E24
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
