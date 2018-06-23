using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B0A RID: 2826
	public class ColorGenerator_Options : ColorGenerator
	{
		// Token: 0x040027E0 RID: 10208
		public List<ColorOption> options = new List<ColorOption>();

		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x06003E8F RID: 16015 RVA: 0x0020F5F0 File Offset: 0x0020D9F0
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

		// Token: 0x06003E90 RID: 16016 RVA: 0x0020F700 File Offset: 0x0020DB00
		public override Color NewRandomizedColor()
		{
			ColorOption colorOption = this.options.RandomElementByWeight((ColorOption pi) => pi.weight);
			return colorOption.RandomizedColor();
		}
	}
}
