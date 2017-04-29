using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RimWorld
{
	public class ScenPart_DisableIncident : ScenPart_IncidentBase
	{
		protected override string IncidentTag
		{
			get
			{
				return "DisableIncident";
			}
		}

		[DebuggerHidden]
		protected override IEnumerable<IncidentDef> RandomizableIncidents()
		{
			ScenPart_DisableIncident.<RandomizableIncidents>c__Iterator11A <RandomizableIncidents>c__Iterator11A = new ScenPart_DisableIncident.<RandomizableIncidents>c__Iterator11A();
			ScenPart_DisableIncident.<RandomizableIncidents>c__Iterator11A expr_07 = <RandomizableIncidents>c__Iterator11A;
			expr_07.$PC = -2;
			return expr_07;
		}
	}
}
