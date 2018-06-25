using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200068B RID: 1675
	internal class Building_ShipComputerCore : Building
	{
		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06002373 RID: 9075 RVA: 0x00130DEC File Offset: 0x0012F1EC
		private bool CanLaunchNow
		{
			get
			{
				return !ShipUtility.LaunchFailReasons(this).Any<string>();
			}
		}

		// Token: 0x06002374 RID: 9076 RVA: 0x00130E10 File Offset: 0x0012F210
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

		// Token: 0x06002375 RID: 9077 RVA: 0x00130E3A File Offset: 0x0012F23A
		private void TryLaunch()
		{
			if (this.CanLaunchNow)
			{
				ShipCountdown.InitiateCountdown(this);
			}
		}
	}
}
