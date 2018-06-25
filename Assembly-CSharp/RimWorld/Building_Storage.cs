using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200068D RID: 1677
	public class Building_Storage : Building, ISlotGroupParent, IStoreSettingsParent, IHaulDestination
	{
		// Token: 0x040013E4 RID: 5092
		public StorageSettings settings;

		// Token: 0x040013E5 RID: 5093
		public SlotGroup slotGroup;

		// Token: 0x040013E6 RID: 5094
		private List<IntVec3> cachedOccupiedCells = null;

		// Token: 0x0600237A RID: 9082 RVA: 0x001314EC File Offset: 0x0012F8EC
		public Building_Storage()
		{
			this.slotGroup = new SlotGroup(this);
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x0600237B RID: 9083 RVA: 0x00131508 File Offset: 0x0012F908
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x0600237C RID: 9084 RVA: 0x00131520 File Offset: 0x0012F920
		public bool IgnoreStoredThingsBeauty
		{
			get
			{
				return this.def.building.ignoreStoredThingsBeauty;
			}
		}

		// Token: 0x0600237D RID: 9085 RVA: 0x00131548 File Offset: 0x0012F948
		public SlotGroup GetSlotGroup()
		{
			return this.slotGroup;
		}

		// Token: 0x0600237E RID: 9086 RVA: 0x00131563 File Offset: 0x0012F963
		public virtual void Notify_ReceivedThing(Thing newItem)
		{
			if (base.Faction == Faction.OfPlayer && newItem.def.storedConceptLearnOpportunity != null)
			{
				LessonAutoActivator.TeachOpportunity(newItem.def.storedConceptLearnOpportunity, OpportunityType.GoodToKnow);
			}
		}

		// Token: 0x0600237F RID: 9087 RVA: 0x00131597 File Offset: 0x0012F997
		public virtual void Notify_LostThing(Thing newItem)
		{
		}

		// Token: 0x06002380 RID: 9088 RVA: 0x0013159C File Offset: 0x0012F99C
		public virtual IEnumerable<IntVec3> AllSlotCells()
		{
			foreach (IntVec3 c in GenAdj.CellsOccupiedBy(this))
			{
				yield return c;
			}
			yield break;
		}

		// Token: 0x06002381 RID: 9089 RVA: 0x001315C8 File Offset: 0x0012F9C8
		public List<IntVec3> AllSlotCellsList()
		{
			if (this.cachedOccupiedCells == null)
			{
				this.cachedOccupiedCells = this.AllSlotCells().ToList<IntVec3>();
			}
			return this.cachedOccupiedCells;
		}

		// Token: 0x06002382 RID: 9090 RVA: 0x00131600 File Offset: 0x0012FA00
		public StorageSettings GetStoreSettings()
		{
			return this.settings;
		}

		// Token: 0x06002383 RID: 9091 RVA: 0x0013161C File Offset: 0x0012FA1C
		public StorageSettings GetParentStoreSettings()
		{
			return this.def.building.fixedStorageSettings;
		}

		// Token: 0x06002384 RID: 9092 RVA: 0x00131644 File Offset: 0x0012FA44
		public string SlotYielderLabel()
		{
			return this.LabelCap;
		}

		// Token: 0x06002385 RID: 9093 RVA: 0x00131660 File Offset: 0x0012FA60
		public bool Accepts(Thing t)
		{
			return this.settings.AllowedToAccept(t);
		}

		// Token: 0x06002386 RID: 9094 RVA: 0x00131684 File Offset: 0x0012FA84
		public override void PostMake()
		{
			base.PostMake();
			this.settings = new StorageSettings(this);
			if (this.def.building.defaultStorageSettings != null)
			{
				this.settings.CopyFrom(this.def.building.defaultStorageSettings);
			}
		}

		// Token: 0x06002387 RID: 9095 RVA: 0x001316D4 File Offset: 0x0012FAD4
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			this.cachedOccupiedCells = null;
			base.SpawnSetup(map, respawningAfterLoad);
		}

		// Token: 0x06002388 RID: 9096 RVA: 0x001316E6 File Offset: 0x0012FAE6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", new object[]
			{
				this
			});
		}

		// Token: 0x06002389 RID: 9097 RVA: 0x0013170C File Offset: 0x0012FB0C
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
	}
}
