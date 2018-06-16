using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000728 RID: 1832
	public class CompPlantHarmRadius : ThingComp
	{
		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x06002858 RID: 10328 RVA: 0x0015862C File Offset: 0x00156A2C
		protected CompProperties_PlantHarmRadius PropsPlantHarmRadius
		{
			get
			{
				return (CompProperties_PlantHarmRadius)this.props;
			}
		}

		// Token: 0x06002859 RID: 10329 RVA: 0x0015864C File Offset: 0x00156A4C
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.plantHarmAge, "plantHarmAge", 0, false);
			Scribe_Values.Look<int>(ref this.ticksToPlantHarm, "ticksToPlantHarm", 0, false);
		}

		// Token: 0x0600285A RID: 10330 RVA: 0x00158674 File Offset: 0x00156A74
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

		// Token: 0x0600285B RID: 10331 RVA: 0x00158760 File Offset: 0x00156B60
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

		// Token: 0x0400160C RID: 5644
		private int plantHarmAge;

		// Token: 0x0400160D RID: 5645
		private int ticksToPlantHarm;
	}
}
