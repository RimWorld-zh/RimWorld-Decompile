using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public class CompTempControl : ThingComp
	{
		[Unsaved]
		public bool operatingAtHighPower;

		public float targetTemperature = -99999f;

		private const float DefaultTargetTemperature = 21f;

		public CompTempControl()
		{
		}

		public CompProperties_TempControl Props
		{
			get
			{
				return (CompProperties_TempControl)this.props;
			}
		}

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (this.targetTemperature < -2000f)
			{
				this.targetTemperature = this.Props.defaultTargetTemperature;
			}
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.targetTemperature, "targetTemperature", 0f, false);
		}

		private float RoundedToCurrentTempModeOffset(float celsiusTemp)
		{
			float num = GenTemperature.CelsiusToOffset(celsiusTemp, Prefs.TemperatureMode);
			num = (float)Mathf.RoundToInt(num);
			return GenTemperature.ConvertTemperatureOffset(num, Prefs.TemperatureMode, TemperatureDisplayMode.Celsius);
		}

		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo c in base.CompGetGizmosExtra())
			{
				yield return c;
			}
			float offset2 = this.RoundedToCurrentTempModeOffset(-10f);
			yield return new Command_Action
			{
				action = delegate()
				{
					this.InterfaceChangeTargetTemperature(offset2);
				},
				defaultLabel = offset2.ToStringTemperatureOffset("F0"),
				defaultDesc = "CommandLowerTempDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc5,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempLower", true)
			};
			float offset3 = this.RoundedToCurrentTempModeOffset(-1f);
			yield return new Command_Action
			{
				action = delegate()
				{
					this.InterfaceChangeTargetTemperature(offset3);
				},
				defaultLabel = offset3.ToStringTemperatureOffset("F0"),
				defaultDesc = "CommandLowerTempDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc4,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempLower", true)
			};
			yield return new Command_Action
			{
				action = delegate()
				{
					this.targetTemperature = 21f;
					SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
					this.ThrowCurrentTemperatureText();
				},
				defaultLabel = "CommandResetTemp".Translate(),
				defaultDesc = "CommandResetTempDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc1,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempReset", true)
			};
			float offset4 = this.RoundedToCurrentTempModeOffset(1f);
			yield return new Command_Action
			{
				action = delegate()
				{
					this.InterfaceChangeTargetTemperature(offset4);
				},
				defaultLabel = "+" + offset4.ToStringTemperatureOffset("F0"),
				defaultDesc = "CommandRaiseTempDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc2,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempRaise", true)
			};
			float offset = this.RoundedToCurrentTempModeOffset(10f);
			yield return new Command_Action
			{
				action = delegate()
				{
					this.InterfaceChangeTargetTemperature(offset);
				},
				defaultLabel = "+" + offset.ToStringTemperatureOffset("F0"),
				defaultDesc = "CommandRaiseTempDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc3,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempRaise", true)
			};
			yield break;
		}

		private void InterfaceChangeTargetTemperature(float offset)
		{
			if (offset > 0f)
			{
				SoundDefOf.AmountIncrement.PlayOneShotOnCamera(null);
			}
			else
			{
				SoundDefOf.AmountDecrement.PlayOneShotOnCamera(null);
			}
			this.targetTemperature += offset;
			this.targetTemperature = Mathf.Clamp(this.targetTemperature, -273.15f, 2000f);
			this.ThrowCurrentTemperatureText();
		}

		private void ThrowCurrentTemperatureText()
		{
			MoteMaker.ThrowText(this.parent.TrueCenter() + new Vector3(0.5f, 0f, 0.5f), this.parent.Map, this.targetTemperature.ToStringTemperature("F0"), Color.white, -1f);
		}

		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("TargetTemperature".Translate() + ": ");
			stringBuilder.AppendLine(this.targetTemperature.ToStringTemperature("F0"));
			stringBuilder.Append("PowerConsumptionMode".Translate() + ": ");
			if (this.operatingAtHighPower)
			{
				stringBuilder.Append("PowerConsumptionHigh".Translate());
			}
			else
			{
				stringBuilder.Append("PowerConsumptionLow".Translate());
			}
			return stringBuilder.ToString();
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

			internal Command_Action <minusTen>__2;

			internal Command_Action <minusOne>__3;

			internal Command_Action <reset>__4;

			internal Command_Action <plusOne>__5;

			internal Command_Action <plusTen>__6;

			internal CompTempControl $this;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

			private CompTempControl.<CompGetGizmosExtra>c__Iterator0.<CompGetGizmosExtra>c__AnonStorey1 $locvar1;

			private CompTempControl.<CompGetGizmosExtra>c__Iterator0.<CompGetGizmosExtra>c__AnonStorey2 $locvar2;

			private CompTempControl.<CompGetGizmosExtra>c__Iterator0.<CompGetGizmosExtra>c__AnonStorey3 $locvar3;

			private CompTempControl.<CompGetGizmosExtra>c__Iterator0.<CompGetGizmosExtra>c__AnonStorey4 $locvar4;

			[DebuggerHidden]
			public <CompGetGizmosExtra>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				float offset;
				switch (num)
				{
				case 0u:
					enumerator = base.<CompGetGizmosExtra>__BaseCallProxy0().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
				{
					float offset = base.RoundedToCurrentTempModeOffset(-1f);
					Command_Action minusOne = new Command_Action();
					minusOne.action = delegate()
					{
						this.InterfaceChangeTargetTemperature(offset);
					};
					minusOne.defaultLabel = offset.ToStringTemperatureOffset("F0");
					minusOne.defaultDesc = "CommandLowerTempDesc".Translate();
					minusOne.hotKey = KeyBindingDefOf.Misc4;
					minusOne.icon = ContentFinder<Texture2D>.Get("UI/Commands/TempLower", true);
					this.$current = minusOne;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				case 3u:
				{
					Command_Action reset = new Command_Action();
					reset.action = delegate()
					{
						this.targetTemperature = 21f;
						SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
						base.ThrowCurrentTemperatureText();
					};
					reset.defaultLabel = "CommandResetTemp".Translate();
					reset.defaultDesc = "CommandResetTempDesc".Translate();
					reset.hotKey = KeyBindingDefOf.Misc1;
					reset.icon = ContentFinder<Texture2D>.Get("UI/Commands/TempReset", true);
					this.$current = reset;
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				case 4u:
				{
					float offset = base.RoundedToCurrentTempModeOffset(1f);
					Command_Action plusOne = new Command_Action();
					plusOne.action = delegate()
					{
						this.InterfaceChangeTargetTemperature(offset);
					};
					plusOne.defaultLabel = "+" + offset.ToStringTemperatureOffset("F0");
					plusOne.defaultDesc = "CommandRaiseTempDesc".Translate();
					plusOne.hotKey = KeyBindingDefOf.Misc2;
					plusOne.icon = ContentFinder<Texture2D>.Get("UI/Commands/TempRaise", true);
					this.$current = plusOne;
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				}
				case 5u:
				{
					float offset = base.RoundedToCurrentTempModeOffset(10f);
					Command_Action plusTen = new Command_Action();
					plusTen.action = delegate()
					{
						this.InterfaceChangeTargetTemperature(offset);
					};
					plusTen.defaultLabel = "+" + offset.ToStringTemperatureOffset("F0");
					plusTen.defaultDesc = "CommandRaiseTempDesc".Translate();
					plusTen.hotKey = KeyBindingDefOf.Misc3;
					plusTen.icon = ContentFinder<Texture2D>.Get("UI/Commands/TempRaise", true);
					this.$current = plusTen;
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				}
				case 6u:
					this.$PC = -1;
					return false;
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
				offset = base.RoundedToCurrentTempModeOffset(-10f);
				Command_Action minusTen = new Command_Action();
				minusTen.action = delegate()
				{
					this.InterfaceChangeTargetTemperature(offset);
				};
				minusTen.defaultLabel = offset.ToStringTemperatureOffset("F0");
				minusTen.defaultDesc = "CommandLowerTempDesc".Translate();
				minusTen.hotKey = KeyBindingDefOf.Misc5;
				minusTen.icon = ContentFinder<Texture2D>.Get("UI/Commands/TempLower", true);
				this.$current = minusTen;
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
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
				CompTempControl.<CompGetGizmosExtra>c__Iterator0 <CompGetGizmosExtra>c__Iterator = new CompTempControl.<CompGetGizmosExtra>c__Iterator0();
				<CompGetGizmosExtra>c__Iterator.$this = this;
				return <CompGetGizmosExtra>c__Iterator;
			}

			internal void <>m__0()
			{
				this.targetTemperature = 21f;
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
				base.ThrowCurrentTemperatureText();
			}

			private sealed class <CompGetGizmosExtra>c__AnonStorey1
			{
				internal float offset;

				internal CompTempControl.<CompGetGizmosExtra>c__Iterator0 <>f__ref$0;

				public <CompGetGizmosExtra>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					this.<>f__ref$0.$this.InterfaceChangeTargetTemperature(this.offset);
				}
			}

			private sealed class <CompGetGizmosExtra>c__AnonStorey2
			{
				internal float offset;

				internal CompTempControl.<CompGetGizmosExtra>c__Iterator0 <>f__ref$0;

				public <CompGetGizmosExtra>c__AnonStorey2()
				{
				}

				internal void <>m__0()
				{
					this.<>f__ref$0.$this.InterfaceChangeTargetTemperature(this.offset);
				}
			}

			private sealed class <CompGetGizmosExtra>c__AnonStorey3
			{
				internal float offset;

				internal CompTempControl.<CompGetGizmosExtra>c__Iterator0 <>f__ref$0;

				public <CompGetGizmosExtra>c__AnonStorey3()
				{
				}

				internal void <>m__0()
				{
					this.<>f__ref$0.$this.InterfaceChangeTargetTemperature(this.offset);
				}
			}

			private sealed class <CompGetGizmosExtra>c__AnonStorey4
			{
				internal float offset;

				internal CompTempControl.<CompGetGizmosExtra>c__Iterator0 <>f__ref$0;

				public <CompGetGizmosExtra>c__AnonStorey4()
				{
				}

				internal void <>m__0()
				{
					this.<>f__ref$0.$this.InterfaceChangeTargetTemperature(this.offset);
				}
			}
		}
	}
}
