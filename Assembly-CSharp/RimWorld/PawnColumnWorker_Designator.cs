using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200087A RID: 2170
	public abstract class PawnColumnWorker_Designator : PawnColumnWorker_Checkbox
	{
		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x06003191 RID: 12689
		protected abstract DesignationDef DesignationType { get; }

		// Token: 0x06003192 RID: 12690 RVA: 0x001AE278 File Offset: 0x001AC678
		protected virtual void Notify_DesignationAdded(Pawn pawn)
		{
		}

		// Token: 0x06003193 RID: 12691 RVA: 0x001AE27C File Offset: 0x001AC67C
		protected override bool GetValue(Pawn pawn)
		{
			return this.GetDesignation(pawn) != null;
		}

		// Token: 0x06003194 RID: 12692 RVA: 0x001AE2A0 File Offset: 0x001AC6A0
		protected override void SetValue(Pawn pawn, bool value)
		{
			if (value != this.GetValue(pawn))
			{
				if (value)
				{
					pawn.MapHeld.designationManager.AddDesignation(new Designation(pawn, this.DesignationType));
					this.Notify_DesignationAdded(pawn);
				}
				else
				{
					Designation designation = this.GetDesignation(pawn);
					if (designation != null)
					{
						pawn.MapHeld.designationManager.RemoveDesignation(designation);
					}
				}
			}
		}

		// Token: 0x06003195 RID: 12693 RVA: 0x001AE318 File Offset: 0x001AC718
		private Designation GetDesignation(Pawn pawn)
		{
			Map mapHeld = pawn.MapHeld;
			Designation result;
			if (mapHeld == null)
			{
				result = null;
			}
			else
			{
				result = mapHeld.designationManager.DesignationOn(pawn, this.DesignationType);
			}
			return result;
		}
	}
}
