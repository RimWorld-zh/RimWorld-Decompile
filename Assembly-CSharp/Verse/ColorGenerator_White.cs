using System;
using UnityEngine;

namespace Verse
{
	public class ColorGenerator_White : ColorGenerator
	{
		public ColorGenerator_White()
		{
		}

		public override Color NewRandomizedColor()
		{
			return Color.white;
		}
	}
}
