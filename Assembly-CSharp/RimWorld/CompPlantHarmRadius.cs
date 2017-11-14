using UnityEngine;
using Verse;

namespace RimWorld
{
	public class CompPlantHarmRadius : ThingComp
	{
		private int plantHarmAge;

		private int ticksToPlantHarm;

		protected CompProperties_PlantHarmRadius PropsPlantHarmRadius
		{
			get
			{
				return (CompProperties_PlantHarmRadius)base.props;
			}
		}

		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.plantHarmAge, "plantHarmAge", 0, false);
			Scribe_Values.Look<int>(ref this.ticksToPlantHarm, "ticksToPlantHarm", 0, false);
		}

		public override void CompTick()
		{
			if (base.parent.Spawned)
			{
				this.plantHarmAge++;
				this.ticksToPlantHarm--;
				if (this.ticksToPlantHarm <= 0)
				{
					float x = (float)((float)this.plantHarmAge / 60000.0);
					float num = this.PropsPlantHarmRadius.radiusPerDayCurve.Evaluate(x);
					float num2 = (float)(3.1415927410125732 * num * num);
					float num3 = num2 * this.PropsPlantHarmRadius.harmFrequencyPerArea;
					float num4 = (float)(60.0 / num3);
					int num5;
					if (num4 >= 1.0)
					{
						this.ticksToPlantHarm = Mathf.RoundToInt(num4);
						num5 = 1;
					}
					else
					{
						this.ticksToPlantHarm = 1;
						num5 = GenMath.RoundRandom((float)(1.0 / num4));
					}
					for (int i = 0; i < num5; i++)
					{
						this.HarmRandomPlantInRadius(num);
					}
				}
			}
		}

		private void HarmRandomPlantInRadius(float radius)
		{
			IntVec3 c = base.parent.Position + (Rand.PointOnDisc * radius).ToIntVec3();
			if (c.InBounds(base.parent.Map))
			{
				Plant plant = c.GetPlant(base.parent.Map);
				if (plant != null)
				{
					if (plant.LeaflessNow)
					{
						if (Rand.Value < 0.20000000298023224)
						{
							plant.Kill(null, null);
						}
					}
					else
					{
						plant.MakeLeafless(Plant.LeaflessCause.Poison);
					}
				}
			}
		}
	}
}
