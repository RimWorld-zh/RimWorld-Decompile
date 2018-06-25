using System;

namespace Verse
{
	// Token: 0x02000F5C RID: 3932
	public class WeakReference<T> : WeakReference where T : class
	{
		// Token: 0x06005F36 RID: 24374 RVA: 0x00308C31 File Offset: 0x00307031
		public WeakReference(T target) : base(target)
		{
		}

		// Token: 0x17000F44 RID: 3908
		// (get) Token: 0x06005F37 RID: 24375 RVA: 0x00308C40 File Offset: 0x00307040
		// (set) Token: 0x06005F38 RID: 24376 RVA: 0x00308C60 File Offset: 0x00307060
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
