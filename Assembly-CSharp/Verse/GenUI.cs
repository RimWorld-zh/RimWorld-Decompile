using RimWorld;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	[StaticConstructorOnStartup]
	public static class GenUI
	{
		public const float Pad = 10f;

		public const float GapTiny = 4f;

		public const float Gap = 17f;

		public const float GapWide = 26f;

		public const float ListSpacing = 28f;

		public const float MouseAttachIconSize = 32f;

		public const float MouseAttachIconOffset = 8f;

		public const float ScrollBarWidth = 16f;

		public const float HorizontalSliderHeight = 16f;

		public static readonly Vector2 TradeableDrawSize = new Vector2(150f, 45f);

		public static readonly Color MouseoverColor = new Color(0.3f, 0.7f, 0.9f);

		public static readonly Vector2 MaxWinSize = new Vector2(1010f, 754f);

		public const float SmallIconSize = 24f;

		public const int RootGUIDepth = 50;

		public const int CameraGUIDepth = 100;

		private const float MouseIconSize = 32f;

		private const float MouseIconOffset = 12f;

		private static readonly Material MouseoverBracketMaterial = MaterialPool.MatFrom("UI/Overlays/MouseoverBracketTex", ShaderDatabase.MetaOverlay);

		private static readonly Texture2D UnderShadowTex = ContentFinder<Texture2D>.Get("UI/Misc/ScreenCornerShadow", true);

		private static readonly Texture2D UIFlash = ContentFinder<Texture2D>.Get("UI/Misc/Flash", true);

		private static Dictionary<string, float> labelWidthCache = new Dictionary<string, float>();

		private static readonly Vector2 PieceBarSize = new Vector2(100f, 17f);

		public const float PawnDirectClickRadius = 0.4f;

		private static List<Pawn> clickedPawns = new List<Pawn>();

		[CompilerGenerated]
		private static Comparison<Pawn> _003C_003Ef__mg_0024cache0;

		[CompilerGenerated]
		private static Comparison<Thing> _003C_003Ef__mg_0024cache1;

		[CompilerGenerated]
		private static Comparison<Pawn> _003C_003Ef__mg_0024cache2;

		public static void SetLabelAlign(TextAnchor a)
		{
			Text.Anchor = a;
		}

		public static void ResetLabelAlign()
		{
			Text.Anchor = TextAnchor.UpperLeft;
		}

		public static float BackgroundDarkAlphaForText()
		{
			if (Find.VisibleMap == null)
			{
				return 0f;
			}
			float num = GenCelestial.CurCelestialSunGlow(Find.VisibleMap);
			float num2 = (float)((Find.VisibleMap.Biome != BiomeDefOf.IceSheet) ? Mathf.Clamp01((float)(Find.VisibleMap.snowGrid.TotalDepth / 1000.0)) : 1.0);
			return (float)(num * num2 * 0.40999999642372131);
		}

		public static void DrawTextWinterShadow(Rect rect)
		{
			float num = GenUI.BackgroundDarkAlphaForText();
			if (num > 0.0010000000474974513)
			{
				GUI.color = new Color(1f, 1f, 1f, num);
				GUI.DrawTexture(rect, GenUI.UnderShadowTex);
				GUI.color = Color.white;
			}
		}

		public static float IconDrawScale(ThingDef tDef)
		{
			if (tDef.graphicData == null)
			{
				return 1f;
			}
			if (tDef.iconDrawScale > 0.0)
			{
				return tDef.iconDrawScale;
			}
			float x = tDef.graphicData.drawSize.x;
			IntVec2 size = tDef.Size;
			if (x > (float)size.x)
			{
				float y = tDef.graphicData.drawSize.y;
				IntVec2 size2 = tDef.Size;
				if (y > (float)size2.z)
				{
					float num = tDef.graphicData.drawSize.x;
					IntVec2 size3 = tDef.Size;
					float a = num / (float)size3.x;
					float num2 = tDef.graphicData.drawSize.y;
					IntVec2 size4 = tDef.Size;
					return Mathf.Min(a, num2 / (float)size4.z);
				}
			}
			return 1f;
		}

		public static void ErrorDialog(string message)
		{
			if (Find.WindowStack != null)
			{
				Find.WindowStack.Add(new Dialog_MessageBox(message, null, null, null, null, null, false));
			}
		}

		public static void DrawFlash(float centerX, float centerY, float size, float alpha, Color color)
		{
			Rect position = new Rect((float)(centerX - size / 2.0), (float)(centerY - size / 2.0), size, size);
			Color color2 = color;
			color2.a = alpha;
			GUI.color = color2;
			GUI.DrawTexture(position, GenUI.UIFlash);
			GUI.color = Color.white;
		}

		public static float GetWidthCached(this string s)
		{
			if (GenUI.labelWidthCache.Count > 2000 || (Time.frameCount % 40000 == 0 && GenUI.labelWidthCache.Count > 100))
			{
				GenUI.labelWidthCache.Clear();
			}
			float x = default(float);
			if (GenUI.labelWidthCache.TryGetValue(s, out x))
			{
				return x;
			}
			Vector2 vector = Text.CalcSize(s);
			x = vector.x;
			GenUI.labelWidthCache.Add(s, x);
			return x;
		}

		public static Rect Rounded(this Rect r)
		{
			return new Rect((float)(int)r.x, (float)(int)r.y, (float)(int)r.width, (float)(int)r.height);
		}

		public static Vector2 Rounded(this Vector2 v)
		{
			return new Vector2((float)(int)v.x, (float)(int)v.y);
		}

		public static float DistFromRect(Rect r, Vector2 p)
		{
			float x = p.x;
			Vector2 center = r.center;
			float num = (float)(Mathf.Abs(x - center.x) - r.width / 2.0);
			if (num < 0.0)
			{
				num = 0f;
			}
			float y = p.y;
			Vector2 center2 = r.center;
			float num2 = (float)(Mathf.Abs(y - center2.y) - r.height / 2.0);
			if (num2 < 0.0)
			{
				num2 = 0f;
			}
			return Mathf.Sqrt(num * num + num2 * num2);
		}

		public static void DrawMouseAttachment(Texture2D iconTex, string text, float angle = 0f)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			float num = (float)(mousePosition.y + 12.0);
			if ((UnityEngine.Object)iconTex != (UnityEngine.Object)null)
			{
				Rect rect = new Rect((float)(mousePosition.x + 12.0), num, 32f, 32f);
				Widgets.DrawTextureRotated(rect, iconTex, angle);
				num += rect.height;
			}
			if (text != string.Empty)
			{
				Rect rect2 = new Rect((float)(mousePosition.x + 12.0), num, 200f, 9999f);
				Widgets.Label(rect2, text);
			}
		}

		public static void DrawMouseAttachment(Texture2D icon)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			GUI.DrawTexture(new Rect((float)(mousePosition.x + 8.0), (float)(mousePosition.y + 8.0), 32f, 32f), icon);
		}

		public static void RenderMouseoverBracket()
		{
			Vector3 position = UI.MouseCell().ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, GenUI.MouseoverBracketMaterial, 0);
		}

		public static void DrawStatusLevel(Need status, Rect rect)
		{
			GUI.BeginGroup(rect);
			Rect rect2 = new Rect(0f, 2f, rect.width, 25f);
			Widgets.Label(rect2, status.LabelCap);
			Vector2 pieceBarSize = GenUI.PieceBarSize;
			float x = pieceBarSize.x;
			Vector2 pieceBarSize2 = GenUI.PieceBarSize;
			Rect rect3 = new Rect(100f, 3f, x, pieceBarSize2.y);
			Widgets.FillableBar(rect3, status.CurLevelPercentage);
			Widgets.FillableBarChangeArrows(rect3, status.GUIChangeArrow);
			GUI.EndGroup();
			TooltipHandler.TipRegion(rect, status.GetTipString());
			if (Mouse.IsOver(rect))
			{
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
		}

		public static IEnumerable<LocalTargetInfo> TargetsAtMouse(TargetingParameters clickParams, bool thingsOnly = false)
		{
			return GenUI.TargetsAt(UI.MouseMapPosition(), clickParams, thingsOnly);
		}

		public static IEnumerable<LocalTargetInfo> TargetsAt(Vector3 clickPos, TargetingParameters clickParams, bool thingsOnly = false)
		{
			List<Thing> clickableList = GenUI.ThingsUnderMouse(clickPos, 0.8f, clickParams);
			int i = 0;
			if (i < clickableList.Count)
			{
				yield return (LocalTargetInfo)clickableList[i];
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (thingsOnly)
				yield break;
			IntVec3 cellTarg = UI.MouseCell();
			if (!cellTarg.InBounds(Find.VisibleMap))
				yield break;
			if (!clickParams.CanTarget(new TargetInfo(cellTarg, Find.VisibleMap, false)))
				yield break;
			yield return (LocalTargetInfo)cellTarg;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public static List<Thing> ThingsUnderMouse(Vector3 clickPos, float pawnWideClickRadius, TargetingParameters clickParams)
		{
			IntVec3 c = IntVec3.FromVector3(clickPos);
			List<Thing> list = new List<Thing>();
			GenUI.clickedPawns.Clear();
			List<Pawn> allPawnsSpawned = Find.VisibleMap.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				Pawn pawn = allPawnsSpawned[i];
				if ((pawn.DrawPos - clickPos).MagnitudeHorizontal() < 0.40000000596046448 && clickParams.CanTarget(pawn))
				{
					GenUI.clickedPawns.Add(pawn);
				}
			}
			GenUI.clickedPawns.Sort(GenUI.CompareThingsByDistanceToMousePointer);
			for (int j = 0; j < GenUI.clickedPawns.Count; j++)
			{
				list.Add(GenUI.clickedPawns[j]);
			}
			List<Thing> list2 = new List<Thing>();
			foreach (Thing item in Find.VisibleMap.thingGrid.ThingsAt(c))
			{
				if (!list.Contains(item) && clickParams.CanTarget(item))
				{
					list2.Add(item);
				}
			}
			list2.Sort(GenUI.CompareThingsByDrawAltitude);
			list.AddRange(list2);
			GenUI.clickedPawns.Clear();
			List<Pawn> allPawnsSpawned2 = Find.VisibleMap.mapPawns.AllPawnsSpawned;
			for (int k = 0; k < allPawnsSpawned2.Count; k++)
			{
				Pawn pawn2 = allPawnsSpawned2[k];
				if ((pawn2.DrawPos - clickPos).MagnitudeHorizontal() < pawnWideClickRadius && clickParams.CanTarget(pawn2))
				{
					GenUI.clickedPawns.Add(pawn2);
				}
			}
			GenUI.clickedPawns.Sort(GenUI.CompareThingsByDistanceToMousePointer);
			for (int l = 0; l < GenUI.clickedPawns.Count; l++)
			{
				if (!list.Contains(GenUI.clickedPawns[l]))
				{
					list.Add(GenUI.clickedPawns[l]);
				}
			}
			list.RemoveAll((Thing t) => !t.Spawned);
			GenUI.clickedPawns.Clear();
			return list;
		}

		private static int CompareThingsByDistanceToMousePointer(Thing a, Thing b)
		{
			Vector3 b2 = UI.MouseMapPosition();
			float num = (a.DrawPos - b2).MagnitudeHorizontalSquared();
			float num2 = (b.DrawPos - b2).MagnitudeHorizontalSquared();
			if (num < num2)
			{
				return -1;
			}
			if (num == num2)
			{
				return 0;
			}
			return 1;
		}

		private static int CompareThingsByDrawAltitude(Thing A, Thing B)
		{
			if (A.def.Altitude < B.def.Altitude)
			{
				return 1;
			}
			if (A.def.Altitude == B.def.Altitude)
			{
				return 0;
			}
			return -1;
		}

		public static int CurrentAdjustmentMultiplier()
		{
			if (KeyBindingDefOf.ModifierIncrement10x.IsDownEvent && KeyBindingDefOf.ModifierIncrement100x.IsDownEvent)
			{
				return 1000;
			}
			if (KeyBindingDefOf.ModifierIncrement100x.IsDownEvent)
			{
				return 100;
			}
			if (KeyBindingDefOf.ModifierIncrement10x.IsDownEvent)
			{
				return 10;
			}
			return 1;
		}

		public static Rect GetInnerRect(this Rect rect)
		{
			return rect.ContractedBy(17f);
		}

		public static Rect ExpandedBy(this Rect rect, float margin)
		{
			return new Rect(rect.x - margin, rect.y - margin, (float)(rect.width + margin * 2.0), (float)(rect.height + margin * 2.0));
		}

		public static Rect ContractedBy(this Rect rect, float margin)
		{
			return new Rect(rect.x + margin, rect.y + margin, (float)(rect.width - margin * 2.0), (float)(rect.height - margin * 2.0));
		}

		public static Rect ScaledBy(this Rect rect, float scale)
		{
			rect.x -= (float)(rect.width * (scale - 1.0) / 2.0);
			rect.y -= (float)(rect.height * (scale - 1.0) / 2.0);
			rect.width *= scale;
			rect.height *= scale;
			return rect;
		}

		public static Rect CenteredOnXIn(this Rect rect, Rect otherRect)
		{
			return new Rect((float)(otherRect.x + (otherRect.width - rect.width) / 2.0), rect.y, rect.width, rect.height);
		}

		public static Rect CenteredOnYIn(this Rect rect, Rect otherRect)
		{
			return new Rect(rect.x, (float)(otherRect.y + (otherRect.height - rect.height) / 2.0), rect.width, rect.height);
		}

		public static Rect AtZero(this Rect rect)
		{
			return new Rect(0f, 0f, rect.width, rect.height);
		}

		public static void AbsorbClicksInRect(Rect r)
		{
			if (Event.current.type == EventType.MouseDown && r.Contains(Event.current.mousePosition))
			{
				Event.current.Use();
			}
		}

		public static Rect LeftHalf(this Rect rect)
		{
			return new Rect(rect.x, rect.y, (float)(rect.width / 2.0), rect.height);
		}

		public static Rect LeftPart(this Rect rect, float pct)
		{
			return new Rect(rect.x, rect.y, rect.width * pct, rect.height);
		}

		public static Rect LeftPartPixels(this Rect rect, float width)
		{
			return new Rect(rect.x, rect.y, width, rect.height);
		}

		public static Rect RightHalf(this Rect rect)
		{
			return new Rect((float)(rect.x + rect.width / 2.0), rect.y, (float)(rect.width / 2.0), rect.height);
		}

		public static Rect RightPart(this Rect rect, float pct)
		{
			return new Rect((float)(rect.x + rect.width * (1.0 - pct)), rect.y, rect.width * pct, rect.height);
		}

		public static Rect RightPartPixels(this Rect rect, float width)
		{
			return new Rect(rect.x + rect.width - width, rect.y, width, rect.height);
		}

		public static Rect TopHalf(this Rect rect)
		{
			return new Rect(rect.x, rect.y, rect.width, (float)(rect.height / 2.0));
		}

		public static Rect TopPart(this Rect rect, float pct)
		{
			return new Rect(rect.x, rect.y, rect.width, rect.height * pct);
		}

		public static Rect TopPartPixels(this Rect rect, float height)
		{
			return new Rect(rect.x, rect.y, rect.width, height);
		}

		public static Rect BottomHalf(this Rect rect)
		{
			return new Rect(rect.x, (float)(rect.y + rect.height / 2.0), rect.width, (float)(rect.height / 2.0));
		}

		public static Rect BottomPart(this Rect rect, float pct)
		{
			return new Rect(rect.x, (float)(rect.y + rect.height * (1.0 - pct)), rect.width, rect.height * pct);
		}

		public static Rect BottomPartPixels(this Rect rect, float height)
		{
			return new Rect(rect.x, rect.y + rect.height - height, rect.width, height);
		}
	}
}
