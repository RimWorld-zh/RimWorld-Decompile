using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B0E RID: 2830
	public class ColorOption
	{
		// Token: 0x040027EA RID: 10218
		public float weight = 10f;

		// Token: 0x040027EB RID: 10219
		public Color min = new Color(-1f, -1f, -1f, -1f);

		// Token: 0x040027EC RID: 10220
		public Color max = new Color(-1f, -1f, -1f, -1f);

		// Token: 0x040027ED RID: 10221
		public Color only = new Color(-1f, -1f, -1f, -1f);

		// Token: 0x06003E97 RID: 16023 RVA: 0x0020FBE8 File Offset: 0x0020DFE8
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

		// Token: 0x06003E98 RID: 16024 RVA: 0x0020FC90 File Offset: 0x0020E090
		public void SetSingle(Color color)
		{
			this.only = color;
		}

		// Token: 0x06003E99 RID: 16025 RVA: 0x0020FC9A File Offset: 0x0020E09A
		public void SetMin(Color color)
		{
			this.min = color;
		}

		// Token: 0x06003E9A RID: 16026 RVA: 0x0020FCA4 File Offset: 0x0020E0A4
		public void SetMax(Color color)
		{
			this.max = color;
		}
	}
}
