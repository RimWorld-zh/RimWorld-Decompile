using System;
using System.Collections.Generic;
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
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetPawns = true;
			targetingParameters.canTargetBuildings = false;
			targetingParameters.validator = (Predicate<TargetInfo>)((TargetInfo x) => base.BaseTargetValidator(x.Thing));
			return targetingParameters;
		}

		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			if (base.parent.MapHeld != null)
			{
				TargetingParameters tp = this.GetTargetingParameters();
				using (List<Pawn>.Enumerator enumerator = base.parent.MapHeld.mapPawns.AllPawnsSpawned.GetEnumerator())
				{
					Pawn p;
					while (true)
					{
						if (enumerator.MoveNext())
						{
							p = enumerator.Current;
							if (tp.CanTarget((Thing)p))
								break;
							continue;
						}
						yield break;
					}
					yield return (Thing)p;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_010d:
			/*Error near IL_010e: Unexpected return in MoveNext()*/;
		}
	}
}
