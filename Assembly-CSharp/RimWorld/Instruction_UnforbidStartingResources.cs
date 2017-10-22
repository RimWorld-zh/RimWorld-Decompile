using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class Instruction_UnforbidStartingResources : Lesson_Instruction
	{
		protected override float ProgressPercent
		{
			get
			{
				return (float)(from it in Find.TutorialState.startingItems
				where !it.IsForbidden(Faction.OfPlayer) || it.Destroyed
				select it).Count() / (float)Find.TutorialState.startingItems.Count;
			}
		}

		private IEnumerable<Thing> NeedUnforbidItems()
		{
			return from it in Find.TutorialState.startingItems
			where it.IsForbidden(Faction.OfPlayer) && !it.Destroyed
			select it;
		}

		public override void PostDeactivated()
		{
			base.PostDeactivated();
			Find.TutorialState.startingItems.RemoveAll((Predicate<Thing>)((Thing it) => !Instruction_EquipWeapons.IsWeapon(it)));
		}

		public override void LessonOnGUI()
		{
			foreach (Thing item in this.NeedUnforbidItems())
			{
				TutorUtility.DrawLabelOnThingOnGUI(item, base.def.onMapInstruction);
			}
			base.LessonOnGUI();
		}

		public override void LessonUpdate()
		{
			if (this.ProgressPercent > 0.99989998340606689)
			{
				Find.ActiveLesson.Deactivate();
			}
			foreach (Thing item in this.NeedUnforbidItems())
			{
				GenDraw.DrawArrowPointingAt(item.DrawPos, true);
			}
		}
	}
}
