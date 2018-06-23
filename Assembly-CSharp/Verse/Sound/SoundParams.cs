using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000DBD RID: 3517
	public class SoundParams
	{
		// Token: 0x0400344C RID: 13388
		private Dictionary<string, float> storedParams = new Dictionary<string, float>();

		// Token: 0x0400344D RID: 13389
		public SoundSizeAggregator sizeAggregator = null;

		// Token: 0x17000CB1 RID: 3249
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

		// Token: 0x06004E94 RID: 20116 RVA: 0x00290FE0 File Offset: 0x0028F3E0
		public bool TryGetValue(string key, out float val)
		{
			return this.storedParams.TryGetValue(key, out val);
		}
	}
}
