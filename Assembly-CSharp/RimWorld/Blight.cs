using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006B5 RID: 1717
	public class Blight : Thing
	{
		// Token: 0x04001462 RID: 5218
		private float severity = 0.2f;

		// Token: 0x04001463 RID: 5219
		private int lastPlantHarmTick;

		// Token: 0x04001464 RID: 5220
		private float lastMapMeshUpdateSeverity;

		// Token: 0x04001465 RID: 5221
		private const float InitialSeverity = 0.2f;

		// Token: 0x04001466 RID: 5222
		private const float SeverityPerDay = 1f;

		// Token: 0x04001467 RID: 5223
		private const int DamagePerDay = 5;

		// Token: 0x04001468 RID: 5224
		private const float MinSeverityToReproduce = 0.28f;

		// Token: 0x04001469 RID: 5225
		private const float ReproduceMTBHoursAtMinSeverity = 16.8f;

		// Token: 0x0400146A RID: 5226
		private const float ReproduceMTBHoursAtMaxSeverity = 2.1f;

		// Token: 0x0400146B RID: 5227
		private const float ReproductionRadius = 4f;

		// Token: 0x0400146C RID: 5228
		private static FloatRange SizeRange = new FloatRange(0.6f, 1f);

		// Token: 0x0400146D RID: 5229
		private static Color32[] workingColors = new Color32[4];

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x060024F1 RID: 9457 RVA: 0x0013DAD4 File Offset: 0x0013BED4
		// (set) Token: 0x060024F2 RID: 9458 RVA: 0x0013DAEF File Offset: 0x0013BEEF
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

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x060024F3 RID: 9459 RVA: 0x0013DB00 File Offset: 0x0013BF00
		public Plant Plant
		{
			get
			{
				Plant result;
				if (!base.Spawned)
				{
					result = null;
				}
				else
				{
					result = BlightUtility.GetFirstBlightableEverPlant(base.Position, base.Map);
				}
				return result;
			}
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x060024F4 RID: 9460 RVA: 0x0013DB38 File Offset: 0x0013BF38
		protected float ReproduceMTBHours
		{
			get
			{
				float result;
				if (this.severity < 0.28f)
				{
					result = -1f;
				}
				else
				{
					result = GenMath.LerpDouble(0.28f, 1f, 16.8f, 2.1f, this.severity);
				}
				return result;
			}
		}

		// Token: 0x060024F5 RID: 9461 RVA: 0x0013DB87 File Offset: 0x0013BF87
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.severity, "severity", 0f, false);
			Scribe_Values.Look<int>(ref this.lastPlantHarmTick, "lastPlantHarmTick", 0, false);
		}

		// Token: 0x060024F6 RID: 9462 RVA: 0x0013DBB8 File Offset: 0x0013BFB8
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.lastPlantHarmTick = Find.TickManager.TicksGame;
			}
			this.lastMapMeshUpdateSeverity = this.Severity;
		}

		// Token: 0x060024F7 RID: 9463 RVA: 0x0013DBE8 File Offset: 0x0013BFE8
		public override void TickLong()
		{
			this.CheckHarmPlant();
			if (!this.DestroyIfNoPlantHere())
			{
				this.Severity += 0.0333333351f;
				float reproduceMTBHours = this.ReproduceMTBHours;
				if (reproduceMTBHours > 0f && Rand.MTBEventOccurs(reproduceMTBHours, 2500f, 2000f))
				{
					this.TryReproduceNow();
				}
				if (Mathf.Abs(this.Severity - this.lastMapMeshUpdateSeverity) >= 0.05f)
				{
					base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
					this.lastMapMeshUpdateSeverity = this.Severity;
				}
			}
		}

		// Token: 0x060024F8 RID: 9464 RVA: 0x0013DC8E File Offset: 0x0013C08E
		public void Notify_PlantDeSpawned()
		{
			this.DestroyIfNoPlantHere();
		}

		// Token: 0x060024F9 RID: 9465 RVA: 0x0013DC98 File Offset: 0x0013C098
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

		// Token: 0x060024FA RID: 9466 RVA: 0x0013DCDC File Offset: 0x0013C0DC
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

		// Token: 0x060024FB RID: 9467 RVA: 0x0013DD58 File Offset: 0x0013C158
		private void HarmPlant(Plant plant)
		{
			bool isCrop = plant.IsCrop;
			IntVec3 position = base.Position;
			Map map = base.Map;
			plant.TakeDamage(new DamageInfo(DamageDefOf.Rotting, 5f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			if (plant.Destroyed && isCrop && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfBlight-" + plant.def.defName, 240f))
			{
				Messages.Message("MessagePlantDiedOfBlight".Translate(new object[]
				{
					plant.Label
				}).CapitalizeFirst(), new TargetInfo(position, map, false), MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x060024FC RID: 9468 RVA: 0x0013DE08 File Offset: 0x0013C208
		public void TryReproduceNow()
		{
			GenRadial.ProcessEquidistantCells(base.Position, 4f, delegate(List<IntVec3> cells)
			{
				IntVec3 c;
				bool result;
				if ((from x in cells
				where BlightUtility.GetFirstBlightableNowPlant(x, base.Map) != null
				select x).TryRandomElement(out c))
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

		// Token: 0x060024FD RID: 9469 RVA: 0x0013DE30 File Offset: 0x0013C230
		public override void Print(SectionLayer layer)
		{
			Plant plant = this.Plant;
			if (plant != null)
			{
				GenPlant.SetWindExposureColors(Blight.workingColors, plant);
			}
			else
			{
				Blight.workingColors[0].a = (Blight.workingColors[1].a = (Blight.workingColors[2].a = (Blight.workingColors[3].a = 0)));
			}
			float num = Blight.SizeRange.LerpThroughRange(this.severity);
			if (plant != null)
			{
				float a = plant.Graphic.drawSize.x * plant.def.plant.visualSizeRange.LerpThroughRange(plant.Growth);
				num *= Mathf.Min(a, 1f);
			}
			num = Mathf.Clamp(num, 0.5f, 0.9f);
			Vector3 center = this.TrueCenter();
			Vector2 size = this.def.graphic.drawSize * num;
			Material mat = this.Graphic.MatAt(base.Rotation, this);
			Color32[] colors = Blight.workingColors;
			Printer_Plane.PrintPlane(layer, center, size, mat, 0f, false, null, colors, 0.1f, 0f);
		}
	}
}
