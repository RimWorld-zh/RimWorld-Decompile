using System;
using UnityEngine;

namespace Verse
{
	public class ColorGenerator_StandardApparel : ColorGenerator
	{
		private const float DarkAmp = 0.4f;

		public ColorGenerator_StandardApparel()
		{
		}

		public override Color ExemplaryColor
		{
			get
			{
				return new Color(0.7f, 0.7f, 0.7f);
			}
		}

		public override Color NewRandomizedColor()
		{
			Color result;
			if (Rand.Value < 0.1f)
			{
				result = Color.white;
			}
			else if (Rand.Value < 0.1f)
			{
				result = new Color(0.4f, 0.4f, 0.4f);
			}
			else
			{
				Color white = Color.white;
				float num = Rand.Range(0f, 0.6f);
				white.r -= num * Rand.Value;
				white.g -= num * Rand.Value;
				white.b -= num * Rand.Value;
				if (Rand.Value < 0.2f)
				{
					white.r *= 0.4f;
					white.g *= 0.4f;
					white.b *= 0.4f;
				}
				result = white;
			}
			return result;
		}
	}
}
