using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;
using UnityEngine;

namespace Verse
{
	[StaticConstructorOnStartup]
	public static class GenUI
	{
		public const float Pad = 10f;

		public const float GapTiny = 4f;

		public const float GapSmall = 10f;

		public const float Gap = 17f;

		public const float GapWide = 26f;

		public const float ListSpacing = 28f;

		public const float MouseAttachIconSize = 32f;

		public const float MouseAttachIconOffset = 8f;

		public const float ScrollBarWidth = 16f;

		public const float HorizontalSliderHeight = 16f;

		public static readonly Vector2 TradeableDrawSize = new Vector2(150f, 45f);

		public static readonly Color MouseoverColor = new Color(0.3f, 0.7f, 0.9f);

		public static readonly Color SubtleMouseoverColor = new Color(0.7f, 0.7f, 0.7f);

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
		private static Comparison<Pawn> <>f__mg$cache0;

		[CompilerGenerated]
		private static Comparison<Thing> <>f__mg$cache1;

		[CompilerGenerated]
		private static Comparison<Pawn> <>f__mg$cache2;

		[CompilerGenerated]
		private static Predicate<Thing> <>f__am$cache0;

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
			float result;
			if (Find.CurrentMap == null)
			{
				result = 0f;
			}
			else
			{
				float num = GenCelestial.CurCelestialSunGlow(Find.CurrentMap);
				float num2 = (Find.CurrentMap.Biome != BiomeDefOf.IceSheet) ? Mathf.Clamp01(Find.CurrentMap.snowGrid.TotalDepth / 1000f) : 1f;
				result = num * num2 * 0.41f;
			}
			return result;
		}

		public static void DrawTextWinterShadow(Rect rect)
		{
			float num = GenUI.BackgroundDarkAlphaForText();
			if (num > 0.001f)
			{
				GUI.color = new Color(1f, 1f, 1f, num);
				GUI.DrawTexture(rect, GenUI.UnderShadowTex);
				GUI.color = Color.white;
			}
		}

		public static float IconDrawScale(ThingDef tDef)
		{
			float num = tDef.uiIconScale;
			if (tDef.uiIconPath.NullOrEmpty() && tDef.graphicData != null)
			{
				IntVec2 intVec = tDef.defaultPlacingRot.IsHorizontal ? tDef.Size.Rotated() : tDef.Size;
				num *= Mathf.Min(tDef.graphicData.drawSize.x / (float)intVec.x, tDef.graphicData.drawSize.y / (float)intVec.z);
			}
			return num;
		}

		public static void ErrorDialog(string message)
		{
			if (Find.WindowStack != null)
			{
				Find.WindowStack.Add(new Dialog_MessageBox(message, null, null, null, null, null, false, null, null));
			}
		}

		public static void DrawFlash(float centerX, float centerY, float size, float alpha, Color color)
		{
			Rect position = new Rect(centerX - size / 2f, centerY - size / 2f, size, size);
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
			float x;
			float result;
			if (GenUI.labelWidthCache.TryGetValue(s, out x))
			{
				result = x;
			}
			else
			{
				x = Text.CalcSize(s).x;
				GenUI.labelWidthCache.Add(s, x);
				result = x;
			}
			return result;
		}

		public static Rect Rounded(this Rect r)
		{
			return new Rect((float)((int)r.x), (float)((int)r.y), (float)((int)r.width), (float)((int)r.height));
		}

		public static Vector2 Rounded(this Vector2 v)
		{
			return new Vector2((float)((int)v.x), (float)((int)v.y));
		}

		public static float DistFromRect(Rect r, Vector2 p)
		{
			float num = Mathf.Abs(p.x - r.center.x) - r.width / 2f;
			if (num < 0f)
			{
				num = 0f;
			}
			float num2 = Mathf.Abs(p.y - r.center.y) - r.height / 2f;
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			return Mathf.Sqrt(num * num + num2 * num2);
		}

