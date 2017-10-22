using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class CompShipPart : ThingComp
	{
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			yield return (Gizmo)new Command_Action
			{
				action = new Action(this.ShowReport),
				defaultLabel = "CommandShipLaunchReport".Translate(),
				defaultDesc = "CommandShipLaunchReportDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc4,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/LaunchReport", true)
			};
		}

		public void ShowReport()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!ShipUtility.LaunchFailReasons((Building)base.parent).Any())
			{
				stringBuilder.AppendLine("ShipReportCanLaunch".Translate());
			}
			else
			{
				stringBuilder.AppendLine("ShipReportCannotLaunch".Translate());
				foreach (string item in ShipUtility.LaunchFailReasons((Building)base.parent))
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine(item);
				}
			}
			Dialog_MessageBox window = new Dialog_MessageBox(stringBuilder.ToString(), (string)null, null, (string)null, null, (string)null, false);
			Find.WindowStack.Add(window);
		}
	}
}
