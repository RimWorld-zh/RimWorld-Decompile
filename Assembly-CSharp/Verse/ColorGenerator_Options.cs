using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public class ColorGenerator_Options : ColorGenerator
	{
		public List<ColorOption> options = new List<ColorOption>();

		public override Color NewRandomizedColor()
		{
			ColorOption colorOption = this.options.RandomElementByWeight((Func<ColorOption, float>)((ColorOption pi) => pi.weight));
			return colorOption.RandomizedColor();
		}
	}
}
