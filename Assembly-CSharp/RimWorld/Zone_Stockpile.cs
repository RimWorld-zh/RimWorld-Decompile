using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000459 RID: 1113
	public class Zone_Stockpile : Zone, ISlotGroupParent, IStoreSettingsParent, IHaulDestination
	{
		// Token: 0x04000BD7 RID: 3031
		public StorageSettings settings;

		// Token: 0x04000BD8 RID: 3032
		public SlotGroup slotGroup;

		// Token: 0x04000BD9 RID: 3033
		private static readonly ITab StorageTab = new ITab_Storage();

		// Token: 0x06001377 RID: 4983 RVA: 0x000A88FA File Offset: 0x000A6CFA
		public Zone_Stockpile()
		{
			this.slotGroup = new SlotGroup(this);
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x000A890F File Offset: 0x000A6D0F
		public Zone_Stockpile(StorageSettingsPreset preset, ZoneManager zoneManager) : base(preset.PresetName(), zoneManager)
		{
			this.settings = new StorageSettings(this);
			this.settings.SetFromPreset(preset);
			this.slotGroup = new SlotGroup(this);
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06001379 RID: 4985 RVA: 0x000A8944 File Offset: 0x000A6D44
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x0600137A RID: 4986 RVA: 0x000A895C File Offset: 0x000A6D5C
		public bool IgnoreStoredThingsBeauty
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x0600137B RID: 4987 RVA: 0x000A8974 File Offset: 0x000A6D74
		protected override Color NextZoneColor
		{
			get
			{
				return ZoneColorUtility.NextStorageZoneColor();
			}
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x000A898E File Offset: 0x000A6D8E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", new object[]
			{
				this
			});
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x000A89B1 File Offset: 0x000A6DB1
		public override void AddCell(IntVec3 sq)
		{
			base.AddCell(sq);
			if (this.slotGroup != null)
			{
				this.slotGroup.Notify_AddedCell(sq);
			}
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x000A89D2 File Offset: 0x000A6DD2
		public override void RemoveCell(IntVec3 sq)
		{
			base.RemoveCell(sq);
			this.slotGroup.Notify_LostCell(sq);
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x000A89E8 File Offset: 0x000A6DE8
		public override void PostDeregister()
		{
			base.PostDeregister();
			BillUtility.Notify_ZoneStockpileRemoved(this);
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x000A89F8 File Offset: 0x000A6DF8
		public override IEnumerable<InspectTabBase> GetInspectTabs()
		{
			yield return Zone_Stockpile.StorageTab;
			yield break;
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x000A8A1C File Offset: 0x000A6E1C
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

		// Token: 0x06001382 RID: 4994 RVA: 0x000A8A48 File Offset: 0x000A6E48
		public override IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			yield return DesignatorUtility.FindAllowedDesignator<Designator_ZoneAddStockpile_Expand>();
			yield break;
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x000A8A6C File Offset: 0x000A6E6C
		public SlotGroup GetSlotGroup()
		{
			return this.slotGroup;
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x000A8A88 File Offset: 0x000A6E88
		public IEnumerable<IntVec3> AllSlotCells()
		{
			for (int i = 0; i < this.cells.Count; i++)
			{
				yield return this.cells[i];
			}
			yield break;
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x000A8AB4 File Offset: 0x000A6EB4
		public List<IntVec3> AllSlotCellsList()
		{
			return this.cells;
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x000A8AD0 File Offset: 0x000A6ED0
		public StorageSettings GetParentStoreSettings()
		{
			return null;
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x000A8AE8 File Offset: 0x000A6EE8
		public StorageSettings GetStoreSettings()
		{
			return this.settings;
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x000A8B04 File Offset: 0x000A6F04
		public bool Accepts(Thing t)
		{
			return this.settings.AllowedToAccept(t);
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x000A8B28 File Offset: 0x000A6F28
		public string SlotYielderLabel()
		{
			return this.label;
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x000A8B43 File Offset: 0x000A6F43
		public void Notify_ReceivedThing(Thing newItem)
		{
			if (newItem.def.storedConceptLearnOpportunity != null)
			{
				LessonAutoActivator.TeachOpportunity(newItem.def.storedConceptLearnOpportunity, OpportunityType.GoodToKnow);
			}
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x000A8B67 File Offset: 0x000A6F67
		public void Notify_LostThing(Thing newItem)
		{
		}
	}
}
