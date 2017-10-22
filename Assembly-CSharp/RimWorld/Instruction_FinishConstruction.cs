using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class Instruction_FinishConstruction : Lesson_Instruction
	{
		private int initialBlueprintsCount = -1;

		protected override float ProgressPercent
		{
			get
			{
				if (this.initialBlueprintsCount < 0)
				{
					this.initialBlueprintsCount = this.ConstructionNeeders().Count();
				}
				if (this.initialBlueprintsCount == 0)
				{
					return 1f;
				}
				return (float)(1.0 - (float)this.ConstructionNeeders().Count() / (float)this.initialBlueprintsCount);
			}
		}

		private IEnumerable<Thing> ConstructionNeeders()
		{
			return from b in base.Map.listerThings.ThingsInGroup(ThingRequestGroup.Blueprint).Concat(base.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingFrame))
			where b.Faction == Faction.OfPlayer
			select b;
		}

		public override void LessonUpdate()
		{
			base.LessonUpdate();
			if (this.ConstructionNeeders().Count() < 3)
			{
				foreach (Thing item in this.ConstructionNeeders())
				{
					GenDraw.DrawArrowPointingAt(item.DrawPos, false);
				}
			}
			if (this.ProgressPercent > 0.99989998340606689)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
