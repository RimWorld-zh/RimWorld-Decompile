using System;

namespace Verse
{
	// Token: 0x02000F58 RID: 3928
	public class WeakReference<T> : WeakReference where T : class
	{
		// Token: 0x06005F05 RID: 24325 RVA: 0x003061ED File Offset: 0x003045ED
		public WeakReference(T target) : base(target)
		{
		}

		// Token: 0x17000F42 RID: 3906
		// (get) Token: 0x06005F06 RID: 24326 RVA: 0x003061FC File Offset: 0x003045FC
		// (set) Token: 0x06005F07 RID: 24327 RVA: 0x0030621C File Offset: 0x0030461C
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
