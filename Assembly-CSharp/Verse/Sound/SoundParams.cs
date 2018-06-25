using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000DC0 RID: 3520
	public class SoundParams
	{
		// Token: 0x04003453 RID: 13395
		private Dictionary<string, float> storedParams = new Dictionary<string, float>();

		// Token: 0x04003454 RID: 13396
		public SoundSizeAggregator sizeAggregator = null;

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

		// Token: 0x06004E98 RID: 20120 RVA: 0x002913EC File Offset: 0x0028F7EC
		public bool TryGetValue(string key, out float val)
		{
			return this.storedParams.TryGetValue(key, out val);
		}
	}
}
