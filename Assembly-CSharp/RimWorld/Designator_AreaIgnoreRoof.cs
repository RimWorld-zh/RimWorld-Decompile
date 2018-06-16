using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007C5 RID: 1989
	public class Designator_AreaIgnoreRoof : Designator_Area
	{
		// Token: 0x06002BFC RID: 11260 RVA: 0x001743E4 File Offset: 0x001727E4
		public Designator_AreaIgnoreRoof()
		{
			this.defaultLabel = "DesignatorAreaIgnoreRoofExpand".Translate();
			this.defaultDesc = "DesignatorAreaIgnoreRoofExpandDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/IgnoreRoofArea", true);
			this.hotKey = KeyBindingDefOf.Misc11;
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_AreaAdd;
			this.useMouseIcon = true;
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06002BFD RID: 11261 RVA: 0x00174458 File Offset: 0x00172858
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06002BFE RID: 11262 RVA: 0x00174470 File Offset: 0x00172870
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002BFF RID: 11263 RVA: 0x00174488 File Offset: 0x00172888
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
				result = (base.Map.areaManager.BuildRoof[c] || base.Map.areaManager.NoRoof[c]);
			}
			return result;
		}

		// Token: 0x06002C00 RID: 11264 RVA: 0x00174510 File Offset: 0x00172910
		public override void DesignateSingleCell(IntVec3 c)
		{
			base.Map.areaManager.BuildRoof[c] = false;
			base.Map.areaManager.NoRoof[c] = false;
		}

		// Token: 0x06002C01 RID: 11265 RVA: 0x00174541 File Offset: 0x00172941
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.NoRoof.MarkForDraw();
			base.Map.areaManager.BuildRoof.MarkForDraw();
		}
	}
}
