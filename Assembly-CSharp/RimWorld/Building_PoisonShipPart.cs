using UnityEngine;

namespace RimWorld
{
	internal class Building_PoisonShipPart : Building_CrashedShipPart
	{
		protected override float PlantHarmRange
		{
			get
			{
				return Mathf.Min((float)(3.0 + 30.0 * ((float)base.age / 60000.0)), 140f);
			}
		}

		protected override int PlantHarmInterval
		{
			get
			{
				float f = (float)(4.0 - 0.60000002384185791 * (float)base.age / 60000.0);
				return Mathf.Clamp(Mathf.RoundToInt(f), 2, 4);
			}
		}
	}
}
