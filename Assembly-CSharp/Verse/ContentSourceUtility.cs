using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000ED8 RID: 3800
	[StaticConstructorOnStartup]
	public static class ContentSourceUtility
	{
		// Token: 0x04003C68 RID: 15464
		public const float IconSize = 24f;

		// Token: 0x04003C69 RID: 15465
		private static readonly Texture2D ContentSourceIcon_LocalFolder = ContentFinder<Texture2D>.Get("UI/Icons/ContentSources/LocalFolder", true);

		// Token: 0x04003C6A RID: 15466
		private static readonly Texture2D ContentSourceIcon_SteamWorkshop = ContentFinder<Texture2D>.Get("UI/Icons/ContentSources/SteamWorkshop", true);

		// Token: 0x06005A02 RID: 23042 RVA: 0x002E3B34 File Offset: 0x002E1F34
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

		// Token: 0x06005A03 RID: 23043 RVA: 0x002E3B84 File Offset: 0x002E1F84
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

		// Token: 0x06005A04 RID: 23044 RVA: 0x002E3C2C File Offset: 0x002E202C
		public static string HumanLabel(this ContentSource s)
		{
			return ("ContentSource_" + s.ToString()).Translate();
		}
	}
}
