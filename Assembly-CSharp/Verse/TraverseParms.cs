using System;

namespace Verse
{
	// Token: 0x02000ADF RID: 2783
	public struct TraverseParms : IEquatable<TraverseParms>
	{
		// Token: 0x06003D9C RID: 15772 RVA: 0x00206360 File Offset: 0x00204760
		public static TraverseParms For(Pawn pawn, Danger maxDanger = Danger.Deadly, TraverseMode mode = TraverseMode.ByPawn, bool canBash = false)
		{
			TraverseParms result;
			if (pawn == null)
			{
				Log.Error("TraverseParms for null pawn.", false);
				result = TraverseParms.For(TraverseMode.NoPassClosedDoors, maxDanger, canBash);
			}
			else
			{
				result = new TraverseParms
				{
					pawn = pawn,
					maxDanger = maxDanger,
					mode = mode,
					canBash = canBash
				};
			}
			return result;
		}

		// Token: 0x06003D9D RID: 15773 RVA: 0x002063C0 File Offset: 0x002047C0
		public static TraverseParms For(TraverseMode mode, Danger maxDanger = Danger.Deadly, bool canBash = false)
		{
			return new TraverseParms
			{
				pawn = null,
				mode = mode,
				maxDanger = maxDanger,
				canBash = canBash
			};
		}

		// Token: 0x06003D9E RID: 15774 RVA: 0x002063FE File Offset: 0x002047FE
		public void Validate()
		{
			if (this.mode == TraverseMode.ByPawn && this.pawn == null)
			{
				Log.Error("Invalid traverse parameters: IfPawnAllowed but traverser = null.", false);
			}
		}

		// Token: 0x06003D9F RID: 15775 RVA: 0x00206424 File Offset: 0x00204824
		public static implicit operator TraverseParms(TraverseMode m)
		{
			if (m == TraverseMode.ByPawn)
			{
				throw new InvalidOperationException("Cannot implicitly convert TraverseMode.ByPawn to RegionTraverseParameters.");
			}
			return TraverseParms.For(m, Danger.Deadly, false);
		}

		// Token: 0x06003DA0 RID: 15776 RVA: 0x00206454 File Offset: 0x00204854
		public static bool operator ==(TraverseParms a, TraverseParms b)
		{
			return a.pawn == b.pawn && a.mode == b.mode && a.canBash == b.canBash && a.maxDanger == b.maxDanger;
		}

		// Token: 0x06003DA1 RID: 15777 RVA: 0x002064B8 File Offset: 0x002048B8
		public static bool operator !=(TraverseParms a, TraverseParms b)
		{
			return a.pawn != b.pawn || a.mode != b.mode || a.canBash != b.canBash || a.maxDanger != b.maxDanger;
		}

		// Token: 0x06003DA2 RID: 15778 RVA: 0x0020651C File Offset: 0x0020491C
		public override bool Equals(object obj)
		{
			return obj is TraverseParms && this.Equals((TraverseParms)obj);
		}

		// Token: 0x06003DA3 RID: 15779 RVA: 0x00206550 File Offset: 0x00204950
		public bool Equals(TraverseParms other)
		{
			return other.pawn == this.pawn && other.mode == this.mode && other.canBash == this.canBash && other.maxDanger == this.maxDanger;
		}

		// Token: 0x06003DA4 RID: 15780 RVA: 0x002065B0 File Offset: 0x002049B0
		public override int GetHashCode()
		{
			int seed = (!this.canBash) ? 0 : 1;
			if (this.pawn != null)
			{
				seed = Gen.HashCombine<Pawn>(seed, this.pawn);
			}
			else
			{
				seed = Gen.HashCombineStruct<TraverseMode>(seed, this.mode);
			}
			return Gen.HashCombineStruct<Danger>(seed, this.maxDanger);
		}

		// Token: 0x06003DA5 RID: 15781 RVA: 0x00206610 File Offset: 0x00204A10
		public override string ToString()
		{
			string text = (!this.canBash) ? "" : " canBash";
			string result;
			if (this.mode == TraverseMode.ByPawn)
			{
				result = string.Concat(new object[]
				{
					"(",
					this.mode,
					" ",
					this.maxDanger,
					" ",
					this.pawn,
					text,
					")"
				});
			}
			else
			{
				result = string.Concat(new object[]
				{
					"(",
					this.mode,
					" ",
					this.maxDanger,
					text,
					")"
				});
			}
			return result;
		}

		// Token: 0x040026CE RID: 9934
		public Pawn pawn;

		// Token: 0x040026CF RID: 9935
		public TraverseMode mode;

		// Token: 0x040026D0 RID: 9936
		public Danger maxDanger;

		// Token: 0x040026D1 RID: 9937
		public bool canBash;
	}
}
