using System;
using System.Runtime.InteropServices;

namespace Ionic.Zlib
{
	// Token: 0x0200001A RID: 26
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000E")]
	public class ZlibException : Exception
	{
		// Token: 0x060000E5 RID: 229 RVA: 0x0000B1FD File Offset: 0x000095FD
		public ZlibException()
		{
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0000B206 File Offset: 0x00009606
		public ZlibException(string s) : base(s)
		{
		}
	}
}
