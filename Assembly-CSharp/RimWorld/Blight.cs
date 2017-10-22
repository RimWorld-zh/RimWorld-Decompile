using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Blight : Thing
	{
		private float severity = 0.1f;

		private int lastPlantHarmTick;

		private float lastMapMeshUpdateSeverity;

		private const float InitialSeverity = 0.1f;

		private const float SeverityPerDay = 1f;

		private const int DamagePerDay = 5;

		private const float MinSeverityToReproduce = 0.3f;

		private const float ReproduceMTBHoursAtMinSeverity = 48f;

		private const float ReproduceMTBHoursAtMaxSeverity = 6f;

		private const float ReproductionRadius = 4f;

		private static FloatRange SizeRange = new FloatRange(0.6f, 1f);

		private static Color32[] workingColors = new Color32[4];

		public float Severity
		{
			get
			{
				return this.severity;
			}
			set
			{
				this.severity = Mathf.Clamp01(value);
			}
		}

		public Plant Plant
		{
			get
			{
				return base.Spawned ? BlightUtility.GetFirstBlightableEverPlant(base.Position, base.Map) : null;
			}
		}

		protected float ReproduceMTBHours
		{
			get
			{
				return (float)((!(this.severity < 0.30000001192092896)) ? GenMath.LerpDouble(0.3f, 1f, 48f, 6f, this.severity) : -1.0);
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.severity, "severity", 0f, false);
			Scribe_Values.Look<int>(ref this.lastPlantHarmTick, "lastPlantHarmTick", 0, false);
		}

		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.lastPlantHarmTick = Find.TickManager.TicksGame;
			}
			this.lastMapMeshUpdateSeverity = this.Severity;
		}

		public override void TickLong()
		{
			this.CheckHarmPlant();
			if (!this.DestroyIfNoPlantHere())
			{
				this.Severity += 0.0333333351f;
				float reproduceMTBHours = this.ReproduceMTBHours;
				if (reproduceMTBHours > 0.0 && Rand.MTBEventOccurs(reproduceMTBHours, 2500f, 2000f))
				{
					this.TryReproduce();
				}
				if (Mathf.Abs(this.Severity - this.lastMapMeshUpdateSeverity) >= 0.05000000074505806)
				{
					base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
					this.lastMapMeshUpdateSeverity = this.Severity;
				}
			}
		}

		public void Notify_PlantDeSpawned()
		{
			this.DestroyIfNoPlantHere();
		}

		private bool DestroyIfNoPlantHere()
		{
			bool result;
			if (base.Destroyed)
			{
				result = true;
			}
			else if (this.Plant == null)
			{
				this.Destroy(DestroyMode.Vanish);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		private void CheckHarmPlant()
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (ticksGame - this.lastPlantHarmTick >= 60000)
			{
				List<Thing> thingList = base.Position.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Plant plant = thingList[i] as Plant;
					if (plant != null)
					{
						this.HarmPlant(plant);
					}
				}
				this.lastPlantHarmTick = ticksGame;
			}
		}

		private void HarmPlant(Plant plant)
		{
			bool isCrop = plant.IsCrop;
			IntVec3 position = base.Position;
			Map map = base.Map;
			plant.TakeDamage(new DamageInfo(DamageDefOf.Rotting, 5, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
			if (plant.Destroyed && isCrop && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfBlight-" + plant.def.defName, 240f))
			{
				Messages.Message("MessagePlantDiedOfBlight".Translate(plant.Label).CapitalizeFirst(), new TargetInfo(position, map, false), MessageTypeDefOf.NegativeEvent);
			}
		}

		private void TryReproduce()
		{
			GenRadial.ProcessEquidistantCells(base.Position, 4f, (Func<List<IntVec3>, bool>)delegate(List<IntVec3> cells)
			{
				IntVec3 c = default(IntVec3);
				bool result;
				if ((from x in cells
				where BlightUtility.GetFirstBlightableNowPlant(x, base.Map) != null
				select x).TryRandomElement<IntVec3>(out c))
				{
					BlightUtility.GetFirstBlightableNowPlant(c, base.Map).CropBlighted();
					result = true;
				}
				else
				{
					result = false;
				}
				return result;
			}, base.Map);
		}

		public override void Print(SectionLayer layer)
		{
			Plant plant = this.Plant;
			if (plant != null)
			{
				GenPlant.SetWindExposureColors(Blight.workingColors, plant);
			}
			else
			{
				Blight.workingColors[0].a = (Blight.workingColors[1].a = (Blight.workingColors[2].a = (Blight.workingColors[3].a = (byte)0)));
			}
			float num = Blight.SizeRange.LerpThroughRange(this.severity);
			if (plant != null)
			{
				float a = plant.Graphic.drawSize.x * plant.def.plant.visualSizeRange.LerpThroughRange(plant.Growth);
				num *= Mathf.Min(a, 1f);
			}
			num = Mathf.Clamp(num, 0.5f, 0.9f);
			Vector3 center = this.TrueCenter();
			Vector2 size = base.def.graphic.drawSize * num;
			Material mat = this.Graphic.MatAt(base.Rotation, this);
			Color32[] colors = Blight.workingColors;
			Printer_Plane.PrintPlane(layer, center, size, mat, 0f, false, null, colors, 0.1f);
		}
	}
}
