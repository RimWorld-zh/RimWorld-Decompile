using System;

namespace Verse
{
	// Token: 0x02000F57 RID: 3927
	public class WeakReference<T> : WeakReference where T : class
	{
		// Token: 0x06005F2C RID: 24364 RVA: 0x0030836D File Offset: 0x0030676D
		public WeakReference(T target) : base(target)
		{
		}

		// Token: 0x17000F45 RID: 3909
		// (get) Token: 0x06005F2D RID: 24365 RVA: 0x0030837C File Offset: 0x0030677C
		// (set) Token: 0x06005F2E RID: 24366 RVA: 0x0030839C File Offset: 0x0030679C
		public new T Target
		{
			get
			{
				return (T)((object)base.Target);
			}
			set
			{
				base.Target = value;
			}
		}
	}
}
