using System;
using System.Collections;
using UnityEngine;

namespace Verse
{
	public static class Text
	{
		private static GameFont fontInt = GameFont.Small;

		private static TextAnchor anchorInt = TextAnchor.UpperLeft;

		private static bool wordWrapInt = true;

		public static readonly GUIStyle[] fontStyles = new GUIStyle[3];

		public static readonly GUIStyle[] textFieldStyles = new GUIStyle[3];

		public static readonly GUIStyle[] textAreaStyles = new GUIStyle[3];

		public static readonly GUIStyle[] textAreaReadOnlyStyles = new GUIStyle[3];

		private static readonly float[] lineHeights = new float[3];

		private static readonly float[] spaceBetweenLines = new float[3];

		private static GUIContent tmpTextGUIContent = new GUIContent();

		private const int NumFonts = 3;

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

		public static float LineHeight
		{
			get
			{
				return Text.lineHeights[(int)Text.Font];
			}
		}

		public static float SpaceBetweenLines
		{
			get
			{
				return Text.spaceBetweenLines[(int)Text.Font];
			}
		}

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

		public static float CalcHeight(string text, float width)
		{
			Text.tmpTextGUIContent.text = text;
			return Text.CurFontStyle.CalcHeight(Text.tmpTextGUIContent, width);
		}

		public static Vector2 CalcSize(string text)
		{
			Text.tmpTextGUIContent.text = text;
			return Text.CurFontStyle.CalcSize(Text.tmpTextGUIContent);
		}

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
	}
}
