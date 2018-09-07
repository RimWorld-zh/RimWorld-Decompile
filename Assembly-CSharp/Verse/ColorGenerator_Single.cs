using System;
using UnityEngine;

namespace Verse
{
	public class ColorGenerator_Single : ColorGenerator
	{
		public Color color;

		public ColorGenerator_Single()
		{
		}

		public override Color NewRandomizedColor()
		{
			return this.color;
		}
	}
}
