using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003A9 RID: 937
	public class SymbolResolver_CultivatedPlants : SymbolResolver
	{
		// Token: 0x04000A11 RID: 2577
		private const float MinPlantGrowth = 0.2f;

		// Token: 0x04000A12 RID: 2578
		private static List<Thing> tmpThings = new List<Thing>();

		// Token: 0x06001041 RID: 4161 RVA: 0x00088D18 File Offset: 0x00087118
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && (rp.cultivatedPlantDef != null || SymbolResolver_CultivatedPlants.DeterminePlantDef(rp.rect) != null);
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x00088D60 File Offset: 0x00087160
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			ThingDef thingDef = rp.cultivatedPlantDef ?? SymbolResolver_CultivatedPlants.DeterminePlantDef(rp.rect);
			if (thingDef != null)
			{
				float growth = Rand.Range(0.2f, 1f);
				int age = (!thingDef.plant.LimitedLifespan) ? 0 : Rand.Range(0, Mathf.Max(thingDef.plant.LifespanTicks - 2500, 0));
				CellRect.CellRectIterator iterator = rp.rect.GetIterator();
				while (!iterator.Done())
				{
					float num = map.fertilityGrid.FertilityAt(iterator.Current);
					if (num >= thingDef.plant.fertilityMin)
					{
						if (this.TryDestroyBlockingThingsAt(iterator.Current))
						{
							Plant plant = (Plant)GenSpawn.Spawn(thingDef, iterator.Current, map, WipeMode.Vanish);
							plant.Growth = growth;
							if (plant.def.plant.LimitedLifespan)
							{
								plant.Age = age;
							}
						}
					}
					iterator.MoveNext();
				}
			}
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x00088E8C File Offset: 0x0008728C
		public static ThingDef DeterminePlantDef(CellRect rect)
		{
			Map map = BaseGen.globalSettings.map;
			ThingDef result;
			if (map.mapTemperature.OutdoorTemp < 0f || map.mapTemperature.OutdoorTemp > 58f)
			{
				result = null;
			}
			else
			{
				float minFertility = float.MaxValue;
				bool flag = false;
				CellRect.CellRectIterator iterator = rect.GetIterator();
				while (!iterator.Done())
				{
					float num = map.fertilityGrid.FertilityAt(iterator.Current);
					if (num > 0f)
					{
						flag = true;
						minFertility = Mathf.Min(minFertility, num);
					}
					iterator.MoveNext();
				}
				ThingDef thingDef;
				if (!flag)
				{
					result = null;
				}
				else if ((from x in DefDatabase<ThingDef>.AllDefsListForReading
				where x.category == ThingCategory.Plant && x.plant.Sowable && !x.plant.IsTree && !x.plant.cavePlant && x.plant.fertilityMin <= minFertility && x.plant.Harvestable
				select x).TryRandomElement(out thingDef))
				{
					result = thingDef;
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x00088F88 File Offset: 0x00087388
		private bool TryDestroyBlockingThingsAt(IntVec3 c)
		{
			Map map = BaseGen.globalSettings.map;
			SymbolResolver_CultivatedPlants.tmpThings.Clear();
			SymbolResolver_CultivatedPlants.tmpThings.AddRange(c.GetThingList(map));
			for (int i = 0; i < SymbolResolver_CultivatedPlants.tmpThings.Count; i++)
			{
				if (!(SymbolResolver_CultivatedPlants.tmpThings[i] is Pawn))
				{
					if (!SymbolResolver_CultivatedPlants.tmpThings[i].def.destroyable)
					{
						SymbolResolver_CultivatedPlants.tmpThings.Clear();
						return false;
					}
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
