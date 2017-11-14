using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class Building_PlantGrower : Building, IPlantToGrowSettable
	{
		private ThingDef plantDefToGrow;

		private CompPowerTrader compPower;

		IEnumerable<IntVec3> IPlantToGrowSettable.Cells
		{
			get
			{
				return this.OccupiedRect().Cells;
			}
		}

		public IEnumerable<Plant> PlantsOnMe
		{
			get
			{
				if (base.Spawned)
				{
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
								/*Error: Unable to find new state assignment for yield return*/;
							}
						}
						cri.MoveNext();
					}
				}
			}
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			using (IEnumerator<Gizmo> enumerator = base.GetGizmos().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Gizmo g = enumerator.Current;
					yield return g;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield return (Gizmo)PlantToGrowSettableUtility.SetPlantToGrowCommand(this);
			/*Error: Unable to find new state assignment for yield return*/;
			IL_00e2:
			/*Error near IL_00e3: Unexpected return in MoveNext()*/;
		}

		public override void PostMake()
		{
			base.PostMake();
			this.plantDefToGrow = base.def.building.defaultPlantToGrow;
		}

		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.compPower = base.GetComp<CompPowerTrader>();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.GrowingFood, KnowledgeAmount.Total);
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.plantDefToGrow, "plantDefToGrow");
		}

		public override void TickRare()
		{
			if (this.compPower != null && !this.compPower.PowerOn)
			{
				foreach (Plant item in this.PlantsOnMe)
				{
					DamageInfo dinfo = new DamageInfo(DamageDefOf.Rotting, 4, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown);
					item.TakeDamage(dinfo);
				}
			}
		}

		public override void DeSpawn()
		{
			foreach (Plant item in this.PlantsOnMe.ToList())
			{
				item.Destroy(DestroyMode.Vanish);
			}
			base.DeSpawn();
		}

		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (base.Spawned)
			{
				text = ((!GenPlant.GrowthSeasonNow(base.Position, base.Map)) ? (text + "\n" + "CannotGrowBadSeasonTemperature".Translate()) : (text + "\n" + "GrowSeasonHereNow".Translate()));
			}
			return text;
		}

		public ThingDef GetPlantDefToGrow()
		{
			return this.plantDefToGrow;
		}

		public void SetPlantDefToGrow(ThingDef plantDef)
		{
			this.plantDefToGrow = plantDef;
		}

		public bool CanAcceptSowNow()
		{
			if (this.compPower != null && !this.compPower.PowerOn)
			{
				return false;
			}
			return true;
		}
	}
}
