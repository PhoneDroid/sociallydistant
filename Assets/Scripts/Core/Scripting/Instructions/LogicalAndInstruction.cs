﻿#nullable enable
using System.Threading.Tasks;
using OS.Devices;

namespace Core.Scripting.Instructions
{
	public sealed class LogicalAndInstruction : ShellInstruction
	{
		private readonly ShellInstruction left;
		private readonly ShellInstruction right;

		public LogicalAndInstruction(ShellInstruction left, ShellInstruction right)
		{
			this.left = left;
			this.right = right;
		}

		/// <inheritdoc />
		public override async Task<int> RunAsync(ITextConsole console)
		{
			int exitCode = await left.RunAsync(console);
			if (exitCode != 0)
				return exitCode;
			
			return await right.RunAsync(console);
		}
	}
}