using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_CropBlight : IncidentWorker
	{
		private const float Radius = 16f;

		private const float BaseBlightChance = 0.1f;

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			Plant plant = default(Plant);
			return this.TryFindRandomBlightablePlant((Map)target, out plant);
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			Plant plant = default(Plant);
			bool result;
			if (!this.TryFindRandomBlightablePlant(map, out plant))
			{
				result = false;
			}
			else
			{
				Room room = plant.GetRoom(RegionType.Set_Passable);
				plant.CropBlighted();
				int num = 0;
				int num2 = GenRadial.NumCellsInRadius(16f);
				while (num < num2)
				{
					IntVec3 intVec = plant.Position + GenRadial.RadialPattern[num];
					if (intVec.InBounds(map) && intVec.GetRoom(map, RegionType.Set_Passable) == room)
					{
						Plant firstBlightableNowPlant = BlightUtility.GetFirstBlightableNowPlant(intVec, map);
						if (firstBlightableNowPlant != null && firstBlightableNowPlant != plant && Rand.Chance((float)(0.10000000149011612 * this.BlightChanceFactor(firstBlightableNowPlant.Position, plant.Position))))
						{
							firstBlightableNowPlant.CropBlighted();
						}
					}
					num++;
				}
				Find.LetterStack.ReceiveLetter("LetterLabelCropBlight".Translate(), "LetterCropBlight".Translate(), LetterDefOf.NegativeEvent, new TargetInfo(plant.Position, map, false), (string)null);
				result = true;
			}
			return result;
		}

		private bool TryFindRandomBlightablePlant(Map map, out Plant plant)
		{
			Thing thing = default(Thing);
			bool result = (from x in map.listerThings.ThingsInGroup(ThingRequestGroup.Plant)
			where ((Plant)x).BlightableNow
			select x).TryRandomElement<Thing>(out thing);
			plant = (Plant)thing;
			return result;
		}

		private float BlightChanceFactor(IntVec3 c, IntVec3 root)
		{
			return Mathf.InverseLerp(16f, 8f, c.DistanceTo(root));
		}
	}
}
