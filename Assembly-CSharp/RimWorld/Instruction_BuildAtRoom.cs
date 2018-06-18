using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008BA RID: 2234
	public abstract class Instruction_BuildAtRoom : Lesson_Instruction
	{
		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x0600330D RID: 13069
		protected abstract CellRect BuildableRect { get; }

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x0600330E RID: 13070 RVA: 0x001B7AB8 File Offset: 0x001B5EB8
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

		// Token: 0x0600330F RID: 13071 RVA: 0x001B7B00 File Offset: 0x001B5F00
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

		// Token: 0x06003310 RID: 13072 RVA: 0x001B7B88 File Offset: 0x001B5F88
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.BuildableRect.ContractedBy(1), this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06003311 RID: 13073 RVA: 0x001B7BBC File Offset: 0x001B5FBC
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(this.BuildableRect.CenterVector3, true);
		}

		// Token: 0x06003312 RID: 13074 RVA: 0x001B7BE0 File Offset: 0x001B5FE0
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

		// Token: 0x06003313 RID: 13075 RVA: 0x001B7C40 File Offset: 0x001B6040
		protected virtual bool AllowBuildAt(IntVec3 c)
		{
			return this.BuildableRect.Contains(c);
		}

		// Token: 0x06003314 RID: 13076 RVA: 0x001B7C64 File Offset: 0x001B6064
		public override void Notify_Event(EventPack ep)
		{
			if (this.NumPlaced() >= this.def.targetCount)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
