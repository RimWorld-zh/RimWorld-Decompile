using System.Linq;
using Verse;

namespace RimWorld
{
	public class Instruction_ChopWood : Lesson_Instruction
	{
		protected override float ProgressPercent
		{
			get
			{
				return (float)(from d in base.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.HarvestPlant)
				where d.target.Thing.def.plant.IsTree
				select d).Count() / (float)base.def.targetCount;
			}
		}

		public override void LessonUpdate()
		{
			if (this.ProgressPercent > 0.99900001287460327)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
