using System;

namespace Verse
{
	public struct TraverseParms : IEquatable<TraverseParms>
	{
		public Pawn pawn;

		public TraverseMode mode;

		public Danger maxDanger;

		public bool canBash;

		public static TraverseParms For(Pawn pawn, Danger maxDanger = Danger.Deadly, TraverseMode mode = TraverseMode.ByPawn, bool canBash = false)
		{
			TraverseParms result;
			if (pawn == null)
			{
				Log.Error("TraverseParms for null pawn.");
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

		public void Validate()
		{
			if (this.mode == TraverseMode.ByPawn && this.pawn == null)
			{
				Log.Error("Invalid traverse parameters: IfPawnAllowed but traverser = null.");
			}
		}

		public static implicit operator TraverseParms(TraverseMode m)
		{
			if (m == TraverseMode.ByPawn)
			{
				throw new InvalidOperationException("Cannot implicitly convert TraverseMode.ByPawn to RegionTraverseParameters.");
			}
			return TraverseParms.For(m, Danger.Deadly, false);
		}

		public static bool operator ==(TraverseParms a, TraverseParms b)
		{
			return a.pawn == b.pawn && a.mode == b.mode && a.canBash == b.canBash && a.maxDanger == b.maxDanger;
		}

		public static bool operator !=(TraverseParms a, TraverseParms b)
		{
			return a.pawn != b.pawn || a.mode != b.mode || a.canBash != b.canBash || a.maxDanger != b.maxDanger;
		}

		public override bool Equals(object obj)
		{
			return obj is TraverseParms && this.Equals((TraverseParms)obj);
		}

		public bool Equals(TraverseParms other)
		{
			return other.pawn == this.pawn && other.mode == this.mode && other.canBash == this.canBash && other.maxDanger == this.maxDanger;
		}

		public override int GetHashCode()
		{
			int seed = this.canBash ? 1 : 0;
			seed = ((this.pawn == null) ? Gen.HashCombineStruct(seed, this.mode) : Gen.HashCombine(seed, this.pawn));
			return Gen.HashCombineStruct(seed, this.maxDanger);
		}

		public override string ToString()
		{
			string text = (!this.canBash) ? "" : " canBash";
			return (this.mode != 0) ? ("(" + this.mode + " " + this.maxDanger + text + ")") : ("(" + this.mode + " " + this.maxDanger + " " + this.pawn + text + ")");
		}
	}
}
