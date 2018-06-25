using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008BE RID: 2238
	public class Instruction_ChopWood : Lesson_Instruction
	{
		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x06003332 RID: 13106 RVA: 0x001B8820 File Offset: 0x001B6C20
		protected override float ProgressPercent
		{
			get
			{
				return (float)(from d in base.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.HarvestPlant)
				where d.target.Thing.def.plant.IsTree
				select d).Count<Designation>() / (float)this.def.targetCount;
			}
		}

		// Token: 0x06003333 RID: 13107 RVA: 0x001B887F File Offset: 0x001B6C7F
		public override void LessonUpdate()
		{
			if (this.ProgressPercent > 0.999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
