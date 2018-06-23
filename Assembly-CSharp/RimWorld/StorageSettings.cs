using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000694 RID: 1684
	public class StorageSettings : IExposable
	{
		// Token: 0x040013F5 RID: 5109
		public IStoreSettingsParent owner = null;

		// Token: 0x040013F6 RID: 5110
		public ThingFilter filter;

		// Token: 0x040013F7 RID: 5111
		[LoadAlias("priority")]
		private StoragePriority priorityInt = StoragePriority.Normal;

		// Token: 0x060023B5 RID: 9141 RVA: 0x00132722 File Offset: 0x00130B22
		public StorageSettings()
		{
			this.filter = new ThingFilter(new Action(this.TryNotifyChanged));
		}

		// Token: 0x060023B6 RID: 9142 RVA: 0x00132750 File Offset: 0x00130B50
		public StorageSettings(IStoreSettingsParent owner) : this()
		{
			this.owner = owner;
			if (owner != null)
			{
				StorageSettings parentStoreSettings = owner.GetParentStoreSettings();
				if (parentStoreSettings != null)
				{
					this.priorityInt = parentStoreSettings.priorityInt;
				}
			}
		}

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x060023B7 RID: 9143 RVA: 0x0013278C File Offset: 0x00130B8C
		private IHaulDestination HaulDestinationOwner
		{
			get
			{
				return this.owner as IHaulDestination;
			}
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x060023B8 RID: 9144 RVA: 0x001327AC File Offset: 0x00130BAC
		private ISlotGroupParent SlotGroupParentOwner
		{
			get
			{
				return this.owner as ISlotGroupParent;
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x060023B9 RID: 9145 RVA: 0x001327CC File Offset: 0x00130BCC
		// (set) Token: 0x060023BA RID: 9146 RVA: 0x001327E8 File Offset: 0x00130BE8
		public StoragePriority Priority
		{
			get
			{
				return this.priorityInt;
			}
			set
			{
				this.priorityInt = value;
				if (Current.ProgramState == ProgramState.Playing && this.HaulDestinationOwner != null && this.HaulDestinationOwner.Map != null)
				{
					this.HaulDestinationOwner.Map.haulDestinationManager.Notify_HaulDestinationChangedPriority();
				}
				if (Current.ProgramState == ProgramState.Playing && this.SlotGroupParentOwner != null && this.SlotGroupParentOwner.Map != null)
				{
					this.SlotGroupParentOwner.Map.listerHaulables.RecalcAllInCells(this.SlotGroupParentOwner.AllSlotCells());
				}
			}
		}

		// Token: 0x060023BB RID: 9147 RVA: 0x0013287E File Offset: 0x00130C7E
		public void ExposeData()
		{
			Scribe_Values.Look<StoragePriority>(ref this.priorityInt, "priority", StoragePriority.Unstored, false);
			Scribe_Deep.Look<ThingFilter>(ref this.filter, "filter", new object[0]);
		}

		// Token: 0x060023BC RID: 9148 RVA: 0x001328A9 File Offset: 0x00130CA9
		public void SetFromPreset(StorageSettingsPreset preset)
		{
			this.filter.SetFromPreset(preset);
			this.TryNotifyChanged();
		}

		// Token: 0x060023BD RID: 9149 RVA: 0x001328BE File Offset: 0x00130CBE
		public void CopyFrom(StorageSettings other)
		{
			this.Priority = other.Priority;
			this.filter.CopyAllowancesFrom(other.filter);
			this.TryNotifyChanged();
		}

		// Token: 0x060023BE RID: 9150 RVA: 0x001328E4 File Offset: 0x00130CE4
		public bool AllowedToAccept(Thing t)
		{
			bool result;
			if (!this.filter.Allows(t))
			{
				result = false;
			}
			else
			{
				if (this.owner != null)
				{
					StorageSettings parentStoreSettings = this.owner.GetParentStoreSettings();
					if (parentStoreSettings != null && !parentStoreSettings.AllowedToAccept(t))
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x060023BF RID: 9151 RVA: 0x00132944 File Offset: 0x00130D44
		public bool AllowedToAccept(ThingDef t)
		{
			bool result;
			if (!this.filter.Allows(t))
			{
				result = false;
			}
			else
			{
				if (this.owner != null)
				{
					StorageSettings parentStoreSettings = this.owner.GetParentStoreSettings();
					if (parentStoreSettings != null && !parentStoreSettings.AllowedToAccept(t))
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x060023C0 RID: 9152 RVA: 0x001329A4 File Offset: 0x00130DA4
		private void TryNotifyChanged()
		{
			if (this.owner != null && this.SlotGroupParentOwner != null && this.SlotGroupParentOwner.GetSlotGroup() != null && this.SlotGroupParentOwner.Map != null)
			{
				this.SlotGroupParentOwner.Map.listerHaulables.Notify_SlotGroupChanged(this.SlotGroupParentOwner.GetSlotGroup());
			}
		}
	}
}
