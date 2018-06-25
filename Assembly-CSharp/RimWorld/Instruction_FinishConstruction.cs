using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C4 RID: 2244
	public class Instruction_FinishConstruction : Lesson_Instruction
	{
		// Token: 0x04001B9D RID: 7069
		private int initialBlueprintsCount = -1;

		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x06003350 RID: 13136 RVA: 0x001B926C File Offset: 0x001B766C
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

		// Token: 0x06003351 RID: 13137 RVA: 0x001B92D0 File Offset: 0x001B76D0
		private IEnumerable<Thing> ConstructionNeeders()
		{
			return from b in base.Map.listerThings.ThingsInGroup(ThingRequestGroup.Blueprint).Concat(base.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingFrame))
			where b.Faction == Faction.OfPlayer
			select b;
		}

		// Token: 0x06003352 RID: 13138 RVA: 0x001B9330 File Offset: 0x001B7730
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
