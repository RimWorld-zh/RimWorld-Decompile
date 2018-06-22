using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008B6 RID: 2230
	public abstract class Instruction_BuildAtRoom : Lesson_Instruction
	{
		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06003306 RID: 13062
		protected abstract CellRect BuildableRect { get; }

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x06003307 RID: 13063 RVA: 0x001B7CA0 File Offset: 0x001B60A0
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

		// Token: 0x06003308 RID: 13064 RVA: 0x001B7CE8 File Offset: 0x001B60E8
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

		// Token: 0x06003309 RID: 13065 RVA: 0x001B7D70 File Offset: 0x001B6170
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.BuildableRect.ContractedBy(1), this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x0600330A RID: 13066 RVA: 0x001B7DA4 File Offset: 0x001B61A4
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(this.BuildableRect.CenterVector3, true);
		}

		// Token: 0x0600330B RID: 13067 RVA: 0x001B7DC8 File Offset: 0x001B61C8
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

		// Token: 0x0600330C RID: 13068 RVA: 0x001B7E28 File Offset: 0x001B6228
		protected virtual bool AllowBuildAt(IntVec3 c)
		{
			return this.BuildableRect.Contains(c);
		}

		// Token: 0x0600330D RID: 13069 RVA: 0x001B7E4C File Offset: 0x001B624C
		public override void Notify_Event(EventPack ep)
		{
			if (this.NumPlaced() >= this.def.targetCount)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
