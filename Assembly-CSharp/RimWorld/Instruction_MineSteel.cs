using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C8 RID: 2248
	public class Instruction_MineSteel : Lesson_Instruction
	{
		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x0600335C RID: 13148 RVA: 0x001B8E6C File Offset: 0x001B726C
		protected override float ProgressPercent
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.mineCells.Count; i++)
				{
					IntVec3 c = this.mineCells[i];
					if (base.Map.designationManager.DesignationAt(c, DesignationDefOf.Mine) != null || c.GetEdifice(base.Map) == null || c.GetEdifice(base.Map).def != ThingDefOf.MineableSteel)
					{
						num++;
					}
				}
				return (float)num / (float)this.mineCells.Count;
			}
		}

		// Token: 0x0600335D RID: 13149 RVA: 0x001B8F08 File Offset: 0x001B7308
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.mineCells, "mineCells", LookMode.Undefined, new object[0]);
		}

		// Token: 0x0600335E RID: 13150 RVA: 0x001B8F28 File Offset: 0x001B7328
		public override void OnActivated()
		{
			base.OnActivated();
			CellRect cellRect = TutorUtility.FindUsableRect(10, 10, base.Map, 0f, true);
			new GenStep_ScatterLumpsMineable
			{
				forcedDefToScatter = ThingDefOf.MineableSteel
			}.ForceScatterAt(cellRect.CenterCell, base.Map);
			this.mineCells = new List<IntVec3>();
			foreach (IntVec3 intVec in cellRect)
			{
				Building edifice = intVec.GetEdifice(base.Map);
				if (edifice != null && edifice.def == ThingDefOf.MineableSteel)
				{
					this.mineCells.Add(intVec);
				}
			}
		}

		// Token: 0x0600335F RID: 13151 RVA: 0x001B8FF8 File Offset: 0x001B73F8
		public override void LessonOnGUI()
		{
			if (!this.mineCells.NullOrEmpty<IntVec3>())
			{
				TutorUtility.DrawLabelOnGUI(Gen.AveragePosition(this.mineCells), this.def.onMapInstruction);
			}
			base.LessonOnGUI();
		}

		// Token: 0x06003360 RID: 13152 RVA: 0x001B902C File Offset: 0x001B742C
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(Gen.AveragePosition(this.mineCells), false);
		}

		// Token: 0x06003361 RID: 13153 RVA: 0x001B9040 File Offset: 0x001B7440
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			AcceptanceReport result;
			if (ep.Tag == "Designate-Mine")
			{
				result = TutorUtility.EventCellsAreWithin(ep, this.mineCells);
			}
			else
			{
				result = base.AllowAction(ep);
			}
			return result;
		}

		// Token: 0x06003362 RID: 13154 RVA: 0x001B9089 File Offset: 0x001B7489
		public override void Notify_Event(EventPack ep)
		{
			if (ep.Tag == "Designate-Mine" && this.ProgressPercent > 0.999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x04001B9B RID: 7067
		private List<IntVec3> mineCells;
	}
}
