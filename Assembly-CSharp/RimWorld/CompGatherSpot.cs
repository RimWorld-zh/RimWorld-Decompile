using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class CompGatherSpot : ThingComp
	{
		private bool active = true;

		public bool Active
		{
			get
			{
				return this.active;
			}
			set
			{
				if (value != this.active)
				{
					this.active = value;
					if (base.parent.Spawned)
					{
						if (this.active)
						{
							base.parent.Map.gatherSpotLister.RegisterActivated(this);
						}
						else
						{
							base.parent.Map.gatherSpotLister.RegisterDeactivated(this);
						}
					}
				}
			}
		}

		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.active, "active", false, false);
		}

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (this.Active)
			{
				base.parent.Map.gatherSpotLister.RegisterActivated(this);
			}
		}

		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (this.Active)
			{
				map.gatherSpotLister.RegisterDeactivated(this);
			}
		}

		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			Command_Toggle com = new Command_Toggle
			{
				hotKey = KeyBindingDefOf.CommandTogglePower,
				defaultLabel = "CommandGatherSpotToggleLabel".Translate(),
				icon = TexCommand.GatherSpotActive,
				isActive = (Func<bool>)(() => ((_003CCompGetGizmosExtra_003Ec__Iterator165)/*Error near IL_0067: stateMachine*/)._003C_003Ef__this.Active),
				toggleAction = (Action)delegate
				{
					((_003CCompGetGizmosExtra_003Ec__Iterator165)/*Error near IL_007e: stateMachine*/)._003C_003Ef__this.Active = !((_003CCompGetGizmosExtra_003Ec__Iterator165)/*Error near IL_007e: stateMachine*/)._003C_003Ef__this.Active;
				}
			};
			if (this.Active)
			{
				com.defaultDesc = "CommandGatherSpotToggleDescActive".Translate();
			}
			else
			{
				com.defaultDesc = "CommandGatherSpotToggleDescInactive".Translate();
			}
			yield return (Gizmo)com;
		}
	}
}
