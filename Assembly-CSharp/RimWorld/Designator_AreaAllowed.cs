using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007BD RID: 1981
	public abstract class Designator_AreaAllowed : Designator_Area
	{
		// Token: 0x06002BDF RID: 11231 RVA: 0x00173D15 File Offset: 0x00172115
		public Designator_AreaAllowed(DesignateMode mode)
		{
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06002BE0 RID: 11232 RVA: 0x00173D3C File Offset: 0x0017213C
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06002BE1 RID: 11233 RVA: 0x00173D54 File Offset: 0x00172154
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06002BE2 RID: 11234 RVA: 0x00173D6C File Offset: 0x0017216C
		public static Area SelectedArea
		{
			get
			{
				return Designator_AreaAllowed.selectedArea;
			}
		}

		// Token: 0x06002BE3 RID: 11235 RVA: 0x00173D86 File Offset: 0x00172186
		public static void ClearSelectedArea()
		{
			Designator_AreaAllowed.selectedArea = null;
		}

		// Token: 0x06002BE4 RID: 11236 RVA: 0x00173D8F File Offset: 0x0017218F
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			if (Designator_AreaAllowed.selectedArea != null && Find.WindowStack.FloatMenu == null)
			{
				Designator_AreaAllowed.selectedArea.MarkForDraw();
			}
		}

		// Token: 0x06002BE5 RID: 11237 RVA: 0x00173DBC File Offset: 0x001721BC
		public override void ProcessInput(Event ev)
		{
			if (base.CheckCanInteract())
			{
				if (Designator_AreaAllowed.selectedArea != null)
				{
					base.ProcessInput(ev);
				}
				AreaUtility.MakeAllowedAreaListFloatMenu(delegate(Area a)
				{
					Designator_AreaAllowed.selectedArea = a;
					this.<ProcessInput>__BaseCallProxy0(ev);
				}, false, true, base.Map);
			}
		}

		// Token: 0x06002BE6 RID: 11238 RVA: 0x00173E1D File Offset: 0x0017221D
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.AllowedAreas, KnowledgeAmount.SpecificInteraction);
		}

		// Token: 0x04001795 RID: 6037
		private static Area selectedArea;
	}
}
