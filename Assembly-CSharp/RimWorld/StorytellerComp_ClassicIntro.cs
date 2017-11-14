using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class StorytellerComp_ClassicIntro : StorytellerComp
	{
		protected int IntervalsPassed
		{
			get
			{
				return Find.TickManager.TicksGame / 1000;
			}
		}

		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			_003CMakeIntervalIncidents_003Ec__Iterator0 _003CMakeIntervalIncidents_003Ec__Iterator = (_003CMakeIntervalIncidents_003Ec__Iterator0)/*Error near IL_003e: stateMachine*/;
			if (target == Find.Maps.Find((Map x) => x.IsPlayerHome))
			{
				if (this.IntervalsPassed == 150)
				{
					IncidentDef inc2 = IncidentDefOf.VisitorGroup;
					if (inc2.TargetAllowed(target))
					{
						yield return new FiringIncident(inc2, this, null)
						{
							parms = 
							{
								target = target,
								points = (float)Rand.Range(40, 100)
							}
						};
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				if (this.IntervalsPassed == 204)
				{
					_003CMakeIntervalIncidents_003Ec__Iterator0 _003CMakeIntervalIncidents_003Ec__Iterator2 = (_003CMakeIntervalIncidents_003Ec__Iterator0)/*Error near IL_015f: stateMachine*/;
					IncidentCategory threatCategory = (IncidentCategory)((!Find.Storyteller.difficulty.allowIntroThreats) ? 1 : 2);
					IncidentDef incDef2;
					if ((from def in DefDatabase<IncidentDef>.AllDefs
					where def.TargetAllowed(target) && def.category == threatCategory
					select def).TryRandomElementByWeight<IncidentDef>((Func<IncidentDef, float>)base.IncidentChanceFinal, out incDef2))
					{
						yield return new FiringIncident(incDef2, this, null)
						{
							parms = StorytellerUtility.DefaultParmsNow(Find.Storyteller.def, incDef2.category, target)
						};
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				IncidentDef incDef;
				if (this.IntervalsPassed == 264 && (from def in DefDatabase<IncidentDef>.AllDefs
				where def.TargetAllowed(target) && def.category == IncidentCategory.Misc
				select def).TryRandomElementByWeight<IncidentDef>((Func<IncidentDef, float>)base.IncidentChanceFinal, out incDef))
				{
					yield return new FiringIncident(incDef, this, null)
					{
						parms = StorytellerUtility.DefaultParmsNow(Find.Storyteller.def, incDef.category, target)
					};
					/*Error: Unable to find new state assignment for yield return*/;
				}
				if (this.IntervalsPassed != 324)
					yield break;
				IncidentDef inc = IncidentDefOf.RaidEnemy;
				if (!Find.Storyteller.difficulty.allowIntroThreats)
				{
					inc = (from def in DefDatabase<IncidentDef>.AllDefs
					where def.TargetAllowed(target) && def.category == IncidentCategory.Misc
					select def).RandomElementByWeightWithFallback(base.IncidentChanceFinal, null);
				}
				if (inc == null)
					yield break;
				if (!inc.TargetAllowed(target))
					yield break;
				yield return new FiringIncident(inc, this, null)
				{
					parms = this.GenerateParms(inc.category, target)
				};
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}
	}
}
