using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200074E RID: 1870
	public class CompShipPart : ThingComp
	{
		// Token: 0x06002973 RID: 10611 RVA: 0x00160080 File Offset: 0x0015E480
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			yield return new Command_Action
			{
				action = new Action(this.ShowReport),
				defaultLabel = "CommandShipLaunchReport".Translate(),
				defaultDesc = "CommandShipLaunchReportDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc4,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/LaunchReport", true)
			};
			yield break;
		}

		// Token: 0x06002974 RID: 10612 RVA: 0x001600AC File Offset: 0x0015E4AC
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
				foreach (string value in ShipUtility.LaunchFailReasons((Building)this.parent))
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine(value);
				}
			}
			Dialog_MessageBox window = new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null);
			Find.WindowStack.Add(window);
		}
	}
}
