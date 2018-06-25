using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000726 RID: 1830
	public class CompPlantHarmRadius : ThingComp
	{
		// Token: 0x0400160E RID: 5646
		private int plantHarmAge;

		// Token: 0x0400160F RID: 5647
		private int ticksToPlantHarm;

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x06002855 RID: 10325 RVA: 0x00158C10 File Offset: 0x00157010
		protected CompProperties_PlantHarmRadius PropsPlantHarmRadius
		{
			get
			{
				return (CompProperties_PlantHarmRadius)this.props;
			}
		}

		// Token: 0x06002856 RID: 10326 RVA: 0x00158C30 File Offset: 0x00157030
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.plantHarmAge, "plantHarmAge", 0, false);
			Scribe_Values.Look<int>(ref this.ticksToPlantHarm, "ticksToPlantHarm", 0, false);
		}

		// Token: 0x06002857 RID: 10327 RVA: 0x00158C58 File Offset: 0x00157058
		public override void CompTick()
		{
			if (this.parent.Spawned)
			{
				this.plantHarmAge++;
				this.ticksToPlantHarm--;
				if (this.ticksToPlantHarm <= 0)
				{
					float x = (float)this.plantHarmAge / 60000f;
					float num = this.PropsPlantHarmRadius.radiusPerDayCurve.Evaluate(x);
					float num2 = 3.14159274f * num * num;
					float num3 = num2 * this.PropsPlantHarmRadius.harmFrequencyPerArea;
					float num4 = 60f / num3;
					int num5;
					if (num4 >= 1f)
					{
						this.ticksToPlantHarm = GenMath.RoundRandom(num4);
						num5 = 1;
					}
					else
					{
						this.ticksToPlantHarm = 1;
						num5 = GenMath.RoundRandom(1f / num4);
					}
					for (int i = 0; i < num5; i++)
					{
						this.HarmRandomPlantInRadius(num);
					}
				}
			}
		}

		// Token: 0x06002858 RID: 10328 RVA: 0x00158D44 File Offset: 0x00157144
		private void HarmRandomPlantInRadius(float radius)
		{
			IntVec3 c = this.parent.Position + (Rand.InsideUnitCircleVec3 * radius).ToIntVec3();
			if (c.InBounds(this.parent.Map))
			{
				Plant plant = c.GetPlant(this.parent.Map);
				if (plant != null)
				{
					if (plant.LeaflessNow)
					{
						if (Rand.Value < 0.2f)
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
