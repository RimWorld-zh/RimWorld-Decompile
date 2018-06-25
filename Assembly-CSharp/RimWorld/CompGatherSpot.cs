using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000714 RID: 1812
	public class CompGatherSpot : ThingComp
	{
		// Token: 0x040015E1 RID: 5601
		private bool active = true;

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x060027CE RID: 10190 RVA: 0x001551A0 File Offset: 0x001535A0
		// (set) Token: 0x060027CF RID: 10191 RVA: 0x001551BC File Offset: 0x001535BC
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
		}

		// Token: 0x060027D0 RID: 10192 RVA: 0x00155230 File Offset: 0x00153630
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.active, "active", false, false);
		}

		// Token: 0x060027D1 RID: 10193 RVA: 0x00155245 File Offset: 0x00153645
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (this.Active)
			{
				this.parent.Map.gatherSpotLister.RegisterActivated(this);
			}
		}

		// Token: 0x060027D2 RID: 10194 RVA: 0x00155270 File Offset: 0x00153670
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (this.Active)
			{
				map.gatherSpotLister.RegisterDeactivated(this);
			}
		}

		// Token: 0x060027D3 RID: 10195 RVA: 0x00155294 File Offset: 0x00153694
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			Command_Toggle com = new Command_Toggle();
			com.hotKey = KeyBindingDefOf.Command_TogglePower;
			com.defaultLabel = "CommandGatherSpotToggleLabel".Translate();
			com.icon = TexCommand.GatherSpotActive;
			com.isActive = (() => this.Active);
			com.toggleAction = delegate()
			{
				this.Active = !this.Active;
			};
			if (this.Active)
			{
				com.defaultDesc = "CommandGatherSpotToggleDescActive".Translate();
			}
			else
			{
				com.defaultDesc = "CommandGatherSpotToggleDescInactive".Translate();
			}
			yield return com;
			yield break;
		}
	}
}
