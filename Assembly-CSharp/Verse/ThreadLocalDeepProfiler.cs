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
		// Token: 0x06005C6E RID: 23662 RVA: 0x002EE264 File Offset: 0x002EC664
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

		// Token: 0x06005C70 RID: 23664 RVA: 0x002EE2DE File Offset: 0x002EC6DE
		public void Start(string label = null)
		{
			if (Prefs.LogVerbose)
			{
				this.watchers.Push(new ThreadLocalDeepProfiler.Watcher(label));
			}
		}

		// Token: 0x06005C71 RID: 23665 RVA: 0x002EE304 File Offset: 0x002EC704
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

		// Token: 0x06005C72 RID: 23666 RVA: 0x002EE388 File Offset: 0x002EC788
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

		// Token: 0x06005C73 RID: 23667 RVA: 0x002EE3F4 File Offset: 0x002EC7F4
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

		// Token: 0x04003D63 RID: 15715
		private Stack<ThreadLocalDeepProfiler.Watcher> watchers = new Stack<ThreadLocalDeepProfiler.Watcher>();

		// Token: 0x04003D64 RID: 15716
		private static readonly string[] Prefixes = new string[50];

		// Token: 0x04003D65 RID: 15717
		private const int MaxDepth = 50;

		// Token: 0x02000F12 RID: 3858
		private class Watcher
		{
			// Token: 0x06005C74 RID: 23668 RVA: 0x002EE48B File Offset: 0x002EC88B
			public Watcher(string label)
			{
				this.label = label;
				this.watch = Stopwatch.StartNew();
				this.children = null;
			}

			// Token: 0x17000ED9 RID: 3801
			// (get) Token: 0x06005C75 RID: 23669 RVA: 0x002EE4B0 File Offset: 0x002EC8B0
			public string Label
			{
				get
				{
					return this.label;
				}
			}

			// Token: 0x17000EDA RID: 3802
			// (get) Token: 0x06005C76 RID: 23670 RVA: 0x002EE4CC File Offset: 0x002EC8CC
			public Stopwatch Watch
			{
				get
				{
					return this.watch;
				}
			}

			// Token: 0x17000EDB RID: 3803
			// (get) Token: 0x06005C77 RID: 23671 RVA: 0x002EE4E8 File Offset: 0x002EC8E8
			public List<ThreadLocalDeepProfiler.Watcher> Children
			{
				get
				{
					return this.children;
				}
			}

			// Token: 0x06005C78 RID: 23672 RVA: 0x002EE503 File Offset: 0x002EC903
			public void AddChildResult(ThreadLocalDeepProfiler.Watcher w)
			{
				if (this.children == null)
				{
					this.children = new List<ThreadLocalDeepProfiler.Watcher>();
				}
				this.children.Add(w);
			}

			// Token: 0x04003D66 RID: 15718
			private string label;

			// Token: 0x04003D67 RID: 15719
			private Stopwatch watch;

			// Token: 0x04003D68 RID: 15720
			private List<ThreadLocalDeepProfiler.Watcher> children;
		}
	}
}
