using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B0D RID: 2829
	public class ColorOption
	{
		// Token: 0x040027E3 RID: 10211
		public float weight = 10f;

		// Token: 0x040027E4 RID: 10212
		public Color min = new Color(-1f, -1f, -1f, -1f);

		// Token: 0x040027E5 RID: 10213
		public Color max = new Color(-1f, -1f, -1f, -1f);

		// Token: 0x040027E6 RID: 10214
		public Color only = new Color(-1f, -1f, -1f, -1f);

		// Token: 0x06003E97 RID: 16023 RVA: 0x0020F908 File Offset: 0x0020DD08
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

		// Token: 0x06003E98 RID: 16024 RVA: 0x0020F9B0 File Offset: 0x0020DDB0
		public void SetSingle(Color color)
		{
			this.only = color;
		}

		// Token: 0x06003E99 RID: 16025 RVA: 0x0020F9BA File Offset: 0x0020DDBA
		public void SetMin(Color color)
		{
			this.min = color;
		}

		// Token: 0x06003E9A RID: 16026 RVA: 0x0020F9C4 File Offset: 0x0020DDC4
		public void SetMax(Color color)
		{
			this.max = color;
		}
	}
}
