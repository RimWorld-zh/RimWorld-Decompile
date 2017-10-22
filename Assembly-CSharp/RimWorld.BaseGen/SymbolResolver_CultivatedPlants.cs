using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_CultivatedPlants : SymbolResolver
	{
		private const float MinPlantGrowth = 0.2f;

		private static List<Thing> tmpThings = new List<Thing>();

		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && (rp.cultivatedPlantDef != null || SymbolResolver_CultivatedPlants.DeterminePlantDef(rp.rect) != null);
		}

		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			ThingDef thingDef = rp.cultivatedPlantDef ?? SymbolResolver_CultivatedPlants.DeterminePlantDef(rp.rect);
			if (thingDef != null)
			{
				float growth = Rand.Range(0.2f, 1f);
				int age = thingDef.plant.LimitedLifespan ? Rand.Range(0, Mathf.Max(thingDef.plant.LifespanTicks - 2500, 0)) : 0;
				CellRect.CellRectIterator iterator = rp.rect.GetIterator();
				while (!iterator.Done())
				{
					float num = map.fertilityGrid.FertilityAt(iterator.Current);
					if (!(num < thingDef.plant.fertilityMin) && this.TryDestroyBlockingThingsAt(iterator.Current))
					{
						Plant plant = (Plant)GenSpawn.Spawn(thingDef, iterator.Current, map);
						plant.Growth = growth;
						if (plant.def.plant.LimitedLifespan)
						{
							plant.Age = age;
						}
					}
					iterator.MoveNext();
				}
			}
		}

		public static ThingDef DeterminePlantDef(CellRect rect)
		{
			Map map = BaseGen.globalSettings.map;
			if (!(map.mapTemperature.OutdoorTemp < 0.0) && !(map.mapTemperature.OutdoorTemp > 58.0))
			{
				float minFertility = 3.40282347E+38f;
				bool flag = false;
				CellRect.CellRectIterator iterator = rect.GetIterator();
				while (!iterator.Done())
				{
					float num = map.fertilityGrid.FertilityAt(iterator.Current);
					if (!(num <= 0.0))
					{
						flag = true;
						minFertility = Mathf.Min(minFertility, num);
					}
					iterator.MoveNext();
				}
				if (!flag)
				{
					return null;
				}
				ThingDef result = default(ThingDef);
				if ((from x in DefDatabase<ThingDef>.AllDefsListForReading
				where x.category == ThingCategory.Plant && x.plant.Sowable && !x.plant.IsTree && x.plant.fertilityMin <= minFertility && x.plant.Harvestable
				select x).TryRandomElement<ThingDef>(out result))
				{
					return result;
				}
				return null;
			}
			return null;
		}

		private bool TryDestroyBlockingThingsAt(IntVec3 c)
		{
			Map map = BaseGen.globalSettings.map;
			SymbolResolver_CultivatedPlants.tmpThings.Clear();
			SymbolResolver_CultivatedPlants.tmpThings.AddRange(c.GetThingList(map));
			for (int i = 0; i < SymbolResolver_CultivatedPlants.tmpThings.Count; i++)
			{
				if (!(SymbolResolver_CultivatedPlants.tmpThings[i] is Pawn) && !SymbolResolver_CultivatedPlants.tmpThings[i].def.destroyable)
				{
					SymbolResolver_CultivatedPlants.tmpThings.Clear();
					return false;
				}
			}
			for (int j = 0; j < SymbolResolver_CultivatedPlants.tmpThings.Count; j++)
			{
				if (!(SymbolResolver_CultivatedPlants.tmpThings[j] is Pawn))
				{
					SymbolResolver_CultivatedPlants.tmpThings[j].Destroy(DestroyMode.Vanish);
				}
			}
			SymbolResolver_CultivatedPlants.tmpThings.Clear();
			return true;
		}
	}
}
