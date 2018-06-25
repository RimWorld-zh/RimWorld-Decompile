using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007BB RID: 1979
	public abstract class Designator_AreaAllowed : Designator_Area
	{
		// Token: 0x04001793 RID: 6035
		private static Area selectedArea;

		// Token: 0x06002BDC RID: 11228 RVA: 0x0017403D File Offset: 0x0017243D
		public Designator_AreaAllowed(DesignateMode mode)
		{
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06002BDD RID: 11229 RVA: 0x00174064 File Offset: 0x00172464
		public override int DraggableDimensions
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06002BDE RID: 11230 RVA: 0x0017407C File Offset: 0x0017247C
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06002BDF RID: 11231 RVA: 0x00174094 File Offset: 0x00172494
		public static Area SelectedArea
		{
			get
			{
				return Designator_AreaAllowed.selectedArea;
			}
		}

		// Token: 0x06002BE0 RID: 11232 RVA: 0x001740AE File Offset: 0x001724AE
		public static void ClearSelectedArea()
		{
			Designator_AreaAllowed.selectedArea = null;
		}

		// Token: 0x06002BE1 RID: 11233 RVA: 0x001740B7 File Offset: 0x001724B7
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			if (Designator_AreaAllowed.selectedArea != null && Find.WindowStack.FloatMenu == null)
			{
				Designator_AreaAllowed.selectedArea.MarkForDraw();
			}
		}

		// Token: 0x06002BE2 RID: 11234 RVA: 0x001740E4 File Offset: 0x001724E4
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

		// Token: 0x06002BE3 RID: 11235 RVA: 0x00174145 File Offset: 0x00172545
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.AllowedAreas, KnowledgeAmount.SpecificInteraction);
		}
	}
}
