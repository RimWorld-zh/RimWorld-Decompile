using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005F4 RID: 1524
	public class Gizmo_CaravanInfo : Gizmo
	{
		// Token: 0x04001202 RID: 4610
		private Caravan caravan;

		// Token: 0x06001E5B RID: 7771 RVA: 0x00106A6D File Offset: 0x00104E6D
		public Gizmo_CaravanInfo(Caravan caravan)
		{
			this.caravan = caravan;
			this.order = -100f;
		}

		// Token: 0x06001E5C RID: 7772 RVA: 0x00106A88 File Offset: 0x00104E88
		public override float GetWidth(float maxWidth)
		{
			return Mathf.Min(520f, maxWidth);
		}

		// Token: 0x06001E5D RID: 7773 RVA: 0x00106AA8 File Offset: 0x00104EA8
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth)
		{
			GizmoResult result;
			if (!this.caravan.Spawned)
			{
				result = new GizmoResult(GizmoState.Clear);
			}
			else
			{
				Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
				Widgets.DrawWindowBackground(rect);
				GUI.BeginGroup(rect);
				Rect rect2 = rect.AtZero();
				int? ticksToArrive = (!this.caravan.pather.Moving) ? null : new int?(CaravanArrivalTimeEstimator.EstimatedTicksToArrive(this.caravan, true));
				StringBuilder stringBuilder = new StringBuilder();
				float tilesPerDay = TilesPerDayCalculator.ApproxTilesPerDay(this.caravan, stringBuilder);
				CaravanUIUtility.DrawCaravanInfo(new CaravanUIUtility.CaravanInfo(this.caravan.MassUsage, this.caravan.MassCapacity, this.caravan.MassCapacityExplanation, tilesPerDay, stringBuilder.ToString(), this.caravan.DaysWorthOfFood, this.caravan.forage.ForagedFoodPerDay, this.caravan.forage.ForagedFoodPerDayExplanation, this.caravan.Visibility, this.caravan.VisibilityExplanation), null, this.caravan.Tile, ticksToArrive, -9999f, rect2, true, null, true);
				GUI.EndGroup();
				GenUI.AbsorbClicksInRect(rect);
				result = new GizmoResult(GizmoState.Clear);
			}
			return result;
		}
	}
}
