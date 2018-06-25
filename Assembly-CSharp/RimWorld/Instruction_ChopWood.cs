using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class Instruction_ChopWood : Lesson_Instruction
	{
		[CompilerGenerated]
		private static Func<Designation, bool> <>f__am$cache0;

		public Instruction_ChopWood()
		{
		}

		protected override float ProgressPercent
		{
			get
			{
				return (float)(from d in base.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.HarvestPlant)
				where d.target.Thing.def.plant.IsTree
				select d).Count<Designation>() / (float)this.def.targetCount;
			}
		}

		public override void LessonUpdate()
		{
			if (this.ProgressPercent > 0.999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		[CompilerGenerated]
		private static bool <get_ProgressPercent>m__0(Designation d)
		{
			return d.target.Thing.def.plant.IsTree;
		}
	}
}
