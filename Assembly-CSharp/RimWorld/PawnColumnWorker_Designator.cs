using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200087E RID: 2174
	public abstract class PawnColumnWorker_Designator : PawnColumnWorker_Checkbox
	{
		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x06003196 RID: 12694
		protected abstract DesignationDef DesignationType { get; }

		// Token: 0x06003197 RID: 12695 RVA: 0x001ADFC8 File Offset: 0x001AC3C8
		protected virtual void Notify_DesignationAdded(Pawn pawn)
		{
		}

		// Token: 0x06003198 RID: 12696 RVA: 0x001ADFCC File Offset: 0x001AC3CC
		protected override bool GetValue(Pawn pawn)
		{
			return this.GetDesignation(pawn) != null;
		}

		// Token: 0x06003199 RID: 12697 RVA: 0x001ADFF0 File Offset: 0x001AC3F0
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

		// Token: 0x0600319A RID: 12698 RVA: 0x001AE068 File Offset: 0x001AC468
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
