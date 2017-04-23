using RimWorld;
using System;

namespace Verse.AI
{
	internal class Reservation : IExposable
	{
		private Pawn claimant;

		private LocalTargetInfo target;

		private ReservationLayer layer;

		private int maxPawns;

		private int stackCount = -1;

		public Pawn Claimant
		{
			get
			{
				return this.claimant;
			}
		}

		public LocalTargetInfo Target
		{
			get
			{
				return this.target;
			}
		}

		public ReservationLayer Layer
		{
			get
			{
				return this.layer;
			}
		}

		public int MaxPawns
		{
			get
			{
				return this.maxPawns;
			}
		}

		public int StackCount
		{
			get
			{
				return this.stackCount;
			}
		}

		public Faction Faction
		{
			get
			{
				return this.claimant.Faction;
			}
		}

		public Reservation()
		{
		}

		public Reservation(Pawn claimant, int maxPawns, int stackCount, LocalTargetInfo target, ReservationLayer layer)
		{
			this.claimant = claimant;
			this.maxPawns = maxPawns;
			this.stackCount = stackCount;
			this.target = target;
			this.layer = layer;
		}

		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
			Scribe_TargetInfo.Look(ref this.target, "target");
			Scribe_Values.Look<int>(ref this.maxPawns, "maxPawns", 0, false);
			Scribe_Values.Look<int>(ref this.stackCount, "stackCount", 0, false);
			Scribe_Values.Look<ReservationLayer>(ref this.layer, "layer", ReservationLayer.Default, false);
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				(this.claimant == null) ? "null" : this.claimant.LabelShort,
				", ",
				this.target.ToString(),
				", ",
				this.layer.ToString(),
				", ",
				this.maxPawns,
				", ",
				this.stackCount
			});
		}
	}
}
