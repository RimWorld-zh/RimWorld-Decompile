using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B0F RID: 2831
	public class ColorOption
	{
		// Token: 0x06003E97 RID: 16023 RVA: 0x0020F4A0 File Offset: 0x0020D8A0
		public Color RandomizedColor()
		{
			Color result;
			if (this.only.a >= 0f)
			{
				result = this.only;
			}
			else
			{
				result = new Color(Rand.Range(this.min.r, this.max.r), Rand.Range(this.min.g, this.max.g), Rand.Range(this.min.b, this.max.b), Rand.Range(this.min.a, this.max.a));
			}
			return result;
		}

		// Token: 0x06003E98 RID: 16024 RVA: 0x0020F548 File Offset: 0x0020D948
		public void SetSingle(Color color)
		{
			this.only = color;
		}

		// Token: 0x06003E99 RID: 16025 RVA: 0x0020F552 File Offset: 0x0020D952
		public void SetMin(Color color)
		{
			this.min = color;
		}

		// Token: 0x06003E9A RID: 16026 RVA: 0x0020F55C File Offset: 0x0020D95C
		public void SetMax(Color color)
		{
			this.max = color;
		}

		// Token: 0x040027E6 RID: 10214
		public float weight = 10f;

		// Token: 0x040027E7 RID: 10215
		public Color min = new Color(-1f, -1f, -1f, -1f);

		// Token: 0x040027E8 RID: 10216
		public Color max = new Color(-1f, -1f, -1f, -1f);

		// Token: 0x040027E9 RID: 10217
		public Color only = new Color(-1f, -1f, -1f, -1f);
	}
}
