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
			if (target == Find.Maps.Find((Predicate<Map>)((Map x) => x.IsPlayerHome)))
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
					}
				}
				if (this.IntervalsPassed == 204)
				{
					IncidentCategory threatCategory = (IncidentCategory)((!Find.Storyteller.difficulty.allowIntroThreats) ? 1 : 2);
					IncidentDef incDef2;
					if ((from def in DefDatabase<IncidentDef>.AllDefs
					where def.TargetAllowed(((_003CMakeIntervalIncidents_003Ec__IteratorA9)/*Error near IL_0135: stateMachine*/).target) && def.category == ((_003CMakeIntervalIncidents_003Ec__IteratorA9)/*Error near IL_0135: stateMachine*/)._003CthreatCategory_003E__2
					select def).TryRandomElementByWeight<IncidentDef>(new Func<IncidentDef, float>(base.IncidentChanceFinal), out incDef2))
					{
						yield return new FiringIncident(incDef2, this, null)
						{
							parms = StorytellerUtility.DefaultParmsNow(Find.Storyteller.def, incDef2.category, target)
						};
					}
				}
				IncidentDef incDef;
				if (this.IntervalsPassed == 264 && (from def in DefDatabase<IncidentDef>.AllDefs
				where def.TargetAllowed(((_003CMakeIntervalIncidents_003Ec__IteratorA9)/*Error near IL_01dc: stateMachine*/).target) && def.category == IncidentCategory.Misc
				select def).TryRandomElementByWeight<IncidentDef>(new Func<IncidentDef, float>(base.IncidentChanceFinal), out incDef))
				{
					yield return new FiringIncident(incDef, this, null)
					{
						parms = StorytellerUtility.DefaultParmsNow(Find.Storyteller.def, incDef.category, target)
					};
				}
				if (this.IntervalsPassed == 324)
				{
					IncidentDef inc = IncidentDefOf.RaidEnemy;
					if (!Find.Storyteller.difficulty.allowIntroThreats)
					{
						inc = (from def in DefDatabase<IncidentDef>.AllDefs
						where def.TargetAllowed(((_003CMakeIntervalIncidents_003Ec__IteratorA9)/*Error near IL_02a3: stateMachine*/).target) && def.category == IncidentCategory.Misc
						select def).RandomElementByWeightWithFallback(new Func<IncidentDef, float>(base.IncidentChanceFinal), null);
					}
					if (inc != null && inc.TargetAllowed(target))
					{
						yield return new FiringIncident(inc, this, null)
						{
							parms = this.GenerateParms(inc.category, target)
						};
					}
				}
			}
		}
	}
}
