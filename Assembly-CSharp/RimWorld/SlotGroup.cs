using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public class SlotGroup
	{
		public ISlotGroupParent parent;

		private Map Map
		{
			get
			{
				return this.parent.Map;
			}
		}

		public StorageSettings Settings
		{
			get
			{
				return this.parent.GetStoreSettings();
			}
		}

		public IEnumerable<Thing> HeldThings
		{
			get
			{
				SlotGroup.<>c__Iterator14C <>c__Iterator14C = new SlotGroup.<>c__Iterator14C();
				<>c__Iterator14C.<>f__this = this;
				SlotGroup.<>c__Iterator14C expr_0E = <>c__Iterator14C;
				expr_0E.$PC = -2;
				return expr_0E;
			}
		}

		public List<IntVec3> CellsList
		{
			get
			{
				return this.parent.AllSlotCellsList();
			}
		}

		public SlotGroup(ISlotGroupParent parent)
		{
			this.parent = parent;
			this.Map.slotGroupManager.AddGroup(this);
		}

		[DebuggerHidden]
		public IEnumerator<IntVec3> GetEnumerator()
		{
			SlotGroup.<GetEnumerator>c__Iterator14D <GetEnumerator>c__Iterator14D = new SlotGroup.<GetEnumerator>c__Iterator14D();
			<GetEnumerator>c__Iterator14D.<>f__this = this;
			return <GetEnumerator>c__Iterator14D;
		}

		public void Notify_AddedCell(IntVec3 c)
		{
			this.Map.slotGroupManager.SetCellFor(c, this);
			this.Map.listerHaulables.RecalcAllInCell(c);
		}

		public void Notify_LostCell(IntVec3 c)
		{
			this.Map.slotGroupManager.ClearCellFor(c, this);
			this.Map.listerHaulables.RecalcAllInCell(c);
		}

		public void Notify_ParentDestroying()
		{
			this.Map.slotGroupManager.RemoveGroup(this);
		}

		public override string ToString()
		{
			if (this.parent != null)
			{
				return this.parent.ToString();
			}
			return "NullParent";
		}
	}
}
