using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B0C RID: 2828
	public class ColorGenerator_StandardApparel : ColorGenerator
	{
		// Token: 0x040027E7 RID: 10215
		private const float DarkAmp = 0.4f;

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x06003E90 RID: 16016 RVA: 0x0020F8C4 File Offset: 0x0020DCC4
		public override Color ExemplaryColor
		{
			get
			{
				return new Color(0.7f, 0.7f, 0.7f);
			}
		}

		// Token: 0x06003E91 RID: 16017 RVA: 0x0020F8F0 File Offset: 0x0020DCF0
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
