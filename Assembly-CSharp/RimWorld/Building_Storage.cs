using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class Building_Storage : Building, ISlotGroupParent, IStoreSettingsParent
	{
		public StorageSettings settings;

		public SlotGroup slotGroup;

		private List<IntVec3> cachedOccupiedCells = null;

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
			using (IEnumerator<IntVec3> enumerator = GenAdj.CellsOccupiedBy(this).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					IntVec3 c = enumerator.Current;
					yield return c;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_00bd:
			/*Error near IL_00be: Unexpected return in MoveNext()*/;
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
			using (IEnumerator<Gizmo> enumerator = this._003CGetGizmos_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Gizmo g2 = enumerator.Current;
					yield return g2;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			using (IEnumerator<Gizmo> enumerator2 = StorageSettingsClipboard.CopyPasteGizmosFor(this.settings).GetEnumerator())
			{
				if (enumerator2.MoveNext())
				{
					Gizmo g = enumerator2.Current;
					yield return g;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_0156:
			/*Error near IL_0157: Unexpected return in MoveNext()*/;
		}
	}
}
