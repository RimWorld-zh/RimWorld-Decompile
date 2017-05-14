using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public class CompTargetable_AllPawnsOnTheMap : CompTargetable
	{
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return false;
			}
		}

		protected override TargetingParameters GetTargetingParameters()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false,
				validator = ((TargetInfo x) => base.BaseTargetValidator(x.Thing))
			};
		}

		[DebuggerHidden]
		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			CompTargetable_AllPawnsOnTheMap.<GetTargets>c__Iterator172 <GetTargets>c__Iterator = new CompTargetable_AllPawnsOnTheMap.<GetTargets>c__Iterator172();
			<GetTargets>c__Iterator.<>f__this = this;
			CompTargetable_AllPawnsOnTheMap.<GetTargets>c__Iterator172 expr_0E = <GetTargets>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
