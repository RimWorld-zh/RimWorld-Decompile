using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000DC1 RID: 3521
	public class SoundParams
	{
		// Token: 0x17000CB0 RID: 3248
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

		// Token: 0x06004E81 RID: 20097 RVA: 0x0028FA50 File Offset: 0x0028DE50
		public bool TryGetValue(string key, out float val)
		{
			return this.storedParams.TryGetValue(key, out val);
		}

		// Token: 0x04003443 RID: 13379
		private Dictionary<string, float> storedParams = new Dictionary<string, float>();

		// Token: 0x04003444 RID: 13380
		public SoundSizeAggregator sizeAggregator = null;
	}
}
