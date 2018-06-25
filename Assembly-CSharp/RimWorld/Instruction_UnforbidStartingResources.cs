using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008CB RID: 2251
	public class Instruction_UnforbidStartingResources : Lesson_Instruction
	{
		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x06003379 RID: 13177 RVA: 0x001B9AC4 File Offset: 0x001B7EC4
		protected override float ProgressPercent
		{
			get
			{
				return (float)(from it in Find.TutorialState.startingItems
				where !it.IsForbidden(Faction.OfPlayer) || it.Destroyed
				select it).Count<Thing>() / (float)Find.TutorialState.startingItems.Count;
			}
		}

		// Token: 0x0600337A RID: 13178 RVA: 0x001B9B1C File Offset: 0x001B7F1C
		private IEnumerable<Thing> NeedUnforbidItems()
		{
			return from it in Find.TutorialState.startingItems
			where it.IsForbidden(Faction.OfPlayer) && !it.Destroyed
			select it;
		}

		// Token: 0x0600337B RID: 13179 RVA: 0x001B9B5D File Offset: 0x001B7F5D
		public override void PostDeactivated()
		{
			base.PostDeactivated();
			Find.TutorialState.startingItems.RemoveAll((Thing it) => !Instruction_EquipWeapons.IsWeapon(it));
		}

		// Token: 0x0600337C RID: 13180 RVA: 0x001B9B94 File Offset: 0x001B7F94
		public override void LessonOnGUI()
		{
			foreach (Thing t in this.NeedUnforbidItems())
			{
				TutorUtility.DrawLabelOnThingOnGUI(t, this.def.onMapInstruction);
			}
			base.LessonOnGUI();
		}

		// Token: 0x0600337D RID: 13181 RVA: 0x001B9C04 File Offset: 0x001B8004
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
