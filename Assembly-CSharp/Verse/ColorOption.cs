using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B0B RID: 2827
	public class ColorOption
	{
		// Token: 0x040027E2 RID: 10210
		public float weight = 10f;

		// Token: 0x040027E3 RID: 10211
		public Color min = new Color(-1f, -1f, -1f, -1f);

		// Token: 0x040027E4 RID: 10212
		public Color max = new Color(-1f, -1f, -1f, -1f);

		// Token: 0x040027E5 RID: 10213
		public Color only = new Color(-1f, -1f, -1f, -1f);

		// Token: 0x06003E93 RID: 16019 RVA: 0x0020F7DC File Offset: 0x0020DBDC
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

		// Token: 0x06003E94 RID: 16020 RVA: 0x0020F884 File Offset: 0x0020DC84
		public void SetSingle(Color color)
		{
			this.only = color;
		}

		// Token: 0x06003E95 RID: 16021 RVA: 0x0020F88E File Offset: 0x0020DC8E
		public void SetMin(Color color)
		{
			this.min = color;
		}

		// Token: 0x06003E96 RID: 16022 RVA: 0x0020F898 File Offset: 0x0020DC98
		public void SetMax(Color color)
		{
			this.max = color;
		}
	}
}
