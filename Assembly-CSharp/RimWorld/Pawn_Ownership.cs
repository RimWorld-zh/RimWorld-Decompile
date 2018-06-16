using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000519 RID: 1305
	public class Pawn_Ownership : IExposable
	{
		// Token: 0x0600179F RID: 6047 RVA: 0x000CE91C File Offset: 0x000CCD1C
		public Pawn_Ownership(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x060017A0 RID: 6048 RVA: 0x000CE92C File Offset: 0x000CCD2C
		// (set) Token: 0x060017A1 RID: 6049 RVA: 0x000CE947 File Offset: 0x000CCD47
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

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x060017A2 RID: 6050 RVA: 0x000CE96C File Offset: 0x000CCD6C
		// (set) Token: 0x060017A3 RID: 6051 RVA: 0x000CE986 File Offset: 0x000CCD86
		public Building_Grave AssignedGrave { get; private set; }

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x060017A4 RID: 6052 RVA: 0x000CE990 File Offset: 0x000CCD90
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

		// Token: 0x060017A5 RID: 6053 RVA: 0x000CE9F0 File Offset: 0x000CCDF0
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

		// Token: 0x060017A6 RID: 6054 RVA: 0x000CEA84 File Offset: 0x000CCE84
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

		// Token: 0x060017A7 RID: 6055 RVA: 0x000CEB42 File Offset: 0x000CCF42
		public void UnclaimBed()
		{
			if (this.OwnedBed != null)
			{
				this.OwnedBed.owners.Remove(this.pawn);
				this.OwnedBed = null;
			}
		}

		// Token: 0x060017A8 RID: 6056 RVA: 0x000CEB70 File Offset: 0x000CCF70
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

		// Token: 0x060017A9 RID: 6057 RVA: 0x000CEBD4 File Offset: 0x000CCFD4
		public void UnclaimGrave()
		{
			if (this.AssignedGrave != null)
			{
				this.AssignedGrave.assignedPawn = null;
				this.AssignedGrave.GetStoreSettings().Priority = StoragePriority.Important;
				this.AssignedGrave = null;
			}
		}

		// Token: 0x060017AA RID: 6058 RVA: 0x000CEC08 File Offset: 0x000CD008
		public void UnclaimAll()
		{
			this.UnclaimBed();
			this.UnclaimGrave();
		}

		// Token: 0x060017AB RID: 6059 RVA: 0x000CEC18 File Offset: 0x000CD018
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

		// Token: 0x04000DF0 RID: 3568
		private Pawn pawn;

		// Token: 0x04000DF1 RID: 3569
		private Building_Bed intOwnedBed;
	}
}
