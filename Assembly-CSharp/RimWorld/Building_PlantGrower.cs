using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020006AB RID: 1707
	public class Building_PlantGrower : Building, IPlantToGrowSettable
	{
		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x060024A5 RID: 9381 RVA: 0x00139EAC File Offset: 0x001382AC
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
		// (get) Token: 0x060024A6 RID: 9382 RVA: 0x00139ED8 File Offset: 0x001382D8
		IEnumerable<IntVec3> IPlantToGrowSettable.Cells
		{
			get
			{
				return this.OccupiedRect().Cells;
			}
		}

		// Token: 0x060024A7 RID: 9383 RVA: 0x00139EFC File Offset: 0x001382FC
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo g in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return g;
			}
			yield return PlantToGrowSettableUtility.SetPlantToGrowCommand(this);
			yield break;
		}

		// Token: 0x060024A8 RID: 9384 RVA: 0x00139F26 File Offset: 0x00138326
		public override void PostMake()
		{
			base.PostMake();
			this.plantDefToGrow = this.def.building.defaultPlantToGrow;
		}

		// Token: 0x060024A9 RID: 9385 RVA: 0x00139F45 File Offset: 0x00138345
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.compPower = base.GetComp<CompPowerTrader>();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.GrowingFood, KnowledgeAmount.Total);
		}

		// Token: 0x060024AA RID: 9386 RVA: 0x00139F67 File Offset: 0x00138367
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.plantDefToGrow, "plantDefToGrow");
		}

		// Token: 0x060024AB RID: 9387 RVA: 0x00139F80 File Offset: 0x00138380
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

		// Token: 0x060024AC RID: 9388 RVA: 0x0013A018 File Offset: 0x00138418
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			foreach (Plant plant in this.PlantsOnMe.ToList<Plant>())
			{
				plant.Destroy(DestroyMode.Vanish);
			}
			base.DeSpawn(mode);
		}

		// Token: 0x060024AD RID: 9389 RVA: 0x0013A084 File Offset: 0x00138484
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

		// Token: 0x060024AE RID: 9390 RVA: 0x0013A0F8 File Offset: 0x001384F8
		public ThingDef GetPlantDefToGrow()
		{
			return this.plantDefToGrow;
		}

		// Token: 0x060024AF RID: 9391 RVA: 0x0013A113 File Offset: 0x00138513
		public void SetPlantDefToGrow(ThingDef plantDef)
		{
			this.plantDefToGrow = plantDef;
		}

		// Token: 0x060024B0 RID: 9392 RVA: 0x0013A120 File Offset: 0x00138520
		public bool CanAcceptSowNow()
		{
			return this.compPower == null || this.compPower.PowerOn;
		}

		// Token: 0x0400143D RID: 5181
		private ThingDef plantDefToGrow;

		// Token: 0x0400143E RID: 5182
		private CompPowerTrader compPower;
	}
}
