using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000689 RID: 1673
	internal class Building_ShipComputerCore : Building
	{
		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06002370 RID: 9072 RVA: 0x00130A34 File Offset: 0x0012EE34
		private bool CanLaunchNow
		{
			get
			{
				return !ShipUtility.LaunchFailReasons(this).Any<string>();
			}
		}

		// Token: 0x06002371 RID: 9073 RVA: 0x00130A58 File Offset: 0x0012EE58
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

		// Token: 0x06002372 RID: 9074 RVA: 0x00130A82 File Offset: 0x0012EE82
		private void TryLaunch()
		{
			if (this.CanLaunchNow)
			{
				ShipCountdown.InitiateCountdown(this);
			}
		}
	}
}
