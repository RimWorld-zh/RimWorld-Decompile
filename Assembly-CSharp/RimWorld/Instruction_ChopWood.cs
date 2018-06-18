using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C0 RID: 2240
	public class Instruction_ChopWood : Lesson_Instruction
	{
		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x06003335 RID: 13109 RVA: 0x001B84F8 File Offset: 0x001B68F8
		protected override float ProgressPercent
		{
			get
			{
				return (float)(from d in base.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.HarvestPlant)
				where d.target.Thing.def.plant.IsTree
				select d).Count<Designation>() / (float)this.def.targetCount;
			}
		}

		// Token: 0x06003336 RID: 13110 RVA: 0x001B8557 File Offset: 0x001B6957
		public override void LessonUpdate()
		{
			if (this.ProgressPercent > 0.999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
