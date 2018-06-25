using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class Instruction_UnforbidStartingResources : Lesson_Instruction
	{
		[CompilerGenerated]
		private static Func<Thing, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Thing, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Predicate<Thing> <>f__am$cache2;

		public Instruction_UnforbidStartingResources()
		{
		}

		protected override float ProgressPercent
		{
			get
			{
				return (float)(from it in Find.TutorialState.startingItems
				where !it.IsForbidden(Faction.OfPlayer) || it.Destroyed
				select it).Count<Thing>() / (float)Find.TutorialState.startingItems.Count;
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
			Find.TutorialState.startingItems.RemoveAll((Thing it) => !Instruction_EquipWeapons.IsWeapon(it));
		}

		public override void LessonOnGUI()
		{
			foreach (Thing t in this.NeedUnforbidItems())
			{
				TutorUtility.DrawLabelOnThingOnGUI(t, this.def.onMapInstruction);
			}
			base.LessonOnGUI();
		}

		public override void LessonUpdate()
		{
			if (this.ProgressPercent > 0.9999f)
			{
				Find.ActiveLesson.Deactivate();
			}
			foreach (Thing thing in this.NeedUnforbidItems())
			{
				GenDraw.DrawArrowPointingAt(thing.DrawPos, true);
			}
		}

		[CompilerGenerated]
		private static bool <get_ProgressPercent>m__0(Thing it)
		{
			return !it.IsForbidden(Faction.OfPlayer) || it.Destroyed;
		}

		[CompilerGenerated]
		private static bool <NeedUnforbidItems>m__1(Thing it)
		{
			return it.IsForbidden(Faction.OfPlayer) && !it.Destroyed;
		}

		[CompilerGenerated]
		private static bool <PostDeactivated>m__2(Thing it)
		{
			return !Instruction_EquipWeapons.IsWeapon(it);
		}
	}
}
