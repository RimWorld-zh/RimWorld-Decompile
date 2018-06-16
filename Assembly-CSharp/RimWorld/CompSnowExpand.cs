using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02000737 RID: 1847
	public class CompSnowExpand : ThingComp
	{
		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x060028B9 RID: 10425 RVA: 0x0015B2F8 File Offset: 0x001596F8
		public CompProperties_SnowExpand Props
		{
			get
			{
				return (CompProperties_SnowExpand)this.props;
			}
		}

		// Token: 0x060028BA RID: 10426 RVA: 0x0015B318 File Offset: 0x00159718
		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.snowRadius, "snowRadius", 0f, false);
		}

		// Token: 0x060028BB RID: 10427 RVA: 0x0015B331 File Offset: 0x00159731
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

		// Token: 0x060028BC RID: 10428 RVA: 0x0015B36C File Offset: 0x0015976C
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

		// Token: 0x04001645 RID: 5701
		private float snowRadius;

		// Token: 0x04001646 RID: 5702
		private ModuleBase snowNoise;

		// Token: 0x04001647 RID: 5703
		private const float MaxOutdoorTemp = 10f;

		// Token: 0x04001648 RID: 5704
		private static HashSet<IntVec3> reachableCells = new HashSet<IntVec3>();
	}
}
