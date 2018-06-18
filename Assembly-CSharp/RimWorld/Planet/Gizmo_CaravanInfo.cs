using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005F6 RID: 1526
	public class Gizmo_CaravanInfo : Gizmo
	{
		// Token: 0x06001E60 RID: 7776 RVA: 0x001068C9 File Offset: 0x00104CC9
		public Gizmo_CaravanInfo(Caravan caravan)
		{
			this.caravan = caravan;
			this.order = -100f;
		}

		// Token: 0x06001E61 RID: 7777 RVA: 0x001068E4 File Offset: 0x00104CE4
		public override float GetWidth(float maxWidth)
		{
			return Mathf.Min(520f, maxWidth);
		}

		// Token: 0x06001E62 RID: 7778 RVA: 0x00106904 File Offset: 0x00104D04
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

		// Token: 0x04001205 RID: 4613
		private Caravan caravan;
	}
}
