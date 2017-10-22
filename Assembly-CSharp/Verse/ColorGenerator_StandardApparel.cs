using UnityEngine;

namespace Verse
{
	public class ColorGenerator_StandardApparel : ColorGenerator
	{
		private const float DarkAmp = 0.4f;

		public override Color NewRandomizedColor()
		{
			if (Rand.Value < 0.10000000149011612)
			{
				return Color.white;
			}
			if (Rand.Value < 0.10000000149011612)
			{
				return new Color(0.4f, 0.4f, 0.4f);
			}
			Color white = Color.white;
			float num = Rand.Range(0f, 0.6f);
			white.r -= num * Rand.Value;
			white.g -= num * Rand.Value;
			white.b -= num * Rand.Value;
			if (Rand.Value < 0.20000000298023224)
			{
				white.r *= 0.4f;
				white.g *= 0.4f;
				white.b *= 0.4f;
			}
			return white;
		}
	}
}
