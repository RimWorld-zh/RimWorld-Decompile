using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	internal class Building_ShipComputerCore : Building
	{
		private bool CanLaunchNow
		{
			get
			{
				return !ShipUtility.LaunchFailReasons(this).Any();
			}
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			Command_Action launch = new Command_Action
			{
				action = new Action(this.TryLaunch),
				defaultLabel = "CommandShipLaunch".Translate(),
				defaultDesc = "CommandShipLaunchDesc".Translate()
			};
			if (!this.CanLaunchNow)
			{
				launch.Disable(ShipUtility.LaunchFailReasons(this).First());
			}
			if (ShipCountdown.CountingDown)
			{
				launch.Disable((string)null);
			}
			launch.hotKey = KeyBindingDefOf.Misc1;
			launch.icon = ContentFinder<Texture2D>.Get("UI/Commands/LaunchShip", true);
			yield return (Gizmo)launch;
		}

		private void TryLaunch()
		{
			if (this.CanLaunchNow)
			{
				ShipCountdown.InitiateCountdown(this, -1);
			}
		}
	}
}
