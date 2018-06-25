using System;

namespace Verse
{
	public class WeakReference<T> : WeakReference where T : class
	{
		public WeakReference(T target) : base(target)
		{
		}

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
