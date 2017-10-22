using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Zone_Stockpile : Zone, ISlotGroupParent, IStoreSettingsParent
	{
		public StorageSettings settings;

		public SlotGroup slotGroup;

		private static readonly ITab StorageTab = new ITab_Storage();

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
				return false;
			}
		}

		protected override Color NextZoneColor
		{
			get
			{
				return ZoneColorUtility.NextStorageZoneColor();
			}
		}

		public Zone_Stockpile()
		{
		}

		public Zone_Stockpile(StorageSettingsPreset preset, ZoneManager zoneManager) : base(preset.PresetName(), zoneManager)
		{
			this.settings = new StorageSettings(this);
			this.settings.SetFromPreset(preset);
			this.slotGroup = new SlotGroup(this);
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", new object[1]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.slotGroup = new SlotGroup(this);
			}
		}

		public override void AddCell(IntVec3 sq)
		{
			base.AddCell(sq);
			if (this.slotGroup != null)
			{
				this.slotGroup.Notify_AddedCell(sq);
			}
		}

		public override void RemoveCell(IntVec3 sq)
		{
			base.RemoveCell(sq);
			this.slotGroup.Notify_LostCell(sq);
		}

		public override void Deregister()
		{
			base.Deregister();
			this.slotGroup.Notify_ParentDestroying();
		}

		public override IEnumerable<InspectTabBase> GetInspectTabs()
		{
			yield return (InspectTabBase)Zone_Stockpile.StorageTab;
			/*Error: Unable to find new state assignment for yield return*/;
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

		public SlotGroup GetSlotGroup()
		{
			return this.slotGroup;
		}

		public IEnumerable<IntVec3> AllSlotCells()
		{
			int i = 0;
			if (i < base.cells.Count)
			{
				yield return base.cells[i];
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		public List<IntVec3> AllSlotCellsList()
		{
			return base.cells;
		}

		public StorageSettings GetParentStoreSettings()
		{
			return null;
		}

		public StorageSettings GetStoreSettings()
		{
			return this.settings;
		}

		public string SlotYielderLabel()
		{
			return base.label;
		}

		public void Notify_ReceivedThing(Thing newItem)
		{
			if (newItem.def.storedConceptLearnOpportunity != null)
			{
				LessonAutoActivator.TeachOpportunity(newItem.def.storedConceptLearnOpportunity, OpportunityType.GoodToKnow);
			}
		}

		public void Notify_LostThing(Thing newItem)
		{
		}
	}
}
