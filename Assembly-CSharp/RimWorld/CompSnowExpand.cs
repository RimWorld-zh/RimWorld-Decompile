using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	public class CompSnowExpand : ThingComp
	{
		private float snowRadius;

		private ModuleBase snowNoise;

		private static HashSet<IntVec3> reachableCells = new HashSet<IntVec3>();

		public CompProperties_SnowExpand Props
		{
			get
			{
				return (CompProperties_SnowExpand)base.props;
			}
		}

		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.snowRadius, "snowRadius", 0f, false);
		}

		public override void CompTick()
		{
			if (base.parent.Spawned && base.parent.IsHashIntervalTick(this.Props.expandInterval))
			{
				this.ExpandSnow();
			}
		}

		private void ExpandSnow()
		{
			if (this.snowNoise == null)
			{
				this.snowNoise = new Perlin(0.054999999701976776, 2.0, 0.5, 5, Rand.Range(0, 651431), QualityMode.Medium);
			}
			if (this.snowRadius < 8.0)
			{
				this.snowRadius += 1.3f;
			}
			else if (this.snowRadius < 17.0)
			{
				this.snowRadius += 0.7f;
			}
			else if (this.snowRadius < 30.0)
			{
				this.snowRadius += 0.4f;
			}
			else
			{
				this.snowRadius += 0.1f;
			}
			this.snowRadius = Mathf.Min(this.snowRadius, this.Props.maxRadius);
			CellRect occupiedRect = base.parent.OccupiedRect();
			CompSnowExpand.reachableCells.Clear();
			base.parent.Map.floodFiller.FloodFill(base.parent.Position, (Predicate<IntVec3>)((IntVec3 x) => !((float)x.DistanceToSquared(base.parent.Position) > this.snowRadius * this.snowRadius) && (occupiedRect.Contains(x) || !x.Filled(base.parent.Map))), (Action<IntVec3>)delegate(IntVec3 x)
			{
				CompSnowExpand.reachableCells.Add(x);
			}, 2147483647, false, null);
			int num = GenRadial.NumCellsInRadius(this.snowRadius);
			for (int num2 = 0; num2 < num; num2++)
			{
				IntVec3 intVec = base.parent.Position + GenRadial.RadialPattern[num2];
				if (intVec.InBounds(base.parent.Map) && CompSnowExpand.reachableCells.Contains(intVec))
				{
					float value = this.snowNoise.GetValue(intVec);
					value = (float)(value + 1.0);
					value = (float)(value * 0.5);
					if (value < 0.10000000149011612)
					{
						value = 0.1f;
					}
					if (!(base.parent.Map.snowGrid.GetDepth(intVec) > value))
					{
						float lengthHorizontal = (intVec - base.parent.Position).LengthHorizontal;
						float num3 = (float)(1.0 - lengthHorizontal / this.snowRadius);
						base.parent.Map.snowGrid.AddDepth(intVec, num3 * this.Props.addAmount * value);
					}
				}
			}
		}
	}
}
