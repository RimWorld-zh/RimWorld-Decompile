using System.Collections.Generic;
using System.Linq;
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

		public float TargetFuelLevel
		{
			get
			{
				if (this.configuredTargetFuelLevel >= 0.0)
				{
					return this.configuredTargetFuelLevel;
				}
				if (this.Props.targetFuelLevelConfigurable)
				{
					return this.Props.initialConfigurableTargetFuelLevel;
				}
				return this.Props.fuelCapacity;
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
				return (CompProperties_Refuelable)base.props;
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
				return this.TargetFuelLevel - this.fuel < 1.0;
			}
		}

		public bool HasFuel
		{
			get
			{
				return this.fuel > 0.0;
			}
		}

		private float ConsumptionRatePerTick
		{
			get
			{
				return (float)(this.Props.fuelConsumptionRate / 60000.0);
			}
		}

		public bool ShouldAutoRefuelNow
		{
			get
			{
				return this.FuelPercentOfTarget <= this.Props.autoRefuelPercent && !this.IsFull && this.TargetFuelLevel > 0.0 && !base.parent.IsBurning() && (this.flickComp == null || this.flickComp.SwitchIsOn) && base.parent.Map.designationManager.DesignationOn(base.parent, DesignationDefOf.Flick) == null && base.parent.Map.designationManager.DesignationOn(base.parent, DesignationDefOf.Deconstruct) == null;
			}
		}

		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			if (this.Props.destroyOnNoFuel)
			{
				this.fuel = this.Props.fuelCapacity;
			}
			this.flickComp = base.parent.GetComp<CompFlickable>();
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
				base.parent.Map.overlayDrawer.DrawOverlay(base.parent, OverlayTypes.OutOfFuel);
			}
			if (this.Props.drawFuelGaugeInMap)
			{
				GenDraw.FillableBarRequest r = default(GenDraw.FillableBarRequest);
				r.center = base.parent.DrawPos + Vector3.up * 0.1f;
				r.size = CompRefuelable.FuelBarSize;
				r.fillPercent = this.FuelPercentOfMax;
				r.filledMat = CompRefuelable.FuelBarFilledMat;
				r.unfilledMat = CompRefuelable.FuelBarUnfilledMat;
				r.margin = 0.15f;
				Rot4 rotation = base.parent.Rotation;
				rotation.Rotate(RotationDirection.Clockwise);
				r.rotation = rotation;
				GenDraw.DrawFillableBar(r);
			}
		}

		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (previousMap != null && this.Props.fuelFilter.AllowedDefCount == 1)
			{
				ThingDef thingDef = this.Props.fuelFilter.AllowedThingDefs.First();
				float num = 1f;
				int num2 = GenMath.RoundRandom(num * this.fuel);
				while (num2 > 0)
				{
					Thing thing = ThingMaker.MakeThing(thingDef, null);
					thing.stackCount = Mathf.Min(num2, thingDef.stackLimit);
					num2 -= thing.stackCount;
					GenPlace.TryPlaceThing(thing, base.parent.Position, previousMap, ThingPlaceMode.Near, null);
				}
			}
		}

		public override string CompInspectStringExtra()
		{
			string text = "Fuel".Translate() + ": " + this.fuel.ToStringDecimalIfSmall() + " / " + this.Props.fuelCapacity.ToStringDecimalIfSmall();
			if (!this.Props.consumeFuelOnlyWhenUsed && this.HasFuel)
			{
				int numTicks = (int)(this.fuel / this.Props.fuelConsumptionRate * 60000.0);
				text = text + " (" + numTicks.ToStringTicksToPeriod(true, false, true) + ")";
			}
			if (this.Props.targetFuelLevelConfigurable)
			{
				text = text + "\n" + "ConfiguredTargetFuelLevel".Translate(this.TargetFuelLevel.ToStringDecimalIfSmall());
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
			if (this.Props.fuelConsumptionPerTickInRain > 0.0 && base.parent.Spawned && base.parent.Map.weatherManager.RainRate > 0.40000000596046448 && !base.parent.Map.roofGrid.Roofed(base.parent.Position))
			{
				this.ConsumeFuel(this.Props.fuelConsumptionPerTickInRain);
			}
		}

		public void ConsumeFuel(float amount)
		{
			if (!(this.fuel <= 0.0))
			{
				this.fuel -= amount;
				if (this.fuel <= 0.0)
				{
					this.fuel = 0f;
					if (this.Props.destroyOnNoFuel)
					{
						base.parent.Destroy(DestroyMode.Vanish);
					}
					base.parent.BroadcastCompSignal("RanOutOfFuel");
				}
			}
		}

		public void Refuel(Thing fuelThing)
		{
			int num = Mathf.Min(fuelThing.stackCount, Mathf.CeilToInt(this.Props.fuelCapacity - this.fuel));
			if (num > 0)
			{
				this.Refuel((float)num);
				fuelThing.SplitOff(num).Destroy(DestroyMode.Vanish);
			}
		}

		public void Refuel(float amount)
		{
			this.fuel += amount;
			if (this.fuel > this.Props.fuelCapacity)
			{
				this.fuel = this.Props.fuelCapacity;
			}
			base.parent.BroadcastCompSignal("Refueled");
		}

		public void Notify_UsedThisTick()
		{
			this.ConsumeFuel(this.ConsumptionRatePerTick);
		}

		public int GetFuelCountToFullyRefuel()
		{
			float f = this.TargetFuelLevel - this.fuel;
			return Mathf.Max(Mathf.CeilToInt(f), 1);
		}

		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.Props.targetFuelLevelConfigurable)
			{
				yield return (Gizmo)new Command_SetTargetFuelLevel
				{
					refuelable = this,
					defaultLabel = "CommandSetTargetFuelLevel".Translate(),
					defaultDesc = "CommandSetTargetFuelLevelDesc".Translate(),
					icon = CompRefuelable.SetTargetFuelLevelCommand
				};
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.Props.showFuelGizmo && Find.Selector.SingleSelectedThing == base.parent)
			{
				yield return (Gizmo)new Gizmo_RefuelableFuelStatus
				{
					refuelable = this
				};
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (!Prefs.DevMode)
				yield break;
			yield return (Gizmo)new Command_Action
			{
				defaultLabel = "Debug: Set fuel to 0.1",
				action = delegate
				{
					((_003CCompGetGizmosExtra_003Ec__Iterator0)/*Error near IL_014e: stateMachine*/)._0024this.fuel = 0.1f;
					((_003CCompGetGizmosExtra_003Ec__Iterator0)/*Error near IL_014e: stateMachine*/)._0024this.parent.BroadcastCompSignal("Refueled");
				}
			};
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
