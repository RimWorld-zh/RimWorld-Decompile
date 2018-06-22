using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200068B RID: 1675
	public class Building_Storage : Building, ISlotGroupParent, IStoreSettingsParent, IHaulDestination
	{
		// Token: 0x06002377 RID: 9079 RVA: 0x00131134 File Offset: 0x0012F534
		public Building_Storage()
		{
			this.slotGroup = new SlotGroup(this);
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06002378 RID: 9080 RVA: 0x00131150 File Offset: 0x0012F550
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06002379 RID: 9081 RVA: 0x00131168 File Offset: 0x0012F568
		public bool IgnoreStoredThingsBeauty
		{
			get
			{
				return this.def.building.ignoreStoredThingsBeauty;
			}
		}

		// Token: 0x0600237A RID: 9082 RVA: 0x00131190 File Offset: 0x0012F590
		public SlotGroup GetSlotGroup()
		{
			return this.slotGroup;
		}

		// Token: 0x0600237B RID: 9083 RVA: 0x001311AB File Offset: 0x0012F5AB
		public virtual void Notify_ReceivedThing(Thing newItem)
		{
			if (base.Faction == Faction.OfPlayer && newItem.def.storedConceptLearnOpportunity != null)
			{
				LessonAutoActivator.TeachOpportunity(newItem.def.storedConceptLearnOpportunity, OpportunityType.GoodToKnow);
			}
		}

		// Token: 0x0600237C RID: 9084 RVA: 0x001311DF File Offset: 0x0012F5DF
		public virtual void Notify_LostThing(Thing newItem)
		{
		}

		// Token: 0x0600237D RID: 9085 RVA: 0x001311E4 File Offset: 0x0012F5E4
		public virtual IEnumerable<IntVec3> AllSlotCells()
		{
			foreach (IntVec3 c in GenAdj.CellsOccupiedBy(this))
			{
				yield return c;
			}
			yield break;
		}

		// Token: 0x0600237E RID: 9086 RVA: 0x00131210 File Offset: 0x0012F610
		public List<IntVec3> AllSlotCellsList()
		{
			if (this.cachedOccupiedCells == null)
			{
				this.cachedOccupiedCells = this.AllSlotCells().ToList<IntVec3>();
			}
			return this.cachedOccupiedCells;
		}

		// Token: 0x0600237F RID: 9087 RVA: 0x00131248 File Offset: 0x0012F648
		public StorageSettings GetStoreSettings()
		{
			return this.settings;
		}

		// Token: 0x06002380 RID: 9088 RVA: 0x00131264 File Offset: 0x0012F664
		public StorageSettings GetParentStoreSettings()
		{
			return this.def.building.fixedStorageSettings;
		}

		// Token: 0x06002381 RID: 9089 RVA: 0x0013128C File Offset: 0x0012F68C
		public string SlotYielderLabel()
		{
			return this.LabelCap;
		}

		// Token: 0x06002382 RID: 9090 RVA: 0x001312A8 File Offset: 0x0012F6A8
		public bool Accepts(Thing t)
		{
			return this.settings.AllowedToAccept(t);
		}

		// Token: 0x06002383 RID: 9091 RVA: 0x001312CC File Offset: 0x0012F6CC
		public override void PostMake()
		{
			base.PostMake();
			this.settings = new StorageSettings(this);
			if (this.def.building.defaultStorageSettings != null)
			{
				this.settings.CopyFrom(this.def.building.defaultStorageSettings);
			}
		}

		// Token: 0x06002384 RID: 9092 RVA: 0x0013131C File Offset: 0x0012F71C
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			this.cachedOccupiedCells = null;
			base.SpawnSetup(map, respawningAfterLoad);
		}

		// Token: 0x06002385 RID: 9093 RVA: 0x0013132E File Offset: 0x0012F72E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", new object[]
			{
				this
			});
		}

		// Token: 0x06002386 RID: 9094 RVA: 0x00131354 File Offset: 0x0012F754
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

		// Token: 0x040013E0 RID: 5088
		public StorageSettings settings;

		// Token: 0x040013E1 RID: 5089
		public SlotGroup slotGroup;

		// Token: 0x040013E2 RID: 5090
		private List<IntVec3> cachedOccupiedCells = null;
	}
}
