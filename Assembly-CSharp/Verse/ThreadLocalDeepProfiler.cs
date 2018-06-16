using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Verse
{
	// Token: 0x02000F12 RID: 3858
	public class ThreadLocalDeepProfiler
	{
		// Token: 0x06005C70 RID: 23664 RVA: 0x002EE188 File Offset: 0x002EC588
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

		// Token: 0x06005C72 RID: 23666 RVA: 0x002EE202 File Offset: 0x002EC602
		public void Start(string label = null)
		{
			if (Prefs.LogVerbose)
			{
				this.watchers.Push(new ThreadLocalDeepProfiler.Watcher(label));
			}
		}

		// Token: 0x06005C73 RID: 23667 RVA: 0x002EE228 File Offset: 0x002EC628
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

		// Token: 0x06005C74 RID: 23668 RVA: 0x002EE2AC File Offset: 0x002EC6AC
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

		// Token: 0x06005C75 RID: 23669 RVA: 0x002EE318 File Offset: 0x002EC718
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

		// Token: 0x04003D64 RID: 15716
		private Stack<ThreadLocalDeepProfiler.Watcher> watchers = new Stack<ThreadLocalDeepProfiler.Watcher>();

		// Token: 0x04003D65 RID: 15717
		private static readonly string[] Prefixes = new string[50];

		// Token: 0x04003D66 RID: 15718
		private const int MaxDepth = 50;

		// Token: 0x02000F13 RID: 3859
		private class Watcher
		{
			// Token: 0x06005C76 RID: 23670 RVA: 0x002EE3AF File Offset: 0x002EC7AF
			public Watcher(string label)
			{
				this.label = label;
				this.watch = Stopwatch.StartNew();
				this.children = null;
			}

			// Token: 0x17000EDA RID: 3802
			// (get) Token: 0x06005C77 RID: 23671 RVA: 0x002EE3D4 File Offset: 0x002EC7D4
			public string Label
			{
				get
				{
					return this.label;
				}
			}

			// Token: 0x17000EDB RID: 3803
			// (get) Token: 0x06005C78 RID: 23672 RVA: 0x002EE3F0 File Offset: 0x002EC7F0
			public Stopwatch Watch
			{
				get
				{
					return this.watch;
				}
			}

			// Token: 0x17000EDC RID: 3804
			// (get) Token: 0x06005C79 RID: 23673 RVA: 0x002EE40C File Offset: 0x002EC80C
			public List<ThreadLocalDeepProfiler.Watcher> Children
			{
				get
				{
					return this.children;
				}
			}

			// Token: 0x06005C7A RID: 23674 RVA: 0x002EE427 File Offset: 0x002EC827
			public void AddChildResult(ThreadLocalDeepProfiler.Watcher w)
			{
				if (this.children == null)
				{
					this.children = new List<ThreadLocalDeepProfiler.Watcher>();
				}
				this.children.Add(w);
			}

			// Token: 0x04003D67 RID: 15719
			private string label;

			// Token: 0x04003D68 RID: 15720
			private Stopwatch watch;

			// Token: 0x04003D69 RID: 15721
			private List<ThreadLocalDeepProfiler.Watcher> children;
		}
	}
}
