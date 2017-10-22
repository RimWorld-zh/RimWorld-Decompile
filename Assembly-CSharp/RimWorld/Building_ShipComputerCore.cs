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
			using (IEnumerator<Gizmo> enumerator = this._003CGetGizmos_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Gizmo c = enumerator.Current;
					yield return c;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (ShipUtility.HasHibernatingParts(this))
			{
				yield return (Gizmo)new Command_Action
				{
					action = (Action)delegate
					{
						DiaNode diaNode = new DiaNode("HibernateWarning".Translate());
						DiaOption diaOption = new DiaOption("Confirm".Translate());
						diaOption.action = (Action)delegate
						{
							ShipUtility.StartupHibernatingParts(((_003CGetGizmos_003Ec__Iterator0)/*Error near IL_00de: stateMachine*/)._0024this);
						};
						diaOption.resolveTree = true;
						diaNode.options.Add(diaOption);
						DiaOption diaOption2 = new DiaOption("GoBack".Translate());
						diaOption2.resolveTree = true;
						diaNode.options.Add(diaOption2);
						Find.WindowStack.Add(new Dialog_NodeTree(diaNode, true, false, (string)null));
					},
					defaultLabel = "CommandShipStartup".Translate(),
					defaultDesc = "CommandShipStartupDesc".Translate(),
					hotKey = KeyBindingDefOf.Misc1,
					icon = ContentFinder<Texture2D>.Get("UI/Commands/DesirePower", true)
				};
				/*Error: Unable to find new state assignment for yield return*/;
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
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0241:
			/*Error near IL_0242: Unexpected return in MoveNext()*/;
		}

		private void TryLaunch()
		{
			if (this.CanLaunchNow)
			{
				ShipCountdown.InitiateCountdown(this);
			}
		}
	}
}
