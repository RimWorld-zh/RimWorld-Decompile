using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200045B RID: 1115
	public class Zone_Stockpile : Zone, ISlotGroupParent, IStoreSettingsParent, IHaulDestination
	{
		// Token: 0x0600137C RID: 4988 RVA: 0x000A878E File Offset: 0x000A6B8E
		public Zone_Stockpile()
		{
			this.slotGroup = new SlotGroup(this);
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x000A87A3 File Offset: 0x000A6BA3
		public Zone_Stockpile(StorageSettingsPreset preset, ZoneManager zoneManager) : base(preset.PresetName(), zoneManager)
		{
			this.settings = new StorageSettings(this);
			this.settings.SetFromPreset(preset);
			this.slotGroup = new SlotGroup(this);
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x0600137E RID: 4990 RVA: 0x000A87D8 File Offset: 0x000A6BD8
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x0600137F RID: 4991 RVA: 0x000A87F0 File Offset: 0x000A6BF0
		public bool IgnoreStoredThingsBeauty
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06001380 RID: 4992 RVA: 0x000A8808 File Offset: 0x000A6C08
		protected override Color NextZoneColor
		{
			get
			{
				return ZoneColorUtility.NextStorageZoneColor();
			}
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x000A8822 File Offset: 0x000A6C22
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", new object[]
			{
				this
			});
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x000A8845 File Offset: 0x000A6C45
		public override void AddCell(IntVec3 sq)
		{
			base.AddCell(sq);
			if (this.slotGroup != null)
			{
				this.slotGroup.Notify_AddedCell(sq);
			}
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x000A8866 File Offset: 0x000A6C66
		public override void RemoveCell(IntVec3 sq)
		{
			base.RemoveCell(sq);
			this.slotGroup.Notify_LostCell(sq);
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x000A887C File Offset: 0x000A6C7C
		public override void PostDeregister()
		{
			base.PostDeregister();
			BillUtility.Notify_ZoneStockpileRemoved(this);
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x000A888C File Offset: 0x000A6C8C
		public override IEnumerable<InspectTabBase> GetInspectTabs()
		{
			yield return Zone_Stockpile.StorageTab;
			yield break;
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x000A88B0 File Offset: 0x000A6CB0
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo g in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return g;
			}
			foreach (Gizmo g2 in StorageSettingsClipboard.CopyPasteGizmosFor(this.settings))
			{
				yield return g2;
			}
			yield break;
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x000A88DC File Offset: 0x000A6CDC
		public override IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			yield return DesignatorUtility.FindAllowedDesignator<Designator_ZoneAddStockpile_Expand>();
			yield break;
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x000A8900 File Offset: 0x000A6D00
		public SlotGroup GetSlotGroup()
		{
			return this.slotGroup;
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x000A891C File Offset: 0x000A6D1C
		public IEnumerable<IntVec3> AllSlotCells()
		{
			for (int i = 0; i < this.cells.Count; i++)
			{
				yield return this.cells[i];
			}
			yield break;
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x000A8948 File Offset: 0x000A6D48
		public List<IntVec3> AllSlotCellsList()
		{
			return this.cells;
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x000A8964 File Offset: 0x000A6D64
		public StorageSettings GetParentStoreSettings()
		{
			return null;
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x000A897C File Offset: 0x000A6D7C
		public StorageSettings GetStoreSettings()
		{
			return this.settings;
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x000A8998 File Offset: 0x000A6D98
		public bool Accepts(Thing t)
		{
			return this.settings.AllowedToAccept(t);
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x000A89BC File Offset: 0x000A6DBC
		public string SlotYielderLabel()
		{
			return this.label;
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x000A89D7 File Offset: 0x000A6DD7
		public void Notify_ReceivedThing(Thing newItem)
		{
			if (newItem.def.storedConceptLearnOpportunity != null)
			{
				LessonAutoActivator.TeachOpportunity(newItem.def.storedConceptLearnOpportunity, OpportunityType.GoodToKnow);
			}
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x000A89FB File Offset: 0x000A6DFB
		public void Notify_LostThing(Thing newItem)
		{
		}

		// Token: 0x04000BDA RID: 3034
		public StorageSettings settings;

		// Token: 0x04000BDB RID: 3035
		public SlotGroup slotGroup;

		// Token: 0x04000BDC RID: 3036
		private static readonly ITab StorageTab = new ITab_Storage();
	}
}
