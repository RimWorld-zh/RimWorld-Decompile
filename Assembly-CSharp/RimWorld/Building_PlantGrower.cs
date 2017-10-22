using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class Building_PlantGrower : Building, IPlantToGrowSettable
	{
		private ThingDef plantDefToGrow;

		private CompPowerTrader compPower;

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
							}
						}
						cri.MoveNext();
					}
				}
			}
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			yield return (Gizmo)PlantToGrowSettableUtility.SetPlantToGrowCommand(this);
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
			List<Plant>.Enumerator enumerator = this.PlantsOnMe.ToList().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Plant current = enumerator.Current;
					current.Destroy(DestroyMode.Vanish);
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
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

		virtual Map get_Map()
		{
			return base.Map;
		}

		Map IPlantToGrowSettable.get_Map()
		{
			//ILSpy generated this explicit interface implementation from .override directive in get_Map
			return this.get_Map();
		}
	}
}
