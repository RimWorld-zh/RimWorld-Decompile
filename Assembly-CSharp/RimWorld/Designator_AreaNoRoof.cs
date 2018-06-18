using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007C6 RID: 1990
	public class Designator_AreaNoRoof : Designator_Area
	{
		// Token: 0x06002C04 RID: 11268 RVA: 0x00174608 File Offset: 0x00172A08
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

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06002C05 RID: 11269 RVA: 0x0017467C File Offset: 0x00172A7C
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06002C06 RID: 11270 RVA: 0x00174694 File Offset: 0x00172A94
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002C07 RID: 11271 RVA: 0x001746AC File Offset: 0x00172AAC
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

		// Token: 0x06002C08 RID: 11272 RVA: 0x00174753 File Offset: 0x00172B53
		public override void DesignateSingleCell(IntVec3 c)
		{
			base.Map.areaManager.NoRoof[c] = true;
			Designator_AreaNoRoof.justAddedCells.Add(c);
		}

		// Token: 0x06002C09 RID: 11273 RVA: 0x00174778 File Offset: 0x00172B78
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			for (int i = 0; i < Designator_AreaNoRoof.justAddedCells.Count; i++)
			{
				base.Map.areaManager.BuildRoof[Designator_AreaNoRoof.justAddedCells[i]] = false;
			}
			Designator_AreaNoRoof.justAddedCells.Clear();
		}

		// Token: 0x06002C0A RID: 11274 RVA: 0x001747D4 File Offset: 0x00172BD4
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.NoRoof.MarkForDraw();
			base.Map.areaManager.BuildRoof.MarkForDraw();
		}

		// Token: 0x0400179A RID: 6042
		private static List<IntVec3> justAddedCells = new List<IntVec3>();
	}
}
