using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	[StaticConstructorOnStartup]
	public static class GenMapUI
	{
		public const float NameBGHeight = 12f;

		public const float NameBGExtraWidth = 4f;

		public const float LabelOffsetYStandard = -0.4f;

		public static readonly Texture2D OverlayHealthTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 0f, 0f, 0.25f));

		public static readonly Color DefaultThingLabelColor = new Color(1f, 1f, 1f, 0.75f);

		public static Vector2 LabelDrawPosFor(Thing thing, float worldOffsetZ)
		{
			Vector3 drawPos = thing.DrawPos;
			drawPos.z += worldOffsetZ;
			Vector2 result = Find.Camera.WorldToScreenPoint(drawPos) / Prefs.UIScale;
			result.y = (float)UI.screenHeight - result.y;
			return result;
		}

		public static Vector2 LabelDrawPosFor(IntVec3 center)
		{
			Vector3 position = center.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			Vector2 result = Find.Camera.WorldToScreenPoint(position) / Prefs.UIScale;
			result.y = (float)UI.screenHeight - result.y;
			result.y -= 1f;
			return result;
		}

		public static void DrawThingLabel(Thing thing, string text)
		{
			GenMapUI.DrawThingLabel(thing, text, GenMapUI.DefaultThingLabelColor);
		}

		public static void DrawThingLabel(Thing thing, string text, Color textColor)
		{
			GenMapUI.DrawThingLabel(GenMapUI.LabelDrawPosFor(thing, -0.4f), text, textColor);
		}

		public static void DrawThingLabel(Vector2 screenPos, string text, Color textColor)
		{
			Text.Font = GameFont.Tiny;
			Vector2 vector = Text.CalcSize(text);
			float x = vector.x;
			Rect position = new Rect((float)(screenPos.x - x / 2.0 - 4.0), screenPos.y, (float)(x + 8.0), 12f);
			GUI.DrawTexture(position, TexUI.GrayTextBG);
			GUI.color = textColor;
			Text.Anchor = TextAnchor.UpperCenter;
			Rect rect = new Rect((float)(screenPos.x - x / 2.0), (float)(screenPos.y - 3.0), x, 999f);
			Widgets.Label(rect, text);
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		public static void DrawPawnLabel(Pawn pawn, Vector2 pos, float alpha = 1f, float truncateToWidth = 9999f, Dictionary<string, string> truncatedLabelsCache = null, GameFont font = GameFont.Tiny, bool alwaysDrawBg = true, bool alignCenter = true)
		{
			float pawnLabelNameWidth = GenMapUI.GetPawnLabelNameWidth(pawn, truncateToWidth, truncatedLabelsCache, font);
			Rect bgRect = new Rect((float)(pos.x - pawnLabelNameWidth / 2.0 - 4.0), pos.y, (float)(pawnLabelNameWidth + 8.0), 12f);
			if (!pawn.RaceProps.Humanlike)
			{
				bgRect.y -= 4f;
			}
			GenMapUI.DrawPawnLabel(pawn, bgRect, alpha, truncateToWidth, truncatedLabelsCache, font, alwaysDrawBg, alignCenter);
		}

		public static void DrawPawnLabel(Pawn pawn, Rect bgRect, float alpha = 1f, float truncateToWidth = 9999f, Dictionary<string, string> truncatedLabelsCache = null, GameFont font = GameFont.Tiny, bool alwaysDrawBg = true, bool alignCenter = true)
		{
			GUI.color = new Color(1f, 1f, 1f, alpha);
			Text.Font = font;
			string pawnLabel = GenMapUI.GetPawnLabel(pawn, truncateToWidth, truncatedLabelsCache, font);
			float pawnLabelNameWidth = GenMapUI.GetPawnLabelNameWidth(pawn, truncateToWidth, truncatedLabelsCache, font);
			float summaryHealthPercent = pawn.health.summaryHealth.SummaryHealthPercent;
			if (alwaysDrawBg || summaryHealthPercent < 0.99900001287460327)
			{
				GUI.DrawTexture(bgRect, TexUI.GrayTextBG);
			}
			if (summaryHealthPercent < 0.99900001287460327)
			{
				Rect rect = bgRect.ContractedBy(1f);
				Widgets.FillableBar(rect, summaryHealthPercent, GenMapUI.OverlayHealthTex, BaseContent.ClearTex, false);
			}
			Color color = PawnNameColorUtility.PawnNameColorOf(pawn);
			color.a = alpha;
			GUI.color = color;
			Rect rect2 = default(Rect);
			if (alignCenter)
			{
				Text.Anchor = TextAnchor.UpperCenter;
				Vector2 center = bgRect.center;
				rect2 = new Rect((float)(center.x - pawnLabelNameWidth / 2.0), (float)(bgRect.y - 2.0), pawnLabelNameWidth, 100f);
			}
			else
			{
				Text.Anchor = TextAnchor.UpperLeft;
				double x = bgRect.x + 2.0;
				Vector2 center2 = bgRect.center;
				float y = center2.y;
				Vector2 vector = Text.CalcSize(pawnLabel);
				rect2 = new Rect((float)x, (float)(y - vector.y / 2.0), pawnLabelNameWidth, 100f);
			}
			Widgets.Label(rect2, pawnLabel);
			if (pawn.Drafted)
			{
				Vector2 center3 = bgRect.center;
				Widgets.DrawLineHorizontal((float)(center3.x - pawnLabelNameWidth / 2.0), (float)(bgRect.y + 11.0), pawnLabelNameWidth);
			}
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		public static void DrawText(Vector2 worldPos, string text, Color textColor)
		{
			Vector3 position = new Vector3(worldPos.x, 0f, worldPos.y);
			Vector2 vector = Find.Camera.WorldToScreenPoint(position) / Prefs.UIScale;
			vector.y = (float)UI.screenHeight - vector.y;
			Text.Font = GameFont.Tiny;
			GUI.color = textColor;
			Text.Anchor = TextAnchor.UpperCenter;
			Vector2 vector2 = Text.CalcSize(text);
			float x = vector2.x;
			Widgets.Label(new Rect((float)(vector.x - x / 2.0), (float)(vector.y - 2.0), x, 999f), text);
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		private static float GetPawnLabelNameWidth(Pawn pawn, float truncateToWidth, Dictionary<string, string> truncatedLabelsCache, GameFont font)
		{
			GameFont font2 = Text.Font;
			Text.Font = font;
			string pawnLabel = GenMapUI.GetPawnLabel(pawn, truncateToWidth, truncatedLabelsCache, font);
			float num;
			if (font == GameFont.Tiny)
			{
				num = pawnLabel.GetWidthCached();
			}
			else
			{
				Vector2 vector = Text.CalcSize(pawnLabel);
				num = vector.x;
			}
			if (num < 20.0)
			{
				num = 20f;
			}
			Text.Font = font2;
			return num;
		}

		private static string GetPawnLabel(Pawn pawn, float truncateToWidth, Dictionary<string, string> truncatedLabelsCache, GameFont font)
		{
			GameFont font2 = Text.Font;
			Text.Font = font;
			string result = pawn.NameStringShort.CapitalizeFirst().Truncate(truncateToWidth, truncatedLabelsCache);
			Text.Font = font2;
			return result;
		}
	}
}
