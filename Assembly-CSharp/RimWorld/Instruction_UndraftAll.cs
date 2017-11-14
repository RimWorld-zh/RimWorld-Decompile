using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class Instruction_UndraftAll : Lesson_Instruction
	{
		protected override float ProgressPercent
		{
			get
			{
				return (float)(1.0 - (float)this.DraftedPawns().Count() / (float)base.Map.mapPawns.FreeColonistsSpawnedCount);
			}
		}

		private IEnumerable<Pawn> DraftedPawns()
		{
			return from p in base.Map.mapPawns.FreeColonistsSpawned
			where p.Drafted
			select p;
		}

		public override void LessonUpdate()
		{
			foreach (Pawn item in this.DraftedPawns())
			{
				GenDraw.DrawArrowPointingAt(item.DrawPos, false);
			}
			if (this.ProgressPercent > 0.99989998340606689)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
