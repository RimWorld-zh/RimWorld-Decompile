using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000716 RID: 1814
	public class CompGatherSpot : ThingComp
	{
		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x060027D1 RID: 10193 RVA: 0x00154BC0 File Offset: 0x00152FC0
		// (set) Token: 0x060027D2 RID: 10194 RVA: 0x00154BDC File Offset: 0x00152FDC
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

		// Token: 0x060027D3 RID: 10195 RVA: 0x00154C50 File Offset: 0x00153050
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.active, "active", false, false);
		}

		// Token: 0x060027D4 RID: 10196 RVA: 0x00154C65 File Offset: 0x00153065
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (this.Active)
			{
				this.parent.Map.gatherSpotLister.RegisterActivated(this);
			}
		}

		// Token: 0x060027D5 RID: 10197 RVA: 0x00154C90 File Offset: 0x00153090
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (this.Active)
			{
				map.gatherSpotLister.RegisterDeactivated(this);
			}
		}

		// Token: 0x060027D6 RID: 10198 RVA: 0x00154CB4 File Offset: 0x001530B4
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

		// Token: 0x040015DF RID: 5599
		private bool active = true;
	}
}
