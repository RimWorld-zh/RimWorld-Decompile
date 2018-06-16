using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000FA4 RID: 4004
	[StaticConstructorOnStartup]
	public static class CustomCursor
	{
		// Token: 0x06006097 RID: 24727 RVA: 0x0030DAC6 File Offset: 0x0030BEC6
		public static void Activate()
		{
			Cursor.SetCursor(CustomCursor.CursorTex, CustomCursor.CursorHotspot, CursorMode.Auto);
		}

		// Token: 0x06006098 RID: 24728 RVA: 0x0030DAD9 File Offset: 0x0030BED9
		public static void Deactivate()
		{
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		}

		// Token: 0x04003F43 RID: 16195
		private static readonly Texture2D CursorTex = ContentFinder<Texture2D>.Get("UI/Cursors/CursorCustom", true);

		// Token: 0x04003F44 RID: 16196
		private static Vector2 CursorHotspot = new Vector2(3f, 3f);
	}
}
