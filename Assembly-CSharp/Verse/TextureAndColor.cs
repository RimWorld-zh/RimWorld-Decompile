using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D26 RID: 3366
	public struct TextureAndColor
	{
		// Token: 0x06004A0C RID: 18956 RVA: 0x0026A90A File Offset: 0x00268D0A
		public TextureAndColor(Texture2D texture, Color color)
		{
			this.texture = texture;
			this.color = color;
		}

		// Token: 0x17000BC4 RID: 3012
		// (get) Token: 0x06004A0D RID: 18957 RVA: 0x0026A91C File Offset: 0x00268D1C
		public bool HasValue
		{
			get
			{
				return this.texture != null;
			}
		}

		// Token: 0x17000BC5 RID: 3013
		// (get) Token: 0x06004A0E RID: 18958 RVA: 0x0026A940 File Offset: 0x00268D40
		public Texture2D Texture
		{
			get
			{
				return this.texture;
			}
		}

		// Token: 0x17000BC6 RID: 3014
		// (get) Token: 0x06004A0F RID: 18959 RVA: 0x0026A95C File Offset: 0x00268D5C
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x17000BC7 RID: 3015
		// (get) Token: 0x06004A10 RID: 18960 RVA: 0x0026A978 File Offset: 0x00268D78
		public static TextureAndColor None
		{
			get
			{
				return new TextureAndColor(null, Color.white);
			}
		}

		// Token: 0x06004A11 RID: 18961 RVA: 0x0026A998 File Offset: 0x00268D98
		public static implicit operator TextureAndColor(Texture2D texture)
		{
			return new TextureAndColor(texture, Color.white);
		}

		// Token: 0x0400322B RID: 12843
		private Texture2D texture;

		// Token: 0x0400322C RID: 12844
		private Color color;
	}
}
