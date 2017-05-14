using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	public class CompShipPart : ThingComp
	{
		[DebuggerHidden]
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompShipPart.<CompGetGizmosExtra>c__Iterator170 <CompGetGizmosExtra>c__Iterator = new CompShipPart.<CompGetGizmosExtra>c__Iterator170();
			<CompGetGizmosExtra>c__Iterator.<>f__this = this;
			CompShipPart.<CompGetGizmosExtra>c__Iterator170 expr_0E = <CompGetGizmosExtra>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public void ShowReport()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!ShipUtility.LaunchFailReasons((Building)this.parent).Any<string>())
			{
				stringBuilder.AppendLine("ShipReportCanLaunch".Translate());
			}
			else
			{
				stringBuilder.AppendLine("ShipReportCannotLaunch".Translate());
				foreach (string current in ShipUtility.LaunchFailReasons((Building)this.parent))
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine(current);
				}
			}
			Dialog_MessageBox window = new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false);
			Find.WindowStack.Add(window);
		}
	}
}
