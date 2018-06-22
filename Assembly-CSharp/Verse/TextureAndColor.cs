using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D23 RID: 3363
	public struct TextureAndColor
	{
		// Token: 0x06004A1D RID: 18973 RVA: 0x0026BD3E File Offset: 0x0026A13E
		public TextureAndColor(Texture2D texture, Color color)
		{
			this.texture = texture;
			this.color = color;
		}

		// Token: 0x17000BC6 RID: 3014
		// (get) Token: 0x06004A1E RID: 18974 RVA: 0x0026BD50 File Offset: 0x0026A150
		public bool HasValue
		{
			get
			{
				return this.texture != null;
			}
		}

		// Token: 0x17000BC7 RID: 3015
		// (get) Token: 0x06004A1F RID: 18975 RVA: 0x0026BD74 File Offset: 0x0026A174
		public Texture2D Texture
		{
			get
			{
				return this.texture;
			}
		}

		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x06004A20 RID: 18976 RVA: 0x0026BD90 File Offset: 0x0026A190
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x17000BC9 RID: 3017
		// (get) Token: 0x06004A21 RID: 18977 RVA: 0x0026BDAC File Offset: 0x0026A1AC
		public static TextureAndColor None
		{
			get
			{
				return new TextureAndColor(null, Color.white);
			}
		}

		// Token: 0x06004A22 RID: 18978 RVA: 0x0026BDCC File Offset: 0x0026A1CC
		public static implicit operator TextureAndColor(Texture2D texture)
		{
			return new TextureAndColor(texture, Color.white);
		}

		// Token: 0x04003236 RID: 12854
		private Texture2D texture;

		// Token: 0x04003237 RID: 12855
		private Color color;
	}
}
