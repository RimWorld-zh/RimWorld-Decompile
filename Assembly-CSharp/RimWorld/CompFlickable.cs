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
		// Token: 0x04000AFB RID: 2811
		private bool switchOnInt = true;

		// Token: 0x04000AFC RID: 2812
		private bool wantSwitchOn = true;

		// Token: 0x04000AFD RID: 2813
		private Graphic offGraphic;

		// Token: 0x04000AFE RID: 2814
		private Texture2D cachedCommandTex;

		// Token: 0x04000AFF RID: 2815
		private const string OffGraphicSuffix = "_Off";

		// Token: 0x04000B00 RID: 2816
		public const string FlickedOnSignal = "FlickedOn";

		// Token: 0x04000B01 RID: 2817
		public const string FlickedOffSignal = "FlickedOff";

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x0600120C RID: 4620 RVA: 0x0009D4D4 File Offset: 0x0009B8D4
		private CompProperties_Flickable Props
		{
			get
			{
				return (CompProperties_Flickable)this.props;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x0600120D RID: 4621 RVA: 0x0009D4F4 File Offset: 0x0009B8F4
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
		// (get) Token: 0x0600120E RID: 4622 RVA: 0x0009D538 File Offset: 0x0009B938
		// (set) Token: 0x0600120F RID: 4623 RVA: 0x0009D554 File Offset: 0x0009B954
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
		// (get) Token: 0x06001210 RID: 4624 RVA: 0x0009D5DC File Offset: 0x0009B9DC
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

		// Token: 0x06001211 RID: 4625 RVA: 0x0009D6A5 File Offset: 0x0009BAA5
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.switchOnInt, "switchOn", true, false);
			Scribe_Values.Look<bool>(ref this.wantSwitchOn, "wantSwitchOn", true, false);
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x0009D6D4 File Offset: 0x0009BAD4
		public bool WantsFlick()
		{
			return this.wantSwitchOn != this.switchOnInt;
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x0009D6FA File Offset: 0x0009BAFA
		public void DoFlick()
		{
			this.SwitchIsOn = !this.SwitchIsOn;
			SoundDefOf.FlickSwitch.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x0009D737 File Offset: 0x0009BB37
		public void ResetToOn()
		{
			this.switchOnInt = true;
			this.wantSwitchOn = true;
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x0009D748 File Offset: 0x0009BB48
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
