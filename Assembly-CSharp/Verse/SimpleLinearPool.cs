using System.Collections.Generic;

namespace Verse
{
	public class SimpleLinearPool<T> where T : new()
	{
		private List<T> items = new List<T>();

		private int readIndex = 0;

		public T Get()
		{
			if (this.readIndex >= this.items.Count)
			{
				this.items.Add(new T());
			}
			List<T> obj = this.items;
			int num = this.readIndex;
			int index = num;
			this.readIndex = num + 1;
			return obj[index];
		}

		public void Clear()
		{
			this.readIndex = 0;
		}
	}
}
