using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class CompProperties_Refuelable : CompProperties
	{
		public float fuelConsumptionRate = 1f;

		public float fuelCapacity = 2f;

		public float initialFuelPercent = 0f;

		public float autoRefuelPercent = 0.3f;

		public float fuelConsumptionPerTickInRain;

		public ThingFilter fuelFilter;

		public bool destroyOnNoFuel;

		public bool consumeFuelOnlyWhenUsed;

		public bool showFuelGizmo;

		public bool targetFuelLevelConfigurable;

		public float initialConfigurableTargetFuelLevel;

		public bool drawOutOfFuelOverlay = true;

		public float minimumFueledThreshold = 0f;

		public bool drawFuelGaugeInMap = false;

		public bool atomicFueling = false;

		public float fuelMultiplier = 1f;

		public string fuelLabel;

		public string fuelGizmoLabel;

		public string outOfFuelMessage;

		public string fuelIconPath;

		private Texture2D fuelIcon;

		public CompProperties_Refuelable()
		{
			this.compClass = typeof(CompRefuelable);
		}

		public string FuelLabel
		{
			get
			{
				return this.fuelLabel.NullOrEmpty() ? "Fuel".Translate() : this.fuelLabel;
			}
		}

		public string FuelGizmoLabel
		{
			get
			{
				return this.fuelGizmoLabel.NullOrEmpty() ? "Fuel".Translate() : this.fuelGizmoLabel;
			}
		}

		public Texture2D FuelIcon
		{
			get
			{
				if (this.fuelIcon == null)
				{
					if (!this.fuelIconPath.NullOrEmpty())
					{
						this.fuelIcon = ContentFinder<Texture2D>.Get(this.fuelIconPath, true);
					}
					else
					{
						ThingDef thingDef;
						if (this.fuelFilter.AnyAllowedDef != null)
						{
							thingDef = this.fuelFilter.AnyAllowedDef;
						}
						else
						{
							thingDef = ThingDefOf.Chemfuel;
						}
						this.fuelIcon = thingDef.uiIcon;
					}
				}
				return this.fuelIcon;
			}
		}

		public override void ResolveReferences(ThingDef parentDef)
		{
			base.ResolveReferences(parentDef);
			this.fuelFilter.ResolveReferences();
		}

		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string err in this.<ConfigErrors>__BaseCallProxy0(parentDef))
			{
				yield return err;
			}
			if (this.destroyOnNoFuel && this.initialFuelPercent <= 0f)
			{
				yield return "Refuelable component has destroyOnNoFuel, but initialFuelPercent <= 0";
			}
			if ((!this.consumeFuelOnlyWhenUsed || this.fuelConsumptionPerTickInRain > 0f) && parentDef.tickerType != TickerType.Normal)
			{
				yield return string.Format("Refuelable component set to consume fuel per tick, but parent tickertype is {0} instead of {1}", parentDef.tickerType, TickerType.Normal);
			}
			yield break;
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<string> <ConfigErrors>__BaseCallProxy0(ThingDef parentDef)
		{
			return base.ConfigErrors(parentDef);
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal ThingDef parentDef;

			internal IEnumerator<string> $locvar0;

			internal string <err>__1;

			internal CompProperties_Refuelable $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <ConfigErrors>c__Iterator0()
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
					enumerator = base.<ConfigErrors>__BaseCallProxy0(parentDef).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_106;
				case 3u:
					goto IL_176;
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
						err = enumerator.Current;
						this.$current = err;
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
				if (this.destroyOnNoFuel && this.initialFuelPercent <= 0f)
				{
					this.$current = "Refuelable component has destroyOnNoFuel, but initialFuelPercent <= 0";
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_106:
				if ((!this.consumeFuelOnlyWhenUsed || this.fuelConsumptionPerTickInRain > 0f) && parentDef.tickerType != TickerType.Normal)
				{
					this.$current = string.Format("Refuelable component set to consume fuel per tick, but parent tickertype is {0} instead of {1}", parentDef.tickerType, TickerType.Normal);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_176:
				this.$PC = -1;
				return false;
			}

			string IEnumerator<string>.Current
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
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				CompProperties_Refuelable.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new CompProperties_Refuelable.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				<ConfigErrors>c__Iterator.parentDef = parentDef;
				return <ConfigErrors>c__Iterator;
			}
		}
	}
}
