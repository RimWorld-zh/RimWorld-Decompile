using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007C3 RID: 1987
	public class Designator_AreaIgnoreRoof : Designator_Area
	{
		// Token: 0x06002BFA RID: 11258 RVA: 0x00174A04 File Offset: 0x00172E04
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

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06002BFB RID: 11259 RVA: 0x00174A78 File Offset: 0x00172E78
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06002BFC RID: 11260 RVA: 0x00174A90 File Offset: 0x00172E90
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002BFD RID: 11261 RVA: 0x00174AA8 File Offset: 0x00172EA8
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

		// Token: 0x06002BFE RID: 11262 RVA: 0x00174B30 File Offset: 0x00172F30
		public override void DesignateSingleCell(IntVec3 c)
		{
			base.Map.areaManager.BuildRoof[c] = false;
			base.Map.areaManager.NoRoof[c] = false;
		}

		// Token: 0x06002BFF RID: 11263 RVA: 0x00174B61 File Offset: 0x00172F61
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.NoRoof.MarkForDraw();
			base.Map.areaManager.BuildRoof.MarkForDraw();
		}
	}
}
