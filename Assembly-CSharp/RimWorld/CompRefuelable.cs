using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class CompRefuelable : ThingComp
	{
		private float fuel;

		private float configuredTargetFuelLevel = -1f;

		private CompFlickable flickComp;

		public const string RefueledSignal = "Refueled";

		public const string RanOutOfFuelSignal = "RanOutOfFuel";

		private static readonly Texture2D SetTargetFuelLevelCommand = ContentFinder<Texture2D>.Get("UI/Commands/SetTargetFuelLevel", true);

		private static readonly Vector2 FuelBarSize = new Vector2(1f, 0.2f);

		private static readonly Material FuelBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.6f, 0.56f, 0.13f), false);

		private static readonly Material FuelBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f), false);

		[CompilerGenerated]
		private static Func<Thing, int> <>f__am$cache0;

		public CompRefuelable()
		{
		}

		public float TargetFuelLevel
		{
			get
			{
				float result;
				if (this.configuredTargetFuelLevel >= 0f)
				{
					result = this.configuredTargetFuelLevel;
				}
				else if (this.Props.targetFuelLevelConfigurable)
				{
					result = this.Props.initialConfigurableTargetFuelLevel;
				}
				else
				{
					result = this.Props.fuelCapacity;
				}
				return result;
			}
			set
			{
				this.configuredTargetFuelLevel = Mathf.Clamp(value, 0f, this.Props.fuelCapacity);
			}
		}

		public CompProperties_Refuelable Props
		{
			get
			{
				return (CompProperties_Refuelable)this.props;
			}
		}

		public float Fuel
		{
			get
			{
				return this.fuel;
			}
		}

		public float FuelPercentOfTarget
		{
			get
			{
				return this.fuel / this.TargetFuelLevel;
			}
		}

		public float FuelPercentOfMax
		{
			get
			{
				return this.fuel / this.Props.fuelCapacity;
			}
		}

		public bool IsFull
		{
			get
			{
				return this.TargetFuelLevel - this.fuel < 1f;
			}
		}

		public bool HasFuel
		{
			get
			{
				return this.fuel > 0f && this.fuel >= this.Props.minimumFueledThreshold;
			}
		}

		private float ConsumptionRatePerTick
		{
			get
			{
				return this.Props.fuelConsumptionRate / 60000f;
			}
		}

		public bool ShouldAutoRefuelNow
		{
			get
			{
				return this.FuelPercentOfTarget <= this.Props.autoRefuelPercent && !this.IsFull && this.TargetFuelLevel > 0f && this.ShouldAutoRefuelNowIgnoringFuelPct;
			}
		}

		public bool ShouldAutoRefuelNowIgnoringFuelPct
		{
			get
			{
				return !this.parent.IsBurning() && (this.flickComp == null || this.flickComp.SwitchIsOn) && this.parent.Map.designationManager.DesignationOn(this.parent, DesignationDefOf.Flick) == null && this.parent.Map.designationManager.DesignationOn(this.parent, DesignationDefOf.Deconstruct) == null;
			}
		}

		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.fuel = this.Props.fuelCapacity * this.Props.initialFuelPercent;
			this.flickComp = this.parent.GetComp<CompFlickable>();
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.fuel, "fuel", 0f, false);
			Scribe_Values.Look<float>(ref this.configuredTargetFuelLevel, "configuredTargetFuelLevel", -1f, false);
		}

		public override void PostDraw()
		{
			base.PostDraw();
			if (!this.HasFuel && this.Props.drawOutOfFuelOverlay)
			{
				this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.OutOfFuel);
			}
			if (this.Props.drawFuelGaugeInMap)
			{
				GenDraw.FillableBarRequest r = default(GenDraw.FillableBarRequest);
				r.center = this.parent.DrawPos + Vector3.up * 0.1f;
				r.size = CompRefuelable.FuelBarSize;
				r.fillPercent = this.FuelPercentOfMax;
				r.filledMat = CompRefuelable.FuelBarFilledMat;
				r.unfilledMat = CompRefuelable.FuelBarUnfilledMat;
				r.margin = 0.15f;
				Rot4 rotation = this.parent.Rotation;
				rotation.Rotate(RotationDirection.Clockwise);
				r.rotation = rotation;
				GenDraw.DrawFillableBar(r);
			}
		}

		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (previousMap != null && this.Props.fuelFilter.AllowedDefCount == 1 && this.Props.initialFuelPercent == 0f)
			{
				ThingDef thingDef = this.Props.fuelFilter.AllowedThingDefs.First<ThingDef>();
				float num = 1f;
				int i = GenMath.RoundRandom(num * this.fuel);
				while (i > 0)
				{
					Thing thing = ThingMaker.MakeThing(thingDef, null);
					thing.stackCount = Mathf.Min(i, thingDef.stackLimit);
					i -= thing.stackCount;
					GenPlace.TryPlaceThing(thing, this.parent.Position, previousMap, ThingPlaceMode.Near, null, null);
				}
			}
		}

		public override string CompInspectStringExtra()
		{
			string text = string.Concat(new string[]
			{
				this.Props.FuelLabel,
				": ",
				this.fuel.ToStringDecimalIfSmall(),
				" / ",
				this.Props.fuelCapacity.ToStringDecimalIfSmall()
			});
			if (!this.Props.consumeFuelOnlyWhenUsed && this.HasFuel)
			{
				int numTicks = (int)(this.fuel / this.Props.fuelConsumptionRate * 60000f);
				text = text + " (" + numTicks.ToStringTicksToPeriod() + ")";
			}
			if (!this.HasFuel && !this.Props.outOfFuelMessage.NullOrEmpty())
			{
				text += string.Format("\n{0} ({1}x {2})", this.Props.outOfFuelMessage, this.GetFuelCountToFullyRefuel(), this.Props.fuelFilter.AnyAllowedDef.label);
			}
			if (this.Props.targetFuelLevelConfigurable)
			{
				text = text + "\n" + "ConfiguredTargetFuelLevel".Translate(new object[]
				{
					this.TargetFuelLevel.ToStringDecimalIfSmall()
				});
			}
			return text;
		}

		public override void CompTick()
		{
			base.CompTick();
			if (!this.Props.consumeFuelOnlyWhenUsed && (this.flickComp == null || this.flickComp.SwitchIsOn))
			{
				this.ConsumeFuel(this.ConsumptionRatePerTick);
			}
			if (this.Props.fuelConsumptionPerTickInRain > 0f && this.parent.Spawned && this.parent.Map.weatherManager.RainRate > 0.4f && !this.parent.Map.roofGrid.Roofed(this.parent.Position))
			{
				this.ConsumeFuel(this.Props.fuelConsumptionPerTickInRain);
			}
		}

		public void ConsumeFuel(float amount)
		{
			if (this.fuel > 0f)
			{
				this.fuel -= amount;
				if (this.fuel <= 0f)
				{
					this.fuel = 0f;
					if (this.Props.destroyOnNoFuel)
					{
						this.parent.Destroy(DestroyMode.Vanish);
					}
					this.parent.BroadcastCompSignal("RanOutOfFuel");
				}
			}
		}

		public void Refuel(List<Thing> fuelThings)
		{
			if (this.Props.atomicFueling)
			{
				if (fuelThings.Sum((Thing t) => t.stackCount) < this.GetFuelCountToFullyRefuel())
				{
					Log.ErrorOnce("Error refueling; not enough fuel available for proper atomic refuel", 19586442, false);
					return;
				}
			}
			int num = this.GetFuelCountToFullyRefuel();
			while (num > 0 && fuelThings.Count > 0)
			{
				Thing thing = fuelThings.Pop<Thing>();
				int num2 = Mathf.Min(num, thing.stackCount);
				this.Refuel((float)num2);
				thing.SplitOff(num2).Destroy(DestroyMode.Vanish);
				num -= num2;
			}
		}

		public void Refuel(float amount)
		{
			this.fuel += amount * this.Props.fuelMultiplier;
			if (this.fuel > this.Props.fuelCapacity)
			{
				this.fuel = this.Props.fuelCapacity;
			}
			this.parent.BroadcastCompSignal("Refueled");
		}

		public void Notify_UsedThisTick()
		{
			this.ConsumeFuel(this.ConsumptionRatePerTick);
		}

		public int GetFuelCountToFullyRefuel()
		{
			int result;
			if (this.Props.atomicFueling)
			{
				result = Mathf.CeilToInt(this.Props.fuelCapacity / this.Props.fuelMultiplier);
			}
			else
			{
				float f = (this.TargetFuelLevel - this.fuel) / this.Props.fuelMultiplier;
				result = Mathf.Max(Mathf.CeilToInt(f), 1);
			}
			return result;
		}

		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.Props.targetFuelLevelConfigurable)
			{
				yield return new Command_SetTargetFuelLevel
				{
					refuelable = this,
					defaultLabel = "CommandSetTargetFuelLevel".Translate(),
					defaultDesc = "CommandSetTargetFuelLevelDesc".Translate(),
					icon = CompRefuelable.SetTargetFuelLevelCommand
				};
			}
			if (this.Props.showFuelGizmo && Find.Selector.SingleSelectedThing == this.parent)
			{
				yield return new Gizmo_RefuelableFuelStatus
				{
					refuelable = this
				};
			}
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "Debug: Set fuel to 0",
					action = delegate()
					{
						this.fuel = 0f;
						this.parent.BroadcastCompSignal("Refueled");
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "Debug: Set fuel to 0.1",
					action = delegate()
					{
						this.fuel = 0.1f;
						this.parent.BroadcastCompSignal("Refueled");
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "Debug: Set fuel to max",
					action = delegate()
					{
						this.fuel = this.Props.fuelCapacity;
						this.parent.BroadcastCompSignal("Refueled");
					}
				};
			}
			yield break;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static CompRefuelable()
		{
		}

		[CompilerGenerated]
		private static int <Refuel>m__0(Thing t)
		{
			return t.stackCount;
		}

		[CompilerGenerated]
		private sealed class <CompGetGizmosExtra>c__Iterator0 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal Command_SetTargetFuelLevel <setTargetFuelLevel>__1;

			internal Gizmo_RefuelableFuelStatus <status>__2;

			internal Command_Action <zerofuel>__3;

			internal Command_Action <defuel>__3;

			internal Command_Action <setFuelToMax>__3;

			internal CompRefuelable $this;

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
				switch (num)
				{
				case 0u:
					if (base.Props.targetFuelLevelConfigurable)
					{
						Command_SetTargetFuelLevel setTargetFuelLevel = new Command_SetTargetFuelLevel();
						setTargetFuelLevel.refuelable = this;
						setTargetFuelLevel.defaultLabel = "CommandSetTargetFuelLevel".Translate();
						setTargetFuelLevel.defaultDesc = "CommandSetTargetFuelLevelDesc".Translate();
						setTargetFuelLevel.icon = CompRefuelable.SetTargetFuelLevelCommand;
						this.$current = setTargetFuelLevel;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1u:
					break;
				case 2u:
					goto IL_12C;
				case 3u:
				{
					Command_Action defuel = new Command_Action();
					defuel.defaultLabel = "Debug: Set fuel to 0.1";
					defuel.action = delegate()
					{
						this.fuel = 0.1f;
						this.parent.BroadcastCompSignal("Refueled");
					};
					this.$current = defuel;
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				case 4u:
				{
					Command_Action setFuelToMax = new Command_Action();
					setFuelToMax.defaultLabel = "Debug: Set fuel to max";
					setFuelToMax.action = delegate()
					{
						this.fuel = base.Props.fuelCapacity;
						this.parent.BroadcastCompSignal("Refueled");
					};
					this.$current = setFuelToMax;
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				}
				case 5u:
					goto IL_22E;
				default:
					return false;
				}
				if (base.Props.showFuelGizmo && Find.Selector.SingleSelectedThing == this.parent)
				{
					Gizmo_RefuelableFuelStatus status = new Gizmo_RefuelableFuelStatus();
					status.refuelable = this;
					this.$current = status;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_12C:
				if (Prefs.DevMode)
				{
					Command_Action zerofuel = new Command_Action();
					zerofuel.defaultLabel = "Debug: Set fuel to 0";
					zerofuel.action = delegate()
					{
						this.fuel = 0f;
						this.parent.BroadcastCompSignal("Refueled");
					};
					this.$current = zerofuel;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_22E:
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
				this.$disposing = true;
				this.$PC = -1;
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
				CompRefuelable.<CompGetGizmosExtra>c__Iterator0 <CompGetGizmosExtra>c__Iterator = new CompRefuelable.<CompGetGizmosExtra>c__Iterator0();
				<CompGetGizmosExtra>c__Iterator.$this = this;
				return <CompGetGizmosExtra>c__Iterator;
			}

			internal void <>m__0()
			{
				this.fuel = 0f;
				this.parent.BroadcastCompSignal("Refueled");
			}

			internal void <>m__1()
			{
				this.fuel = 0.1f;
				this.parent.BroadcastCompSignal("Refueled");
			}

			internal void <>m__2()
			{
				this.fuel = base.Props.fuelCapacity;
				this.parent.BroadcastCompSignal("Refueled");
			}
		}
	}
}
