using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000418 RID: 1048
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
		// (get) Token: 0x0600120D RID: 4621 RVA: 0x0009D4C4 File Offset: 0x0009B8C4
		private CompProperties_Flickable Props
		{
			get
			{
				return (CompProperties_Flickable)this.props;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x0600120E RID: 4622 RVA: 0x0009D4E4 File Offset: 0x0009B8E4
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
		// (get) Token: 0x0600120F RID: 4623 RVA: 0x0009D528 File Offset: 0x0009B928
		// (set) Token: 0x06001210 RID: 4624 RVA: 0x0009D544 File Offset: 0x0009B944
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
		// (get) Token: 0x06001211 RID: 4625 RVA: 0x0009D5CC File Offset: 0x0009B9CC
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

		// Token: 0x06001212 RID: 4626 RVA: 0x0009D695 File Offset: 0x0009BA95
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.switchOnInt, "switchOn", true, false);
			Scribe_Values.Look<bool>(ref this.wantSwitchOn, "wantSwitchOn", true, false);
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x0009D6C4 File Offset: 0x0009BAC4
		public bool WantsFlick()
		{
			return this.wantSwitchOn != this.switchOnInt;
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x0009D6EA File Offset: 0x0009BAEA
		public void DoFlick()
		{
			this.SwitchIsOn = !this.SwitchIsOn;
			SoundDefOf.FlickSwitch.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x0009D727 File Offset: 0x0009BB27
		public void ResetToOn()
		{
			this.switchOnInt = true;
			this.wantSwitchOn = true;
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x0009D738 File Offset: 0x0009BB38
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
