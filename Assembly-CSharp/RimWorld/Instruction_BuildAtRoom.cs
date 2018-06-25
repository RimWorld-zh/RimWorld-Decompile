using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008B8 RID: 2232
	public abstract class Instruction_BuildAtRoom : Lesson_Instruction
	{
		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x0600330A RID: 13066
		protected abstract CellRect BuildableRect { get; }

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x0600330B RID: 13067 RVA: 0x001B7DE0 File Offset: 0x001B61E0
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

		// Token: 0x0600330C RID: 13068 RVA: 0x001B7E28 File Offset: 0x001B6228
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

		// Token: 0x0600330D RID: 13069 RVA: 0x001B7EB0 File Offset: 0x001B62B0
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.BuildableRect.ContractedBy(1), this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x0600330E RID: 13070 RVA: 0x001B7EE4 File Offset: 0x001B62E4
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(this.BuildableRect.CenterVector3, true);
		}

		// Token: 0x0600330F RID: 13071 RVA: 0x001B7F08 File Offset: 0x001B6308
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

		// Token: 0x06003310 RID: 13072 RVA: 0x001B7F68 File Offset: 0x001B6368
		protected virtual bool AllowBuildAt(IntVec3 c)
		{
			return this.BuildableRect.Contains(c);
		}

		// Token: 0x06003311 RID: 13073 RVA: 0x001B7F8C File Offset: 0x001B638C
		public override void Notify_Event(EventPack ep)
		{
			if (this.NumPlaced() >= this.def.targetCount)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
