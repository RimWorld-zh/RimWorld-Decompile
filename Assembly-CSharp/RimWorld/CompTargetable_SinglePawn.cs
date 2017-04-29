using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public class CompTargetable_SinglePawn : CompTargetable
	{
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return true;
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
			CompTargetable_SinglePawn.<GetTargets>c__Iterator171 <GetTargets>c__Iterator = new CompTargetable_SinglePawn.<GetTargets>c__Iterator171();
			<GetTargets>c__Iterator.targetChosenByPlayer = targetChosenByPlayer;
			<GetTargets>c__Iterator.<$>targetChosenByPlayer = targetChosenByPlayer;
			CompTargetable_SinglePawn.<GetTargets>c__Iterator171 expr_15 = <GetTargets>c__Iterator;
			expr_15.$PC = -2;
			return expr_15;
		}
	}
}
