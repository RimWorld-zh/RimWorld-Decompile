using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000517 RID: 1303
	public class Pawn_Ownership : IExposable
	{
		// Token: 0x04000DED RID: 3565
		private Pawn pawn;

		// Token: 0x04000DEE RID: 3566
		private Building_Bed intOwnedBed;

		// Token: 0x0600179B RID: 6043 RVA: 0x000CEAB8 File Offset: 0x000CCEB8
		public Pawn_Ownership(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x0600179C RID: 6044 RVA: 0x000CEAC8 File Offset: 0x000CCEC8
		// (set) Token: 0x0600179D RID: 6045 RVA: 0x000CEAE3 File Offset: 0x000CCEE3
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
		// (get) Token: 0x0600179E RID: 6046 RVA: 0x000CEB08 File Offset: 0x000CCF08
		// (set) Token: 0x0600179F RID: 6047 RVA: 0x000CEB22 File Offset: 0x000CCF22
		public Building_Grave AssignedGrave { get; private set; }

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x060017A0 RID: 6048 RVA: 0x000CEB2C File Offset: 0x000CCF2C
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

		// Token: 0x060017A1 RID: 6049 RVA: 0x000CEB8C File Offset: 0x000CCF8C
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

		// Token: 0x060017A2 RID: 6050 RVA: 0x000CEC20 File Offset: 0x000CD020
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

		// Token: 0x060017A3 RID: 6051 RVA: 0x000CECDE File Offset: 0x000CD0DE
		public void UnclaimBed()
		{
			if (this.OwnedBed != null)
			{
				this.OwnedBed.owners.Remove(this.pawn);
				this.OwnedBed = null;
			}
		}

		// Token: 0x060017A4 RID: 6052 RVA: 0x000CED0C File Offset: 0x000CD10C
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

		// Token: 0x060017A5 RID: 6053 RVA: 0x000CED70 File Offset: 0x000CD170
		public void UnclaimGrave()
		{
			if (this.AssignedGrave != null)
			{
				this.AssignedGrave.assignedPawn = null;
				this.AssignedGrave.GetStoreSettings().Priority = StoragePriority.Important;
				this.AssignedGrave = null;
			}
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x000CEDA4 File Offset: 0x000CD1A4
		public void UnclaimAll()
		{
			this.UnclaimBed();
			this.UnclaimGrave();
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x000CEDB4 File Offset: 0x000CD1B4
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
