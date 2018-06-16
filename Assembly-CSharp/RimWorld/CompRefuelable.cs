using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200072D RID: 1837
	[StaticConstructorOnStartup]
	public class CompRefuelable : ThingComp
	{
		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06002870 RID: 10352 RVA: 0x00158F60 File Offset: 0x00157360
		// (set) Token: 0x06002871 RID: 10353 RVA: 0x00158FBD File Offset: 0x001573BD
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

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x06002872 RID: 10354 RVA: 0x00158FDC File Offset: 0x001573DC
		public CompProperties_Refuelable Props
		{
			get
			{
				return (CompProperties_Refuelable)this.props;
			}
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x06002873 RID: 10355 RVA: 0x00158FFC File Offset: 0x001573FC
		public float Fuel
		{
			get
			{
				return this.fuel;
			}
		}

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x06002874 RID: 10356 RVA: 0x00159018 File Offset: 0x00157418
		public float FuelPercentOfTarget
		{
			get
			{
				return this.fuel / this.TargetFuelLevel;
			}
		}

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x06002875 RID: 10357 RVA: 0x0015903C File Offset: 0x0015743C
		public float FuelPercentOfMax
		{
			get
			{
				return this.fuel / this.Props.fuelCapacity;
			}
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x06002876 RID: 10358 RVA: 0x00159064 File Offset: 0x00157464
		public bool IsFull
		{
			get
			{
				return this.TargetFuelLevel - this.fuel < 1f;
			}
		}

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x06002877 RID: 10359 RVA: 0x00159090 File Offset: 0x00157490
		public bool HasFuel
		{
			get
			{
				return this.fuel > 0f && this.fuel >= this.Props.minimumFueledThreshold;
			}
		}

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x06002878 RID: 10360 RVA: 0x001590D0 File Offset: 0x001574D0
		private float ConsumptionRatePerTick
		{
			get
			{
				return this.Props.fuelConsumptionRate / 60000f;
			}
		}

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x06002879 RID: 10361 RVA: 0x001590F8 File Offset: 0x001574F8
		public bool ShouldAutoRefuelNow
		{
			get
			{
				return this.FuelPercentOfTarget <= this.Props.autoRefuelPercent && !this.IsFull && this.TargetFuelLevel > 0f && !this.parent.IsBurning() && (this.flickComp == null || this.flickComp.SwitchIsOn) && this.parent.Map.designationManager.DesignationOn(this.parent, DesignationDefOf.Flick) == null && this.parent.Map.designationManager.DesignationOn(this.parent, DesignationDefOf.Deconstruct) == null;
			}
		}

		// Token: 0x0600287A RID: 10362 RVA: 0x001591B4 File Offset: 0x001575B4
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.fuel = this.Props.fuelCapacity * this.Props.initialFuelPercent;
			this.flickComp = this.parent.GetComp<CompFlickable>();
		}

		// Token: 0x0600287B RID: 10363 RVA: 0x001591EC File Offset: 0x001575EC
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.fuel, "fuel", 0f, false);
			Scribe_Values.Look<float>(ref this.configuredTargetFuelLevel, "configuredTargetFuelLevel", -1f, false);
		}

		// Token: 0x0600287C RID: 10364 RVA: 0x00159224 File Offset: 0x00157624
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

		// Token: 0x0600287D RID: 10365 RVA: 0x00159314 File Offset: 0x00157714
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

		// Token: 0x0600287E RID: 10366 RVA: 0x001593D0 File Offset: 0x001577D0
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

		// Token: 0x0600287F RID: 10367 RVA: 0x00159518 File Offset: 0x00157918
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

		// Token: 0x06002880 RID: 10368 RVA: 0x001595E0 File Offset: 0x001579E0
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

		// Token: 0x06002881 RID: 10369 RVA: 0x0015965C File Offset: 0x00157A5C
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

		// Token: 0x06002882 RID: 10370 RVA: 0x0015970C File Offset: 0x00157B0C
		public void Refuel(float amount)
		{
			this.fuel += amount * this.Props.fuelMultiplier;
			if (this.fuel > this.Props.fuelCapacity)
			{
				this.fuel = this.Props.fuelCapacity;
			}
			this.parent.BroadcastCompSignal("Refueled");
		}

		// Token: 0x06002883 RID: 10371 RVA: 0x0015976B File Offset: 0x00157B6B
		public void Notify_UsedThisTick()
		{
			this.ConsumeFuel(this.ConsumptionRatePerTick);
		}

		// Token: 0x06002884 RID: 10372 RVA: 0x0015977C File Offset: 0x00157B7C
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

		// Token: 0x06002885 RID: 10373 RVA: 0x001597EC File Offset: 0x00157BEC
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

		// Token: 0x0400162C RID: 5676
		private float fuel;

		// Token: 0x0400162D RID: 5677
		private float configuredTargetFuelLevel = -1f;

		// Token: 0x0400162E RID: 5678
		private CompFlickable flickComp;

		// Token: 0x0400162F RID: 5679
		public const string RefueledSignal = "Refueled";

		// Token: 0x04001630 RID: 5680
		public const string RanOutOfFuelSignal = "RanOutOfFuel";

		// Token: 0x04001631 RID: 5681
		private static readonly Texture2D SetTargetFuelLevelCommand = ContentFinder<Texture2D>.Get("UI/Commands/SetTargetFuelLevel", true);

		// Token: 0x04001632 RID: 5682
		private static readonly Vector2 FuelBarSize = new Vector2(1f, 0.2f);

		// Token: 0x04001633 RID: 5683
		private static readonly Material FuelBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.6f, 0.56f, 0.13f), false);

		// Token: 0x04001634 RID: 5684
		private static readonly Material FuelBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f), false);
	}
}
