using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C6 RID: 2246
	public class Instruction_FinishConstruction : Lesson_Instruction
	{
		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x06003351 RID: 13137 RVA: 0x001B8BA8 File Offset: 0x001B6FA8
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

		// Token: 0x06003352 RID: 13138 RVA: 0x001B8C0C File Offset: 0x001B700C
		private IEnumerable<Thing> ConstructionNeeders()
		{
			return from b in base.Map.listerThings.ThingsInGroup(ThingRequestGroup.Blueprint).Concat(base.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingFrame))
			where b.Faction == Faction.OfPlayer
			select b;
		}

		// Token: 0x06003353 RID: 13139 RVA: 0x001B8C6C File Offset: 0x001B706C
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

		// Token: 0x04001B99 RID: 7065
		private int initialBlueprintsCount = -1;
	}
}
