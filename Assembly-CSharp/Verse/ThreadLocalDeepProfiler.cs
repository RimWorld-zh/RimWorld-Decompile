using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Verse
{
	// Token: 0x02000F11 RID: 3857
	public class ThreadLocalDeepProfiler
	{
		// Token: 0x06005C96 RID: 23702 RVA: 0x002F0290 File Offset: 0x002EE690
		static ThreadLocalDeepProfiler()
		{
			for (int i = 0; i < 50; i++)
			{
				ThreadLocalDeepProfiler.Prefixes[i] = "";
				for (int j = 0; j < i; j++)
				{
					string[] prefixes;
					int num;
					(prefixes = ThreadLocalDeepProfiler.Prefixes)[num = i] = prefixes[num] + " -";
				}
			}
		}

		// Token: 0x06005C98 RID: 23704 RVA: 0x002F030A File Offset: 0x002EE70A
		public void Start(string label = null)
		{
			if (Prefs.LogVerbose)
			{
				this.watchers.Push(new ThreadLocalDeepProfiler.Watcher(label));
			}
		}

		// Token: 0x06005C99 RID: 23705 RVA: 0x002F0330 File Offset: 0x002EE730
		public void End()
		{
			if (Prefs.LogVerbose)
			{
				if (this.watchers.Count == 0)
				{
					Log.Error("Ended deep profiling while not profiling.", false);
				}
				else
				{
					ThreadLocalDeepProfiler.Watcher watcher = this.watchers.Pop();
					watcher.Watch.Stop();
					if (this.watchers.Count > 0)
					{
						this.watchers.Peek().AddChildResult(watcher);
					}
					else
					{
						this.Output(watcher);
					}
				}
			}
		}

		// Token: 0x06005C9A RID: 23706 RVA: 0x002F03B4 File Offset: 0x002EE7B4
		private void Output(ThreadLocalDeepProfiler.Watcher root)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (UnityData.IsInMainThread)
			{
				stringBuilder.AppendLine("--- Main thread ---");
			}
			else
			{
				stringBuilder.AppendLine("--- Thread " + Thread.CurrentThread.ManagedThreadId + " ---");
			}
			this.AppendStringRecursive(stringBuilder, root, 0);
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x06005C9B RID: 23707 RVA: 0x002F0420 File Offset: 0x002EE820
		private void AppendStringRecursive(StringBuilder sb, ThreadLocalDeepProfiler.Watcher w, int depth)
		{
			sb.AppendLine(string.Concat(new object[]
			{
				ThreadLocalDeepProfiler.Prefixes[depth],
				" ",
				w.Watch.ElapsedMilliseconds,
				"ms ",
				w.Label
			}));
			if (w.Children != null)
			{
				for (int i = 0; i < w.Children.Count; i++)
				{
					this.AppendStringRecursive(sb, w.Children[i], depth + 1);
				}
			}
		}

		// Token: 0x04003D75 RID: 15733
		private Stack<ThreadLocalDeepProfiler.Watcher> watchers = new Stack<ThreadLocalDeepProfiler.Watcher>();

		// Token: 0x04003D76 RID: 15734
		private static readonly string[] Prefixes = new string[50];

		// Token: 0x04003D77 RID: 15735
		private const int MaxDepth = 50;

		// Token: 0x02000F12 RID: 3858
		private class Watcher
		{
			// Token: 0x06005C9C RID: 23708 RVA: 0x002F04B7 File Offset: 0x002EE8B7
			public Watcher(string label)
			{
				this.label = label;
				this.watch = Stopwatch.StartNew();
				this.children = null;
			}

			// Token: 0x17000EDD RID: 3805
			// (get) Token: 0x06005C9D RID: 23709 RVA: 0x002F04DC File Offset: 0x002EE8DC
			public string Label
			{
				get
				{
					return this.label;
				}
			}

			// Token: 0x17000EDE RID: 3806
			// (get) Token: 0x06005C9E RID: 23710 RVA: 0x002F04F8 File Offset: 0x002EE8F8
			public Stopwatch Watch
			{
				get
				{
					return this.watch;
				}
			}

			// Token: 0x17000EDF RID: 3807
			// (get) Token: 0x06005C9F RID: 23711 RVA: 0x002F0514 File Offset: 0x002EE914
			public List<ThreadLocalDeepProfiler.Watcher> Children
			{
				get
				{
					return this.children;
				}
			}

			// Token: 0x06005CA0 RID: 23712 RVA: 0x002F052F File Offset: 0x002EE92F
			public void AddChildResult(ThreadLocalDeepProfiler.Watcher w)
			{
				if (this.children == null)
				{
					this.children = new List<ThreadLocalDeepProfiler.Watcher>();
				}
				this.children.Add(w);
			}

			// Token: 0x04003D78 RID: 15736
			private string label;

			// Token: 0x04003D79 RID: 15737
			private Stopwatch watch;

			// Token: 0x04003D7A RID: 15738
			private List<ThreadLocalDeepProfiler.Watcher> children;
		}
	}
}
