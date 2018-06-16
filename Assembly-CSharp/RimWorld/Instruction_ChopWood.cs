using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C0 RID: 2240
	public class Instruction_ChopWood : Lesson_Instruction
	{
		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x06003333 RID: 13107 RVA: 0x001B8430 File Offset: 0x001B6830
		protected override float ProgressPercent
		{
			get
			{
				return (float)(from d in base.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.HarvestPlant)
				where d.target.Thing.def.plant.IsTree
				select d).Count<Designation>() / (float)this.def.targetCount;
			}
		}

		// Token: 0x06003334 RID: 13108 RVA: 0x001B848F File Offset: 0x001B688F
		public override void LessonUpdate()
		{
			if (this.ProgressPercent > 0.999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
