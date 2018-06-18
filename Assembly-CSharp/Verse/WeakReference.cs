using System;

namespace Verse
{
	// Token: 0x02000F57 RID: 3927
	public class WeakReference<T> : WeakReference where T : class
	{
		// Token: 0x06005F03 RID: 24323 RVA: 0x003062C9 File Offset: 0x003046C9
		public WeakReference(T target) : base(target)
		{
		}

		// Token: 0x17000F41 RID: 3905
		// (get) Token: 0x06005F04 RID: 24324 RVA: 0x003062D8 File Offset: 0x003046D8
		// (set) Token: 0x06005F05 RID: 24325 RVA: 0x003062F8 File Offset: 0x003046F8
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
