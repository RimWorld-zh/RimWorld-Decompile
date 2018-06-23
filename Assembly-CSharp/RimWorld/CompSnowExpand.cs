using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02000733 RID: 1843
	public class CompSnowExpand : ThingComp
	{
		// Token: 0x04001643 RID: 5699
		private float snowRadius;

		// Token: 0x04001644 RID: 5700
		private ModuleBase snowNoise;

		// Token: 0x04001645 RID: 5701
		private const float MaxOutdoorTemp = 10f;

		// Token: 0x04001646 RID: 5702
		private static HashSet<IntVec3> reachableCells = new HashSet<IntVec3>();

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x060028B4 RID: 10420 RVA: 0x0015B564 File Offset: 0x00159964
		public CompProperties_SnowExpand Props
		{
			get
			{
				return (CompProperties_SnowExpand)this.props;
			}
		}

		// Token: 0x060028B5 RID: 10421 RVA: 0x0015B584 File Offset: 0x00159984
		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.snowRadius, "snowRadius", 0f, false);
		}

		// Token: 0x060028B6 RID: 10422 RVA: 0x0015B59D File Offset: 0x0015999D
		public override void CompTick()
		{
			if (this.parent.Spawned)
			{
				if (this.parent.IsHashIntervalTick(this.Props.expandInterval))
				{
					this.TryExpandSnow();
				}
			}
		}

		// Token: 0x060028B7 RID: 10423 RVA: 0x0015B5D8 File Offset: 0x001599D8
		private void TryExpandSnow()
		{
			if (this.parent.Map.mapTemperature.OutdoorTemp > 10f)
			{
				this.snowRadius = 0f;
			}
			else
			{
				if (this.snowNoise == null)
				{
					this.snowNoise = new Perlin(0.054999999701976776, 2.0, 0.5, 5, Rand.Range(0, 651431), QualityMode.Medium);
				}
				if (this.snowRadius < 8f)
				{
					this.snowRadius += 1.3f;
				}
				else if (this.snowRadius < 17f)
				{
					this.snowRadius += 0.7f;
				}
				else if (this.snowRadius < 30f)
				{
					this.snowRadius += 0.4f;
				}
				else
				{
					this.snowRadius += 0.1f;
				}
				this.snowRadius = Mathf.Min(this.snowRadius, this.Props.maxRadius);
				CellRect occupiedRect = this.parent.OccupiedRect();
				CompSnowExpand.reachableCells.Clear();
				this.parent.Map.floodFiller.FloodFill(this.parent.Position, (IntVec3 x) => (float)x.DistanceToSquared(this.parent.Position) <= this.snowRadius * this.snowRadius && (occupiedRect.Contains(x) || !x.Filled(this.parent.Map)), delegate(IntVec3 x)
				{
					CompSnowExpand.reachableCells.Add(x);
				}, int.MaxValue, false, null);
				int num = GenRadial.NumCellsInRadius(this.snowRadius);
				for (int i = 0; i < num; i++)
				{
					IntVec3 intVec = this.parent.Position + GenRadial.RadialPattern[i];
					if (intVec.InBounds(this.parent.Map))
					{
						if (CompSnowExpand.reachableCells.Contains(intVec))
						{
							float num2 = this.snowNoise.GetValue(intVec);
							num2 += 1f;
							num2 *= 0.5f;
							if (num2 < 0.1f)
							{
								num2 = 0.1f;
							}
							if (this.parent.Map.snowGrid.GetDepth(intVec) <= num2)
							{
								float lengthHorizontal = (intVec - this.parent.Position).LengthHorizontal;
								float num3 = 1f - lengthHorizontal / this.snowRadius;
								this.parent.Map.snowGrid.AddDepth(intVec, num3 * this.Props.addAmount * num2);
							}
						}
					}
				}
			}
		}
	}
}
