using System;
using System.Collections.Generic;
using System.Diagnostics;
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
				if (value == this.active)
				{
					return;
				}
				this.active = value;
				if (this.parent.Spawned)
				{
					if (this.active)
					{
						this.parent.Map.gatherSpotLister.RegisterActivated(this);
					}
					else
					{
						this.parent.Map.gatherSpotLister.RegisterDeactivated(this);
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
				this.parent.Map.gatherSpotLister.RegisterActivated(this);
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

		[DebuggerHidden]
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompGatherSpot.<CompGetGizmosExtra>c__Iterator163 <CompGetGizmosExtra>c__Iterator = new CompGatherSpot.<CompGetGizmosExtra>c__Iterator163();
			<CompGetGizmosExtra>c__Iterator.<>f__this = this;
			CompGatherSpot.<CompGetGizmosExtra>c__Iterator163 expr_0E = <CompGetGizmosExtra>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
