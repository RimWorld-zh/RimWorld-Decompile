using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000515 RID: 1301
	public class Pawn_Ownership : IExposable
	{
		// Token: 0x04000DED RID: 3565
		private Pawn pawn;

		// Token: 0x04000DEE RID: 3566
		private Building_Bed intOwnedBed;

		// Token: 0x06001797 RID: 6039 RVA: 0x000CE968 File Offset: 0x000CCD68
		public Pawn_Ownership(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06001798 RID: 6040 RVA: 0x000CE978 File Offset: 0x000CCD78
		// (set) Token: 0x06001799 RID: 6041 RVA: 0x000CE993 File Offset: 0x000CCD93
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
		// (get) Token: 0x0600179A RID: 6042 RVA: 0x000CE9B8 File Offset: 0x000CCDB8
		// (set) Token: 0x0600179B RID: 6043 RVA: 0x000CE9D2 File Offset: 0x000CCDD2
		public Building_Grave AssignedGrave { get; private set; }

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x0600179C RID: 6044 RVA: 0x000CE9DC File Offset: 0x000CCDDC
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

		// Token: 0x0600179D RID: 6045 RVA: 0x000CEA3C File Offset: 0x000CCE3C
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

		// Token: 0x0600179E RID: 6046 RVA: 0x000CEAD0 File Offset: 0x000CCED0
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

		// Token: 0x0600179F RID: 6047 RVA: 0x000CEB8E File Offset: 0x000CCF8E
		public void UnclaimBed()
		{
			if (this.OwnedBed != null)
			{
				this.OwnedBed.owners.Remove(this.pawn);
				this.OwnedBed = null;
			}
		}

		// Token: 0x060017A0 RID: 6048 RVA: 0x000CEBBC File Offset: 0x000CCFBC
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

		// Token: 0x060017A1 RID: 6049 RVA: 0x000CEC20 File Offset: 0x000CD020
		public void UnclaimGrave()
		{
			if (this.AssignedGrave != null)
			{
				this.AssignedGrave.assignedPawn = null;
				this.AssignedGrave.GetStoreSettings().Priority = StoragePriority.Important;
				this.AssignedGrave = null;
			}
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x000CEC54 File Offset: 0x000CD054
		public void UnclaimAll()
		{
			this.UnclaimBed();
			this.UnclaimGrave();
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x000CEC64 File Offset: 0x000CD064
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
