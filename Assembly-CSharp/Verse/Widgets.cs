using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using UnityEngine.Profiling;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000EB0 RID: 3760
	[StaticConstructorOnStartup]
	public static class Widgets
	{
		// Token: 0x06005889 RID: 22665 RVA: 0x002D5758 File Offset: 0x002D3B58
		static Widgets()
		{
			ColorInt colorInt = new ColorInt(78, 109, 129, 130);
			Widgets.ButtonBarTex = SolidColorMaterials.NewSolidColorTexture(colorInt.ToColor);
			Widgets.buttonInvisibleDraggable_activeControl = 0;
			Widgets.buttonInvisibleDraggable_dragged = false;
			Widgets.buttonInvisibleDraggable_mouseStart = Vector3.zero;
			Widgets.RangeControlTextColor = new Color(0.6f, 0.6f, 0.6f);
			Widgets.draggingId = 0;
			Widgets.curDragEnd = Widgets.RangeEnd.None;
			Widgets.lastDragSliderSoundTime = -1f;
			Widgets.FillableBarChangeRateDisplayRatio = 1E+08f;
			Widgets.MaxFillableBarChangeRate = 3;
			ColorInt colorInt2 = new ColorInt(97, 108, 122);
			Widgets.WindowBGBorderColor = colorInt2.ToColor;
			ColorInt colorInt3 = new ColorInt(21, 25, 29);
			Widgets.WindowBGFillColor = colorInt3.ToColor;
			ColorInt colorInt4 = new ColorInt(42, 43, 44);
			Widgets.MenuSectionBGFillColor = colorInt4.ToColor;
			ColorInt colorInt5 = new ColorInt(135, 135, 135);
			Widgets.MenuSectionBGBorderColor = colorInt5.ToColor;
			ColorInt colorInt6 = new ColorInt(133, 85, 44);
			Widgets.TutorWindowBGFillColor = colorInt6.ToColor;
			ColorInt colorInt7 = new ColorInt(176, 139, 61);
			Widgets.TutorWindowBGBorderColor = colorInt7.ToColor;
			Widgets.OptionUnselectedBGFillColor = new Color(0.21f, 0.21f, 0.21f);
			Widgets.OptionUnselectedBGBorderColor = Widgets.OptionUnselectedBGFillColor * 1.8f;
			Widgets.OptionSelectedBGFillColor = new Color(0.32f, 0.28f, 0.21f);
			Widgets.OptionSelectedBGBorderColor = Widgets.OptionSelectedBGFillColor * 1.8f;
			Widgets.dropdownPainting = false;
			Widgets.dropdownPainting_Payload = null;
			Widgets.dropdownPainting_Type = null;
			Widgets.dropdownPainting_Text = "";
			Widgets.dropdownPainting_Icon = null;
			Color color = new Color(1f, 1f, 1f, 0f);
			Widgets.LineTexAA = new Texture2D(1, 3, TextureFormat.ARGB32, false);
			Widgets.LineTexAA.name = "LineTexAA";
			Widgets.LineTexAA.SetPixel(0, 0, color);
			Widgets.LineTexAA.SetPixel(0, 1, Color.white);
			Widgets.LineTexAA.SetPixel(0, 2, color);
			Widgets.LineTexAA.Apply();
			Widgets.LineMat = (Material)typeof(GUI).GetMethod("get_blendMaterial", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, null);
		}

		// Token: 0x0600588A RID: 22666 RVA: 0x002D5BBC File Offset: 0x002D3FBC
		public static void ThingIcon(Rect rect, Thing thing, float alpha = 1f)
		{
			thing = thing.GetInnerIfMinified();
			GUI.color = thing.DrawColor;
			float resolvedIconAngle = 0f;
			Texture resolvedIcon;
			if (!thing.def.uiIconPath.NullOrEmpty())
			{
				resolvedIcon = thing.def.uiIcon;
				resolvedIconAngle = thing.def.uiIconAngle;
			}
			else if (thing is Pawn || thing is Corpse)
			{
				Pawn pawn = thing as Pawn;
				if (pawn == null)
				{
					pawn = ((Corpse)thing).InnerPawn;
				}
				if (!pawn.RaceProps.Humanlike)
				{
					if (!pawn.Drawer.renderer.graphics.AllResolved)
					{
						pawn.Drawer.renderer.graphics.ResolveAllGraphics();
					}
					Material material = pawn.Drawer.renderer.graphics.nakedGraphic.MatAt(Rot4.East, null);
					resolvedIcon = material.mainTexture;
					GUI.color = material.color;
				}
				else
				{
					rect = rect.ScaledBy(1.8f);
					rect.y += 3f;
					rect = rect.Rounded();
					resolvedIcon = PortraitsCache.Get(pawn, new Vector2(rect.width, rect.height), default(Vector3), 1f);
				}
			}
			else
			{
				resolvedIcon = thing.Graphic.ExtractInnerGraphicFor(thing).MatAt(thing.def.defaultPlacingRot, null).mainTexture;
			}
			if (alpha != 1f)
			{
				Color color = GUI.color;
				color.a *= alpha;
				GUI.color = color;
			}
			Widgets.ThingIconWorker(rect, thing.def, resolvedIcon, resolvedIconAngle);
			GUI.color = Color.white;
		}

		// Token: 0x0600588B RID: 22667 RVA: 0x002D5D80 File Offset: 0x002D4180
		public static void ThingIcon(Rect rect, ThingDef thingDef)
		{
			if (!(thingDef.uiIcon == null) && !(thingDef.uiIcon == BaseContent.BadTex))
			{
				GUI.color = thingDef.uiIconColor;
				Widgets.ThingIconWorker(rect, thingDef, thingDef.uiIcon, thingDef.uiIconAngle);
				GUI.color = Color.white;
			}
		}

		// Token: 0x0600588C RID: 22668 RVA: 0x002D5DE4 File Offset: 0x002D41E4
		private static void ThingIconWorker(Rect rect, ThingDef thingDef, Texture resolvedIcon, float resolvedIconAngle)
		{
			float num = GenUI.IconDrawScale(thingDef);
			if (num != 1f)
			{
				Vector2 center = rect.center;
				rect.width *= num;
				rect.height *= num;
				rect.center = center;
			}
			rect.position += new Vector2(thingDef.uiIconOffset.x * rect.size.x, thingDef.uiIconOffset.y * rect.size.y);
			Widgets.DrawTextureRotated(rect, resolvedIcon, resolvedIconAngle);
		}

		// Token: 0x0600588D RID: 22669 RVA: 0x002D5E89 File Offset: 0x002D4289
		public static void DrawAltRect(Rect rect)
		{
			GUI.DrawTexture(rect, Widgets.AltTexture);
		}

		// Token: 0x0600588E RID: 22670 RVA: 0x002D5E98 File Offset: 0x002D4298
		public static void ListSeparator(ref float curY, float width, string label)
		{
			Color color = GUI.color;
			curY += 3f;
			GUI.color = Widgets.SeparatorLabelColor;
			Rect rect = new Rect(0f, curY, width, 30f);
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.Label(rect, label);
			curY += 20f;
			GUI.color = Widgets.SeparatorLineColor;
			Widgets.DrawLineHorizontal(0f, curY, width);
			curY += 2f;
			GUI.color = color;
		}

		// Token: 0x0600588F RID: 22671 RVA: 0x002D5F14 File Offset: 0x002D4314
		public static void DrawLine(Vector2 start, Vector2 end, Color color, float width)
		{
			float num = end.x - start.x;
			float num2 = end.y - start.y;
			float num3 = Mathf.Sqrt(num * num + num2 * num2);
			if (num3 >= 0.01f)
			{
				width *= 3f;
				float num4 = width * num2 / num3;
				float num5 = width * num / num3;
				Matrix4x4 identity = Matrix4x4.identity;
				identity.m00 = num;
				identity.m01 = -num4;
				identity.m03 = start.x + 0.5f * num4;
				identity.m10 = num2;
				identity.m11 = num5;
				identity.m13 = start.y - 0.5f * num5;
				GL.PushMatrix();
				GL.MultMatrix(identity);
				Graphics.DrawTexture(Widgets.LineRect, Widgets.LineTexAA, Widgets.LineRect, 0, 0, 0, 0, color, Widgets.LineMat);
				GL.PopMatrix();
			}
		}

		// Token: 0x06005890 RID: 22672 RVA: 0x002D5FFC File Offset: 0x002D43FC
		public static void DrawLineHorizontal(float x, float y, float length)
		{
			Rect position = new Rect(x, y, length, 1f);
			GUI.DrawTexture(position, BaseContent.WhiteTex);
		}

		// Token: 0x06005891 RID: 22673 RVA: 0x002D6024 File Offset: 0x002D4424
		public static void DrawLineVertical(float x, float y, float length)
		{
			Rect position = new Rect(x, y, 1f, length);
			GUI.DrawTexture(position, BaseContent.WhiteTex);
		}

		// Token: 0x06005892 RID: 22674 RVA: 0x002D604C File Offset: 0x002D444C
		public static void DrawBoxSolid(Rect rect, Color color)
		{
			Color color2 = GUI.color;
			GUI.color = color;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = color2;
		}

		// Token: 0x06005893 RID: 22675 RVA: 0x002D6078 File Offset: 0x002D4478
		public static void DrawBox(Rect rect, int thickness = 1)
		{
			Vector2 b = new Vector2(rect.x, rect.y);
			Vector2 a = new Vector2(rect.x + rect.width, rect.y + rect.height);
			if (b.x > a.x)
			{
				float x = b.x;
				b.x = a.x;
				a.x = x;
			}
			if (b.y > a.y)
			{
				float y = b.y;
				b.y = a.y;
				a.y = y;
			}
			Vector3 vector = a - b;
			GUI.DrawTexture(new Rect(b.x, b.y, (float)thickness, vector.y), BaseContent.WhiteTex);
			GUI.DrawTexture(new Rect(a.x - (float)thickness, b.y, (float)thickness, vector.y), BaseContent.WhiteTex);
			GUI.DrawTexture(new Rect(b.x + (float)thickness, b.y, vector.x - (float)(thickness * 2), (float)thickness), BaseContent.WhiteTex);
			GUI.DrawTexture(new Rect(b.x + (float)thickness, a.y - (float)thickness, vector.x - (float)(thickness * 2), (float)thickness), BaseContent.WhiteTex);
		}

		// Token: 0x06005894 RID: 22676 RVA: 0x002D61E4 File Offset: 0x002D45E4
		public static void LabelCacheHeight(ref Rect rect, string label, bool renderLabel = true, bool forceInvalidation = false)
		{
			bool flag = Widgets.LabelCache.ContainsKey(label);
			if (forceInvalidation)
			{
				flag = false;
			}
			float height;
			if (flag)
			{
				height = Widgets.LabelCache[label];
			}
			else
			{
				height = Text.CalcHeight(label, rect.width);
			}
			rect.height = height;
			if (renderLabel)
			{
				Widgets.Label(rect, label);
			}
		}

		// Token: 0x06005895 RID: 22677 RVA: 0x002D6249 File Offset: 0x002D4649
		public static void Label(Rect rect, GUIContent content)
		{
			GUI.Label(rect, content, Text.CurFontStyle);
		}

		// Token: 0x06005896 RID: 22678 RVA: 0x002D6258 File Offset: 0x002D4658
		public static void Label(Rect rect, string label)
		{
			GUI.Label(rect, label, Text.CurFontStyle);
		}

		// Token: 0x06005897 RID: 22679 RVA: 0x002D6268 File Offset: 0x002D4668
		public static void LabelScrollable(Rect rect, string label, ref Vector2 scrollbarPosition, bool dontConsumeScrollEventsIfNoScrollbar = false)
		{
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f, Mathf.Max(Text.CalcHeight(label, rect.width) + 10f, rect.height));
			bool flag = !dontConsumeScrollEventsIfNoScrollbar || Text.CalcHeight(label, rect.width) > rect.height;
			if (flag)
			{
				Widgets.BeginScrollView(rect, ref scrollbarPosition, rect2, true);
			}
			else
			{
				GUI.BeginGroup(rect);
			}
			Widgets.Label(rect2, label);
			if (flag)
			{
				Widgets.EndScrollView();
			}
			else
			{
				GUI.EndGroup();
			}
		}

		// Token: 0x06005898 RID: 22680 RVA: 0x002D6309 File Offset: 0x002D4709
		public static void Checkbox(Vector2 topLeft, ref bool checkOn, float size = 24f, bool disabled = false, bool paintable = false, Texture2D texChecked = null, Texture2D texUnchecked = null)
		{
			Widgets.Checkbox(topLeft.x, topLeft.y, ref checkOn, size, disabled, paintable, texChecked, texUnchecked);
		}

		// Token: 0x06005899 RID: 22681 RVA: 0x002D6328 File Offset: 0x002D4728
		public static void Checkbox(float x, float y, ref bool checkOn, float size = 24f, bool disabled = false, bool paintable = false, Texture2D texChecked = null, Texture2D texUnchecked = null)
		{
			if (disabled)
			{
				GUI.color = Widgets.InactiveColor;
			}
			Rect rect = new Rect(x, y, size, size);
			Widgets.CheckboxDraw(x, y, checkOn, disabled, size, texChecked, texUnchecked);
			if (!disabled)
			{
				MouseoverSounds.DoRegion(rect);
				bool flag = false;
				Widgets.DraggableResult draggableResult = Widgets.ButtonInvisibleDraggable(rect, false);
				if (draggableResult == Widgets.DraggableResult.Pressed)
				{
					checkOn = !checkOn;
					flag = true;
				}
				else if (draggableResult == Widgets.DraggableResult.Dragged && paintable)
				{
					checkOn = !checkOn;
					flag = true;
					Widgets.checkboxPainting = true;
					Widgets.checkboxPaintingState = checkOn;
				}
				if (paintable && Mouse.IsOver(rect))
				{
					if (Widgets.checkboxPainting && Input.GetMouseButton(0) && checkOn != Widgets.checkboxPaintingState)
					{
						checkOn = Widgets.checkboxPaintingState;
						flag = true;
					}
				}
				if (flag)
				{
					if (checkOn)
					{
						SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
					}
					else
					{
						SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
					}
				}
			}
			if (disabled)
			{
				GUI.color = Color.white;
			}
		}

		// Token: 0x0600589A RID: 22682 RVA: 0x002D6434 File Offset: 0x002D4834
		public static void CheckboxLabeled(Rect rect, string label, ref bool checkOn, bool disabled = false, Texture2D texChecked = null, Texture2D texUnchecked = null, bool placeCheckboxNearText = false)
		{
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleLeft;
			if (placeCheckboxNearText)
			{
				rect.width = Mathf.Min(rect.width, Text.CalcSize(label).x + 24f + 10f);
			}
			Widgets.Label(rect, label);
			if (!disabled)
			{
				if (Widgets.ButtonInvisible(rect, false))
				{
					checkOn = !checkOn;
					if (checkOn)
					{
						SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
					}
					else
					{
						SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
					}
				}
			}
			Widgets.CheckboxDraw(rect.x + rect.width - 24f, rect.y, checkOn, disabled, 24f, null, null);
			Text.Anchor = anchor;
		}

		// Token: 0x0600589B RID: 22683 RVA: 0x002D64FC File Offset: 0x002D48FC
		public static bool CheckboxLabeledSelectable(Rect rect, string label, ref bool selected, ref bool checkOn)
		{
			if (selected)
			{
				Widgets.DrawHighlight(rect);
			}
			Widgets.Label(rect, label);
			bool flag = selected;
			Rect butRect = rect;
			butRect.width -= 24f;
			if (!selected && Widgets.ButtonInvisible(butRect, false))
			{
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
				selected = true;
			}
			Color color = GUI.color;
			GUI.color = Color.white;
			Widgets.CheckboxDraw(rect.xMax - 24f, rect.y, checkOn, false, 24f, null, null);
			GUI.color = color;
			Rect butRect2 = new Rect(rect.xMax - 24f, rect.y, 24f, 24f);
			if (Widgets.ButtonInvisible(butRect2, false))
			{
				checkOn = !checkOn;
				if (checkOn)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
			}
			return selected && !flag;
		}

		// Token: 0x0600589C RID: 22684 RVA: 0x002D6608 File Offset: 0x002D4A08
		private static void CheckboxDraw(float x, float y, bool active, bool disabled, float size = 24f, Texture2D texChecked = null, Texture2D texUnchecked = null)
		{
			Color color = GUI.color;
			if (disabled)
			{
				GUI.color = Widgets.InactiveColor;
			}
			Texture2D image;
			if (active)
			{
				image = ((!(texChecked != null)) ? Widgets.CheckboxOnTex : texChecked);
			}
			else
			{
				image = ((!(texUnchecked != null)) ? Widgets.CheckboxOffTex : texUnchecked);
			}
			Rect position = new Rect(x, y, size, size);
			GUI.DrawTexture(position, image);
			if (disabled)
			{
				GUI.color = color;
			}
		}

		// Token: 0x0600589D RID: 22685 RVA: 0x002D668C File Offset: 0x002D4A8C
		public static MultiCheckboxState CheckboxMulti(Rect rect, MultiCheckboxState state, bool paintable = false)
		{
			Texture2D tex;
			if (state == MultiCheckboxState.On)
			{
				tex = Widgets.CheckboxOnTex;
			}
			else if (state == MultiCheckboxState.Off)
			{
				tex = Widgets.CheckboxOffTex;
			}
			else
			{
				tex = Widgets.CheckboxPartialTex;
			}
			MouseoverSounds.DoRegion(rect);
			MultiCheckboxState multiCheckboxState = (state != MultiCheckboxState.Off) ? MultiCheckboxState.Off : MultiCheckboxState.On;
			bool flag = false;
			Widgets.DraggableResult draggableResult = Widgets.ButtonImageDraggable(rect, tex);
			if (paintable && draggableResult == Widgets.DraggableResult.Dragged)
			{
				Widgets.checkboxPainting = true;
				Widgets.checkboxPaintingState = (multiCheckboxState == MultiCheckboxState.On);
				flag = true;
			}
			else if (draggableResult.AnyPressed())
			{
				flag = true;
			}
			else if (paintable && Widgets.checkboxPainting && Mouse.IsOver(rect))
			{
				multiCheckboxState = ((!Widgets.checkboxPaintingState) ? MultiCheckboxState.Off : MultiCheckboxState.On);
				if (state != multiCheckboxState)
				{
					flag = true;
				}
			}
			MultiCheckboxState result;
			if (flag)
			{
				if (multiCheckboxState == MultiCheckboxState.On)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
				result = multiCheckboxState;
			}
			else
			{
				result = state;
			}
			return result;
		}

		// Token: 0x0600589E RID: 22686 RVA: 0x002D6790 File Offset: 0x002D4B90
		public static bool RadioButton(Vector2 topLeft, bool chosen)
		{
			return Widgets.RadioButton(topLeft.x, topLeft.y, chosen);
		}

		// Token: 0x0600589F RID: 22687 RVA: 0x002D67BC File Offset: 0x002D4BBC
		public static bool RadioButton(float x, float y, bool chosen)
		{
			Rect rect = new Rect(x, y, 24f, 24f);
			MouseoverSounds.DoRegion(rect);
			Widgets.RadioButtonDraw(x, y, chosen);
			bool flag = Widgets.ButtonInvisible(rect, false);
			if (flag && !chosen)
			{
				SoundDefOf.RadioButtonClicked.PlayOneShotOnCamera(null);
			}
			return flag;
		}

		// Token: 0x060058A0 RID: 22688 RVA: 0x002D6814 File Offset: 0x002D4C14
		public static bool RadioButtonLabeled(Rect rect, string labelText, bool chosen)
		{
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect, labelText);
			Text.Anchor = anchor;
			bool flag = Widgets.ButtonInvisible(rect, false);
			if (flag && !chosen)
			{
				SoundDefOf.RadioButtonClicked.PlayOneShotOnCamera(null);
			}
			Widgets.RadioButtonDraw(rect.x + rect.width - 24f, rect.y + rect.height / 2f - 12f, chosen);
			return flag;
		}

		// Token: 0x060058A1 RID: 22689 RVA: 0x002D6898 File Offset: 0x002D4C98
		private static void RadioButtonDraw(float x, float y, bool chosen)
		{
			Texture2D image;
			if (chosen)
			{
				image = Widgets.RadioButOnTex;
			}
			else
			{
				image = Widgets.RadioButOffTex;
			}
			Rect position = new Rect(x, y, 24f, 24f);
			GUI.DrawTexture(position, image);
		}

		// Token: 0x060058A2 RID: 22690 RVA: 0x002D68D8 File Offset: 0x002D4CD8
		public static bool ButtonText(Rect rect, string label, bool drawBackground = true, bool doMouseoverSound = false, bool active = true)
		{
			return Widgets.ButtonText(rect, label, drawBackground, doMouseoverSound, Widgets.NormalOptionColor, active);
		}

		// Token: 0x060058A3 RID: 22691 RVA: 0x002D6900 File Offset: 0x002D4D00
		public static bool ButtonText(Rect rect, string label, bool drawBackground, bool doMouseoverSound, Color textColor, bool active = true)
		{
			return Widgets.ButtonTextWorker(rect, label, drawBackground, doMouseoverSound, textColor, active, false).AnyPressed();
		}

		// Token: 0x060058A4 RID: 22692 RVA: 0x002D6928 File Offset: 0x002D4D28
		public static Widgets.DraggableResult ButtonTextDraggable(Rect rect, string label, bool drawBackground = true, bool doMouseoverSound = false, bool active = true)
		{
			return Widgets.ButtonTextDraggable(rect, label, drawBackground, doMouseoverSound, Widgets.NormalOptionColor, active);
		}

		// Token: 0x060058A5 RID: 22693 RVA: 0x002D6950 File Offset: 0x002D4D50
		public static Widgets.DraggableResult ButtonTextDraggable(Rect rect, string label, bool drawBackground, bool doMouseoverSound, Color textColor, bool active = true)
		{
			return Widgets.ButtonTextWorker(rect, label, drawBackground, doMouseoverSound, Widgets.NormalOptionColor, active, true);
		}

		// Token: 0x060058A6 RID: 22694 RVA: 0x002D6978 File Offset: 0x002D4D78
		private static Widgets.DraggableResult ButtonTextWorker(Rect rect, string label, bool drawBackground, bool doMouseoverSound, Color textColor, bool active, bool draggable)
		{
			TextAnchor anchor = Text.Anchor;
			Color color = GUI.color;
			if (drawBackground)
			{
				Texture2D atlas = Widgets.ButtonBGAtlas;
				if (Mouse.IsOver(rect))
				{
					atlas = Widgets.ButtonBGAtlasMouseover;
					if (Input.GetMouseButton(0))
					{
						atlas = Widgets.ButtonBGAtlasClick;
					}
				}
				Widgets.DrawAtlas(rect, atlas);
			}
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(rect);
			}
			if (!drawBackground)
			{
				GUI.color = textColor;
				if (Mouse.IsOver(rect))
				{
					GUI.color = Widgets.MouseoverOptionColor;
				}
			}
			if (drawBackground)
			{
				Text.Anchor = TextAnchor.MiddleCenter;
			}
			else
			{
				Text.Anchor = TextAnchor.MiddleLeft;
			}
			Widgets.Label(rect, label);
			Text.Anchor = anchor;
			GUI.color = color;
			Widgets.DraggableResult result;
			if (active && draggable)
			{
				result = Widgets.ButtonInvisibleDraggable(rect, false);
			}
			else if (active)
			{
				result = ((!Widgets.ButtonInvisible(rect, false)) ? Widgets.DraggableResult.Idle : Widgets.DraggableResult.Pressed);
			}
			else
			{
				result = Widgets.DraggableResult.Idle;
			}
			return result;
		}

		// Token: 0x060058A7 RID: 22695 RVA: 0x002D6A68 File Offset: 0x002D4E68
		public static void DrawRectFast(Rect position, Color color, GUIContent content = null)
		{
			Color backgroundColor = GUI.backgroundColor;
			GUI.backgroundColor = color;
			GUI.Box(position, content ?? GUIContent.none, TexUI.FastFillStyle);
			GUI.backgroundColor = backgroundColor;
		}

		// Token: 0x060058A8 RID: 22696 RVA: 0x002D6AA0 File Offset: 0x002D4EA0
		public static bool CustomButtonText(ref Rect rect, string label, Color bgColor, Color textColor, Color borderColor, bool cacheHeight = false, int borderSize = 1, bool doMouseoverSound = true, bool active = true)
		{
			if (cacheHeight)
			{
				Widgets.LabelCacheHeight(ref rect, label, false, false);
			}
			Rect position = new Rect(rect);
			position.x += (float)borderSize;
			position.y += (float)borderSize;
			position.width -= (float)(borderSize * 2);
			position.height -= (float)(borderSize * 2);
			Widgets.DrawRectFast(rect, borderColor, null);
			Widgets.DrawRectFast(position, bgColor, null);
			TextAnchor anchor = Text.Anchor;
			Color color = GUI.color;
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(rect);
			}
			GUI.color = textColor;
			if (Mouse.IsOver(rect))
			{
				GUI.color = Widgets.MouseoverOptionColor;
			}
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, label);
			Text.Anchor = anchor;
			GUI.color = color;
			return active && Widgets.ButtonInvisible(rect, false);
		}

		// Token: 0x060058A9 RID: 22697 RVA: 0x002D6BA8 File Offset: 0x002D4FA8
		public static bool ButtonTextSubtle(Rect rect, string label, float barPercent = 0f, float textLeftMargin = -1f, SoundDef mouseoverSound = null, Vector2 functionalSizeOffset = default(Vector2))
		{
			Rect rect2 = rect;
			rect2.width += functionalSizeOffset.x;
			rect2.height += functionalSizeOffset.y;
			Profiler.BeginSample("ButtonTextSubtle");
			bool flag = false;
			if (Mouse.IsOver(rect2))
			{
				flag = true;
				GUI.color = GenUI.MouseoverColor;
			}
			if (mouseoverSound != null)
			{
				MouseoverSounds.DoRegion(rect2, mouseoverSound);
			}
			Profiler.BeginSample("atlas");
			Widgets.DrawAtlas(rect, Widgets.ButtonSubtleAtlas);
			Profiler.EndSample();
			GUI.color = Color.white;
			if (barPercent > 0.001f)
			{
				Rect rect3 = rect.ContractedBy(1f);
				Widgets.FillableBar(rect3, barPercent, Widgets.ButtonBarTex, null, false);
			}
			Rect rect4 = new Rect(rect);
			if (textLeftMargin < 0f)
			{
				textLeftMargin = rect.width * 0.15f;
			}
			rect4.x += textLeftMargin;
			if (flag)
			{
				rect4.x += 2f;
				rect4.y -= 2f;
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Text.Font = GameFont.Small;
			Widgets.Label(rect4, label);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.WordWrap = true;
			bool result = Widgets.ButtonInvisible(rect2, false);
			Profiler.EndSample();
			return result;
		}

		// Token: 0x060058AA RID: 22698 RVA: 0x002D6D00 File Offset: 0x002D5100
		public static bool ButtonImage(Rect butRect, Texture2D tex)
		{
			return Widgets.ButtonImage(butRect, tex, Color.white);
		}

		// Token: 0x060058AB RID: 22699 RVA: 0x002D6D24 File Offset: 0x002D5124
		public static bool ButtonImage(Rect butRect, Texture2D tex, Color baseColor)
		{
			return Widgets.ButtonImage(butRect, tex, baseColor, GenUI.MouseoverColor);
		}

		// Token: 0x060058AC RID: 22700 RVA: 0x002D6D48 File Offset: 0x002D5148
		public static bool ButtonImage(Rect butRect, Texture2D tex, Color baseColor, Color mouseoverColor)
		{
			if (Mouse.IsOver(butRect))
			{
				GUI.color = mouseoverColor;
			}
			else
			{
				GUI.color = baseColor;
			}
			GUI.DrawTexture(butRect, tex);
			GUI.color = baseColor;
			return Widgets.ButtonInvisible(butRect, false);
		}

		// Token: 0x060058AD RID: 22701 RVA: 0x002D6D90 File Offset: 0x002D5190
		public static Widgets.DraggableResult ButtonImageDraggable(Rect butRect, Texture2D tex)
		{
			return Widgets.ButtonImageDraggable(butRect, tex, Color.white);
		}

		// Token: 0x060058AE RID: 22702 RVA: 0x002D6DB4 File Offset: 0x002D51B4
		public static Widgets.DraggableResult ButtonImageDraggable(Rect butRect, Texture2D tex, Color baseColor)
		{
			return Widgets.ButtonImageDraggable(butRect, tex, baseColor, GenUI.MouseoverColor);
		}

		// Token: 0x060058AF RID: 22703 RVA: 0x002D6DD8 File Offset: 0x002D51D8
		public static Widgets.DraggableResult ButtonImageDraggable(Rect butRect, Texture2D tex, Color baseColor, Color mouseoverColor)
		{
			if (Mouse.IsOver(butRect))
			{
				GUI.color = mouseoverColor;
			}
			else
			{
				GUI.color = baseColor;
			}
			GUI.DrawTexture(butRect, tex);
			GUI.color = baseColor;
			return Widgets.ButtonInvisibleDraggable(butRect, false);
		}

		// Token: 0x060058B0 RID: 22704 RVA: 0x002D6E20 File Offset: 0x002D5220
		public static bool ButtonImageFitted(Rect butRect, Texture2D tex)
		{
			return Widgets.ButtonImageFitted(butRect, tex, Color.white);
		}

		// Token: 0x060058B1 RID: 22705 RVA: 0x002D6E44 File Offset: 0x002D5244
		public static bool ButtonImageFitted(Rect butRect, Texture2D tex, Color baseColor)
		{
			return Widgets.ButtonImageFitted(butRect, tex, baseColor, GenUI.MouseoverColor);
		}

		// Token: 0x060058B2 RID: 22706 RVA: 0x002D6E68 File Offset: 0x002D5268
		public static bool ButtonImageFitted(Rect butRect, Texture2D tex, Color baseColor, Color mouseoverColor)
		{
			if (Mouse.IsOver(butRect))
			{
				GUI.color = mouseoverColor;
			}
			else
			{
				GUI.color = baseColor;
			}
			Widgets.DrawTextureFitted(butRect, tex, 1f);
			GUI.color = baseColor;
			return Widgets.ButtonInvisible(butRect, false);
		}

		// Token: 0x060058B3 RID: 22707 RVA: 0x002D6EB4 File Offset: 0x002D52B4
		public static bool ButtonImageWithBG(Rect butRect, Texture2D image, Vector2? imageSize = null)
		{
			bool result = Widgets.ButtonText(butRect, "", true, false, true);
			Rect position;
			if (imageSize != null)
			{
				position = new Rect(Mathf.Floor(butRect.x + butRect.width / 2f - imageSize.Value.x / 2f), Mathf.Floor(butRect.y + butRect.height / 2f - imageSize.Value.y / 2f), imageSize.Value.x, imageSize.Value.y);
			}
			else
			{
				position = butRect;
			}
			GUI.DrawTexture(position, image);
			return result;
		}

		// Token: 0x060058B4 RID: 22708 RVA: 0x002D6F80 File Offset: 0x002D5380
		public static bool CloseButtonFor(Rect rectToClose)
		{
			Rect butRect = new Rect(rectToClose.x + rectToClose.width - 18f - 4f, rectToClose.y + 4f, 18f, 18f);
			return Widgets.ButtonImage(butRect, TexButton.CloseXSmall);
		}

		// Token: 0x060058B5 RID: 22709 RVA: 0x002D6FDC File Offset: 0x002D53DC
		public static bool ButtonInvisible(Rect butRect, bool doMouseoverSound = false)
		{
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(butRect);
			}
			return GUI.Button(butRect, "", Widgets.EmptyStyle);
		}

		// Token: 0x060058B6 RID: 22710 RVA: 0x002D7010 File Offset: 0x002D5410
		public static Widgets.DraggableResult ButtonInvisibleDraggable(Rect butRect, bool doMouseoverSound = false)
		{
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(butRect);
			}
			int controlID = GUIUtility.GetControlID(FocusType.Passive, butRect);
			if (Input.GetMouseButtonDown(0) && Mouse.IsOver(butRect))
			{
				Widgets.buttonInvisibleDraggable_activeControl = controlID;
				Widgets.buttonInvisibleDraggable_mouseStart = Input.mousePosition;
				Widgets.buttonInvisibleDraggable_dragged = false;
			}
			if (Widgets.buttonInvisibleDraggable_activeControl == controlID)
			{
				if (Input.GetMouseButtonUp(0))
				{
					Widgets.buttonInvisibleDraggable_activeControl = 0;
					if (Mouse.IsOver(butRect))
					{
						return (!Widgets.buttonInvisibleDraggable_dragged) ? Widgets.DraggableResult.Pressed : Widgets.DraggableResult.DraggedThenPressed;
					}
					return Widgets.DraggableResult.Idle;
				}
				else
				{
					if (!Input.GetMouseButton(0))
					{
						Widgets.buttonInvisibleDraggable_activeControl = 0;
						return Widgets.DraggableResult.Idle;
					}
					if (!Widgets.buttonInvisibleDraggable_dragged && (Widgets.buttonInvisibleDraggable_mouseStart - Input.mousePosition).sqrMagnitude > Widgets.DragStartDistanceSquared)
					{
						Widgets.buttonInvisibleDraggable_dragged = true;
						return Widgets.DraggableResult.Dragged;
					}
				}
			}
			return Widgets.DraggableResult.Idle;
		}

		// Token: 0x060058B7 RID: 22711 RVA: 0x002D7104 File Offset: 0x002D5504
		public static string TextField(Rect rect, string text)
		{
			if (text == null)
			{
				text = "";
			}
			return GUI.TextField(rect, text, Text.CurTextFieldStyle);
		}

		// Token: 0x060058B8 RID: 22712 RVA: 0x002D7134 File Offset: 0x002D5534
		public static string TextField(Rect rect, string text, int maxLength, Regex inputValidator)
		{
			string text2 = Widgets.TextField(rect, text);
			string result;
			if (text2.Length <= maxLength && inputValidator.IsMatch(text2))
			{
				result = text2;
			}
			else
			{
				result = text;
			}
			return result;
		}

		// Token: 0x060058B9 RID: 22713 RVA: 0x002D7174 File Offset: 0x002D5574
		public static string TextArea(Rect rect, string text, bool readOnly = false)
		{
			if (text == null)
			{
				text = "";
			}
			return GUI.TextArea(rect, text, (!readOnly) ? Text.CurTextAreaStyle : Text.CurTextAreaReadOnlyStyle);
		}

		// Token: 0x060058BA RID: 22714 RVA: 0x002D71B4 File Offset: 0x002D55B4
		public static string TextAreaScrollable(Rect rect, string text, ref Vector2 scrollbarPosition, bool readOnly = false)
		{
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f, Mathf.Max(Text.CalcHeight(text, rect.width) + 10f, rect.height));
			Widgets.BeginScrollView(rect, ref scrollbarPosition, rect2, true);
			string result = Widgets.TextArea(rect2, text, readOnly);
			Widgets.EndScrollView();
			return result;
		}

		// Token: 0x060058BB RID: 22715 RVA: 0x002D7220 File Offset: 0x002D5620
		public static string TextEntryLabeled(Rect rect, string label, string text)
		{
			Rect rect2 = rect.LeftHalf().Rounded();
			Rect rect3 = rect.RightHalf().Rounded();
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect2, label);
			Text.Anchor = anchor;
			string result;
			if (rect.height <= 30f)
			{
				result = Widgets.TextField(rect3, text);
			}
			else
			{
				result = Widgets.TextArea(rect3, text, false);
			}
			return result;
		}

		// Token: 0x060058BC RID: 22716 RVA: 0x002D728C File Offset: 0x002D568C
		public static void TextFieldNumeric<T>(Rect rect, ref T val, ref string buffer, float min = 0f, float max = 1E+09f) where T : struct
		{
			if (buffer == null)
			{
				buffer = val.ToString();
			}
			string text = "TextField" + rect.y.ToString("F0") + rect.x.ToString("F0");
			GUI.SetNextControlName(text);
			string text2 = GUI.TextField(rect, buffer, Text.CurTextFieldStyle);
			if (GUI.GetNameOfFocusedControl() != text)
			{
				Widgets.ResolveParseNow<T>(buffer, ref val, ref buffer, min, max, true);
			}
			else if (text2 != buffer && Widgets.IsPartiallyOrFullyTypedNumber<T>(ref val, text2, min, max))
			{
				buffer = text2;
				if (text2.IsFullyTypedNumber<T>())
				{
					Widgets.ResolveParseNow<T>(text2, ref val, ref buffer, min, max, false);
				}
			}
		}

		// Token: 0x060058BD RID: 22717 RVA: 0x002D7354 File Offset: 0x002D5754
		private static void ResolveParseNow<T>(string edited, ref T val, ref string buffer, float min, float max, bool force)
		{
			if (typeof(T) == typeof(int))
			{
				int num;
				if (edited.NullOrEmpty())
				{
					Widgets.ResetValue<T>(edited, ref val, ref buffer, min, max);
				}
				else if (int.TryParse(edited, out num))
				{
					val = (T)((object)Mathf.RoundToInt(Mathf.Clamp((float)num, min, max)));
					buffer = Widgets.ToStringTypedIn<T>(val);
				}
				else if (force)
				{
					Widgets.ResetValue<T>(edited, ref val, ref buffer, min, max);
				}
			}
			else if (typeof(T) == typeof(float))
			{
				float value;
				if (float.TryParse(edited, out value))
				{
					val = (T)((object)Mathf.Clamp(value, min, max));
					buffer = Widgets.ToStringTypedIn<T>(val);
				}
				else if (force)
				{
					Widgets.ResetValue<T>(edited, ref val, ref buffer, min, max);
				}
			}
			else
			{
				Log.Error("TextField<T> does not support " + typeof(T), false);
			}
		}

		// Token: 0x060058BE RID: 22718 RVA: 0x002D7474 File Offset: 0x002D5874
		private static void ResetValue<T>(string edited, ref T val, ref string buffer, float min, float max)
		{
			val = default(T);
			if (min > 0f)
			{
				val = (T)((object)Mathf.RoundToInt(min));
			}
			if (max < 0f)
			{
				val = (T)((object)Mathf.RoundToInt(max));
			}
			buffer = Widgets.ToStringTypedIn<T>(val);
		}

		// Token: 0x060058BF RID: 22719 RVA: 0x002D74E4 File Offset: 0x002D58E4
		private static string ToStringTypedIn<T>(T val)
		{
			string result;
			if (typeof(T) == typeof(float))
			{
				result = ((float)((object)val)).ToString("0.##########");
			}
			else
			{
				result = val.ToString();
			}
			return result;
		}

		// Token: 0x060058C0 RID: 22720 RVA: 0x002D7540 File Offset: 0x002D5940
		private static bool IsPartiallyOrFullyTypedNumber<T>(ref T val, string s, float min, float max)
		{
			bool result;
			if (s == "")
			{
				result = true;
			}
			else if (s[0] == '-' && min >= 0f)
			{
				result = false;
			}
			else if (s.Length > 1 && s[s.Length - 1] == '-')
			{
				result = false;
			}
			else if (s == "00")
			{
				result = false;
			}
			else if (s.Length > 12)
			{
				result = false;
			}
			else
			{
				if (typeof(T) == typeof(float))
				{
					int num = s.CharacterCount('.');
					if (num <= 1 && s.ContainsOnlyCharacters("-.0123456789"))
					{
						return true;
					}
				}
				result = s.IsFullyTypedNumber<T>();
			}
			return result;
		}

		// Token: 0x060058C1 RID: 22721 RVA: 0x002D7634 File Offset: 0x002D5A34
		private static bool IsFullyTypedNumber<T>(this string s)
		{
			bool result;
			if (s == "")
			{
				result = false;
			}
			else
			{
				if (typeof(T) == typeof(float))
				{
					string[] array = s.Split(new char[]
					{
						'.'
					});
					if (array.Length > 2 || array.Length < 1)
					{
						return false;
					}
					if (!array[0].ContainsOnlyCharacters("-0123456789"))
					{
						return false;
					}
					if (array.Length == 2 && (array[1].Length == 0 || !array[1].ContainsOnlyCharacters("0123456789")))
					{
						return false;
					}
				}
				if (typeof(T) == typeof(int))
				{
					if (!s.ContainsOnlyCharacters("-0123456789"))
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x060058C2 RID: 22722 RVA: 0x002D7724 File Offset: 0x002D5B24
		private static bool ContainsOnlyCharacters(this string s, string allowedChars)
		{
			for (int i = 0; i < s.Length; i++)
			{
				if (!allowedChars.Contains(s[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060058C3 RID: 22723 RVA: 0x002D776C File Offset: 0x002D5B6C
		private static int CharacterCount(this string s, char c)
		{
			int num = 0;
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] == c)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060058C4 RID: 22724 RVA: 0x002D77B0 File Offset: 0x002D5BB0
		public static void TextFieldNumericLabeled<T>(Rect rect, string label, ref T val, ref string buffer, float min = 0f, float max = 1E+09f) where T : struct
		{
			Rect rect2 = rect.LeftHalf().Rounded();
			Rect rect3 = rect.RightHalf().Rounded();
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect2, label);
			Text.Anchor = anchor;
			Widgets.TextFieldNumeric<T>(rect3, ref val, ref buffer, min, max);
		}

		// Token: 0x060058C5 RID: 22725 RVA: 0x002D77FC File Offset: 0x002D5BFC
		public static void TextFieldPercent(Rect rect, ref float val, ref string buffer, float min = 0f, float max = 1f)
		{
			Rect rect2 = new Rect(rect.x, rect.y, rect.width - 25f, rect.height);
			Rect rect3 = new Rect(rect2.xMax, rect.y, 25f, rect2.height);
			Widgets.Label(rect3, "%");
			float num = val * 100f;
			Widgets.TextFieldNumeric<float>(rect2, ref num, ref buffer, min * 100f, max * 100f);
			val = num / 100f;
			if (val > max)
			{
				val = max;
				buffer = val.ToString();
			}
		}

		// Token: 0x060058C6 RID: 22726 RVA: 0x002D78A8 File Offset: 0x002D5CA8
		public static T ChangeType<T>(this object obj)
		{
			return (T)((object)Convert.ChangeType(obj, typeof(T)));
		}

		// Token: 0x060058C7 RID: 22727 RVA: 0x002D78D4 File Offset: 0x002D5CD4
		public static float HorizontalSlider(Rect rect, float value, float leftValue, float rightValue, bool middleAlignment = false, string label = null, string leftAlignedLabel = null, string rightAlignedLabel = null, float roundTo = -1f)
		{
			if (middleAlignment || !label.NullOrEmpty())
			{
				rect.y += Mathf.Round((rect.height - 16f) / 2f);
			}
			if (!label.NullOrEmpty())
			{
				rect.y += 5f;
			}
			float num = GUI.HorizontalSlider(rect, value, leftValue, rightValue);
			if (!label.NullOrEmpty() || !leftAlignedLabel.NullOrEmpty() || !rightAlignedLabel.NullOrEmpty())
			{
				TextAnchor anchor = Text.Anchor;
				GameFont font = Text.Font;
				Text.Font = GameFont.Tiny;
				float num2 = (!label.NullOrEmpty()) ? Text.CalcSize(label).y : 18f;
				rect.y = rect.y - num2 + 3f;
				if (!leftAlignedLabel.NullOrEmpty())
				{
					Text.Anchor = TextAnchor.UpperLeft;
					Widgets.Label(rect, leftAlignedLabel);
				}
				if (!rightAlignedLabel.NullOrEmpty())
				{
					Text.Anchor = TextAnchor.UpperRight;
					Widgets.Label(rect, rightAlignedLabel);
				}
				if (!label.NullOrEmpty())
				{
					Text.Anchor = TextAnchor.UpperCenter;
					Widgets.Label(rect, label);
				}
				Text.Anchor = anchor;
				Text.Font = font;
			}
			if (roundTo > 0f)
			{
				num = (float)Mathf.RoundToInt(num / roundTo) * roundTo;
			}
			return num;
		}

		// Token: 0x060058C8 RID: 22728 RVA: 0x002D7A40 File Offset: 0x002D5E40
		public static float FrequencyHorizontalSlider(Rect rect, float freq, float minFreq, float maxFreq, bool roundToInt = false)
		{
			float num;
			if (freq < 1f)
			{
				float x = 1f / freq;
				num = GenMath.LerpDouble(1f, 1f / minFreq, 0.5f, 1f, x);
			}
			else
			{
				num = GenMath.LerpDouble(maxFreq, 1f, 0f, 0.5f, freq);
			}
			string label;
			if (freq == 1f)
			{
				label = "EveryDay".Translate();
			}
			else if (freq < 1f)
			{
				label = "TimesPerDay".Translate(new object[]
				{
					(1f / freq).ToString("0.##")
				});
			}
			else
			{
				label = "EveryDays".Translate(new object[]
				{
					freq.ToString("0.##")
				});
			}
			float num2 = Widgets.HorizontalSlider(rect, num, 0f, 1f, true, label, null, null, -1f);
			if (num != num2)
			{
				float num3;
				if (num2 < 0.5f)
				{
					num3 = GenMath.LerpDouble(0.5f, 0f, 1f, maxFreq, num2);
					if (roundToInt)
					{
						num3 = Mathf.Round(num3);
					}
				}
				else
				{
					float num4 = GenMath.LerpDouble(1f, 0.5f, 1f / minFreq, 1f, num2);
					if (roundToInt)
					{
						num4 = Mathf.Round(num4);
					}
					num3 = 1f / num4;
				}
				freq = num3;
			}
			return freq;
		}

		// Token: 0x060058C9 RID: 22729 RVA: 0x002D7BBC File Offset: 0x002D5FBC
		public static void IntEntry(Rect rect, ref int value, ref string editBuffer, int multiplier = 1)
		{
			int num = Mathf.Min(Widgets.IntEntryButtonWidth, (int)rect.width / 5);
			if (Widgets.ButtonText(new Rect(rect.xMin, rect.yMin, (float)num, rect.height), (-10 * multiplier).ToStringCached(), true, false, true))
			{
				value -= 10 * multiplier * GenUI.CurrentAdjustmentMultiplier();
				editBuffer = value.ToStringCached();
			}
			if (Widgets.ButtonText(new Rect(rect.xMin + (float)num, rect.yMin, (float)num, rect.height), (-1 * multiplier).ToStringCached(), true, false, true))
			{
				value -= multiplier * GenUI.CurrentAdjustmentMultiplier();
				editBuffer = value.ToStringCached();
			}
			if (Widgets.ButtonText(new Rect(rect.xMax - (float)num, rect.yMin, (float)num, rect.height), (10 * multiplier).ToStringCached(), true, false, true))
			{
				value += 10 * multiplier * GenUI.CurrentAdjustmentMultiplier();
				editBuffer = value.ToStringCached();
			}
			if (Widgets.ButtonText(new Rect(rect.xMax - (float)(num * 2), rect.yMin, (float)num, rect.height), multiplier.ToStringCached(), true, false, true))
			{
				value += multiplier * GenUI.CurrentAdjustmentMultiplier();
				editBuffer = value.ToStringCached();
			}
			Widgets.TextFieldNumeric<int>(new Rect(rect.xMin + (float)(num * 2), rect.yMin, rect.width - (float)(num * 4), rect.height), ref value, ref editBuffer, 0f, 1E+09f);
		}

		// Token: 0x060058CA RID: 22730 RVA: 0x002D7D4C File Offset: 0x002D614C
		public static void FloatRange(Rect rect, int id, ref FloatRange range, float min = 0f, float max = 1f, string labelKey = null, ToStringStyle valueStyle = ToStringStyle.FloatTwo)
		{
			Rect rect2 = rect;
			rect2.xMin += 8f;
			rect2.xMax -= 8f;
			GUI.color = Widgets.RangeControlTextColor;
			string text = range.min.ToStringByStyle(valueStyle, ToStringNumberSense.Absolute) + " - " + range.max.ToStringByStyle(valueStyle, ToStringNumberSense.Absolute);
			if (labelKey != null)
			{
				text = labelKey.Translate(new object[]
				{
					text
				});
			}
			GameFont font = Text.Font;
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.UpperCenter;
			Rect rect3 = rect2;
			rect3.yMin -= 2f;
			Widgets.Label(rect3, text);
			Text.Anchor = TextAnchor.UpperLeft;
			Rect position = new Rect(rect2.x, rect2.yMax - 8f - 1f, rect2.width, 2f);
			GUI.DrawTexture(position, BaseContent.WhiteTex);
			GUI.color = Color.white;
			float num = rect2.x + (rect2.width * range.min - min / (max - min));
			float num2 = rect2.x + (rect2.width * range.max - min / (max - min));
			Rect position2 = new Rect(num - 16f, position.center.y - 8f, 16f, 16f);
			GUI.DrawTexture(position2, Widgets.FloatRangeSliderTex);
			Rect position3 = new Rect(num2 + 16f, position.center.y - 8f, -16f, 16f);
			GUI.DrawTexture(position3, Widgets.FloatRangeSliderTex);
			if (Widgets.curDragEnd != Widgets.RangeEnd.None && (Event.current.type == EventType.MouseUp || Event.current.rawType == EventType.MouseDown))
			{
				Widgets.draggingId = 0;
				Widgets.curDragEnd = Widgets.RangeEnd.None;
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			}
			bool flag = false;
			if (Mouse.IsOver(rect) || Widgets.draggingId == id)
			{
				if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && id != Widgets.draggingId)
				{
					Widgets.draggingId = id;
					float x = Event.current.mousePosition.x;
					if (x < position2.xMax)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Min;
					}
					else if (x > position3.xMin)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Max;
					}
					else
					{
						float num3 = Mathf.Abs(x - position2.xMax);
						float num4 = Mathf.Abs(x - (position3.x - 16f));
						Widgets.curDragEnd = ((num3 >= num4) ? Widgets.RangeEnd.Max : Widgets.RangeEnd.Min);
					}
					flag = true;
					Event.current.Use();
					SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				}
				if (flag || (Widgets.curDragEnd != Widgets.RangeEnd.None && Event.current.type == EventType.MouseDrag))
				{
					float num5 = (Event.current.mousePosition.x - rect2.x) / rect2.width * (max - min) + min;
					num5 = Mathf.Clamp(num5, min, max);
					if (Widgets.curDragEnd == Widgets.RangeEnd.Min)
					{
						if (num5 != range.min)
						{
							range.min = num5;
							if (range.max < range.min)
							{
								range.max = range.min;
							}
							Widgets.CheckPlayDragSliderSound();
						}
					}
					else if (Widgets.curDragEnd == Widgets.RangeEnd.Max)
					{
						if (num5 != range.max)
						{
							range.max = num5;
							if (range.min > range.max)
							{
								range.min = range.max;
							}
							Widgets.CheckPlayDragSliderSound();
						}
					}
					Event.current.Use();
				}
			}
			Text.Font = font;
		}

		// Token: 0x060058CB RID: 22731 RVA: 0x002D8120 File Offset: 0x002D6520
		public static void IntRange(Rect rect, int id, ref IntRange range, int min = 0, int max = 100, string labelKey = null, int minWidth = 0)
		{
			Rect rect2 = rect;
			rect2.xMin += 8f;
			rect2.xMax -= 8f;
			GUI.color = Widgets.RangeControlTextColor;
			string text = range.min.ToStringCached() + " - " + range.max.ToStringCached();
			if (labelKey != null)
			{
				text = labelKey.Translate(new object[]
				{
					text
				});
			}
			GameFont font = Text.Font;
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.UpperCenter;
			Rect rect3 = rect2;
			rect3.yMin -= 2f;
			Widgets.Label(rect3, text);
			Text.Anchor = TextAnchor.UpperLeft;
			Rect position = new Rect(rect2.x, rect2.yMax - 8f - 1f, rect2.width, 2f);
			GUI.DrawTexture(position, BaseContent.WhiteTex);
			GUI.color = Color.white;
			float num = rect2.x + rect2.width * (float)(range.min - min) / (float)(max - min);
			float num2 = rect2.x + rect2.width * (float)(range.max - min) / (float)(max - min);
			Rect position2 = new Rect(num - 16f, position.center.y - 8f, 16f, 16f);
			GUI.DrawTexture(position2, Widgets.FloatRangeSliderTex);
			Rect position3 = new Rect(num2 + 16f, position.center.y - 8f, -16f, 16f);
			GUI.DrawTexture(position3, Widgets.FloatRangeSliderTex);
			if (Widgets.curDragEnd != Widgets.RangeEnd.None && (Event.current.type == EventType.MouseUp || Event.current.rawType == EventType.MouseDown))
			{
				Widgets.draggingId = 0;
				Widgets.curDragEnd = Widgets.RangeEnd.None;
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			}
			bool flag = false;
			if (Mouse.IsOver(rect) || Widgets.draggingId == id)
			{
				if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && id != Widgets.draggingId)
				{
					Widgets.draggingId = id;
					float x = Event.current.mousePosition.x;
					if (x < position2.xMax)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Min;
					}
					else if (x > position3.xMin)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Max;
					}
					else
					{
						float num3 = Mathf.Abs(x - position2.xMax);
						float num4 = Mathf.Abs(x - (position3.x - 16f));
						Widgets.curDragEnd = ((num3 >= num4) ? Widgets.RangeEnd.Max : Widgets.RangeEnd.Min);
					}
					flag = true;
					Event.current.Use();
					SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				}
				if (flag || (Widgets.curDragEnd != Widgets.RangeEnd.None && Event.current.type == EventType.MouseDrag))
				{
					float num5 = (Event.current.mousePosition.x - rect2.x) / rect2.width * (float)(max - min) + (float)min;
					num5 = Mathf.Clamp(num5, (float)min, (float)max);
					int num6 = Mathf.RoundToInt(num5);
					if (Widgets.curDragEnd == Widgets.RangeEnd.Min)
					{
						if (num6 != range.min)
						{
							range.min = num6;
							if (range.min > max - minWidth)
							{
								range.min = max - minWidth;
							}
							int num7 = Mathf.Max(min, range.min + minWidth);
							if (range.max < num7)
							{
								range.max = num7;
							}
							Widgets.CheckPlayDragSliderSound();
						}
					}
					else if (Widgets.curDragEnd == Widgets.RangeEnd.Max)
					{
						if (num6 != range.max)
						{
							range.max = num6;
							if (range.max < min + minWidth)
							{
								range.max = min + minWidth;
							}
							int num8 = Mathf.Min(max, range.max - minWidth);
							if (range.min > num8)
							{
								range.min = num8;
							}
							Widgets.CheckPlayDragSliderSound();
						}
					}
					Event.current.Use();
				}
			}
			Text.Font = font;
		}

		// Token: 0x060058CC RID: 22732 RVA: 0x002D8545 File Offset: 0x002D6945
		private static void CheckPlayDragSliderSound()
		{
			if (Time.realtimeSinceStartup > Widgets.lastDragSliderSoundTime + 0.075f)
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				Widgets.lastDragSliderSoundTime = Time.realtimeSinceStartup;
			}
		}

		// Token: 0x060058CD RID: 22733 RVA: 0x002D8574 File Offset: 0x002D6974
		public static void QualityRange(Rect rect, int id, ref QualityRange range)
		{
			Rect rect2 = rect;
			rect2.xMin += 8f;
			rect2.xMax -= 8f;
			GUI.color = Widgets.RangeControlTextColor;
			string label;
			if (range == RimWorld.QualityRange.All)
			{
				label = "AnyQuality".Translate();
			}
			else if (range.max == range.min)
			{
				label = "OnlyQuality".Translate(new object[]
				{
					range.min.GetLabel()
				});
			}
			else
			{
				label = range.min.GetLabel() + " - " + range.max.GetLabel();
			}
			GameFont font = Text.Font;
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.UpperCenter;
			Rect rect3 = rect2;
			rect3.yMin -= 2f;
			Widgets.Label(rect3, label);
			Text.Anchor = TextAnchor.UpperLeft;
			Rect position = new Rect(rect2.x, rect2.yMax - 8f - 1f, rect2.width, 2f);
			GUI.DrawTexture(position, BaseContent.WhiteTex);
			GUI.color = Color.white;
			int length = Enum.GetValues(typeof(QualityCategory)).Length;
			float num = rect2.x + rect2.width / (float)(length - 1) * (float)range.min;
			float num2 = rect2.x + rect2.width / (float)(length - 1) * (float)range.max;
			Rect position2 = new Rect(num - 16f, position.center.y - 8f, 16f, 16f);
			GUI.DrawTexture(position2, Widgets.FloatRangeSliderTex);
			Rect position3 = new Rect(num2 + 16f, position.center.y - 8f, -16f, 16f);
			GUI.DrawTexture(position3, Widgets.FloatRangeSliderTex);
			if (Widgets.curDragEnd != Widgets.RangeEnd.None && (Event.current.type == EventType.MouseUp || Event.current.type == EventType.MouseDown))
			{
				Widgets.draggingId = 0;
				Widgets.curDragEnd = Widgets.RangeEnd.None;
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			}
			bool flag = false;
			if (Mouse.IsOver(rect) || id == Widgets.draggingId)
			{
				if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && id != Widgets.draggingId)
				{
					Widgets.draggingId = id;
					float x = Event.current.mousePosition.x;
					if (x < position2.xMax)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Min;
					}
					else if (x > position3.xMin)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Max;
					}
					else
					{
						float num3 = Mathf.Abs(x - position2.xMax);
						float num4 = Mathf.Abs(x - (position3.x - 16f));
						Widgets.curDragEnd = ((num3 >= num4) ? Widgets.RangeEnd.Max : Widgets.RangeEnd.Min);
					}
					flag = true;
					Event.current.Use();
					SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				}
				if (flag || (Widgets.curDragEnd != Widgets.RangeEnd.None && Event.current.type == EventType.MouseDrag))
				{
					float num5 = (Event.current.mousePosition.x - rect2.x) / rect2.width;
					int num6 = Mathf.RoundToInt(num5 * (float)(length - 1));
					num6 = Mathf.Clamp(num6, 0, length - 1);
					if (Widgets.curDragEnd == Widgets.RangeEnd.Min)
					{
						if (range.min != (QualityCategory)num6)
						{
							range.min = (QualityCategory)num6;
							if (range.max < range.min)
							{
								range.max = range.min;
							}
							SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
						}
					}
					else if (Widgets.curDragEnd == Widgets.RangeEnd.Max)
					{
						if (range.max != (QualityCategory)num6)
						{
							range.max = (QualityCategory)num6;
							if (range.min > range.max)
							{
								range.min = range.max;
							}
							SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
						}
					}
					Event.current.Use();
				}
			}
			Text.Font = font;
		}

		// Token: 0x060058CE RID: 22734 RVA: 0x002D89B4 File Offset: 0x002D6DB4
		public static void FloatRangeWithTypeIn(Rect rect, int id, ref FloatRange fRange, float sliderMin = 0f, float sliderMax = 1f, ToStringStyle valueStyle = ToStringStyle.FloatTwo, string labelKey = null)
		{
			Rect rect2 = new Rect(rect);
			rect2.width = rect.width / 4f;
			Rect rect3 = new Rect(rect);
			rect3.width = rect.width / 2f;
			rect3.x = rect.x + rect.width / 4f;
			rect3.height = rect.height / 2f;
			rect3.width -= rect.height;
			Rect butRect = new Rect(rect3);
			butRect.x = rect3.xMax;
			butRect.height = rect.height;
			butRect.width = rect.height;
			Rect rect4 = new Rect(rect);
			rect4.x = rect.x + rect.width * 0.75f;
			rect4.width = rect.width / 4f;
			Widgets.FloatRange(rect3, id, ref fRange, sliderMin, sliderMax, labelKey, valueStyle);
			if (Widgets.ButtonImage(butRect, TexButton.RangeMatch))
			{
				fRange.max = fRange.min;
			}
			float.TryParse(Widgets.TextField(rect2, fRange.min.ToString()), out fRange.min);
			float.TryParse(Widgets.TextField(rect4, fRange.max.ToString()), out fRange.max);
		}

		// Token: 0x060058CF RID: 22735 RVA: 0x002D8B1C File Offset: 0x002D6F1C
		public static Rect FillableBar(Rect rect, float fillPercent)
		{
			return Widgets.FillableBar(rect, fillPercent, Widgets.BarFullTexHor);
		}

		// Token: 0x060058D0 RID: 22736 RVA: 0x002D8B40 File Offset: 0x002D6F40
		public static Rect FillableBar(Rect rect, float fillPercent, Texture2D fillTex)
		{
			bool doBorder = rect.height > 15f && rect.width > 20f;
			return Widgets.FillableBar(rect, fillPercent, fillTex, Widgets.DefaultBarBgTex, doBorder);
		}

		// Token: 0x060058D1 RID: 22737 RVA: 0x002D8B88 File Offset: 0x002D6F88
		public static Rect FillableBar(Rect rect, float fillPercent, Texture2D fillTex, Texture2D bgTex, bool doBorder)
		{
			if (doBorder)
			{
				GUI.DrawTexture(rect, BaseContent.BlackTex);
				rect = rect.ContractedBy(3f);
			}
			if (bgTex != null)
			{
				GUI.DrawTexture(rect, bgTex);
			}
			Rect result = rect;
			rect.width *= fillPercent;
			GUI.DrawTexture(rect, fillTex);
			return result;
		}

		// Token: 0x060058D2 RID: 22738 RVA: 0x002D8BEC File Offset: 0x002D6FEC
		public static void FillableBarLabeled(Rect rect, float fillPercent, int labelWidth, string label)
		{
			if (fillPercent < 0f)
			{
				fillPercent = 0f;
			}
			if (fillPercent > 1f)
			{
				fillPercent = 1f;
			}
			Rect rect2 = rect;
			rect2.width = (float)labelWidth;
			Widgets.Label(rect2, label);
			Rect rect3 = rect;
			rect3.x += (float)labelWidth;
			rect3.width -= (float)labelWidth;
			Widgets.FillableBar(rect3, fillPercent);
		}

		// Token: 0x060058D3 RID: 22739 RVA: 0x002D8C5C File Offset: 0x002D705C
		public static void FillableBarChangeArrows(Rect barRect, float changeRate)
		{
			int changeRate2 = (int)(changeRate * Widgets.FillableBarChangeRateDisplayRatio);
			Widgets.FillableBarChangeArrows(barRect, changeRate2);
		}

		// Token: 0x060058D4 RID: 22740 RVA: 0x002D8C7C File Offset: 0x002D707C
		public static void FillableBarChangeArrows(Rect barRect, int changeRate)
		{
			if (changeRate != 0)
			{
				if (changeRate > Widgets.MaxFillableBarChangeRate)
				{
					changeRate = Widgets.MaxFillableBarChangeRate;
				}
				if (changeRate < -Widgets.MaxFillableBarChangeRate)
				{
					changeRate = -Widgets.MaxFillableBarChangeRate;
				}
				float num = barRect.height;
				if (num > 16f)
				{
					num = 16f;
				}
				int num2 = Mathf.Abs(changeRate);
				float y = barRect.y + barRect.height / 2f - num / 2f;
				float num3;
				float num4;
				Texture2D image;
				if (changeRate > 0)
				{
					num3 = barRect.x + barRect.width + 2f;
					num4 = 8f;
					image = Widgets.FillArrowTexRight;
				}
				else
				{
					num3 = barRect.x - 8f - 2f;
					num4 = -8f;
					image = Widgets.FillArrowTexLeft;
				}
				for (int i = 0; i < num2; i++)
				{
					Rect position = new Rect(num3, y, 8f, num);
					GUI.DrawTexture(position, image);
					num3 += num4;
				}
			}
		}

		// Token: 0x060058D5 RID: 22741 RVA: 0x002D8D84 File Offset: 0x002D7184
		public static void DrawWindowBackground(Rect rect)
		{
			GUI.color = Widgets.WindowBGFillColor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.WindowBGBorderColor;
			Widgets.DrawBox(rect, 1);
			GUI.color = Color.white;
		}

		// Token: 0x060058D6 RID: 22742 RVA: 0x002D8DB7 File Offset: 0x002D71B7
		public static void DrawMenuSection(Rect rect)
		{
			GUI.color = Widgets.MenuSectionBGFillColor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.MenuSectionBGBorderColor;
			Widgets.DrawBox(rect, 1);
			GUI.color = Color.white;
		}

		// Token: 0x060058D7 RID: 22743 RVA: 0x002D8DEA File Offset: 0x002D71EA
		public static void DrawWindowBackgroundTutor(Rect rect)
		{
			GUI.color = Widgets.TutorWindowBGFillColor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.TutorWindowBGBorderColor;
			Widgets.DrawBox(rect, 1);
			GUI.color = Color.white;
		}

		// Token: 0x060058D8 RID: 22744 RVA: 0x002D8E1D File Offset: 0x002D721D
		public static void DrawOptionUnselected(Rect rect)
		{
			GUI.color = Widgets.OptionUnselectedBGFillColor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.OptionUnselectedBGBorderColor;
			Widgets.DrawBox(rect, 1);
			GUI.color = Color.white;
		}

		// Token: 0x060058D9 RID: 22745 RVA: 0x002D8E50 File Offset: 0x002D7250
		public static void DrawOptionSelected(Rect rect)
		{
			GUI.color = Widgets.OptionSelectedBGFillColor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.OptionSelectedBGBorderColor;
			Widgets.DrawBox(rect, 1);
			GUI.color = Color.white;
		}

		// Token: 0x060058DA RID: 22746 RVA: 0x002D8E83 File Offset: 0x002D7283
		public static void DrawOptionBackground(Rect rect, bool selected)
		{
			if (selected)
			{
				Widgets.DrawOptionSelected(rect);
			}
			else
			{
				Widgets.DrawOptionUnselected(rect);
			}
			Widgets.DrawHighlightIfMouseover(rect);
		}

		// Token: 0x060058DB RID: 22747 RVA: 0x002D8EA4 File Offset: 0x002D72A4
		public static void DrawShadowAround(Rect rect)
		{
			Rect rect2 = rect.ContractedBy(-9f);
			rect2.x += 2f;
			rect2.y += 2f;
			Widgets.DrawAtlas(rect2, Widgets.ShadowAtlas);
		}

		// Token: 0x060058DC RID: 22748 RVA: 0x002D8EEF File Offset: 0x002D72EF
		public static void DrawAtlas(Rect rect, Texture2D atlas)
		{
			Widgets.DrawAtlas(rect, atlas, true);
		}

		// Token: 0x060058DD RID: 22749 RVA: 0x002D8EFC File Offset: 0x002D72FC
		public static void DrawAtlas(Rect rect, Texture2D atlas, bool drawTop)
		{
			rect.x = Mathf.Round(rect.x);
			rect.y = Mathf.Round(rect.y);
			rect.width = Mathf.Round(rect.width);
			rect.height = Mathf.Round(rect.height);
			float num = (float)atlas.width * 0.25f;
			num = Mathf.Floor(GenMath.Min(num, rect.height / 2f, rect.width / 2f));
			GUI.BeginGroup(rect);
			Rect drawRect;
			Rect uvRect;
			if (drawTop)
			{
				drawRect = new Rect(0f, 0f, num, num);
				uvRect = new Rect(0f, 0f, 0.25f, 0.25f);
				Widgets.DrawTexturePart(drawRect, uvRect, atlas);
				drawRect = new Rect(rect.width - num, 0f, num, num);
				uvRect = new Rect(0.75f, 0f, 0.25f, 0.25f);
				Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			}
			drawRect = new Rect(0f, rect.height - num, num, num);
			uvRect = new Rect(0f, 0.75f, 0.25f, 0.25f);
			Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			drawRect = new Rect(rect.width - num, rect.height - num, num, num);
			uvRect = new Rect(0.75f, 0.75f, 0.25f, 0.25f);
			Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			drawRect = new Rect(num, num, rect.width - num * 2f, rect.height - num * 2f);
			if (!drawTop)
			{
				drawRect.height += num;
				drawRect.y -= num;
			}
			uvRect = new Rect(0.25f, 0.25f, 0.5f, 0.5f);
			Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			if (drawTop)
			{
				drawRect = new Rect(num, 0f, rect.width - num * 2f, num);
				uvRect = new Rect(0.25f, 0f, 0.5f, 0.25f);
				Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			}
			drawRect = new Rect(num, rect.height - num, rect.width - num * 2f, num);
			uvRect = new Rect(0.25f, 0.75f, 0.5f, 0.25f);
			Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			drawRect = new Rect(0f, num, num, rect.height - num * 2f);
			if (!drawTop)
			{
				drawRect.height += num;
				drawRect.y -= num;
			}
			uvRect = new Rect(0f, 0.25f, 0.25f, 0.5f);
			Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			drawRect = new Rect(rect.width - num, num, num, rect.height - num * 2f);
			if (!drawTop)
			{
				drawRect.height += num;
				drawRect.y -= num;
			}
			uvRect = new Rect(0.75f, 0.25f, 0.25f, 0.5f);
			Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			GUI.EndGroup();
		}

		// Token: 0x060058DE RID: 22750 RVA: 0x002D925C File Offset: 0x002D765C
		public static Rect ToUVRect(this Rect r, Vector2 texSize)
		{
			return new Rect(r.x / texSize.x, r.y / texSize.y, r.width / texSize.x, r.height / texSize.y);
		}

		// Token: 0x060058DF RID: 22751 RVA: 0x002D92B2 File Offset: 0x002D76B2
		public static void DrawTexturePart(Rect drawRect, Rect uvRect, Texture2D tex)
		{
			uvRect.y = 1f - uvRect.y - uvRect.height;
			GUI.DrawTextureWithTexCoords(drawRect, tex, uvRect);
		}

		// Token: 0x060058E0 RID: 22752 RVA: 0x002D92DC File Offset: 0x002D76DC
		public static void ScrollHorizontal(Rect outRect, ref Vector2 scrollPosition, Rect viewRect, float ScrollWheelSpeed = 20f)
		{
			if (Event.current.type == EventType.ScrollWheel && Mouse.IsOver(outRect))
			{
				scrollPosition.x += Event.current.delta.y * ScrollWheelSpeed;
				float num = 0f;
				float num2 = viewRect.width - outRect.width + 16f;
				if (scrollPosition.x < num)
				{
					scrollPosition.x = num;
				}
				if (scrollPosition.x > num2)
				{
					scrollPosition.x = num2;
				}
				Event.current.Use();
			}
		}

		// Token: 0x060058E1 RID: 22753 RVA: 0x002D9378 File Offset: 0x002D7778
		public static void BeginScrollView(Rect outRect, ref Vector2 scrollPosition, Rect viewRect, bool showScrollbars = true)
		{
			if (Widgets.mouseOverScrollViewStack.Count > 0)
			{
				Widgets.mouseOverScrollViewStack.Push(Widgets.mouseOverScrollViewStack.Peek() && outRect.Contains(Event.current.mousePosition));
			}
			else
			{
				Widgets.mouseOverScrollViewStack.Push(outRect.Contains(Event.current.mousePosition));
			}
			if (showScrollbars)
			{
				scrollPosition = GUI.BeginScrollView(outRect, scrollPosition, viewRect);
			}
			else
			{
				scrollPosition = GUI.BeginScrollView(outRect, scrollPosition, viewRect, GUIStyle.none, GUIStyle.none);
			}
		}

		// Token: 0x060058E2 RID: 22754 RVA: 0x002D941E File Offset: 0x002D781E
		public static void EndScrollView()
		{
			Widgets.mouseOverScrollViewStack.Pop();
			GUI.EndScrollView();
		}

		// Token: 0x060058E3 RID: 22755 RVA: 0x002D9431 File Offset: 0x002D7831
		public static void EnsureMousePositionStackEmpty()
		{
			if (Widgets.mouseOverScrollViewStack.Count > 0)
			{
				Log.Error("Mouse position stack is not empty. There were more calls to BeginScrollView than EndScrollView. Fixing.", false);
				Widgets.mouseOverScrollViewStack.Clear();
			}
		}

		// Token: 0x060058E4 RID: 22756 RVA: 0x002D945B File Offset: 0x002D785B
		public static void DrawHighlightSelected(Rect rect)
		{
			GUI.DrawTexture(rect, TexUI.HighlightSelectedTex);
		}

		// Token: 0x060058E5 RID: 22757 RVA: 0x002D9469 File Offset: 0x002D7869
		public static void DrawHighlightIfMouseover(Rect rect)
		{
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
		}

		// Token: 0x060058E6 RID: 22758 RVA: 0x002D947D File Offset: 0x002D787D
		public static void DrawHighlight(Rect rect)
		{
			GUI.DrawTexture(rect, TexUI.HighlightTex);
		}

		// Token: 0x060058E7 RID: 22759 RVA: 0x002D948B File Offset: 0x002D788B
		public static void DrawLightHighlight(Rect rect)
		{
			GUI.DrawTexture(rect, Widgets.LightHighlight);
		}

		// Token: 0x060058E8 RID: 22760 RVA: 0x002D9499 File Offset: 0x002D7899
		public static void DrawTitleBG(Rect rect)
		{
			GUI.DrawTexture(rect, TexUI.TitleBGTex);
		}

		// Token: 0x060058E9 RID: 22761 RVA: 0x002D94A8 File Offset: 0x002D78A8
		public static bool InfoCardButton(float x, float y, Thing thing)
		{
			IConstructible constructible = thing as IConstructible;
			bool result;
			if (constructible != null)
			{
				ThingDef thingDef = thing.def.entityDefToBuild as ThingDef;
				if (thingDef != null)
				{
					result = Widgets.InfoCardButton(x, y, thingDef, constructible.UIStuff());
				}
				else
				{
					result = Widgets.InfoCardButton(x, y, thing.def.entityDefToBuild);
				}
			}
			else if (Widgets.InfoCardButtonWorker(x, y))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(thing));
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060058EA RID: 22762 RVA: 0x002D9534 File Offset: 0x002D7934
		public static bool InfoCardButton(float x, float y, Def def)
		{
			bool result;
			if (Widgets.InfoCardButtonWorker(x, y))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(def));
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060058EB RID: 22763 RVA: 0x002D9570 File Offset: 0x002D7970
		public static bool InfoCardButton(float x, float y, ThingDef thingDef, ThingDef stuffDef)
		{
			bool result;
			if (Widgets.InfoCardButtonWorker(x, y))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(thingDef, stuffDef));
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060058EC RID: 22764 RVA: 0x002D95AC File Offset: 0x002D79AC
		public static bool InfoCardButton(float x, float y, WorldObject worldObject)
		{
			bool result;
			if (Widgets.InfoCardButtonWorker(x, y))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(worldObject));
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060058ED RID: 22765 RVA: 0x002D95E8 File Offset: 0x002D79E8
		public static bool InfoCardButtonCentered(Rect rect, Thing thing)
		{
			return Widgets.InfoCardButton(rect.center.x - 12f, rect.center.y - 12f, thing);
		}

		// Token: 0x060058EE RID: 22766 RVA: 0x002D9630 File Offset: 0x002D7A30
		private static bool InfoCardButtonWorker(float x, float y)
		{
			Rect rect = new Rect(x, y, 24f, 24f);
			TooltipHandler.TipRegion(rect, "DefInfoTip".Translate());
			bool result = Widgets.ButtonImage(rect, TexButton.Info, GUI.color);
			UIHighlighter.HighlightOpportunity(rect, "InfoCard");
			return result;
		}

		// Token: 0x060058EF RID: 22767 RVA: 0x002D968A File Offset: 0x002D7A8A
		public static void DrawTextureFitted(Rect outerRect, Texture tex, float scale)
		{
			Widgets.DrawTextureFitted(outerRect, tex, scale, new Vector2((float)tex.width, (float)tex.height), new Rect(0f, 0f, 1f, 1f), 0f);
		}

		// Token: 0x060058F0 RID: 22768 RVA: 0x002D96C8 File Offset: 0x002D7AC8
		public static void DrawTextureFitted(Rect outerRect, Texture tex, float scale, Vector2 texProportions, Rect texCoords, float angle = 0f)
		{
			if (Event.current.type == EventType.Repaint)
			{
				Rect position = new Rect(0f, 0f, texProportions.x, texProportions.y);
				float num;
				if (position.width / position.height < outerRect.width / outerRect.height)
				{
					num = outerRect.height / position.height;
				}
				else
				{
					num = outerRect.width / position.width;
				}
				num *= scale;
				position.width *= num;
				position.height *= num;
				position.x = outerRect.x + outerRect.width / 2f - position.width / 2f;
				position.y = outerRect.y + outerRect.height / 2f - position.height / 2f;
				if (angle == 0f)
				{
					GUI.DrawTextureWithTexCoords(position, tex, texCoords);
				}
				else
				{
					Matrix4x4 matrix = GUI.matrix;
					UI.RotateAroundPivot(angle, position.center);
					GUI.DrawTextureWithTexCoords(position, tex, texCoords);
					GUI.matrix = matrix;
				}
			}
		}

		// Token: 0x060058F1 RID: 22769 RVA: 0x002D9808 File Offset: 0x002D7C08
		public static void DrawTextureRotated(Vector2 center, Texture tex, float angle, float scale = 1f)
		{
			float num = (float)tex.width * scale;
			float num2 = (float)tex.height * scale;
			Rect rect = new Rect(center.x - num / 2f, center.y - num2 / 2f, num, num2);
			Widgets.DrawTextureRotated(rect, tex, angle);
		}

		// Token: 0x060058F2 RID: 22770 RVA: 0x002D985C File Offset: 0x002D7C5C
		public static void DrawTextureRotated(Rect rect, Texture tex, float angle)
		{
			if (Event.current.type == EventType.Repaint)
			{
				if (angle == 0f)
				{
					GUI.DrawTexture(rect, tex);
				}
				else
				{
					Matrix4x4 matrix = GUI.matrix;
					UI.RotateAroundPivot(angle, rect.center);
					GUI.DrawTexture(rect, tex);
					GUI.matrix = matrix;
				}
			}
		}

		// Token: 0x060058F3 RID: 22771 RVA: 0x002D98B8 File Offset: 0x002D7CB8
		public static void NoneLabel(float y, float width, string customLabel = null)
		{
			Widgets.NoneLabel(ref y, width, customLabel);
		}

		// Token: 0x060058F4 RID: 22772 RVA: 0x002D98C4 File Offset: 0x002D7CC4
		public static void NoneLabel(ref float curY, float width, string customLabel = null)
		{
			GUI.color = Color.gray;
			Text.Anchor = TextAnchor.UpperCenter;
			Widgets.Label(new Rect(0f, curY, width, 30f), customLabel ?? "NoneBrackets".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			curY += 25f;
			GUI.color = Color.white;
		}

		// Token: 0x060058F5 RID: 22773 RVA: 0x002D9928 File Offset: 0x002D7D28
		public static void Dropdown<Target, Payload>(Rect rect, Target target, Func<Target, Payload> getPayload, Func<Target, IEnumerable<Widgets.DropdownMenuElement<Payload>>> menuGenerator, string buttonLabel = null, Texture2D buttonIcon = null, string dragLabel = null, Texture2D dragIcon = null, Action dropdownOpened = null, bool paintable = false)
		{
			Widgets.DraggableResult draggableResult;
			if (buttonIcon != null)
			{
				Widgets.DrawHighlightIfMouseover(rect);
				Widgets.DrawTextureFitted(rect, buttonIcon, 1f);
				draggableResult = Widgets.ButtonInvisibleDraggable(rect, false);
			}
			else
			{
				draggableResult = Widgets.ButtonTextDraggable(rect, buttonLabel, true, false, true);
			}
			if (draggableResult == Widgets.DraggableResult.Pressed)
			{
				List<FloatMenuOption> options = (from opt in menuGenerator(target)
				select opt.option).ToList<FloatMenuOption>();
				Find.WindowStack.Add(new FloatMenu(options));
				if (dropdownOpened != null)
				{
					dropdownOpened();
				}
			}
			else if (paintable && draggableResult == Widgets.DraggableResult.Dragged)
			{
				Widgets.dropdownPainting = true;
				Widgets.dropdownPainting_Payload = getPayload(target);
				Widgets.dropdownPainting_Type = typeof(Payload);
				Widgets.dropdownPainting_Text = ((dragLabel == null) ? buttonLabel : dragLabel);
				Widgets.dropdownPainting_Icon = ((!(dragIcon != null)) ? buttonIcon : dragIcon);
			}
			else if (paintable && Widgets.dropdownPainting && Mouse.IsOver(rect) && Widgets.dropdownPainting_Type == typeof(Payload))
			{
				FloatMenuOption floatMenuOption = (from opt in menuGenerator(target)
				where object.Equals(opt.payload, Widgets.dropdownPainting_Payload)
				select opt.option).FirstOrDefault<FloatMenuOption>();
				if (floatMenuOption != null && !floatMenuOption.Disabled)
				{
					Payload x = getPayload(target);
					floatMenuOption.action();
					Payload y = getPayload(target);
					if (!EqualityComparer<Payload>.Default.Equals(x, y))
					{
						SoundDefOf.Click.PlayOneShotOnCamera(null);
					}
				}
			}
		}

		// Token: 0x060058F6 RID: 22774 RVA: 0x002D9AD4 File Offset: 0x002D7ED4
		public static void WidgetsOnGUI()
		{
			if (Event.current.rawType == EventType.MouseUp || Input.GetMouseButtonUp(0))
			{
				Widgets.checkboxPainting = false;
				Widgets.dropdownPainting = false;
			}
			if (Widgets.checkboxPainting)
			{
				GenUI.DrawMouseAttachment((!Widgets.checkboxPaintingState) ? Widgets.CheckboxOffTex : Widgets.CheckboxOnTex);
			}
			if (Widgets.dropdownPainting)
			{
				GenUI.DrawMouseAttachment(Widgets.dropdownPainting_Icon, Widgets.dropdownPainting_Text, 0f, default(Vector2), null);
			}
		}

		// Token: 0x04003B03 RID: 15107
		public static Stack<bool> mouseOverScrollViewStack = new Stack<bool>();

		// Token: 0x04003B04 RID: 15108
		public static readonly GUIStyle EmptyStyle = new GUIStyle();

		// Token: 0x04003B05 RID: 15109
		[TweakValue("Input", 0f, 100f)]
		private static float DragStartDistanceSquared = 20f;

		// Token: 0x04003B06 RID: 15110
		private static readonly Color InactiveColor = new Color(0.37f, 0.37f, 0.37f, 0.8f);

		// Token: 0x04003B07 RID: 15111
		private static readonly Texture2D DefaultBarBgTex = BaseContent.BlackTex;

		// Token: 0x04003B08 RID: 15112
		private static readonly Texture2D BarFullTexHor = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.8f, 0.85f));

		// Token: 0x04003B09 RID: 15113
		public static readonly Texture2D CheckboxOnTex = ContentFinder<Texture2D>.Get("UI/Widgets/CheckOn", true);

		// Token: 0x04003B0A RID: 15114
		public static readonly Texture2D CheckboxOffTex = ContentFinder<Texture2D>.Get("UI/Widgets/CheckOff", true);

		// Token: 0x04003B0B RID: 15115
		public static readonly Texture2D CheckboxPartialTex = ContentFinder<Texture2D>.Get("UI/Widgets/CheckPartial", true);

		// Token: 0x04003B0C RID: 15116
		public const float CheckboxSize = 24f;

		// Token: 0x04003B0D RID: 15117
		public const float RadioButtonSize = 24f;

		// Token: 0x04003B0E RID: 15118
		private static readonly Texture2D RadioButOnTex = ContentFinder<Texture2D>.Get("UI/Widgets/RadioButOn", true);

		// Token: 0x04003B0F RID: 15119
		private static readonly Texture2D RadioButOffTex = ContentFinder<Texture2D>.Get("UI/Widgets/RadioButOff", true);

		// Token: 0x04003B10 RID: 15120
		private static readonly Texture2D FillArrowTexRight = ContentFinder<Texture2D>.Get("UI/Widgets/FillChangeArrowRight", true);

		// Token: 0x04003B11 RID: 15121
		private static readonly Texture2D FillArrowTexLeft = ContentFinder<Texture2D>.Get("UI/Widgets/FillChangeArrowLeft", true);

		// Token: 0x04003B12 RID: 15122
		private const int FillableBarBorderWidth = 3;

		// Token: 0x04003B13 RID: 15123
		private const int MaxFillChangeArrowHeight = 16;

		// Token: 0x04003B14 RID: 15124
		private const int FillChangeArrowWidth = 8;

		// Token: 0x04003B15 RID: 15125
		private const float CloseButtonSize = 18f;

		// Token: 0x04003B16 RID: 15126
		private const float CloseButtonMargin = 4f;

		// Token: 0x04003B17 RID: 15127
		private static readonly Texture2D ShadowAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/DropShadow", true);

		// Token: 0x04003B18 RID: 15128
		private static readonly Texture2D ButtonBGAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/ButtonBG", true);

		// Token: 0x04003B19 RID: 15129
		private static readonly Texture2D ButtonBGAtlasMouseover = ContentFinder<Texture2D>.Get("UI/Widgets/ButtonBGMouseover", true);

		// Token: 0x04003B1A RID: 15130
		private static readonly Texture2D ButtonBGAtlasClick = ContentFinder<Texture2D>.Get("UI/Widgets/ButtonBGClick", true);

		// Token: 0x04003B1B RID: 15131
		private static readonly Texture2D FloatRangeSliderTex = ContentFinder<Texture2D>.Get("UI/Widgets/RangeSlider", true);

		// Token: 0x04003B1C RID: 15132
		public static readonly Texture2D LightHighlight = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.04f));

		// Token: 0x04003B1D RID: 15133
		[TweakValue("Input", 0f, 100f)]
		private static int IntEntryButtonWidth = 40;

		// Token: 0x04003B1E RID: 15134
		private static Texture2D LineTexAA = null;

		// Token: 0x04003B1F RID: 15135
		private static readonly Rect LineRect = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x04003B20 RID: 15136
		private static readonly Material LineMat = null;

		// Token: 0x04003B21 RID: 15137
		private static readonly Texture2D AltTexture = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.05f));

		// Token: 0x04003B22 RID: 15138
		public static readonly Color NormalOptionColor = new Color(0.8f, 0.85f, 1f);

		// Token: 0x04003B23 RID: 15139
		public static readonly Color MouseoverOptionColor = Color.yellow;

		// Token: 0x04003B24 RID: 15140
		private static Dictionary<string, float> LabelCache = new Dictionary<string, float>();

		// Token: 0x04003B25 RID: 15141
		public static readonly Color SeparatorLabelColor = new Color(0.8f, 0.8f, 0.8f, 1f);

		// Token: 0x04003B26 RID: 15142
		private static readonly Color SeparatorLineColor = new Color(0.3f, 0.3f, 0.3f, 1f);

		// Token: 0x04003B27 RID: 15143
		private const float SeparatorLabelHeight = 20f;

		// Token: 0x04003B28 RID: 15144
		public const float ListSeparatorHeight = 25f;

		// Token: 0x04003B29 RID: 15145
		private static bool checkboxPainting = false;

		// Token: 0x04003B2A RID: 15146
		private static bool checkboxPaintingState = false;

		// Token: 0x04003B2B RID: 15147
		public static readonly Texture2D ButtonSubtleAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/ButtonSubtleAtlas", true);

		// Token: 0x04003B2C RID: 15148
		private static readonly Texture2D ButtonBarTex;

		// Token: 0x04003B2D RID: 15149
		public const float ButtonSubtleDefaultMarginPct = 0.15f;

		// Token: 0x04003B2E RID: 15150
		private static int buttonInvisibleDraggable_activeControl;

		// Token: 0x04003B2F RID: 15151
		private static bool buttonInvisibleDraggable_dragged;

		// Token: 0x04003B30 RID: 15152
		private static Vector3 buttonInvisibleDraggable_mouseStart;

		// Token: 0x04003B31 RID: 15153
		public const float RangeControlIdealHeight = 31f;

		// Token: 0x04003B32 RID: 15154
		public const float RangeControlCompactHeight = 28f;

		// Token: 0x04003B33 RID: 15155
		private const float RangeSliderSize = 16f;

		// Token: 0x04003B34 RID: 15156
		private static readonly Color RangeControlTextColor;

		// Token: 0x04003B35 RID: 15157
		private static int draggingId;

		// Token: 0x04003B36 RID: 15158
		private static Widgets.RangeEnd curDragEnd;

		// Token: 0x04003B37 RID: 15159
		private static float lastDragSliderSoundTime;

		// Token: 0x04003B38 RID: 15160
		private static float FillableBarChangeRateDisplayRatio;

		// Token: 0x04003B39 RID: 15161
		public static int MaxFillableBarChangeRate;

		// Token: 0x04003B3A RID: 15162
		private static readonly Color WindowBGBorderColor;

		// Token: 0x04003B3B RID: 15163
		public static readonly Color WindowBGFillColor;

		// Token: 0x04003B3C RID: 15164
		private static readonly Color MenuSectionBGFillColor;

		// Token: 0x04003B3D RID: 15165
		private static readonly Color MenuSectionBGBorderColor;

		// Token: 0x04003B3E RID: 15166
		private static readonly Color TutorWindowBGFillColor;

		// Token: 0x04003B3F RID: 15167
		private static readonly Color TutorWindowBGBorderColor;

		// Token: 0x04003B40 RID: 15168
		private static readonly Color OptionUnselectedBGFillColor;

		// Token: 0x04003B41 RID: 15169
		private static readonly Color OptionUnselectedBGBorderColor;

		// Token: 0x04003B42 RID: 15170
		private static readonly Color OptionSelectedBGFillColor;

		// Token: 0x04003B43 RID: 15171
		private static readonly Color OptionSelectedBGBorderColor;

		// Token: 0x04003B44 RID: 15172
		public const float InfoCardButtonSize = 24f;

		// Token: 0x04003B45 RID: 15173
		private static bool dropdownPainting;

		// Token: 0x04003B46 RID: 15174
		private static object dropdownPainting_Payload;

		// Token: 0x04003B47 RID: 15175
		private static Type dropdownPainting_Type;

		// Token: 0x04003B48 RID: 15176
		private static string dropdownPainting_Text;

		// Token: 0x04003B49 RID: 15177
		private static Texture2D dropdownPainting_Icon;

		// Token: 0x02000EB1 RID: 3761
		public enum DraggableResult
		{
			// Token: 0x04003B4B RID: 15179
			Idle,
			// Token: 0x04003B4C RID: 15180
			Pressed,
			// Token: 0x04003B4D RID: 15181
			Dragged,
			// Token: 0x04003B4E RID: 15182
			DraggedThenPressed
		}

		// Token: 0x02000EB2 RID: 3762
		private enum RangeEnd : byte
		{
			// Token: 0x04003B50 RID: 15184
			None,
			// Token: 0x04003B51 RID: 15185
			Min,
			// Token: 0x04003B52 RID: 15186
			Max
		}

		// Token: 0x02000EB3 RID: 3763
		public struct DropdownMenuElement<Payload>
		{
			// Token: 0x04003B53 RID: 15187
			public FloatMenuOption option;

			// Token: 0x04003B54 RID: 15188
			public Payload payload;
		}
	}
}
