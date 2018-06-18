using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B0D RID: 2829
	public class ColorGenerator_StandardApparel : ColorGenerator
	{
		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x06003E90 RID: 16016 RVA: 0x0020F17C File Offset: 0x0020D57C
		public override Color ExemplaryColor
		{
			get
			{
				return new Color(0.7f, 0.7f, 0.7f);
			}
		}

		// Token: 0x06003E91 RID: 16017 RVA: 0x0020F1A8 File Offset: 0x0020D5A8
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

		// Token: 0x040027E3 RID: 10211
		private const float DarkAmp = 0.4f;
	}
}
