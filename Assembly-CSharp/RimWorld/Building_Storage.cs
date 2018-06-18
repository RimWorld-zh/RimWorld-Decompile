using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200068F RID: 1679
	public class Building_Storage : Building, ISlotGroupParent, IStoreSettingsParent, IHaulDestination
	{
		// Token: 0x0600237F RID: 9087 RVA: 0x00130FEC File Offset: 0x0012F3EC
		public Building_Storage()
		{
			this.slotGroup = new SlotGroup(this);
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06002380 RID: 9088 RVA: 0x00131008 File Offset: 0x0012F408
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06002381 RID: 9089 RVA: 0x00131020 File Offset: 0x0012F420
		public bool IgnoreStoredThingsBeauty
		{
			get
			{
				return this.def.building.ignoreStoredThingsBeauty;
			}
		}

		// Token: 0x06002382 RID: 9090 RVA: 0x00131048 File Offset: 0x0012F448
		public SlotGroup GetSlotGroup()
		{
			return this.slotGroup;
		}

		// Token: 0x06002383 RID: 9091 RVA: 0x00131063 File Offset: 0x0012F463
		public virtual void Notify_ReceivedThing(Thing newItem)
		{
			if (base.Faction == Faction.OfPlayer && newItem.def.storedConceptLearnOpportunity != null)
			{
				LessonAutoActivator.TeachOpportunity(newItem.def.storedConceptLearnOpportunity, OpportunityType.GoodToKnow);
			}
		}

		// Token: 0x06002384 RID: 9092 RVA: 0x00131097 File Offset: 0x0012F497
		public virtual void Notify_LostThing(Thing newItem)
		{
		}

		// Token: 0x06002385 RID: 9093 RVA: 0x0013109C File Offset: 0x0012F49C
		public virtual IEnumerable<IntVec3> AllSlotCells()
		{
			foreach (IntVec3 c in GenAdj.CellsOccupiedBy(this))
			{
				yield return c;
			}
			yield break;
		}

		// Token: 0x06002386 RID: 9094 RVA: 0x001310C8 File Offset: 0x0012F4C8
		public List<IntVec3> AllSlotCellsList()
		{
			if (this.cachedOccupiedCells == null)
			{
				this.cachedOccupiedCells = this.AllSlotCells().ToList<IntVec3>();
			}
			return this.cachedOccupiedCells;
		}

		// Token: 0x06002387 RID: 9095 RVA: 0x00131100 File Offset: 0x0012F500
		public StorageSettings GetStoreSettings()
		{
			return this.settings;
		}

		// Token: 0x06002388 RID: 9096 RVA: 0x0013111C File Offset: 0x0012F51C
		public StorageSettings GetParentStoreSettings()
		{
			return this.def.building.fixedStorageSettings;
		}

		// Token: 0x06002389 RID: 9097 RVA: 0x00131144 File Offset: 0x0012F544
		public string SlotYielderLabel()
		{
			return this.LabelCap;
		}

		// Token: 0x0600238A RID: 9098 RVA: 0x00131160 File Offset: 0x0012F560
		public bool Accepts(Thing t)
		{
			return this.settings.AllowedToAccept(t);
		}

		// Token: 0x0600238B RID: 9099 RVA: 0x00131184 File Offset: 0x0012F584
		public override void PostMake()
		{
			base.PostMake();
			this.settings = new StorageSettings(this);
			if (this.def.building.defaultStorageSettings != null)
			{
				this.settings.CopyFrom(this.def.building.defaultStorageSettings);
			}
		}

		// Token: 0x0600238C RID: 9100 RVA: 0x001311D4 File Offset: 0x0012F5D4
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			this.cachedOccupiedCells = null;
			base.SpawnSetup(map, respawningAfterLoad);
		}

		// Token: 0x0600238D RID: 9101 RVA: 0x001311E6 File Offset: 0x0012F5E6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", new object[]
			{
				this
			});
		}

		// Token: 0x0600238E RID: 9102 RVA: 0x0013120C File Offset: 0x0012F60C
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

		// Token: 0x040013E2 RID: 5090
		public StorageSettings settings;

		// Token: 0x040013E3 RID: 5091
		public SlotGroup slotGroup;

		// Token: 0x040013E4 RID: 5092
		private List<IntVec3> cachedOccupiedCells = null;
	}
}
