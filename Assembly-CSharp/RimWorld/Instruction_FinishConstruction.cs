using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C2 RID: 2242
	public class Instruction_FinishConstruction : Lesson_Instruction
	{
		// Token: 0x04001B97 RID: 7063
		private int initialBlueprintsCount = -1;

		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x0600334C RID: 13132 RVA: 0x001B8E58 File Offset: 0x001B7258
		protected override float ProgressPercent
		{
			get
			{
				if (this.initialBlueprintsCount < 0)
				{
					this.initialBlueprintsCount = this.ConstructionNeeders().Count<Thing>();
				}
				float result;
				if (this.initialBlueprintsCount == 0)
				{
					result = 1f;
				}
				else
				{
					result = 1f - (float)this.ConstructionNeeders().Count<Thing>() / (float)this.initialBlueprintsCount;
				}
				return result;
			}
		}

		// Token: 0x0600334D RID: 13133 RVA: 0x001B8EBC File Offset: 0x001B72BC
		private IEnumerable<Thing> ConstructionNeeders()
		{
			return from b in base.Map.listerThings.ThingsInGroup(ThingRequestGroup.Blueprint).Concat(base.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingFrame))
			where b.Faction == Faction.OfPlayer
			select b;
		}

		// Token: 0x0600334E RID: 13134 RVA: 0x001B8F1C File Offset: 0x001B731C
		public override void LessonUpdate()
		{
			base.LessonUpdate();
			if (this.ConstructionNeeders().Count<Thing>() < 3)
			{
				foreach (Thing thing in this.ConstructionNeeders())
				{
					GenDraw.DrawArrowPointingAt(thing.DrawPos, false);
				}
			}
			if (this.ProgressPercent > 0.9999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
