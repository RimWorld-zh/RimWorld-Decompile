using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000696 RID: 1686
	public class StorageSettings : IExposable
	{
		// Token: 0x040013F9 RID: 5113
		public IStoreSettingsParent owner = null;

		// Token: 0x040013FA RID: 5114
		public ThingFilter filter;

		// Token: 0x040013FB RID: 5115
		[LoadAlias("priority")]
		private StoragePriority priorityInt = StoragePriority.Normal;

		// Token: 0x060023B8 RID: 9144 RVA: 0x00132ADA File Offset: 0x00130EDA
		public StorageSettings()
		{
			this.filter = new ThingFilter(new Action(this.TryNotifyChanged));
		}

		// Token: 0x060023B9 RID: 9145 RVA: 0x00132B08 File Offset: 0x00130F08
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
		// (get) Token: 0x060023BA RID: 9146 RVA: 0x00132B44 File Offset: 0x00130F44
		private IHaulDestination HaulDestinationOwner
		{
			get
			{
				return this.owner as IHaulDestination;
			}
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x060023BB RID: 9147 RVA: 0x00132B64 File Offset: 0x00130F64
		private ISlotGroupParent SlotGroupParentOwner
		{
			get
			{
				return this.owner as ISlotGroupParent;
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x060023BC RID: 9148 RVA: 0x00132B84 File Offset: 0x00130F84
		// (set) Token: 0x060023BD RID: 9149 RVA: 0x00132BA0 File Offset: 0x00130FA0
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

		// Token: 0x060023BE RID: 9150 RVA: 0x00132C36 File Offset: 0x00131036
		public void ExposeData()
		{
			Scribe_Values.Look<StoragePriority>(ref this.priorityInt, "priority", StoragePriority.Unstored, false);
			Scribe_Deep.Look<ThingFilter>(ref this.filter, "filter", new object[0]);
		}

		// Token: 0x060023BF RID: 9151 RVA: 0x00132C61 File Offset: 0x00131061
		public void SetFromPreset(StorageSettingsPreset preset)
		{
			this.filter.SetFromPreset(preset);
			this.TryNotifyChanged();
		}

		// Token: 0x060023C0 RID: 9152 RVA: 0x00132C76 File Offset: 0x00131076
		public void CopyFrom(StorageSettings other)
		{
			this.Priority = other.Priority;
			this.filter.CopyAllowancesFrom(other.filter);
			this.TryNotifyChanged();
		}

		// Token: 0x060023C1 RID: 9153 RVA: 0x00132C9C File Offset: 0x0013109C
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

		// Token: 0x060023C2 RID: 9154 RVA: 0x00132CFC File Offset: 0x001310FC
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

		// Token: 0x060023C3 RID: 9155 RVA: 0x00132D5C File Offset: 0x0013115C
		private void TryNotifyChanged()
		{
			if (this.owner != null && this.SlotGroupParentOwner != null && this.SlotGroupParentOwner.GetSlotGroup() != null && this.SlotGroupParentOwner.Map != null)
			{
				this.SlotGroupParentOwner.Map.listerHaulables.Notify_SlotGroupChanged(this.SlotGroupParentOwner.GetSlotGroup());
			}
		}
	}
}
