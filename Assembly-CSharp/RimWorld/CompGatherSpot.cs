using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000712 RID: 1810
	public class CompGatherSpot : ThingComp
	{
		// Token: 0x040015DD RID: 5597
		private bool active = true;

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x060027CB RID: 10187 RVA: 0x00154DF0 File Offset: 0x001531F0
		// (set) Token: 0x060027CC RID: 10188 RVA: 0x00154E0C File Offset: 0x0015320C
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

		// Token: 0x060027CD RID: 10189 RVA: 0x00154E80 File Offset: 0x00153280
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.active, "active", false, false);
		}

		// Token: 0x060027CE RID: 10190 RVA: 0x00154E95 File Offset: 0x00153295
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (this.Active)
			{
				this.parent.Map.gatherSpotLister.RegisterActivated(this);
			}
		}

		// Token: 0x060027CF RID: 10191 RVA: 0x00154EC0 File Offset: 0x001532C0
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (this.Active)
			{
				map.gatherSpotLister.RegisterDeactivated(this);
			}
		}

		// Token: 0x060027D0 RID: 10192 RVA: 0x00154EE4 File Offset: 0x001532E4
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
