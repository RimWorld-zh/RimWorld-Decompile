using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class Instruction_FinishConstruction : Lesson_Instruction
	{
		private int initialBlueprintsCount = -1;

		[CompilerGenerated]
		private static Func<Thing, bool> <>f__am$cache0;

		public Instruction_FinishConstruction()
		{
		}

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

		private IEnumerable<Thing> ConstructionNeeders()
		{
			return from b in base.Map.listerThings.ThingsInGroup(ThingRequestGroup.Blueprint).Concat(base.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingFrame))
			where b.Faction == Faction.OfPlayer
			select b;
		}

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

		[CompilerGenerated]
		private static bool <ConstructionNeeders>m__0(Thing b)
		{
			return b.Faction == Faction.OfPlayer;
		}
	}
}
