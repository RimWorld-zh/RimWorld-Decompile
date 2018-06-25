using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B0C RID: 2828
	public class ColorGenerator_Options : ColorGenerator
	{
		// Token: 0x040027E1 RID: 10209
		public List<ColorOption> options = new List<ColorOption>();

		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x06003E93 RID: 16019 RVA: 0x0020F71C File Offset: 0x0020DB1C
		public override Color ExemplaryColor
		{
			get
			{
				ColorOption colorOption = null;
				for (int i = 0; i < this.options.Count; i++)
				{
					if (colorOption == null || this.options[i].weight > colorOption.weight)
					{
						colorOption = this.options[i];
					}
				}
				Color result;
				if (colorOption == null)
				{
					result = Color.white;
				}
				else if (colorOption.only.a >= 0f)
				{
					result = colorOption.only;
				}
				else
				{
					result = new Color((colorOption.min.r + colorOption.max.r) / 2f, (colorOption.min.g + colorOption.max.g) / 2f, (colorOption.min.b + colorOption.max.b) / 2f, (colorOption.min.a + colorOption.max.a) / 2f);
				}
				return result;
			}
		}

		// Token: 0x06003E94 RID: 16020 RVA: 0x0020F82C File Offset: 0x0020DC2C
		public override Color NewRandomizedColor()
		{
			ColorOption colorOption = this.options.RandomElementByWeight((ColorOption pi) => pi.weight);
			return colorOption.RandomizedColor();
		}
	}
}
