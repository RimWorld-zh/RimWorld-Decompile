using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class Instruction_BuildSandbags : Lesson_Instruction
	{
		private List<IntVec3> sandbagCells;

		protected override float ProgressPercent
		{
			get
			{
				int num = 0;
				int num2 = 0;
				foreach (IntVec3 sandbagCell in this.sandbagCells)
				{
					if (TutorUtility.BuildingOrBlueprintOrFrameCenterExists(sandbagCell, base.Map, ThingDefOf.Sandbags))
					{
						num2++;
					}
					num++;
				}
				return (float)num2 / (float)num;
			}
		}

		public override void OnActivated()
		{
			base.OnActivated();
			Find.TutorialState.sandbagsRect = TutorUtility.FindUsableRect(7, 7, base.Map, 0f, false);
			this.sandbagCells = new List<IntVec3>();
			foreach (IntVec3 edgeCell in Find.TutorialState.sandbagsRect.EdgeCells)
			{
				IntVec3 current = edgeCell;
				int x = current.x;
				IntVec3 centerCell = Find.TutorialState.sandbagsRect.CenterCell;
				if (x != centerCell.x)
				{
					int z = current.z;
					IntVec3 centerCell2 = Find.TutorialState.sandbagsRect.CenterCell;
					if (z != centerCell2.z)
					{
						this.sandbagCells.Add(current);
					}
				}
			}
			foreach (IntVec3 item in Find.TutorialState.sandbagsRect.ContractedBy(1))
			{
				if (!Find.TutorialState.sandbagsRect.ContractedBy(2).Contains(item))
				{
					List<Thing> thingList = item.GetThingList(base.Map);
					for (int num = thingList.Count - 1; num >= 0; num--)
					{
						Thing thing = thingList[num];
						if (thing.def.passability != 0 && (thing.def.category == ThingCategory.Plant || thing.def.category == ThingCategory.Item))
						{
							thing.Destroy(DestroyMode.Vanish);
						}
					}
				}
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.sandbagCells, "sandbagCells", LookMode.Undefined, new object[0]);
		}

		public override void LessonOnGUI()
		{
			TutorUtility.DrawLabelOnGUI(Gen.AveragePosition(this.sandbagCells), base.def.onMapInstruction);
			base.LessonOnGUI();
		}

		public override void LessonUpdate()
		{
			List<IntVec3> cells = (from c in this.sandbagCells
			where !TutorUtility.BuildingOrBlueprintOrFrameCenterExists(c, base.Map, ThingDefOf.Sandbags)
			select c).ToList();
			GenDraw.DrawFieldEdges(cells);
			GenDraw.DrawArrowPointingAt(Gen.AveragePosition(this.sandbagCells), false);
			if (this.ProgressPercent > 0.99989998340606689)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		public override AcceptanceReport AllowAction(EventPack ep)
		{
			return (!(ep.Tag == "Designate-Sandbags")) ? base.AllowAction(ep) : TutorUtility.EventCellsAreWithin(ep, this.sandbagCells);
		}
	}
}
