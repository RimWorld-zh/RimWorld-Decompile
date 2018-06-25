using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000FA7 RID: 4007
	[StaticConstructorOnStartup]
	public static class CustomCursor
	{
		// Token: 0x04003F57 RID: 16215
		private static readonly Texture2D CursorTex = ContentFinder<Texture2D>.Get("UI/Cursors/CursorCustom", true);

		// Token: 0x04003F58 RID: 16216
		private static Vector2 CursorHotspot = new Vector2(3f, 3f);

		// Token: 0x060060C8 RID: 24776 RVA: 0x003102C6 File Offset: 0x0030E6C6
		public static void Activate()
		{
			Cursor.SetCursor(CustomCursor.CursorTex, CustomCursor.CursorHotspot, CursorMode.Auto);
		}

		// Token: 0x060060C9 RID: 24777 RVA: 0x003102D9 File Offset: 0x0030E6D9
		public static void Deactivate()
		{
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		}
	}
}
