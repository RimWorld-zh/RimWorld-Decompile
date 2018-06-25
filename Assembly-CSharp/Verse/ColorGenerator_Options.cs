using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	public class ColorGenerator_Options : ColorGenerator
	{
		public List<ColorOption> options = new List<ColorOption>();

		[CompilerGenerated]
		private static Func<ColorOption, float> <>f__am$cache0;

		public ColorGenerator_Options()
		{
		}

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

		public override Color NewRandomizedColor()
		{
			ColorOption colorOption = this.options.RandomElementByWeight((ColorOption pi) => pi.weight);
			return colorOption.RandomizedColor();
		}

		[CompilerGenerated]
		private static float <NewRandomizedColor>m__0(ColorOption pi)
		{
			return pi.weight;
		}
	}
}
