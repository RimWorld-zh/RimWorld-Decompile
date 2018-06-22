using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008BC RID: 2236
	public class Instruction_ChopWood : Lesson_Instruction
	{
		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x0600332E RID: 13102 RVA: 0x001B86E0 File Offset: 0x001B6AE0
		protected override float ProgressPercent
		{
			get
			{
				return (float)(from d in base.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.HarvestPlant)
				where d.target.Thing.def.plant.IsTree
				select d).Count<Designation>() / (float)this.def.targetCount;
			}
		}

		// Token: 0x0600332F RID: 13103 RVA: 0x001B873F File Offset: 0x001B6B3F
		public override void LessonUpdate()
		{
			if (this.ProgressPercent > 0.999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
