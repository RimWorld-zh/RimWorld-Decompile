using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000FA3 RID: 4003
	[StaticConstructorOnStartup]
	public static class CustomCursor
	{
		// Token: 0x04003F54 RID: 16212
		private static readonly Texture2D CursorTex = ContentFinder<Texture2D>.Get("UI/Cursors/CursorCustom", true);

		// Token: 0x04003F55 RID: 16213
		private static Vector2 CursorHotspot = new Vector2(3f, 3f);

		// Token: 0x060060BE RID: 24766 RVA: 0x0030FC46 File Offset: 0x0030E046
		public static void Activate()
		{
			Cursor.SetCursor(CustomCursor.CursorTex, CustomCursor.CursorHotspot, CursorMode.Auto);
		}

		// Token: 0x060060BF RID: 24767 RVA: 0x0030FC59 File Offset: 0x0030E059
		public static void Deactivate()
		{
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		}
	}
}
