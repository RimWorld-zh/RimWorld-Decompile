using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public class CompFlickable : ThingComp
	{
		private bool switchOnInt = true;

		private bool wantSwitchOn = true;

		private Graphic offGraphic;

		private Texture2D cachedCommandTex;

		private const string OffGraphicSuffix = "_Off";

		public const string FlickedOnSignal = "FlickedOn";

		public const string FlickedOffSignal = "FlickedOff";

		public CompFlickable()
		{
		}

		private CompProperties_Flickable Props
		{
			get
			{
				return (CompProperties_Flickable)this.props;
			}
		}

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

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.switchOnInt, "switchOn", true, false);
			Scribe_Values.Look<bool>(ref this.wantSwitchOn, "wantSwitchOn", true, false);
		}

		public bool WantsFlick()
		{
			return this.wantSwitchOn != this.switchOnInt;
		}

		public void DoFlick()
		{
			this.SwitchIsOn = !this.SwitchIsOn;
			SoundDefOf.FlickSwitch.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
		}

		public void ResetToOn()
		{
			this.switchOnInt = true;
			this.wantSwitchOn = true;
		}

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

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<Gizmo> <CompGetGizmosExtra>__BaseCallProxy0()
		{
			return base.CompGetGizmosExtra();
		}

		[CompilerGenerated]
		private sealed class <CompGetGizmosExtra>c__Iterator0 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal IEnumerator<Gizmo> $locvar0;

			internal Gizmo <c>__1;

			internal Command_Toggle <com>__2;

			internal CompFlickable $this;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <CompGetGizmosExtra>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = base.<CompGetGizmosExtra>__BaseCallProxy0().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_193;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						c = enumerator.Current;
						this.$current = c;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				if (this.parent.Faction != Faction.OfPlayer)
				{
					goto IL_193;
				}
				Command_Toggle com = new Command_Toggle();
				com.hotKey = KeyBindingDefOf.Command_TogglePower;
				com.icon = base.CommandTex;
				com.defaultLabel = base.Props.commandLabelKey.Translate();
				com.defaultDesc = base.Props.commandDescKey.Translate();
				com.isActive = (() => this.wantSwitchOn);
				com.toggleAction = delegate()
				{
					this.wantSwitchOn = !this.wantSwitchOn;
					FlickUtility.UpdateFlickDesignation(this.parent);
				};
				this.$current = com;
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
				IL_193:
				this.$PC = -1;
				return false;
			}

			Gizmo IEnumerator<Gizmo>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Gizmo>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				CompFlickable.<CompGetGizmosExtra>c__Iterator0 <CompGetGizmosExtra>c__Iterator = new CompFlickable.<CompGetGizmosExtra>c__Iterator0();
				<CompGetGizmosExtra>c__Iterator.$this = this;
				return <CompGetGizmosExtra>c__Iterator;
			}

			internal bool <>m__0()
			{
				return this.wantSwitchOn;
			}

			internal void <>m__1()
			{
				this.wantSwitchOn = !this.wantSwitchOn;
				FlickUtility.UpdateFlickDesignation(this.parent);
			}
		}
	}
}
