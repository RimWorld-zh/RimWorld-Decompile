using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000517 RID: 1303
	public class Pawn_Ownership : IExposable
	{
		// Token: 0x04000DF1 RID: 3569
		private Pawn pawn;

		// Token: 0x04000DF2 RID: 3570
		private Building_Bed intOwnedBed;

		// Token: 0x0600179A RID: 6042 RVA: 0x000CED20 File Offset: 0x000CD120
		public Pawn_Ownership(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x0600179B RID: 6043 RVA: 0x000CED30 File Offset: 0x000CD130
		// (set) Token: 0x0600179C RID: 6044 RVA: 0x000CED4B File Offset: 0x000CD14B
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
		// (get) Token: 0x0600179D RID: 6045 RVA: 0x000CED70 File Offset: 0x000CD170
		// (set) Token: 0x0600179E RID: 6046 RVA: 0x000CED8A File Offset: 0x000CD18A
		public Building_Grave AssignedGrave { get; private set; }

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x0600179F RID: 6047 RVA: 0x000CED94 File Offset: 0x000CD194
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

		// Token: 0x060017A0 RID: 6048 RVA: 0x000CEDF4 File Offset: 0x000CD1F4
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

		// Token: 0x060017A1 RID: 6049 RVA: 0x000CEE88 File Offset: 0x000CD288
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

		// Token: 0x060017A2 RID: 6050 RVA: 0x000CEF46 File Offset: 0x000CD346
		public void UnclaimBed()
		{
			if (this.OwnedBed != null)
			{
				this.OwnedBed.owners.Remove(this.pawn);
				this.OwnedBed = null;
			}
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x000CEF74 File Offset: 0x000CD374
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

		// Token: 0x060017A4 RID: 6052 RVA: 0x000CEFD8 File Offset: 0x000CD3D8
		public void UnclaimGrave()
		{
			if (this.AssignedGrave != null)
			{
				this.AssignedGrave.assignedPawn = null;
				this.AssignedGrave.GetStoreSettings().Priority = StoragePriority.Important;
				this.AssignedGrave = null;
			}
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x000CF00C File Offset: 0x000CD40C
		public void UnclaimAll()
		{
			this.UnclaimBed();
			this.UnclaimGrave();
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x000CF01C File Offset: 0x000CD41C
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
