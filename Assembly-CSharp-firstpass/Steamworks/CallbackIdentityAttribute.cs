using System;

namespace Steamworks
{
	// Token: 0x0200000D RID: 13
	[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false)]
	internal class CallbackIdentityAttribute : Attribute
	{
		// Token: 0x06000035 RID: 53 RVA: 0x00002899 File Offset: 0x00000A99
		public CallbackIdentityAttribute(int callbackNum)
		{
			this.Identity = callbackNum;
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000036 RID: 54 RVA: 0x000028AC File Offset: 0x00000AAC
		// (set) Token: 0x06000037 RID: 55 RVA: 0x000028C6 File Offset: 0x00000AC6
		public int Identity { get; set; }
	}
}
