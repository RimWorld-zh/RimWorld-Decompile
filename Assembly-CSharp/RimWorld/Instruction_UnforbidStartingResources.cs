using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C9 RID: 2249
	public class Instruction_UnforbidStartingResources : Lesson_Instruction
	{
		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x06003375 RID: 13173 RVA: 0x001B96B0 File Offset: 0x001B7AB0
		protected override float ProgressPercent
		{
			get
			{
				return (float)(from it in Find.TutorialState.startingItems
				where !it.IsForbidden(Faction.OfPlayer) || it.Destroyed
				select it).Count<Thing>() / (float)Find.TutorialState.startingItems.Count;
			}
		}

		// Token: 0x06003376 RID: 13174 RVA: 0x001B9708 File Offset: 0x001B7B08
		private IEnumerable<Thing> NeedUnforbidItems()
		{
			return from it in Find.TutorialState.startingItems
			where it.IsForbidden(Faction.OfPlayer) && !it.Destroyed
			select it;
		}

		// Token: 0x06003377 RID: 13175 RVA: 0x001B9749 File Offset: 0x001B7B49
		public override void PostDeactivated()
		{
			base.PostDeactivated();
			Find.TutorialState.startingItems.RemoveAll((Thing it) => !Instruction_EquipWeapons.IsWeapon(it));
		}

		// Token: 0x06003378 RID: 13176 RVA: 0x001B9780 File Offset: 0x001B7B80
		public override void LessonOnGUI()
		{
			foreach (Thing t in this.NeedUnforbidItems())
			{
				TutorUtility.DrawLabelOnThingOnGUI(t, this.def.onMapInstruction);
			}
			base.LessonOnGUI();
		}

		// Token: 0x06003379 RID: 13177 RVA: 0x001B97F0 File Offset: 0x001B7BF0
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
	}
}
