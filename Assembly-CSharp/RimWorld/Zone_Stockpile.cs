using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000459 RID: 1113
	public class Zone_Stockpile : Zone, ISlotGroupParent, IStoreSettingsParent, IHaulDestination
	{
		// Token: 0x04000BDA RID: 3034
		public StorageSettings settings;

		// Token: 0x04000BDB RID: 3035
		public SlotGroup slotGroup;

		// Token: 0x04000BDC RID: 3036
		private static readonly ITab StorageTab = new ITab_Storage();

		// Token: 0x06001376 RID: 4982 RVA: 0x000A8AFA File Offset: 0x000A6EFA
		public Zone_Stockpile()
		{
			this.slotGroup = new SlotGroup(this);
		}

		// Token: 0x06001377 RID: 4983 RVA: 0x000A8B0F File Offset: 0x000A6F0F
		public Zone_Stockpile(StorageSettingsPreset preset, ZoneManager zoneManager) : base(preset.PresetName(), zoneManager)
		{
			this.settings = new StorageSettings(this);
			this.settings.SetFromPreset(preset);
			this.slotGroup = new SlotGroup(this);
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06001378 RID: 4984 RVA: 0x000A8B44 File Offset: 0x000A6F44
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06001379 RID: 4985 RVA: 0x000A8B5C File Offset: 0x000A6F5C
		public bool IgnoreStoredThingsBeauty
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x0600137A RID: 4986 RVA: 0x000A8B74 File Offset: 0x000A6F74
		protected override Color NextZoneColor
		{
			get
			{
				return ZoneColorUtility.NextStorageZoneColor();
			}
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x000A8B8E File Offset: 0x000A6F8E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", new object[]
			{
				this
			});
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x000A8BB1 File Offset: 0x000A6FB1
		public override void AddCell(IntVec3 sq)
		{
			base.AddCell(sq);
			if (this.slotGroup != null)
			{
				this.slotGroup.Notify_AddedCell(sq);
			}
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x000A8BD2 File Offset: 0x000A6FD2
		public override void RemoveCell(IntVec3 sq)
		{
			base.RemoveCell(sq);
			this.slotGroup.Notify_LostCell(sq);
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x000A8BE8 File Offset: 0x000A6FE8
		public override void PostDeregister()
		{
			base.PostDeregister();
			BillUtility.Notify_ZoneStockpileRemoved(this);
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x000A8BF8 File Offset: 0x000A6FF8
		public override IEnumerable<InspectTabBase> GetInspectTabs()
		{
			yield return Zone_Stockpile.StorageTab;
			yield break;
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x000A8C1C File Offset: 0x000A701C
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

		// Token: 0x06001381 RID: 4993 RVA: 0x000A8C48 File Offset: 0x000A7048
		public override IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			yield return DesignatorUtility.FindAllowedDesignator<Designator_ZoneAddStockpile_Expand>();
			yield break;
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x000A8C6C File Offset: 0x000A706C
		public SlotGroup GetSlotGroup()
		{
			return this.slotGroup;
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x000A8C88 File Offset: 0x000A7088
		public IEnumerable<IntVec3> AllSlotCells()
		{
			for (int i = 0; i < this.cells.Count; i++)
			{
				yield return this.cells[i];
			}
			yield break;
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x000A8CB4 File Offset: 0x000A70B4
		public List<IntVec3> AllSlotCellsList()
		{
			return this.cells;
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x000A8CD0 File Offset: 0x000A70D0
		public StorageSettings GetParentStoreSettings()
		{
			return null;
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x000A8CE8 File Offset: 0x000A70E8
		public StorageSettings GetStoreSettings()
		{
			return this.settings;
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x000A8D04 File Offset: 0x000A7104
		public bool Accepts(Thing t)
		{
			return this.settings.AllowedToAccept(t);
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x000A8D28 File Offset: 0x000A7128
		public string SlotYielderLabel()
		{
			return this.label;
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x000A8D43 File Offset: 0x000A7143
		public void Notify_ReceivedThing(Thing newItem)
		{
			if (newItem.def.storedConceptLearnOpportunity != null)
			{
				LessonAutoActivator.TeachOpportunity(newItem.def.storedConceptLearnOpportunity, OpportunityType.GoodToKnow);
			}
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x000A8D67 File Offset: 0x000A7167
		public void Notify_LostThing(Thing newItem)
		{
		}
	}
}
