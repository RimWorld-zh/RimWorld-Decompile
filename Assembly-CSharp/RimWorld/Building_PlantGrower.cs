using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020006AF RID: 1711
	public class Building_PlantGrower : Building, IPlantToGrowSettable
	{
		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x060024AB RID: 9387 RVA: 0x00139CEC File Offset: 0x001380EC
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
		// (get) Token: 0x060024AC RID: 9388 RVA: 0x00139D18 File Offset: 0x00138118
		IEnumerable<IntVec3> IPlantToGrowSettable.Cells
		{
			get
			{
				return this.OccupiedRect().Cells;
			}
		}

		// Token: 0x060024AD RID: 9389 RVA: 0x00139D3C File Offset: 0x0013813C
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo g in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return g;
			}
			yield return PlantToGrowSettableUtility.SetPlantToGrowCommand(this);
			yield break;
		}

		// Token: 0x060024AE RID: 9390 RVA: 0x00139D66 File Offset: 0x00138166
		public override void PostMake()
		{
			base.PostMake();
			this.plantDefToGrow = this.def.building.defaultPlantToGrow;
		}

		// Token: 0x060024AF RID: 9391 RVA: 0x00139D85 File Offset: 0x00138185
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.compPower = base.GetComp<CompPowerTrader>();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.GrowingFood, KnowledgeAmount.Total);
		}

		// Token: 0x060024B0 RID: 9392 RVA: 0x00139DA7 File Offset: 0x001381A7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.plantDefToGrow, "plantDefToGrow");
		}

		// Token: 0x060024B1 RID: 9393 RVA: 0x00139DC0 File Offset: 0x001381C0
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

		// Token: 0x060024B2 RID: 9394 RVA: 0x00139E58 File Offset: 0x00138258
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			foreach (Plant plant in this.PlantsOnMe.ToList<Plant>())
			{
				plant.Destroy(DestroyMode.Vanish);
			}
			base.DeSpawn(mode);
		}

		// Token: 0x060024B3 RID: 9395 RVA: 0x00139EC4 File Offset: 0x001382C4
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

		// Token: 0x060024B4 RID: 9396 RVA: 0x00139F38 File Offset: 0x00138338
		public ThingDef GetPlantDefToGrow()
		{
			return this.plantDefToGrow;
		}

		// Token: 0x060024B5 RID: 9397 RVA: 0x00139F53 File Offset: 0x00138353
		public void SetPlantDefToGrow(ThingDef plantDef)
		{
			this.plantDefToGrow = plantDef;
		}

		// Token: 0x060024B6 RID: 9398 RVA: 0x00139F60 File Offset: 0x00138360
		public bool CanAcceptSowNow()
		{
			return this.compPower == null || this.compPower.PowerOn;
		}

		// Token: 0x0400143F RID: 5183
		private ThingDef plantDefToGrow;

		// Token: 0x04001440 RID: 5184
		private CompPowerTrader compPower;
	}
}
