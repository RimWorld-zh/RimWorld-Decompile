using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000FA3 RID: 4003
	[StaticConstructorOnStartup]
	public static class CustomCursor
	{
		// Token: 0x06006095 RID: 24725 RVA: 0x0030DBA2 File Offset: 0x0030BFA2
		public static void Activate()
		{
			Cursor.SetCursor(CustomCursor.CursorTex, CustomCursor.CursorHotspot, CursorMode.Auto);
		}

		// Token: 0x06006096 RID: 24726 RVA: 0x0030DBB5 File Offset: 0x0030BFB5
		public static void Deactivate()
		{
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		}

		// Token: 0x04003F42 RID: 16194
		private static readonly Texture2D CursorTex = ContentFinder<Texture2D>.Get("UI/Cursors/CursorCustom", true);

		// Token: 0x04003F43 RID: 16195
		private static Vector2 CursorHotspot = new Vector2(3f, 3f);
	}
}
