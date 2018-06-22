using System;

namespace Verse
{
	// Token: 0x02000ADB RID: 2779
	public struct TraverseParms : IEquatable<TraverseParms>
	{
		// Token: 0x06003D97 RID: 15767 RVA: 0x00206684 File Offset: 0x00204A84
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

		// Token: 0x06003D98 RID: 15768 RVA: 0x002066E4 File Offset: 0x00204AE4
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

		// Token: 0x06003D99 RID: 15769 RVA: 0x00206722 File Offset: 0x00204B22
		public void Validate()
		{
			if (this.mode == TraverseMode.ByPawn && this.pawn == null)
			{
				Log.Error("Invalid traverse parameters: IfPawnAllowed but traverser = null.", false);
			}
		}

		// Token: 0x06003D9A RID: 15770 RVA: 0x00206748 File Offset: 0x00204B48
		public static implicit operator TraverseParms(TraverseMode m)
		{
			if (m == TraverseMode.ByPawn)
			{
				throw new InvalidOperationException("Cannot implicitly convert TraverseMode.ByPawn to RegionTraverseParameters.");
			}
			return TraverseParms.For(m, Danger.Deadly, false);
		}

		// Token: 0x06003D9B RID: 15771 RVA: 0x00206778 File Offset: 0x00204B78
		public static bool operator ==(TraverseParms a, TraverseParms b)
		{
			return a.pawn == b.pawn && a.mode == b.mode && a.canBash == b.canBash && a.maxDanger == b.maxDanger;
		}

		// Token: 0x06003D9C RID: 15772 RVA: 0x002067DC File Offset: 0x00204BDC
		public static bool operator !=(TraverseParms a, TraverseParms b)
		{
			return a.pawn != b.pawn || a.mode != b.mode || a.canBash != b.canBash || a.maxDanger != b.maxDanger;
		}

		// Token: 0x06003D9D RID: 15773 RVA: 0x00206840 File Offset: 0x00204C40
		public override bool Equals(object obj)
		{
			return obj is TraverseParms && this.Equals((TraverseParms)obj);
		}

		// Token: 0x06003D9E RID: 15774 RVA: 0x00206874 File Offset: 0x00204C74
		public bool Equals(TraverseParms other)
		{
			return other.pawn == this.pawn && other.mode == this.mode && other.canBash == this.canBash && other.maxDanger == this.maxDanger;
		}

		// Token: 0x06003D9F RID: 15775 RVA: 0x002068D4 File Offset: 0x00204CD4
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

		// Token: 0x06003DA0 RID: 15776 RVA: 0x00206934 File Offset: 0x00204D34
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

		// Token: 0x040026C9 RID: 9929
		public Pawn pawn;

		// Token: 0x040026CA RID: 9930
		public TraverseMode mode;

		// Token: 0x040026CB RID: 9931
		public Danger maxDanger;

		// Token: 0x040026CC RID: 9932
		public bool canBash;
	}
}
