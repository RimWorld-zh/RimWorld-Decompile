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
			ScenPart_DisableIncident.<RandomizableIncidents>c__Iterator11B <RandomizableIncidents>c__Iterator11B = new ScenPart_DisableIncident.<RandomizableIncidents>c__Iterator11B();
			ScenPart_DisableIncident.<RandomizableIncidents>c__Iterator11B expr_07 = <RandomizableIncidents>c__Iterator11B;
			expr_07.$PC = -2;
			return expr_07;
		}
	}
}
