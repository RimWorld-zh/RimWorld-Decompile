using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B0E RID: 2830
	public class ColorGenerator_Options : ColorGenerator
	{
		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x06003E91 RID: 16017 RVA: 0x0020F1E0 File Offset: 0x0020D5E0
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

		// Token: 0x06003E92 RID: 16018 RVA: 0x0020F2F0 File Offset: 0x0020D6F0
		public override Color NewRandomizedColor()
		{
			ColorOption colorOption = this.options.RandomElementByWeight((ColorOption pi) => pi.weight);
			return colorOption.RandomizedColor();
		}

		// Token: 0x040027E4 RID: 10212
		public List<ColorOption> options = new List<ColorOption>();
	}
}
