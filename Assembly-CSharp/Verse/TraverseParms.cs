using System;

namespace Verse
{
	// Token: 0x02000ADD RID: 2781
	public struct TraverseParms : IEquatable<TraverseParms>
	{
		// Token: 0x040026CA RID: 9930
		public Pawn pawn;

		// Token: 0x040026CB RID: 9931
		public TraverseMode mode;

		// Token: 0x040026CC RID: 9932
		public Danger maxDanger;

		// Token: 0x040026CD RID: 9933
		public bool canBash;

		// Token: 0x06003D9B RID: 15771 RVA: 0x002067B0 File Offset: 0x00204BB0
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

		// Token: 0x06003D9C RID: 15772 RVA: 0x00206810 File Offset: 0x00204C10
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

		// Token: 0x06003D9D RID: 15773 RVA: 0x0020684E File Offset: 0x00204C4E
		public void Validate()
		{
			if (this.mode == TraverseMode.ByPawn && this.pawn == null)
			{
				Log.Error("Invalid traverse parameters: IfPawnAllowed but traverser = null.", false);
			}
		}

		// Token: 0x06003D9E RID: 15774 RVA: 0x00206874 File Offset: 0x00204C74
		public static implicit operator TraverseParms(TraverseMode m)
		{
			if (m == TraverseMode.ByPawn)
			{
				throw new InvalidOperationException("Cannot implicitly convert TraverseMode.ByPawn to RegionTraverseParameters.");
			}
			return TraverseParms.For(m, Danger.Deadly, false);
		}

		// Token: 0x06003D9F RID: 15775 RVA: 0x002068A4 File Offset: 0x00204CA4
		public static bool operator ==(TraverseParms a, TraverseParms b)
		{
			return a.pawn == b.pawn && a.mode == b.mode && a.canBash == b.canBash && a.maxDanger == b.maxDanger;
		}

		// Token: 0x06003DA0 RID: 15776 RVA: 0x00206908 File Offset: 0x00204D08
		public static bool operator !=(TraverseParms a, TraverseParms b)
		{
			return a.pawn != b.pawn || a.mode != b.mode || a.canBash != b.canBash || a.maxDanger != b.maxDanger;
		}

		// Token: 0x06003DA1 RID: 15777 RVA: 0x0020696C File Offset: 0x00204D6C
		public override bool Equals(object obj)
		{
			return obj is TraverseParms && this.Equals((TraverseParms)obj);
		}

		// Token: 0x06003DA2 RID: 15778 RVA: 0x002069A0 File Offset: 0x00204DA0
		public bool Equals(TraverseParms other)
		{
			return other.pawn == this.pawn && other.mode == this.mode && other.canBash == this.canBash && other.maxDanger == this.maxDanger;
		}

		// Token: 0x06003DA3 RID: 15779 RVA: 0x00206A00 File Offset: 0x00204E00
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

		// Token: 0x06003DA4 RID: 15780 RVA: 0x00206A60 File Offset: 0x00204E60
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
	}
}
