using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007C2 RID: 1986
	public class Designator_AreaNoRoof : Designator_Area
	{
		// Token: 0x06002BFD RID: 11261 RVA: 0x001747E0 File Offset: 0x00172BE0
		public Designator_AreaNoRoof()
		{
			this.defaultLabel = "DesignatorAreaNoRoofExpand".Translate();
			this.defaultDesc = "DesignatorAreaNoRoofExpandDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/NoRoofArea", true);
			this.hotKey = KeyBindingDefOf.Misc5;
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_AreaAdd;
			this.useMouseIcon = true;
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06002BFE RID: 11262 RVA: 0x00174854 File Offset: 0x00172C54
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x06002BFF RID: 11263 RVA: 0x0017486C File Offset: 0x00172C6C
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002C00 RID: 11264 RVA: 0x00174884 File Offset: 0x00172C84
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (!c.InBounds(base.Map))
			{
				result = false;
			}
			else if (c.Fogged(base.Map))
			{
				result = false;
			}
			else
			{
				RoofDef roofDef = base.Map.roofGrid.RoofAt(c);
				if (roofDef != null && roofDef.isThickRoof)
				{
					result = "MessageNothingCanRemoveThickRoofs".Translate();
				}
				else
				{
					bool flag = base.Map.areaManager.NoRoof[c];
					result = !flag;
				}
			}
			return result;
		}

		// Token: 0x06002C01 RID: 11265 RVA: 0x0017492B File Offset: 0x00172D2B
		public override void DesignateSingleCell(IntVec3 c)
		{
			base.Map.areaManager.NoRoof[c] = true;
			Designator_AreaNoRoof.justAddedCells.Add(c);
		}

		// Token: 0x06002C02 RID: 11266 RVA: 0x00174950 File Offset: 0x00172D50
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			for (int i = 0; i < Designator_AreaNoRoof.justAddedCells.Count; i++)
			{
				base.Map.areaManager.BuildRoof[Designator_AreaNoRoof.justAddedCells[i]] = false;
			}
			Designator_AreaNoRoof.justAddedCells.Clear();
		}

		// Token: 0x06002C03 RID: 11267 RVA: 0x001749AC File Offset: 0x00172DAC
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.NoRoof.MarkForDraw();
			base.Map.areaManager.BuildRoof.MarkForDraw();
		}

		// Token: 0x04001798 RID: 6040
		private static List<IntVec3> justAddedCells = new List<IntVec3>();
	}
}
