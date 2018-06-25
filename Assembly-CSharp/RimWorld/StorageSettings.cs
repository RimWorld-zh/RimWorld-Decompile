using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000696 RID: 1686
	public class StorageSettings : IExposable
	{
		// Token: 0x040013F5 RID: 5109
		public IStoreSettingsParent owner = null;

		// Token: 0x040013F6 RID: 5110
		public ThingFilter filter;

		// Token: 0x040013F7 RID: 5111
		[LoadAlias("priority")]
		private StoragePriority priorityInt = StoragePriority.Normal;

		// Token: 0x060023B9 RID: 9145 RVA: 0x00132872 File Offset: 0x00130C72
		public StorageSettings()
		{
			this.filter = new ThingFilter(new Action(this.TryNotifyChanged));
		}

		// Token: 0x060023BA RID: 9146 RVA: 0x001328A0 File Offset: 0x00130CA0
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
		// (get) Token: 0x060023BB RID: 9147 RVA: 0x001328DC File Offset: 0x00130CDC
		private IHaulDestination HaulDestinationOwner
		{
			get
			{
				return this.owner as IHaulDestination;
			}
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x060023BC RID: 9148 RVA: 0x001328FC File Offset: 0x00130CFC
		private ISlotGroupParent SlotGroupParentOwner
		{
			get
			{
				return this.owner as ISlotGroupParent;
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x060023BD RID: 9149 RVA: 0x0013291C File Offset: 0x00130D1C
		// (set) Token: 0x060023BE RID: 9150 RVA: 0x00132938 File Offset: 0x00130D38
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

		// Token: 0x060023BF RID: 9151 RVA: 0x001329CE File Offset: 0x00130DCE
		public void ExposeData()
		{
			Scribe_Values.Look<StoragePriority>(ref this.priorityInt, "priority", StoragePriority.Unstored, false);
			Scribe_Deep.Look<ThingFilter>(ref this.filter, "filter", new object[0]);
		}

		// Token: 0x060023C0 RID: 9152 RVA: 0x001329F9 File Offset: 0x00130DF9
		public void SetFromPreset(StorageSettingsPreset preset)
		{
			this.filter.SetFromPreset(preset);
			this.TryNotifyChanged();
		}

		// Token: 0x060023C1 RID: 9153 RVA: 0x00132A0E File Offset: 0x00130E0E
		public void CopyFrom(StorageSettings other)
		{
			this.Priority = other.Priority;
			this.filter.CopyAllowancesFrom(other.filter);
			this.TryNotifyChanged();
		}

		// Token: 0x060023C2 RID: 9154 RVA: 0x00132A34 File Offset: 0x00130E34
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

		// Token: 0x060023C3 RID: 9155 RVA: 0x00132A94 File Offset: 0x00130E94
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

		// Token: 0x060023C4 RID: 9156 RVA: 0x00132AF4 File Offset: 0x00130EF4
		private void TryNotifyChanged()
		{
			if (this.owner != null && this.SlotGroupParentOwner != null && this.SlotGroupParentOwner.GetSlotGroup() != null && this.SlotGroupParentOwner.Map != null)
			{
				this.SlotGroupParentOwner.Map.listerHaulables.Notify_SlotGroupChanged(this.SlotGroupParentOwner.GetSlotGroup());
			}
		}
	}
}