		public static void DrawMouseAttachment(Texture iconTex, string text = "", float angle = 0f, Vector2 offset = default(Vector2), Rect? customRect = null)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			float num = mousePosition.y + 12f;
			if (iconTex != null)
			{
				Rect mouseRect;
				if (customRect != null)
				{
					mouseRect = customRect.Value;
				}
				else
				{
					mouseRect = new Rect(mousePosition.x + 8f, num + 8f, 32f, 32f);
				}
				Find.WindowStack.ImmediateWindow(34003428, mouseRect, WindowLayer.Super, delegate
				{
					Rect rect = mouseRect.AtZero();
					rect.position += new Vector2(offset.x * rect.size.x, offset.y * rect.size.y);
					Widgets.DrawTextureRotated(rect, iconTex, angle);
				}, false, false, 0f);
				num += mouseRect.height;
			}
			if (text != "")
			{
				Rect textRect = new Rect(mousePosition.x + 12f, num, 200f, 9999f);
				Find.WindowStack.ImmediateWindow(34003429, textRect, WindowLayer.Super, delegate
				{
					Widgets.Label(textRect.AtZero(), text);
				}, false, false, 0f);
			}
		}

		public static void DrawMouseAttachment(Texture2D icon)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			Rect mouseRect = new Rect(mousePosition.x + 8f, mousePosition.y + 8f, 32f, 32f);
			Find.WindowStack.ImmediateWindow(34003428, mouseRect, WindowLayer.Super, delegate
			{
				GUI.DrawTexture(mouseRect.AtZero(), icon);
			}, false, false, 0f);
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
			Rect rect3 = new Rect(100f, 3f, GenUI.PieceBarSize.x, GenUI.PieceBarSize.y);
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
			for (int i = 0; i < clickableList.Count; i++)
			{
				yield return clickableList[i];
			}
			if (!thingsOnly)
			{
				IntVec3 cellTarg = UI.MouseCell();
				if (cellTarg.InBounds(Find.CurrentMap) && clickParams.CanTarget(new TargetInfo(cellTarg, Find.CurrentMap, false)))
				{
					yield return cellTarg;
				}
			}
			yield break;
		}

		public static List<Thing> ThingsUnderMouse(Vector3 clickPos, float pawnWideClickRadius, TargetingParameters clickParams)
		{
			IntVec3 c = IntVec3.FromVector3(clickPos);
			List<Thing> list = new List<Thing>();
			GenUI.clickedPawns.Clear();
			List<Pawn> allPawnsSpawned = Find.CurrentMap.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				Pawn pawn = allPawnsSpawned[i];
				if ((pawn.DrawPos - clickPos).MagnitudeHorizontal() < 0.4f && clickParams.CanTarget(pawn))
				{
					GenUI.clickedPawns.Add(pawn);
				}
			}
			List<Pawn> list2 = GenUI.clickedPawns;
			if (GenUI.<>f__mg$cache0 == null)
			{
				GenUI.<>f__mg$cache0 = new Comparison<Pawn>(GenUI.CompareThingsByDistanceToMousePointer);
			}
			list2.Sort(GenUI.<>f__mg$cache0);
			for (int j = 0; j < GenUI.clickedPawns.Count; j++)
			{
				list.Add(GenUI.clickedPawns[j]);
			}
			List<Thing> list3 = new List<Thing>();
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(c))
			{
				if (!list.Contains(thing) && clickParams.CanTarget(thing))
				{
					list3.Add(thing);
				}
			}
			List<Thing> list4 = list3;
			if (GenUI.<>f__mg$cache1 == null)
			{
				GenUI.<>f__mg$cache1 = new Comparison<Thing>(GenUI.CompareThingsByDrawAltitude);
			}
			list4.Sort(GenUI.<>f__mg$cache1);
			list.AddRange(list3);
			GenUI.clickedPawns.Clear();
			List<Pawn> allPawnsSpawned2 = Find.CurrentMap.mapPawns.AllPawnsSpawned;
			for (int k = 0; k < allPawnsSpawned2.Count; k++)
			{
				Pawn pawn2 = allPawnsSpawned2[k];
				if ((pawn2.DrawPos - clickPos).MagnitudeHorizontal() < pawnWideClickRadius && clickParams.CanTarget(pawn2))
				{
					GenUI.clickedPawns.Add(pawn2);
				}
			}
			List<Pawn> list5 = GenUI.clickedPawns;
			if (GenUI.<>f__mg$cache2 == null)
			{
				GenUI.<>f__mg$cache2 = new Comparison<Pawn>(GenUI.CompareThingsByDistanceToMousePointer);
			}
			list5.Sort(GenUI.<>f__mg$cache2);
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
			int result;
			if (num < num2)
			{
				result = -1;
			}
			else if (num == num2)
			{
				result = 0;
			}
			else
			{
				result = 1;
			}
			return result;
		}

		private static int CompareThingsByDrawAltitude(Thing A, Thing B)
		{
			int result;
			if (A.def.Altitude < B.def.Altitude)
			{
				result = 1;
			}
			else if (A.def.Altitude == B.def.Altitude)
			{
				result = 0;
			}
			else
			{
				result = -1;
			}
			return result;
		}

		public static int CurrentAdjustmentMultiplier()
		{
			int result;
			if (KeyBindingDefOf.ModifierIncrement_10x.IsDownEvent && KeyBindingDefOf.ModifierIncrement_100x.IsDownEvent)
			{
				result = 1000;
			}
			else if (KeyBindingDefOf.ModifierIncrement_100x.IsDownEvent)
			{
				result = 100;
			}
			else if (KeyBindingDefOf.ModifierIncrement_10x.IsDownEvent)
			{
				result = 10;
			}
			else
			{
				result = 1;
			}
			return result;
		}

		public static Rect GetInnerRect(this Rect rect)
		{
			return rect.ContractedBy(17f);
		}

		public static Rect ExpandedBy(this Rect rect, float margin)
		{
			return new Rect(rect.x - margin, rect.y - margin, rect.width + margin * 2f, rect.height + margin * 2f);
		}

		public static Rect ContractedBy(this Rect rect, float margin)
		{
			return new Rect(rect.x + margin, rect.y + margin, rect.width - margin * 2f, rect.height - margin * 2f);
		}

		public static Rect ScaledBy(this Rect rect, float scale)
		{
			rect.x -= rect.width * (scale - 1f) / 2f;
			rect.y -= rect.height * (scale - 1f) / 2f;
			rect.width *= scale;
			rect.height *= scale;
			return rect;
		}

		public static Rect CenteredOnXIn(this Rect rect, Rect otherRect)
		{
			return new Rect(otherRect.x + (otherRect.width - rect.width) / 2f, rect.y, rect.width, rect.height);
		}

		public static Rect CenteredOnYIn(this Rect rect, Rect otherRect)
		{
			return new Rect(rect.x, otherRect.y + (otherRect.height - rect.height) / 2f, rect.width, rect.height);
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
			return new Rect(rect.x, rect.y, rect.width / 2f, rect.height);
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
			return new Rect(rect.x + rect.width / 2f, rect.y, rect.width / 2f, rect.height);
		}

		public static Rect RightPart(this Rect rect, float pct)
		{
			return new Rect(rect.x + rect.width * (1f - pct), rect.y, rect.width * pct, rect.height);
		}

		public static Rect RightPartPixels(this Rect rect, float width)
		{
			return new Rect(rect.x + rect.width - width, rect.y, width, rect.height);
		}

		public static Rect TopHalf(this Rect rect)
		{
			return new Rect(rect.x, rect.y, rect.width, rect.height / 2f);
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
			return new Rect(rect.x, rect.y + rect.height / 2f, rect.width, rect.height / 2f);
		}

		public static Rect BottomPart(this Rect rect, float pct)
		{
			return new Rect(rect.x, rect.y + rect.height * (1f - pct), rect.width, rect.height * pct);
		}

		public static Rect BottomPartPixels(this Rect rect, float height)
		{
			return new Rect(rect.x, rect.y + rect.height - height, rect.width, height);
		}

		public static Color LerpColor(List<Pair<float, Color>> colors, float value)
		{
			Color result;
			if (colors.Count == 0)
			{
				result = Color.white;
			}
			else
			{
				int i = 0;
				while (i < colors.Count)
				{
					if (value < colors[i].First)
					{
						if (i == 0)
						{
							return colors[i].Second;
						}
						return Color.Lerp(colors[i - 1].Second, colors[i].Second, Mathf.InverseLerp(colors[i - 1].First, colors[i].First, value));
					}
					else
					{
						i++;
					}
				}
				result = colors.Last<Pair<float, Color>>().Second;
			}
			return result;
		}

		public static Vector2 GetMouseAttachedWindowPos(float width, float height)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			float y;
			if (mousePosition.y + 14f + height < (float)UI.screenHeight)
			{
				y = mousePosition.y + 14f;
			}
			else if (mousePosition.y - 5f - height >= 0f)
			{
				y = mousePosition.y - 5f - height;
			}
			else
			{
				y = 0f;
			}
			float x;
			if (mousePosition.x + 16f + width < (float)UI.screenWidth)
			{
				x = mousePosition.x + 16f;
			}
			else
			{
				x = mousePosition.x - 4f - width;
			}
			return new Vector2(x, y);
		}

		public static float GetCenteredButtonPos(int buttonIndex, int buttonsCount, float totalWidth, float buttonWidth, float pad = 10f)
		{
			float num = (float)buttonsCount * buttonWidth + (float)(buttonsCount - 1) * pad;
			return Mathf.Floor((totalWidth - num) / 2f + (float)buttonIndex * (buttonWidth + pad));
		}

		// Note: this type is marked as 'beforefieldinit'.
		static GenUI()
		{
		}

		[CompilerGenerated]
		private static bool <ThingsUnderMouse>m__0(Thing t)
		{
			return !t.Spawned;
		}

		[CompilerGenerated]
		private sealed class <DrawMouseAttachment>c__AnonStorey2
		{
			internal Vector2 offset;

			internal Texture iconTex;

			internal float angle;

			internal string text;

			public <DrawMouseAttachment>c__AnonStorey2()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <DrawMouseAttachment>c__AnonStorey1
		{
			internal Rect mouseRect;

			internal GenUI.<DrawMouseAttachment>c__AnonStorey2 <>f__ref$2;

			public <DrawMouseAttachment>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				Rect rect = this.mouseRect.AtZero();
				rect.position += new Vector2(this.<>f__ref$2.offset.x * rect.size.x, this.<>f__ref$2.offset.y * rect.size.y);
				Widgets.DrawTextureRotated(rect, this.<>f__ref$2.iconTex, this.<>f__ref$2.angle);
			}
		}

		[CompilerGenerated]
		private sealed class <DrawMouseAttachment>c__AnonStorey3
		{
			internal Rect textRect;

			internal GenUI.<DrawMouseAttachment>c__AnonStorey2 <>f__ref$2;

			public <DrawMouseAttachment>c__AnonStorey3()
			{
			}

			internal void <>m__0()
			{
				Widgets.Label(this.textRect.AtZero(), this.<>f__ref$2.text);
			}
		}

		[CompilerGenerated]
		private sealed class <DrawMouseAttachment>c__AnonStorey4
		{
			internal Rect mouseRect;

			internal Texture2D icon;

			public <DrawMouseAttachment>c__AnonStorey4()
			{
			}

			internal void <>m__0()
			{
				GUI.DrawTexture(this.mouseRect.AtZero(), this.icon);
			}
		}

		[CompilerGenerated]
		private sealed class <TargetsAt>c__Iterator0 : IEnumerable, IEnumerable<LocalTargetInfo>, IEnumerator, IDisposable, IEnumerator<LocalTargetInfo>
		{
			internal Vector3 clickPos;

			internal TargetingParameters clickParams;

			internal List<Thing> <clickableList>__0;

			internal int <i>__1;

			internal bool thingsOnly;

			internal IntVec3 <cellTarg>__2;

			internal LocalTargetInfo $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <TargetsAt>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					clickableList = GenUI.ThingsUnderMouse(clickPos, 0.8f, clickParams);
					i = 0;
					break;
				case 1u:
					i++;
					break;
				case 2u:
					IL_116:
					goto IL_117;
				default:
					return false;
				}
				if (i >= clickableList.Count)
				{
					if (thingsOnly)
					{
						goto IL_117;
					}
					cellTarg = UI.MouseCell();
					if (!cellTarg.InBounds(Find.CurrentMap) || !clickParams.CanTarget(new TargetInfo(cellTarg, Find.CurrentMap, false)))
					{
						goto IL_116;
					}
					this.$current = cellTarg;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
				}
				else
				{
					this.$current = clickableList[i];
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
				}
				return true;
				IL_117:
				this.$PC = -1;
				return false;
			}

			LocalTargetInfo IEnumerator<LocalTargetInfo>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.LocalTargetInfo>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<LocalTargetInfo> IEnumerable<LocalTargetInfo>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				GenUI.<TargetsAt>c__Iterator0 <TargetsAt>c__Iterator = new GenUI.<TargetsAt>c__Iterator0();
				<TargetsAt>c__Iterator.clickPos = clickPos;
				<TargetsAt>c__Iterator.clickParams = clickParams;
				<TargetsAt>c__Iterator.thingsOnly = thingsOnly;
				return <TargetsAt>c__Iterator;
			}
		}
	}
}
