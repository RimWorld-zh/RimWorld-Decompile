using System;

namespace Verse
{
	// Token: 0x02000F5B RID: 3931
	public class WeakReference<T> : WeakReference where T : class
	{
		// Token: 0x06005F36 RID: 24374 RVA: 0x003089ED File Offset: 0x00306DED
		public WeakReference(T target) : base(target)
		{
		}

		// Token: 0x17000F44 RID: 3908
		// (get) Token: 0x06005F37 RID: 24375 RVA: 0x003089FC File Offset: 0x00306DFC
		// (set) Token: 0x06005F38 RID: 24376 RVA: 0x00308A1C File Offset: 0x00306E1C
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
