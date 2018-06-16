using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D27 RID: 3367
	public struct TextureAndColor
	{
		// Token: 0x06004A0E RID: 18958 RVA: 0x0026A932 File Offset: 0x00268D32
		public TextureAndColor(Texture2D texture, Color color)
		{
			this.texture = texture;
			this.color = color;
		}

		// Token: 0x17000BC5 RID: 3013
		// (get) Token: 0x06004A0F RID: 18959 RVA: 0x0026A944 File Offset: 0x00268D44
		public bool HasValue
		{
			get
			{
				return this.texture != null;
			}
		}

		// Token: 0x17000BC6 RID: 3014
		// (get) Token: 0x06004A10 RID: 18960 RVA: 0x0026A968 File Offset: 0x00268D68
		public Texture2D Texture
		{
			get
			{
				return this.texture;
			}
		}

		// Token: 0x17000BC7 RID: 3015
		// (get) Token: 0x06004A11 RID: 18961 RVA: 0x0026A984 File Offset: 0x00268D84
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x06004A12 RID: 18962 RVA: 0x0026A9A0 File Offset: 0x00268DA0
		public static TextureAndColor None
		{
			get
			{
				return new TextureAndColor(null, Color.white);
			}
		}

		// Token: 0x06004A13 RID: 18963 RVA: 0x0026A9C0 File Offset: 0x00268DC0
		public static implicit operator TextureAndColor(Texture2D texture)
		{
			return new TextureAndColor(texture, Color.white);
		}

		// Token: 0x0400322D RID: 12845
		private Texture2D texture;

		// Token: 0x0400322E RID: 12846
		private Color color;
	}
}
