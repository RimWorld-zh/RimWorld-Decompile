using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020006AD RID: 1709
	public class Building_PlantGrower : Building, IPlantToGrowSettable
	{
		// Token: 0x04001441 RID: 5185
		private ThingDef plantDefToGrow;

		// Token: 0x04001442 RID: 5186
		private CompPowerTrader compPower;

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x060024A8 RID: 9384 RVA: 0x0013A264 File Offset: 0x00138664
		public IEnumerable<Plant> PlantsOnMe
		{
			get
			{
				if (!base.Spawned)
				{
					yield break;
				}
				CellRect.CellRectIterator cri = this.OccupiedRect().GetIterator();
				while (!cri.Done())
				{
					List<Thing> thingList = base.Map.thingGrid.ThingsListAt(cri.Current);
					for (int i = 0; i < thingList.Count; i++)
					{
						Plant p = thingList[i] as Plant;
						if (p != null)
						{
							yield return p;
						}
					}
					cri.MoveNext();
				}
				yield break;
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x060024A9 RID: 9385 RVA: 0x0013A290 File Offset: 0x00138690
		IEnumerable<IntVec3> IPlantToGrowSettable.Cells
		{
			get
			{
				return this.OccupiedRect().Cells;
			}
		}

		// Token: 0x060024AA RID: 9386 RVA: 0x0013A2B4 File Offset: 0x001386B4
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo g in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return g;
			}
			yield return PlantToGrowSettableUtility.SetPlantToGrowCommand(this);
			yield break;
		}

		// Token: 0x060024AB RID: 9387 RVA: 0x0013A2DE File Offset: 0x001386DE
		public override void PostMake()
		{
			base.PostMake();
			this.plantDefToGrow = this.def.building.defaultPlantToGrow;
		}

		// Token: 0x060024AC RID: 9388 RVA: 0x0013A2FD File Offset: 0x001386FD
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.compPower = base.GetComp<CompPowerTrader>();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.GrowingFood, KnowledgeAmount.Total);
		}

		// Token: 0x060024AD RID: 9389 RVA: 0x0013A31F File Offset: 0x0013871F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.plantDefToGrow, "plantDefToGrow");
		}

		// Token: 0x060024AE RID: 9390 RVA: 0x0013A338 File Offset: 0x00138738
		public override void TickRare()
		{
			if (this.compPower != null && !this.compPower.PowerOn)
			{
				foreach (Plant plant in this.PlantsOnMe)
				{
					DamageInfo dinfo = new DamageInfo(DamageDefOf.Rotting, 1f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
					plant.TakeDamage(dinfo);
				}
			}
		}

		// Token: 0x060024AF RID: 9391 RVA: 0x0013A3D0 File Offset: 0x001387D0
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			foreach (Plant plant in this.PlantsOnMe.ToList<Plant>())
			{
				plant.Destroy(DestroyMode.Vanish);
			}
			base.DeSpawn(mode);
		}

		// Token: 0x060024B0 RID: 9392 RVA: 0x0013A43C File Offset: 0x0013883C
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (base.Spawned)
			{
				if (GenPlant.GrowthSeasonNow(base.Position, base.Map, true))
				{
					text = text + "\n" + "GrowSeasonHereNow".Translate();
				}
				else
				{
					text = text + "\n" + "CannotGrowBadSeasonTemperature".Translate();
				}
			}
			return text;
		}

		// Token: 0x060024B1 RID: 9393 RVA: 0x0013A4B0 File Offset: 0x001388B0
		public ThingDef GetPlantDefToGrow()
		{
			return this.plantDefToGrow;
		}

		// Token: 0x060024B2 RID: 9394 RVA: 0x0013A4CB File Offset: 0x001388CB
		public void SetPlantDefToGrow(ThingDef plantDef)
		{
			this.plantDefToGrow = plantDef;
		}

		// Token: 0x060024B3 RID: 9395 RVA: 0x0013A4D8 File Offset: 0x001388D8
		public bool CanAcceptSowNow()
		{
			return this.compPower == null || this.compPower.PowerOn;
		}
	}
}
