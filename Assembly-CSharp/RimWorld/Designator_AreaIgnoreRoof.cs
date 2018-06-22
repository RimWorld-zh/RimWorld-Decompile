using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007C1 RID: 1985
	public class Designator_AreaIgnoreRoof : Designator_Area
	{
		// Token: 0x06002BF7 RID: 11255 RVA: 0x00174650 File Offset: 0x00172A50
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
		// (get) Token: 0x06002BF8 RID: 11256 RVA: 0x001746C4 File Offset: 0x00172AC4
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06002BF9 RID: 11257 RVA: 0x001746DC File Offset: 0x00172ADC
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002BFA RID: 11258 RVA: 0x001746F4 File Offset: 0x00172AF4
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

		// Token: 0x06002BFB RID: 11259 RVA: 0x0017477C File Offset: 0x00172B7C
		public override void DesignateSingleCell(IntVec3 c)
		{
			base.Map.areaManager.BuildRoof[c] = false;
			base.Map.areaManager.NoRoof[c] = false;
		}

		// Token: 0x06002BFC RID: 11260 RVA: 0x001747AD File Offset: 0x00172BAD
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.NoRoof.MarkForDraw();
			base.Map.areaManager.BuildRoof.MarkForDraw();
		}
	}
}
