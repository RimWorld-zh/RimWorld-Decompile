using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000DC0 RID: 3520
	public class SoundParams
	{
		// Token: 0x17000CAF RID: 3247
		public float this[string key]
		{
			get
			{
				return this.storedParams[key];
			}
			set
			{
				this.storedParams[key] = value;
			}
		}

		// Token: 0x06004E7F RID: 20095 RVA: 0x0028FA30 File Offset: 0x0028DE30
		public bool TryGetValue(string key, out float val)
		{
			return this.storedParams.TryGetValue(key, out val);
		}

		// Token: 0x04003441 RID: 13377
		private Dictionary<string, float> storedParams = new Dictionary<string, float>();

		// Token: 0x04003442 RID: 13378
		public SoundSizeAggregator sizeAggregator = null;
	}
}
