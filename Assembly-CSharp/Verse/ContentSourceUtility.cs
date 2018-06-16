using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000ED8 RID: 3800
	[StaticConstructorOnStartup]
	public static class ContentSourceUtility
	{
		// Token: 0x060059E0 RID: 23008 RVA: 0x002E1B28 File Offset: 0x002DFF28
		public static Texture2D GetIcon(this ContentSource s)
		{
			Texture2D result;
			if (s != ContentSource.Undefined)
			{
				if (s != ContentSource.LocalFolder)
				{
					if (s != ContentSource.SteamWorkshop)
					{
						throw new NotImplementedException();
					}
					result = ContentSourceUtility.ContentSourceIcon_SteamWorkshop;
				}
				else
				{
					result = ContentSourceUtility.ContentSourceIcon_LocalFolder;
				}
			}
			else
			{
				result = BaseContent.BadTex;
			}
			return result;
		}

		// Token: 0x060059E1 RID: 23009 RVA: 0x002E1B78 File Offset: 0x002DFF78
		public static void DrawContentSource(Rect r, ContentSource source, Action clickAction = null)
		{
			Rect rect = new Rect(r.x, r.y + r.height / 2f - 12f, 24f, 24f);
			GUI.DrawTexture(rect, source.GetIcon());
			Widgets.DrawHighlightIfMouseover(rect);
			TooltipHandler.TipRegion(rect, () => "Source".Translate() + ": " + source.HumanLabel(), (int)(r.x + r.y * 56161f));
			if (clickAction != null && Widgets.ButtonInvisible(rect, false))
			{
				clickAction();
			}
		}

		// Token: 0x060059E2 RID: 23010 RVA: 0x002E1C20 File Offset: 0x002E0020
		public static string HumanLabel(this ContentSource s)
		{
			return ("ContentSource_" + s.ToString()).Translate();
		}

		// Token: 0x04003C59 RID: 15449
		public const float IconSize = 24f;

		// Token: 0x04003C5A RID: 15450
		private static readonly Texture2D ContentSourceIcon_LocalFolder = ContentFinder<Texture2D>.Get("UI/Icons/ContentSources/LocalFolder", true);

		// Token: 0x04003C5B RID: 15451
		private static readonly Texture2D ContentSourceIcon_SteamWorkshop = ContentFinder<Texture2D>.Get("UI/Icons/ContentSources/SteamWorkshop", true);
	}
}
