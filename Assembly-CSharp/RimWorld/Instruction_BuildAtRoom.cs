using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008BA RID: 2234
	public abstract class Instruction_BuildAtRoom : Lesson_Instruction
	{
		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x0600330B RID: 13067
		protected abstract CellRect BuildableRect { get; }

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x0600330C RID: 13068 RVA: 0x001B79F0 File Offset: 0x001B5DF0
		protected override float ProgressPercent
		{
			get
			{
				float result;
				if (this.def.targetCount <= 1)
				{
					result = -1f;
				}
				else
				{
					result = (float)this.NumPlaced() / (float)this.def.targetCount;
				}
				return result;
			}
		}

		// Token: 0x0600330D RID: 13069 RVA: 0x001B7A38 File Offset: 0x001B5E38
		protected int NumPlaced()
		{
			int num = 0;
			foreach (IntVec3 c in this.BuildableRect)
			{
				if (TutorUtility.BuildingOrBlueprintOrFrameCenterExists(c, base.Map, this.def.thingDef))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600330E RID: 13070 RVA: 0x001B7AC0 File Offset: 0x001B5EC0
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.BuildableRect.ContractedBy(1), this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x0600330F RID: 13071 RVA: 0x001B7AF4 File Offset: 0x001B5EF4
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(this.BuildableRect.CenterVector3, true);
		}

		// Token: 0x06003310 RID: 13072 RVA: 0x001B7B18 File Offset: 0x001B5F18
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			AcceptanceReport result;
			if (ep.Tag == "Designate-" + this.def.thingDef.defName)
			{
				result = this.AllowBuildAt(ep.Cell);
			}
			else
			{
				result = base.AllowAction(ep);
			}
			return result;
		}

		// Token: 0x06003311 RID: 13073 RVA: 0x001B7B78 File Offset: 0x001B5F78
		protected virtual bool AllowBuildAt(IntVec3 c)
		{
			return this.BuildableRect.Contains(c);
		}

		// Token: 0x06003312 RID: 13074 RVA: 0x001B7B9C File Offset: 0x001B5F9C
		public override void Notify_Event(EventPack ep)
		{
			if (this.NumPlaced() >= this.def.targetCount)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
