using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007C4 RID: 1988
	public class Designator_AreaNoRoof : Designator_Area
	{
		// Token: 0x04001798 RID: 6040
		private static List<IntVec3> justAddedCells = new List<IntVec3>();

		// Token: 0x06002C01 RID: 11265 RVA: 0x00174930 File Offset: 0x00172D30
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
		// (get) Token: 0x06002C02 RID: 11266 RVA: 0x001749A4 File Offset: 0x00172DA4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x06002C03 RID: 11267 RVA: 0x001749BC File Offset: 0x00172DBC
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002C04 RID: 11268 RVA: 0x001749D4 File Offset: 0x00172DD4
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

		// Token: 0x06002C05 RID: 11269 RVA: 0x00174A7B File Offset: 0x00172E7B
		public override void DesignateSingleCell(IntVec3 c)
		{
			base.Map.areaManager.NoRoof[c] = true;
			Designator_AreaNoRoof.justAddedCells.Add(c);
		}

		// Token: 0x06002C06 RID: 11270 RVA: 0x00174AA0 File Offset: 0x00172EA0
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			for (int i = 0; i < Designator_AreaNoRoof.justAddedCells.Count; i++)
			{
				base.Map.areaManager.BuildRoof[Designator_AreaNoRoof.justAddedCells[i]] = false;
			}
			Designator_AreaNoRoof.justAddedCells.Clear();
		}

		// Token: 0x06002C07 RID: 11271 RVA: 0x00174AFC File Offset: 0x00172EFC
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.NoRoof.MarkForDraw();
			base.Map.areaManager.BuildRoof.MarkForDraw();
		}
	}
}
