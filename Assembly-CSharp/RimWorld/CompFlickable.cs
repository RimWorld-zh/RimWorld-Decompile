using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000416 RID: 1046
	public class CompFlickable : ThingComp
	{
		// Token: 0x04000AF8 RID: 2808
		private bool switchOnInt = true;

		// Token: 0x04000AF9 RID: 2809
		private bool wantSwitchOn = true;

		// Token: 0x04000AFA RID: 2810
		private Graphic offGraphic;

		// Token: 0x04000AFB RID: 2811
		private Texture2D cachedCommandTex;

		// Token: 0x04000AFC RID: 2812
		private const string OffGraphicSuffix = "_Off";

		// Token: 0x04000AFD RID: 2813
		public const string FlickedOnSignal = "FlickedOn";

		// Token: 0x04000AFE RID: 2814
		public const string FlickedOffSignal = "FlickedOff";

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06001209 RID: 4617 RVA: 0x0009D374 File Offset: 0x0009B774
		private CompProperties_Flickable Props
		{
			get
			{
				return (CompProperties_Flickable)this.props;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x0600120A RID: 4618 RVA: 0x0009D394 File Offset: 0x0009B794
		private Texture2D CommandTex
		{
			get
			{
				if (this.cachedCommandTex == null)
				{
					this.cachedCommandTex = ContentFinder<Texture2D>.Get(this.Props.commandTexture, true);
				}
				return this.cachedCommandTex;
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x0600120B RID: 4619 RVA: 0x0009D3D8 File Offset: 0x0009B7D8
		// (set) Token: 0x0600120C RID: 4620 RVA: 0x0009D3F4 File Offset: 0x0009B7F4
		public bool SwitchIsOn
		{
			get
			{
				return this.switchOnInt;
			}
			set
			{
				if (this.switchOnInt != value)
				{
					this.switchOnInt = value;
					if (this.switchOnInt)
					{
						this.parent.BroadcastCompSignal("FlickedOn");
					}
					else
					{
						this.parent.BroadcastCompSignal("FlickedOff");
					}
					if (this.parent.Spawned)
					{
						this.parent.Map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlag.Things | MapMeshFlag.Buildings);
					}
				}
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x0600120D RID: 4621 RVA: 0x0009D47C File Offset: 0x0009B87C
		public Graphic CurrentGraphic
		{
			get
			{
				Graphic defaultGraphic;
				if (this.SwitchIsOn)
				{
					defaultGraphic = this.parent.DefaultGraphic;
				}
				else
				{
					if (this.offGraphic == null)
					{
						this.offGraphic = GraphicDatabase.Get(this.parent.def.graphicData.graphicClass, this.parent.def.graphicData.texPath + "_Off", this.parent.def.graphicData.shaderType.Shader, this.parent.def.graphicData.drawSize, this.parent.DrawColor, this.parent.DrawColorTwo);
					}
					defaultGraphic = this.offGraphic;
				}
				return defaultGraphic;
			}
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x0009D545 File Offset: 0x0009B945
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.switchOnInt, "switchOn", true, false);
			Scribe_Values.Look<bool>(ref this.wantSwitchOn, "wantSwitchOn", true, false);
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x0009D574 File Offset: 0x0009B974
		public bool WantsFlick()
		{
			return this.wantSwitchOn != this.switchOnInt;
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x0009D59A File Offset: 0x0009B99A
		public void DoFlick()
		{
			this.SwitchIsOn = !this.SwitchIsOn;
			SoundDefOf.FlickSwitch.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x0009D5D7 File Offset: 0x0009B9D7
		public void ResetToOn()
		{
			this.switchOnInt = true;
			this.wantSwitchOn = true;
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x0009D5E8 File Offset: 0x0009B9E8
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo c in this.<CompGetGizmosExtra>__BaseCallProxy0())
			{
				yield return c;
			}
			if (this.parent.Faction == Faction.OfPlayer)
			{
				yield return new Command_Toggle
				{
					hotKey = KeyBindingDefOf.Command_TogglePower,
					icon = this.CommandTex,
					defaultLabel = this.Props.commandLabelKey.Translate(),
					defaultDesc = this.Props.commandDescKey.Translate(),
					isActive = (() => this.wantSwitchOn),
					toggleAction = delegate()
					{
						this.wantSwitchOn = !this.wantSwitchOn;
						FlickUtility.UpdateFlickDesignation(this.parent);
					}
				};
			}
			yield break;
		}
	}
}
