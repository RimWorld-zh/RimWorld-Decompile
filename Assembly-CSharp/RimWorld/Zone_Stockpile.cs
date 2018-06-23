using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000457 RID: 1111
	public class Zone_Stockpile : Zone, ISlotGroupParent, IStoreSettingsParent, IHaulDestination
	{
		// Token: 0x04000BD7 RID: 3031
		public StorageSettings settings;

		// Token: 0x04000BD8 RID: 3032
		public SlotGroup slotGroup;

		// Token: 0x04000BD9 RID: 3033
		private static readonly ITab StorageTab = new ITab_Storage();

		// Token: 0x06001373 RID: 4979 RVA: 0x000A87AA File Offset: 0x000A6BAA
		public Zone_Stockpile()
		{
			this.slotGroup = new SlotGroup(this);
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x000A87BF File Offset: 0x000A6BBF
		public Zone_Stockpile(StorageSettingsPreset preset, ZoneManager zoneManager) : base(preset.PresetName(), zoneManager)
		{
			this.settings = new StorageSettings(this);
			this.settings.SetFromPreset(preset);
			this.slotGroup = new SlotGroup(this);
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06001375 RID: 4981 RVA: 0x000A87F4 File Offset: 0x000A6BF4
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06001376 RID: 4982 RVA: 0x000A880C File Offset: 0x000A6C0C
		public bool IgnoreStoredThingsBeauty
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06001377 RID: 4983 RVA: 0x000A8824 File Offset: 0x000A6C24
		protected override Color NextZoneColor
		{
			get
			{
				return ZoneColorUtility.NextStorageZoneColor();
			}
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x000A883E File Offset: 0x000A6C3E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", new object[]
			{
				this
			});
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x000A8861 File Offset: 0x000A6C61
		public override void AddCell(IntVec3 sq)
		{
			base.AddCell(sq);
			if (this.slotGroup != null)
			{
				this.slotGroup.Notify_AddedCell(sq);
			}
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x000A8882 File Offset: 0x000A6C82
		public override void RemoveCell(IntVec3 sq)
		{
			base.RemoveCell(sq);
			this.slotGroup.Notify_LostCell(sq);
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x000A8898 File Offset: 0x000A6C98
		public override void PostDeregister()
		{
			base.PostDeregister();
			BillUtility.Notify_ZoneStockpileRemoved(this);
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x000A88A8 File Offset: 0x000A6CA8
		public override IEnumerable<InspectTabBase> GetInspectTabs()
		{
			yield return Zone_Stockpile.StorageTab;
			yield break;
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x000A88CC File Offset: 0x000A6CCC
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

		// Token: 0x0600137E RID: 4990 RVA: 0x000A88F8 File Offset: 0x000A6CF8
		public override IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			yield return DesignatorUtility.FindAllowedDesignator<Designator_ZoneAddStockpile_Expand>();
			yield break;
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x000A891C File Offset: 0x000A6D1C
		public SlotGroup GetSlotGroup()
		{
			return this.slotGroup;
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x000A8938 File Offset: 0x000A6D38
		public IEnumerable<IntVec3> AllSlotCells()
		{
			for (int i = 0; i < this.cells.Count; i++)
			{
				yield return this.cells[i];
			}
			yield break;
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x000A8964 File Offset: 0x000A6D64
		public List<IntVec3> AllSlotCellsList()
		{
			return this.cells;
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x000A8980 File Offset: 0x000A6D80
		public StorageSettings GetParentStoreSettings()
		{
			return null;
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x000A8998 File Offset: 0x000A6D98
		public StorageSettings GetStoreSettings()
		{
			return this.settings;
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x000A89B4 File Offset: 0x000A6DB4
		public bool Accepts(Thing t)
		{
			return this.settings.AllowedToAccept(t);
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x000A89D8 File Offset: 0x000A6DD8
		public string SlotYielderLabel()
		{
			return this.label;
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x000A89F3 File Offset: 0x000A6DF3
		public void Notify_ReceivedThing(Thing newItem)
		{
			if (newItem.def.storedConceptLearnOpportunity != null)
			{
				LessonAutoActivator.TeachOpportunity(newItem.def.storedConceptLearnOpportunity, OpportunityType.GoodToKnow);
			}
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x000A8A17 File Offset: 0x000A6E17
		public void Notify_LostThing(Thing newItem)
		{
		}
	}
}
