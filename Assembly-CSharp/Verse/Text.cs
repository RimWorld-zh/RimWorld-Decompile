using System;
using System.Collections;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E8D RID: 3725
	public static class Text
	{
		// Token: 0x060057CF RID: 22479 RVA: 0x002D02C8 File Offset: 0x002CE6C8
		static Text()
		{
			Font font = (Font)Resources.Load("Fonts/Calibri_tiny");
			Font font2 = (Font)Resources.Load("Fonts/Arial_small");
			Font font3 = (Font)Resources.Load("Fonts/Arial_medium");
			Text.fontStyles[0] = new GUIStyle(GUI.skin.label);
			Text.fontStyles[0].font = font;
			Text.fontStyles[1] = new GUIStyle(GUI.skin.label);
			Text.fontStyles[1].font = font2;
			Text.fontStyles[1].contentOffset = new Vector2(0f, -1f);
			Text.fontStyles[2] = new GUIStyle(GUI.skin.label);
			Text.fontStyles[2].font = font3;
			for (int i = 0; i < Text.textFieldStyles.Length; i++)
			{
				Text.textFieldStyles[i] = new GUIStyle(GUI.skin.textField);
				Text.textFieldStyles[i].alignment = TextAnchor.MiddleLeft;
			}
			Text.textFieldStyles[0].font = font;
			Text.textFieldStyles[1].font = font2;
			Text.textFieldStyles[2].font = font3;
			for (int j = 0; j < Text.textAreaStyles.Length; j++)
			{
				Text.textAreaStyles[j] = new GUIStyle(Text.textFieldStyles[j]);
				Text.textAreaStyles[j].alignment = TextAnchor.UpperLeft;
				Text.textAreaStyles[j].wordWrap = true;
			}
			for (int k = 0; k < Text.textAreaReadOnlyStyles.Length; k++)
			{
				Text.textAreaReadOnlyStyles[k] = new GUIStyle(Text.textAreaStyles[k]);
				GUIStyle guistyle = Text.textAreaReadOnlyStyles[k];
				guistyle.normal.background = null;
				guistyle.active.background = null;
				guistyle.onHover.background = null;
				guistyle.hover.background = null;
				guistyle.onFocused.background = null;
				guistyle.focused.background = null;
			}
			GUI.skin.settings.doubleClickSelectsWord = true;
			int num = 0;
			IEnumerator enumerator = Enum.GetValues(typeof(GameFont)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					GameFont font4 = (GameFont)obj;
					Text.Font = font4;
					Text.lineHeights[num] = Text.CalcHeight("W", 999f);
					Text.spaceBetweenLines[num] = Text.CalcHeight("W\nW", 999f) - Text.CalcHeight("W", 999f) * 2f;
					num++;
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			Text.Font = GameFont.Small;
		}

		// Token: 0x17000DE6 RID: 3558
		// (get) Token: 0x060057D0 RID: 22480 RVA: 0x002D05F4 File Offset: 0x002CE9F4
		// (set) Token: 0x060057D1 RID: 22481 RVA: 0x002D060E File Offset: 0x002CEA0E
		public static GameFont Font
		{
			get
			{
				return Text.fontInt;
			}
			set
			{
				if (value == GameFont.Tiny && !LongEventHandler.AnyEventNowOrWaiting && !LanguageDatabase.activeLanguage.info.canBeTiny)
				{
					Text.fontInt = GameFont.Small;
				}
				else
				{
					Text.fontInt = value;
				}
			}
		}

		// Token: 0x17000DE7 RID: 3559
		// (get) Token: 0x060057D2 RID: 22482 RVA: 0x002D0648 File Offset: 0x002CEA48
		// (set) Token: 0x060057D3 RID: 22483 RVA: 0x002D0662 File Offset: 0x002CEA62
		public static TextAnchor Anchor
		{
			get
			{
				return Text.anchorInt;
			}
			set
			{
				Text.anchorInt = value;
			}
		}

		// Token: 0x17000DE8 RID: 3560
		// (get) Token: 0x060057D4 RID: 22484 RVA: 0x002D066C File Offset: 0x002CEA6C
		// (set) Token: 0x060057D5 RID: 22485 RVA: 0x002D0686 File Offset: 0x002CEA86
		public static bool WordWrap
		{
			get
			{
				return Text.wordWrapInt;
			}
			set
			{
				Text.wordWrapInt = value;
			}
		}

		// Token: 0x17000DE9 RID: 3561
		// (get) Token: 0x060057D6 RID: 22486 RVA: 0x002D0690 File Offset: 0x002CEA90
		public static float LineHeight
		{
			get
			{
				return Text.lineHeights[(int)Text.Font];
			}
		}

		// Token: 0x17000DEA RID: 3562
		// (get) Token: 0x060057D7 RID: 22487 RVA: 0x002D06B0 File Offset: 0x002CEAB0
		public static float SpaceBetweenLines
		{
			get
			{
				return Text.spaceBetweenLines[(int)Text.Font];
			}
		}

		// Token: 0x17000DEB RID: 3563
		// (get) Token: 0x060057D8 RID: 22488 RVA: 0x002D06D0 File Offset: 0x002CEAD0
		internal static GUIStyle CurFontStyle
		{
			get
			{
				GUIStyle guistyle;
				switch (Text.fontInt)
				{
				case GameFont.Tiny:
					guistyle = Text.fontStyles[0];
					break;
				case GameFont.Small:
					guistyle = Text.fontStyles[1];
					break;
				case GameFont.Medium:
					guistyle = Text.fontStyles[2];
					break;
				default:
					throw new NotImplementedException();
				}
				guistyle.alignment = Text.anchorInt;
				guistyle.wordWrap = Text.wordWrapInt;
				return guistyle;
			}
		}

		// Token: 0x17000DEC RID: 3564
		// (get) Token: 0x060057D9 RID: 22489 RVA: 0x002D0748 File Offset: 0x002CEB48
		public static GUIStyle CurTextFieldStyle
		{
			get
			{
				GameFont gameFont = Text.fontInt;
				GUIStyle result;
				if (gameFont != GameFont.Tiny)
				{
					if (gameFont != GameFont.Small)
					{
						if (gameFont != GameFont.Medium)
						{
							throw new NotImplementedException();
						}
						result = Text.textFieldStyles[2];
					}
					else
					{
						result = Text.textFieldStyles[1];
					}
				}
				else
				{
					result = Text.textFieldStyles[0];
				}
				return result;
			}
		}

		// Token: 0x17000DED RID: 3565
		// (get) Token: 0x060057DA RID: 22490 RVA: 0x002D07A4 File Offset: 0x002CEBA4
		public static GUIStyle CurTextAreaStyle
		{
			get
			{
				GameFont gameFont = Text.fontInt;
				GUIStyle result;
				if (gameFont != GameFont.Tiny)
				{
					if (gameFont != GameFont.Small)
					{
						if (gameFont != GameFont.Medium)
						{
							throw new NotImplementedException();
						}
						result = Text.textAreaStyles[2];
					}
					else
					{
						result = Text.textAreaStyles[1];
					}
				}
				else
				{
					result = Text.textAreaStyles[0];
				}
				return result;
			}
		}

		// Token: 0x17000DEE RID: 3566
		// (get) Token: 0x060057DB RID: 22491 RVA: 0x002D0800 File Offset: 0x002CEC00
		public static GUIStyle CurTextAreaReadOnlyStyle
		{
			get
			{
				GameFont gameFont = Text.fontInt;
				GUIStyle result;
				if (gameFont != GameFont.Tiny)
				{
					if (gameFont != GameFont.Small)
					{
						if (gameFont != GameFont.Medium)
						{
							throw new NotImplementedException();
						}
						result = Text.textAreaReadOnlyStyles[2];
					}
					else
					{
						result = Text.textAreaReadOnlyStyles[1];
					}
				}
				else
				{
					result = Text.textAreaReadOnlyStyles[0];
				}
				return result;
			}
		}

		// Token: 0x060057DC RID: 22492 RVA: 0x002D085C File Offset: 0x002CEC5C
		public static float CalcHeight(string text, float width)
		{
			Text.tmpTextGUIContent.text = text;
			return Text.CurFontStyle.CalcHeight(Text.tmpTextGUIContent, width);
		}

		// Token: 0x060057DD RID: 22493 RVA: 0x002D088C File Offset: 0x002CEC8C
		public static Vector2 CalcSize(string text)
		{
			Text.tmpTextGUIContent.text = text;
			return Text.CurFontStyle.CalcSize(Text.tmpTextGUIContent);
		}

		// Token: 0x060057DE RID: 22494 RVA: 0x002D08BC File Offset: 0x002CECBC
		internal static void StartOfOnGUI()
		{
			if (!Text.WordWrap)
			{
				Log.ErrorOnce("Word wrap was false at end of frame.", 764362, false);
				Text.WordWrap = true;
			}
			if (Text.Anchor != TextAnchor.UpperLeft)
			{
				Log.ErrorOnce("Alignment was " + Text.Anchor + " at end of frame.", 15558, false);
				Text.Anchor = TextAnchor.UpperLeft;
			}
			Text.Font = GameFont.Small;
		}

		// Token: 0x04003A14 RID: 14868
		private static GameFont fontInt = GameFont.Small;

		// Token: 0x04003A15 RID: 14869
		private static TextAnchor anchorInt = TextAnchor.UpperLeft;

		// Token: 0x04003A16 RID: 14870
		private static bool wordWrapInt = true;

		// Token: 0x04003A17 RID: 14871
		public static readonly GUIStyle[] fontStyles = new GUIStyle[3];

		// Token: 0x04003A18 RID: 14872
		public static readonly GUIStyle[] textFieldStyles = new GUIStyle[3];

		// Token: 0x04003A19 RID: 14873
		public static readonly GUIStyle[] textAreaStyles = new GUIStyle[3];

		// Token: 0x04003A1A RID: 14874
		public static readonly GUIStyle[] textAreaReadOnlyStyles = new GUIStyle[3];

		// Token: 0x04003A1B RID: 14875
		private static readonly float[] lineHeights = new float[3];

		// Token: 0x04003A1C RID: 14876
		private static readonly float[] spaceBetweenLines = new float[3];

		// Token: 0x04003A1D RID: 14877
		private static GUIContent tmpTextGUIContent = new GUIContent();

		// Token: 0x04003A1E RID: 14878
		private const int NumFonts = 3;
	}
}
