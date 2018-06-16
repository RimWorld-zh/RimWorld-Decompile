using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008BF RID: 2239
	public class Instruction_BuildSandbags : Lesson_Instruction
	{
		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x0600332B RID: 13099 RVA: 0x001B80B4 File Offset: 0x001B64B4
		protected override float ProgressPercent
		{
			get
			{
				int num = 0;
				int num2 = 0;
				foreach (IntVec3 c in this.sandbagCells)
				{
					if (TutorUtility.BuildingOrBlueprintOrFrameCenterExists(c, base.Map, ThingDefOf.Sandbags))
					{
						num2++;
					}
					num++;
				}
				return (float)num2 / (float)num;
			}
		}

		// Token: 0x0600332C RID: 13100 RVA: 0x001B8140 File Offset: 0x001B6540
		public override void OnActivated()
		{
			base.OnActivated();
			Find.TutorialState.sandbagsRect = TutorUtility.FindUsableRect(7, 7, base.Map, 0f, false);
			this.sandbagCells = new List<IntVec3>();
			foreach (IntVec3 item in Find.TutorialState.sandbagsRect.EdgeCells)
			{
				if (item.x != Find.TutorialState.sandbagsRect.CenterCell.x && item.z != Find.TutorialState.sandbagsRect.CenterCell.z)
				{
					this.sandbagCells.Add(item);
				}
			}
			foreach (IntVec3 c in Find.TutorialState.sandbagsRect.ContractedBy(1))
			{
				if (!Find.TutorialState.sandbagsRect.ContractedBy(2).Contains(c))
				{
					List<Thing> thingList = c.GetThingList(base.Map);
					for (int i = thingList.Count - 1; i >= 0; i--)
					{
						Thing thing = thingList[i];
						if (thing.def.passability != Traversability.Standable && (thing.def.category == ThingCategory.Plant || thing.def.category == ThingCategory.Item))
						{
							thing.Destroy(DestroyMode.Vanish);
						}
					}
				}
			}
		}

		// Token: 0x0600332D RID: 13101 RVA: 0x001B8314 File Offset: 0x001B6714
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.sandbagCells, "sandbagCells", LookMode.Undefined, new object[0]);
		}

		// Token: 0x0600332E RID: 13102 RVA: 0x001B8334 File Offset: 0x001B6734
		public override void LessonOnGUI()
		{
			TutorUtility.DrawLabelOnGUI(Gen.AveragePosition(this.sandbagCells), this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x0600332F RID: 13103 RVA: 0x001B8358 File Offset: 0x001B6758
		public override void LessonUpdate()
		{
			List<IntVec3> cells = (from c in this.sandbagCells
			where !TutorUtility.BuildingOrBlueprintOrFrameCenterExists(c, base.Map, ThingDefOf.Sandbags)
			select c).ToList<IntVec3>();
			GenDraw.DrawFieldEdges(cells);
			GenDraw.DrawArrowPointingAt(Gen.AveragePosition(this.sandbagCells), false);
			if (this.ProgressPercent > 0.9999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x06003330 RID: 13104 RVA: 0x001B83B4 File Offset: 0x001B67B4
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			AcceptanceReport result;
			if (ep.Tag == "Designate-Sandbags")
			{
				result = TutorUtility.EventCellsAreWithin(ep, this.sandbagCells);
			}
			else
			{
				result = base.AllowAction(ep);
			}
			return result;
		}

		// Token: 0x04001B91 RID: 7057
		private List<IntVec3> sandbagCells;
	}
}
