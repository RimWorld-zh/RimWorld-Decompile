using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000DBF RID: 3519
	public class SoundParams
	{
		// Token: 0x0400344C RID: 13388
		private Dictionary<string, float> storedParams = new Dictionary<string, float>();

		// Token: 0x0400344D RID: 13389
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

		// Token: 0x06004E98 RID: 20120 RVA: 0x0029110C File Offset: 0x0028F50C
		public bool TryGetValue(string key, out float val)
		{
			return this.storedParams.TryGetValue(key, out val);
		}
	}
}
