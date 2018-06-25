using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C6 RID: 2246
	public class Instruction_MineSteel : Lesson_Instruction
	{
		// Token: 0x04001B99 RID: 7065
		private List<IntVec3> mineCells;

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x06003359 RID: 13145 RVA: 0x001B9194 File Offset: 0x001B7594
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

		// Token: 0x0600335A RID: 13146 RVA: 0x001B9230 File Offset: 0x001B7630
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.mineCells, "mineCells", LookMode.Undefined, new object[0]);
		}

		// Token: 0x0600335B RID: 13147 RVA: 0x001B9250 File Offset: 0x001B7650
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

		// Token: 0x0600335C RID: 13148 RVA: 0x001B9320 File Offset: 0x001B7720
		public override void LessonOnGUI()
		{
			if (!this.mineCells.NullOrEmpty<IntVec3>())
			{
				TutorUtility.DrawLabelOnGUI(Gen.AveragePosition(this.mineCells), this.def.onMapInstruction);
			}
			base.LessonOnGUI();
		}

		// Token: 0x0600335D RID: 13149 RVA: 0x001B9354 File Offset: 0x001B7754
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(Gen.AveragePosition(this.mineCells), false);
		}

		// Token: 0x0600335E RID: 13150 RVA: 0x001B9368 File Offset: 0x001B7768
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

		// Token: 0x0600335F RID: 13151 RVA: 0x001B93B1 File Offset: 0x001B77B1
		public override void Notify_Event(EventPack ep)
		{
			if (ep.Tag == "Designate-Mine" && this.ProgressPercent > 0.999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
