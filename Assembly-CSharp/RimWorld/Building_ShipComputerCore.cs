using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200068D RID: 1677
	internal class Building_ShipComputerCore : Building
	{
		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06002376 RID: 9078 RVA: 0x00130874 File Offset: 0x0012EC74
		private bool CanLaunchNow
		{
			get
			{
				return !ShipUtility.LaunchFailReasons(this).Any<string>();
			}
		}

		// Token: 0x06002377 RID: 9079 RVA: 0x00130898 File Offset: 0x0012EC98
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo c in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return c;
			}
			foreach (Gizmo c2 in ShipUtility.ShipStartupGizmos(this))
			{
				yield return c2;
			}
			Command_Action launch = new Command_Action();
			launch.action = new Action(this.TryLaunch);
			launch.defaultLabel = "CommandShipLaunch".Translate();
			launch.defaultDesc = "CommandShipLaunchDesc".Translate();
			if (!this.CanLaunchNow)
			{
				launch.Disable(ShipUtility.LaunchFailReasons(this).First<string>());
			}
			if (ShipCountdown.CountingDown)
			{
				launch.Disable(null);
			}
			launch.hotKey = KeyBindingDefOf.Misc1;
			launch.icon = ContentFinder<Texture2D>.Get("UI/Commands/LaunchShip", true);
			yield return launch;
			yield break;
		}

		// Token: 0x06002378 RID: 9080 RVA: 0x001308C2 File Offset: 0x0012ECC2
		private void TryLaunch()
		{
			if (this.CanLaunchNow)
			{
				ShipCountdown.InitiateCountdown(this);
			}
		}
	}
}
