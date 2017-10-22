using RimWorld.Planet;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_CropBlight : IncidentWorker
	{
		private const float MaxDaysToGrown = 15f;

		private const float KillChance = 0.8f;

		public override bool TryExecute(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			List<Thing> list = map.listerThings.ThingsInGroup(ThingRequestGroup.Plant);
			bool flag = false;
			IntVec3 cell = IntVec3.Invalid;
			for (int num = list.Count - 1; num >= 0; num--)
			{
				Plant plant = (Plant)list[num];
				if (map.Biome.CommonalityOfPlant(plant.def) == 0.0 && !(plant.def.plant.growDays > 15.0) && plant.sown && (plant.LifeStage == PlantLifeStage.Growing || plant.LifeStage == PlantLifeStage.Mature) && Rand.Value < 0.800000011920929)
				{
					flag = true;
					cell = plant.Position;
					plant.CropBlighted();
				}
			}
			if (!flag)
			{
				return false;
			}
			Find.LetterStack.ReceiveLetter("LetterLabelCropBlight".Translate(), "CropBlight".Translate(), LetterDefOf.BadNonUrgent, new GlobalTargetInfo(cell, map, false), (string)null);
			return true;
		}
	}
}
