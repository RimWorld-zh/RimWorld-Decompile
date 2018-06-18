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
		// (get) Token: 0x060024AD RID: 9389 RVA: 0x00139D64 File Offset: 0x00138164
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
		// (get) Token: 0x060024AE RID: 9390 RVA: 0x00139D90 File Offset: 0x00138190
		IEnumerable<IntVec3> IPlantToGrowSettable.Cells
		{
			get
			{
				return this.OccupiedRect().Cells;
			}
		}

		// Token: 0x060024AF RID: 9391 RVA: 0x00139DB4 File Offset: 0x001381B4
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo g in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return g;
			}
			yield return PlantToGrowSettableUtility.SetPlantToGrowCommand(this);
			yield break;
		}

		// Token: 0x060024B0 RID: 9392 RVA: 0x00139DDE File Offset: 0x001381DE
		public override void PostMake()
		{
			base.PostMake();
			this.plantDefToGrow = this.def.building.defaultPlantToGrow;
		}

		// Token: 0x060024B1 RID: 9393 RVA: 0x00139DFD File Offset: 0x001381FD
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.compPower = base.GetComp<CompPowerTrader>();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.GrowingFood, KnowledgeAmount.Total);
		}

		// Token: 0x060024B2 RID: 9394 RVA: 0x00139E1F File Offset: 0x0013821F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.plantDefToGrow, "plantDefToGrow");
		}

		// Token: 0x060024B3 RID: 9395 RVA: 0x00139E38 File Offset: 0x00138238
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

		// Token: 0x060024B4 RID: 9396 RVA: 0x00139ED0 File Offset: 0x001382D0
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			foreach (Plant plant in this.PlantsOnMe.ToList<Plant>())
			{
				plant.Destroy(DestroyMode.Vanish);
			}
			base.DeSpawn(mode);
		}

		// Token: 0x060024B5 RID: 9397 RVA: 0x00139F3C File Offset: 0x0013833C
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

		// Token: 0x060024B6 RID: 9398 RVA: 0x00139FB0 File Offset: 0x001383B0
		public ThingDef GetPlantDefToGrow()
		{
			return this.plantDefToGrow;
		}

		// Token: 0x060024B7 RID: 9399 RVA: 0x00139FCB File Offset: 0x001383CB
		public void SetPlantDefToGrow(ThingDef plantDef)
		{
			this.plantDefToGrow = plantDef;
		}

		// Token: 0x060024B8 RID: 9400 RVA: 0x00139FD8 File Offset: 0x001383D8
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
