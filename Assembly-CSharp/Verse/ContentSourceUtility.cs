using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000ED9 RID: 3801
	[StaticConstructorOnStartup]
	public static class ContentSourceUtility
	{
		// Token: 0x04003C70 RID: 15472
		public const float IconSize = 24f;

		// Token: 0x04003C71 RID: 15473
		private static readonly Texture2D ContentSourceIcon_LocalFolder = ContentFinder<Texture2D>.Get("UI/Icons/ContentSources/LocalFolder", true);

		// Token: 0x04003C72 RID: 15474
		private static readonly Texture2D ContentSourceIcon_SteamWorkshop = ContentFinder<Texture2D>.Get("UI/Icons/ContentSources/SteamWorkshop", true);

		// Token: 0x06005A02 RID: 23042 RVA: 0x002E3D54 File Offset: 0x002E2154
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

		// Token: 0x06005A03 RID: 23043 RVA: 0x002E3DA4 File Offset: 0x002E21A4
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

		// Token: 0x06005A04 RID: 23044 RVA: 0x002E3E4C File Offset: 0x002E224C
		public static string HumanLabel(this ContentSource s)
		{
			return ("ContentSource_" + s.ToString()).Translate();
		}
	}
}
