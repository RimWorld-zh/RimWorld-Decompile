using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class Pawn_Ownership : IExposable
	{
		private Pawn pawn;

		private Building_Bed intOwnedBed;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private Building_Grave <AssignedGrave>k__BackingField;

		public Pawn_Ownership(Pawn pawn)
		{
			this.pawn = pawn;
		}

		public Building_Bed OwnedBed
		{
			get
			{
				return this.intOwnedBed;
			}
			private set
			{
				if (this.intOwnedBed != value)
				{
					this.intOwnedBed = value;
					ThoughtUtility.RemovePositiveBedroomThoughts(this.pawn);
				}
			}
		}

		public Building_Grave AssignedGrave
		{
			[CompilerGenerated]
			get
			{
				return this.<AssignedGrave>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<AssignedGrave>k__BackingField = value;
			}
		}

		public Room OwnedRoom
		{
			get
			{
				Room result;
				if (this.OwnedBed == null)
				{
					result = null;
				}
				else
				{
					Room room = this.OwnedBed.GetRoom(RegionType.Set_Passable);
					if (room == null)
					{
						result = null;
					}
					else if (room.Owners.Contains(this.pawn))
					{
						result = room;
					}
					else
					{
						result = null;
					}
				}
				return result;
			}
		}

		public void ExposeData()
		{
			Building_Grave assignedGrave = this.AssignedGrave;
			Scribe_References.Look<Building_Bed>(ref this.intOwnedBed, "ownedBed", false);
			Scribe_References.Look<Building_Grave>(ref assignedGrave, "assignedGrave", false);
			this.AssignedGrave = assignedGrave;
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.AssignedGrave != null)
				{
					this.AssignedGrave.assignedPawn = this.pawn;
				}
				if (this.OwnedBed != null)
				{
					this.OwnedBed.owners.Add(this.pawn);
					this.OwnedBed.SortOwners();
				}
			}
		}

		public void ClaimBedIfNonMedical(Building_Bed newBed)
		{
			if (!newBed.owners.Contains(this.pawn) && !newBed.Medical)
			{
				this.UnclaimBed();
				if (newBed.owners.Count == newBed.SleepingSlotsCount)
				{
					newBed.owners[newBed.owners.Count - 1].ownership.UnclaimBed();
				}
				newBed.owners.Add(this.pawn);
				newBed.SortOwners();
				this.OwnedBed = newBed;
				if (newBed.Medical)
				{
					Log.Warning(this.pawn.LabelCap + " claimed medical bed.", false);
					this.UnclaimBed();
				}
			}
		}

		public void UnclaimBed()
		{
			if (this.OwnedBed != null)
			{
				this.OwnedBed.owners.Remove(this.pawn);
				this.OwnedBed = null;
			}
		}

		public void ClaimGrave(Building_Grave newGrave)
		{
			if (newGrave.assignedPawn != this.pawn)
			{
				this.UnclaimGrave();
				if (newGrave.assignedPawn != null)
				{
					newGrave.assignedPawn.ownership.UnclaimBed();
				}
				newGrave.assignedPawn = this.pawn;
				newGrave.GetStoreSettings().Priority = StoragePriority.Critical;
				this.AssignedGrave = newGrave;
			}
		}

		public void UnclaimGrave()
		{
			if (this.AssignedGrave != null)
			{
				this.AssignedGrave.assignedPawn = null;
				this.AssignedGrave.GetStoreSettings().Priority = StoragePriority.Important;
				this.AssignedGrave = null;
			}
		}

		public void UnclaimAll()
		{
			this.UnclaimBed();
			this.UnclaimGrave();
		}

		public void Notify_ChangedGuestStatus()
		{
			if (this.OwnedBed != null)
			{
				if ((this.OwnedBed.ForPrisoners && !this.pawn.IsPrisoner) || (!this.OwnedBed.ForPrisoners && this.pawn.IsPrisoner))
				{
					this.UnclaimBed();
				}
			}
		}
	}
}
