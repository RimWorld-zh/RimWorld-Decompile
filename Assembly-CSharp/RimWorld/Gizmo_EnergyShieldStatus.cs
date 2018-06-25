using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006DD RID: 1757
	[StaticConstructorOnStartup]
	internal class Gizmo_EnergyShieldStatus : Gizmo
	{
		// Token: 0x04001547 RID: 5447
		public ShieldBelt shield;

		// Token: 0x04001548 RID: 5448
		private static readonly Texture2D FullShieldBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.2f, 0.24f));

		// Token: 0x04001549 RID: 5449
		private static readonly Texture2D EmptyShieldBarTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);

		// Token: 0x06002634 RID: 9780 RVA: 0x00147F50 File Offset: 0x00146350
		public Gizmo_EnergyShieldStatus()
		{
			this.order = -100f;
		}

		// Token: 0x06002635 RID: 9781 RVA: 0x00147F64 File Offset: 0x00146364
		public override float GetWidth(float maxWidth)
		{
			return 140f;
		}

		// Token: 0x06002636 RID: 9782 RVA: 0x00147F80 File Offset: 0x00146380
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth)
		{
			Rect overRect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Find.WindowStack.ImmediateWindow(984688, overRect, WindowLayer.GameUI, delegate
			{
				Rect rect = overRect.AtZero().ContractedBy(6f);
				Rect rect2 = rect;
				rect2.height = overRect.height / 2f;
				Text.Font = GameFont.Tiny;
				Widgets.Label(rect2, this.shield.LabelCap);
				Rect rect3 = rect;
				rect3.yMin = overRect.height / 2f;
				float fillPercent = this.shield.Energy / Mathf.Max(1f, this.shield.GetStatValue(StatDefOf.EnergyShieldEnergyMax, true));
				Widgets.FillableBar(rect3, fillPercent, Gizmo_EnergyShieldStatus.FullShieldBarTex, Gizmo_EnergyShieldStatus.EmptyShieldBarTex, false);
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect3, (this.shield.Energy * 100f).ToString("F0") + " / " + (this.shield.GetStatValue(StatDefOf.EnergyShieldEnergyMax, true) * 100f).ToString("F0"));
				Text.Anchor = TextAnchor.UpperLeft;
			}, true, false, 1f);
			return new GizmoResult(GizmoState.Clear);
		}
	}
}
