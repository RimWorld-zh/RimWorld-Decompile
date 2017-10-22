using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class Building_Storage : Building, IStoreSettingsParent, ISlotGroupParent
	{
		public SlotGroup slotGroup;

		public StorageSettings settings;

		private List<IntVec3> cachedOccupiedCells;

		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		public bool IgnoreStoredThingsBeauty
		{
			get
			{
				return base.def.building.ignoreStoredThingsBeauty;
			}
		}

		public SlotGroup GetSlotGroup()
		{
			return this.slotGroup;
		}

		public virtual void Notify_ReceivedThing(Thing newItem)
		{
			if (base.Faction == Faction.OfPlayer && newItem.def.storedConceptLearnOpportunity != null)
			{
				LessonAutoActivator.TeachOpportunity(newItem.def.storedConceptLearnOpportunity, OpportunityType.GoodToKnow);
			}
		}

		public virtual void Notify_LostThing(Thing newItem)
		{
		}

		public virtual IEnumerable<IntVec3> AllSlotCells()
		{
			foreach (IntVec3 item in GenAdj.CellsOccupiedBy(this))
			{
				yield return item;
			}
		}

		public List<IntVec3> AllSlotCellsList()
		{
			if (this.cachedOccupiedCells == null)
			{
				this.cachedOccupiedCells = this.AllSlotCells().ToList();
			}
			return this.cachedOccupiedCells;
		}

		public StorageSettings GetStoreSettings()
		{
			return this.settings;
		}

		public StorageSettings GetParentStoreSettings()
		{
			return base.def.building.fixedStorageSettings;
		}

		public string SlotYielderLabel()
		{
			return this.LabelCap;
		}

		public override void PostMake()
		{
			base.PostMake();
			this.settings = new StorageSettings(this);
			if (base.def.building.defaultStorageSettings != null)
			{
				this.settings.CopyFrom(base.def.building.defaultStorageSettings);
			}
		}

		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.cachedOccupiedCells = this.AllSlotCells().ToList();
			this.slotGroup = new SlotGroup(this);
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", new object[1]
			{
				this
			});
		}

		public override void DeSpawn()
		{
			if (this.slotGroup != null)
			{
				this.slotGroup.Notify_ParentDestroying();
			}
			base.DeSpawn();
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			foreach (Gizmo item in StorageSettingsClipboard.CopyPasteGizmosFor(this.settings))
			{
				yield return item;
			}
		}

		virtual Map get_Map()
		{
			return base.Map;
		}

		Map ISlotGroupParent.get_Map()
		{
			//ILSpy generated this explicit interface implementation from .override directive in get_Map
			return this.get_Map();
		}
	}
}
