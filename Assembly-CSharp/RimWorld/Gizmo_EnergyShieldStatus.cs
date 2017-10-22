using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	internal class Gizmo_EnergyShieldStatus : Gizmo
	{
		public ShieldBelt shield;

		private static readonly Texture2D FullShieldBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.2f, 0.24f));

		private static readonly Texture2D EmptyShieldBarTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);

		public override float Width
		{
			get
			{
				return 140f;
			}
		}

		public override GizmoResult GizmoOnGUI(Vector2 topLeft)
		{
			Rect overRect = new Rect(topLeft.x, topLeft.y, this.Width, 75f);
			Find.WindowStack.ImmediateWindow(984688, overRect, WindowLayer.GameUI, (Action)delegate
			{
				Rect rect;
				Rect rect2 = rect = overRect.AtZero().ContractedBy(6f);
				rect.height = (float)(overRect.height / 2.0);
				Text.Font = GameFont.Tiny;
				Widgets.Label(rect, this.shield.LabelCap);
				Rect rect3 = rect2;
				rect3.yMin = (float)(overRect.height / 2.0);
				float fillPercent = this.shield.Energy / Mathf.Max(1f, this.shield.GetStatValue(StatDefOf.EnergyShieldEnergyMax, true));
				Widgets.FillableBar(rect3, fillPercent, Gizmo_EnergyShieldStatus.FullShieldBarTex, Gizmo_EnergyShieldStatus.EmptyShieldBarTex, false);
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect3, ((float)(this.shield.Energy * 100.0)).ToString("F0") + " / " + ((float)(this.shield.GetStatValue(StatDefOf.EnergyShieldEnergyMax, true) * 100.0)).ToString("F0"));
				Text.Anchor = TextAnchor.UpperLeft;
			}, true, false, 1f);
			return new GizmoResult(GizmoState.Clear);
		}
	}
}
