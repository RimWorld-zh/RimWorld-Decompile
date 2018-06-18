using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200072C RID: 1836
	public class CompProperties_Refuelable : CompProperties
	{
		// Token: 0x0600286A RID: 10346 RVA: 0x00158B30 File Offset: 0x00156F30
		public CompProperties_Refuelable()
		{
			this.compClass = typeof(CompRefuelable);
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x0600286B RID: 10347 RVA: 0x00158BAC File Offset: 0x00156FAC
		public string FuelLabel
		{
			get
			{
				return this.fuelLabel.NullOrEmpty() ? "Fuel".Translate() : this.fuelLabel;
			}
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x0600286C RID: 10348 RVA: 0x00158BE8 File Offset: 0x00156FE8
		public string FuelGizmoLabel
		{
			get
			{
				return this.fuelGizmoLabel.NullOrEmpty() ? "Fuel".Translate() : this.fuelGizmoLabel;
			}
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x0600286D RID: 10349 RVA: 0x00158C24 File Offset: 0x00157024
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

		// Token: 0x0600286E RID: 10350 RVA: 0x00158CAE File Offset: 0x001570AE
		public override void ResolveReferences(ThingDef parentDef)
		{
			base.ResolveReferences(parentDef);
			this.fuelFilter.ResolveReferences();
		}

		// Token: 0x0600286F RID: 10351 RVA: 0x00158CC4 File Offset: 0x001570C4
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

		// Token: 0x04001617 RID: 5655
		public float fuelConsumptionRate = 1f;

		// Token: 0x04001618 RID: 5656
		public float fuelCapacity = 2f;

		// Token: 0x04001619 RID: 5657
		public float initialFuelPercent = 0f;

		// Token: 0x0400161A RID: 5658
		public float autoRefuelPercent = 0.3f;

		// Token: 0x0400161B RID: 5659
		public float fuelConsumptionPerTickInRain;

		// Token: 0x0400161C RID: 5660
		public ThingFilter fuelFilter;

		// Token: 0x0400161D RID: 5661
		public bool destroyOnNoFuel;

		// Token: 0x0400161E RID: 5662
		public bool consumeFuelOnlyWhenUsed;

		// Token: 0x0400161F RID: 5663
		public bool showFuelGizmo;

		// Token: 0x04001620 RID: 5664
		public bool targetFuelLevelConfigurable;

		// Token: 0x04001621 RID: 5665
		public float initialConfigurableTargetFuelLevel;

		// Token: 0x04001622 RID: 5666
		public bool drawOutOfFuelOverlay = true;

		// Token: 0x04001623 RID: 5667
		public float minimumFueledThreshold = 0f;

		// Token: 0x04001624 RID: 5668
		public bool drawFuelGaugeInMap = false;

		// Token: 0x04001625 RID: 5669
		public bool atomicFueling = false;

		// Token: 0x04001626 RID: 5670
		public float fuelMultiplier = 1f;

		// Token: 0x04001627 RID: 5671
		public string fuelLabel;

		// Token: 0x04001628 RID: 5672
		public string fuelGizmoLabel;

		// Token: 0x04001629 RID: 5673
		public string outOfFuelMessage;

		// Token: 0x0400162A RID: 5674
		public string fuelIconPath;

		// Token: 0x0400162B RID: 5675
		private Texture2D fuelIcon;
	}
}
