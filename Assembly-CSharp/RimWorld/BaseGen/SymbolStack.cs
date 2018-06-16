using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003E2 RID: 994
	public class SymbolStack
	{
		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06001102 RID: 4354 RVA: 0x0009185C File Offset: 0x0008FC5C
		public bool Empty
		{
			get
			{
				return this.stack.Count == 0;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06001103 RID: 4355 RVA: 0x00091880 File Offset: 0x0008FC80
		public int Count
		{
			get
			{
				return this.stack.Count;
			}
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x000918A0 File Offset: 0x0008FCA0
		public void Push(string symbol, ResolveParams resolveParams)
		{
			this.stack.Push(new Pair<string, ResolveParams>(symbol, resolveParams));
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x000918B8 File Offset: 0x0008FCB8
		public void Push(string symbol, CellRect rect)
		{
			this.Push(symbol, new ResolveParams
			{
				rect = rect
			});
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x000918E0 File Offset: 0x0008FCE0
		public void PushMany(ResolveParams resolveParams, params string[] symbols)
		{
			for (int i = 0; i < symbols.Length; i++)
			{
				this.Push(symbols[i], resolveParams);
			}
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x00091910 File Offset: 0x0008FD10
		public void PushMany(CellRect rect, params string[] symbols)
		{
			for (int i = 0; i < symbols.Length; i++)
			{
				this.Push(symbols[i], rect);
			}
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x00091940 File Offset: 0x0008FD40
		public Pair<string, ResolveParams> Pop()
		{
			return this.stack.Pop();
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x00091960 File Offset: 0x0008FD60
		public void Clear()
		{
			this.stack.Clear();
		}

		// Token: 0x04000A53 RID: 2643
		private Stack<Pair<string, ResolveParams>> stack = new Stack<Pair<string, ResolveParams>>();
	}
}
