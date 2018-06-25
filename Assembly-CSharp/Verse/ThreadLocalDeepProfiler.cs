using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Verse
{
	// Token: 0x02000F15 RID: 3861
	public class ThreadLocalDeepProfiler
	{
		// Token: 0x04003D78 RID: 15736
		private Stack<ThreadLocalDeepProfiler.Watcher> watchers = new Stack<ThreadLocalDeepProfiler.Watcher>();

		// Token: 0x04003D79 RID: 15737
		private static readonly string[] Prefixes = new string[50];

		// Token: 0x04003D7A RID: 15738
		private const int MaxDepth = 50;

		// Token: 0x06005CA0 RID: 23712 RVA: 0x002F0910 File Offset: 0x002EED10
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

		// Token: 0x06005CA2 RID: 23714 RVA: 0x002F098A File Offset: 0x002EED8A
		public void Start(string label = null)
		{
			if (Prefs.LogVerbose)
			{
				this.watchers.Push(new ThreadLocalDeepProfiler.Watcher(label));
			}
		}

		// Token: 0x06005CA3 RID: 23715 RVA: 0x002F09B0 File Offset: 0x002EEDB0
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

		// Token: 0x06005CA4 RID: 23716 RVA: 0x002F0A34 File Offset: 0x002EEE34
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

		// Token: 0x06005CA5 RID: 23717 RVA: 0x002F0AA0 File Offset: 0x002EEEA0
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

		// Token: 0x02000F16 RID: 3862
		private class Watcher
		{
			// Token: 0x04003D7B RID: 15739
			private string label;

			// Token: 0x04003D7C RID: 15740
			private Stopwatch watch;

			// Token: 0x04003D7D RID: 15741
			private List<ThreadLocalDeepProfiler.Watcher> children;

			// Token: 0x06005CA6 RID: 23718 RVA: 0x002F0B37 File Offset: 0x002EEF37
			public Watcher(string label)
			{
				this.label = label;
				this.watch = Stopwatch.StartNew();
				this.children = null;
			}

			// Token: 0x17000EDC RID: 3804
			// (get) Token: 0x06005CA7 RID: 23719 RVA: 0x002F0B5C File Offset: 0x002EEF5C
			public string Label
			{
				get
				{
					return this.label;
				}
			}

			// Token: 0x17000EDD RID: 3805
			// (get) Token: 0x06005CA8 RID: 23720 RVA: 0x002F0B78 File Offset: 0x002EEF78
			public Stopwatch Watch
			{
				get
				{
					return this.watch;
				}
			}

			// Token: 0x17000EDE RID: 3806
			// (get) Token: 0x06005CA9 RID: 23721 RVA: 0x002F0B94 File Offset: 0x002EEF94
			public List<ThreadLocalDeepProfiler.Watcher> Children
			{
				get
				{
					return this.children;
				}
			}

			// Token: 0x06005CAA RID: 23722 RVA: 0x002F0BAF File Offset: 0x002EEFAF
			public void AddChildResult(ThreadLocalDeepProfiler.Watcher w)
			{
				if (this.children == null)
				{
					this.children = new List<ThreadLocalDeepProfiler.Watcher>();
				}
				this.children.Add(w);
			}
		}
	}
}
