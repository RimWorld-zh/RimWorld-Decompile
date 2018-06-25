using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000FA8 RID: 4008
	[StaticConstructorOnStartup]
	public static class CustomCursor
	{
		// Token: 0x04003F5F RID: 16223
		private static readonly Texture2D CursorTex = ContentFinder<Texture2D>.Get("UI/Cursors/CursorCustom", true);

		// Token: 0x04003F60 RID: 16224
		private static Vector2 CursorHotspot = new Vector2(3f, 3f);

		// Token: 0x060060C8 RID: 24776 RVA: 0x0031050A File Offset: 0x0030E90A
		public static void Activate()
		{
			Cursor.SetCursor(CustomCursor.CursorTex, CustomCursor.CursorHotspot, CursorMode.Auto);
		}

		// Token: 0x060060C9 RID: 24777 RVA: 0x0031051D File Offset: 0x0030E91D
		public static void Deactivate()
		{
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		}
	}
}
