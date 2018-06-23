using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B09 RID: 2825
	public class ColorGenerator_StandardApparel : ColorGenerator
	{
		// Token: 0x040027DF RID: 10207
		private const float DarkAmp = 0.4f;

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x06003E8C RID: 16012 RVA: 0x0020F4B8 File Offset: 0x0020D8B8
		public override Color ExemplaryColor
		{
			get
			{
				return new Color(0.7f, 0.7f, 0.7f);
			}
		}

		// Token: 0x06003E8D RID: 16013 RVA: 0x0020F4E4 File Offset: 0x0020D8E4
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
