using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using Deptorygen.XUnit.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using TestHelper;

namespace Deptorygen.XUnit.Verifiers
{
	/*
	 * どんな仕様にしたい？
	 * 確実なテストとしての理想は以下のような感じ：
	 * - 最初の状態での診断をAssertする。
	 * - そのうちのどれをCodeFixするかを選択する。
	 * - 次の状態での診断をAssertする。
	 * - そのうちのどれをCodeFixするかを選択する。
	 * - 繰り返し。
	 *
	 * 以下のようなものを用意させる？
	 * - 期待する診断の情報.json (複数)
	 * - 期待するCodeFixの結果 (フォルダ内にファイルを並べる)(複数)
	 *
	 * これらを以下のようなAPIでAssertしていく：
	 * - GetDiagnostics() : Diagnostic[]
	 * - VerifyDiagnostics(json) : void
	 * - ApplyCodeFix() : Project
	 * - VerifyCodeFix(CodeFix folder) : void
	 *
	 * 例：
	 * ユーザーは以下のものを用意する：
	 * - 最初の状態での診断
	 *     - 最初の状態にどの診断に基づくCodeFixを適用するかの情報を含む
	 * - 最初の状態にCodeFixを施した結果
	 * - 次の状態での診断
	 *     - 次の状態にどの診断に基づくCodeFixを適用するかの情報を含む
	 * - 次の状態にCodeFixを施した結果
	 */

	class CodeFixApplier
	{
		private readonly DiagnosticAnalyzer _analyzer;
		private readonly CodeFixProvider _fix;

		public CodeFixApplier(DiagnosticAnalyzer analyzer, CodeFixProvider fix)
		{
			_analyzer = analyzer;
			_fix = fix;
		}

		public Project ApplyFix(Project project, int fixIndex, DiagnosticResult[] expetctResults)
		{
			// - CodeFixを適用したProjectを返す。
			// - 一度適用したCodeFixは再度適用しない。
			// - expectedに登録されている順番で適用する。

			var fixableDiagnostics = Helper.GetDiagnostics(project, _analyzer)
				.Where(d => _fix.FixableDiagnosticIds.Contains(d.Id))
				.ToArray();
			var attempts = fixableDiagnostics.Length;
			var performed = new HashSet<int>();

			for (int i = 0; i < attempts; i++)
			{
				var diag = GetNextDiagnostic(fixableDiagnostics, performed);
				if (diag is null)
				{
					break;
				}

				var doc = project.Documents.FirstOrDefault(d => d.Name == diag.Location.SourceTree.FilePath);
				if (doc == null)
				{
					fixableDiagnostics = fixableDiagnostics.Skip(1).ToArray();
					continue;
				}

				var operations = GetOperations(_fix, fixIndex, doc, diag);
				if (operations is null)
				{
					break;
				}

				var solution = operations.OfType<ApplyChangesOperation>().Single().ChangedSolution;
				project = solution.GetProject(project.Id);

				performed.Add(diag.GetHashCode());

				fixableDiagnostics = Helper.GetDiagnostics(project, _analyzer)
					.Where(d => _fix.FixableDiagnosticIds.Contains(d.Id)).ToArray();

				if (!fixableDiagnostics.Any()) break;
			}

			return project;
		}

		private static Diagnostic GetNextDiagnostic(Diagnostic[] fixableDiagnostics, HashSet<int> performed)
		{
			int index = -1;
			for (int j = 0; j < fixableDiagnostics.Length; j++)
			{
				var hash = fixableDiagnostics[j].GetHashCode();
				if (!performed.Contains(hash))
				{
					index = j;
					break;
				}
			}

			if (index == -1)
			{
				return null;
			}

			var diagnostic = fixableDiagnostics[index];
			return diagnostic;
		}

		private static ImmutableArray<CodeActionOperation>? GetOperations(CodeFixProvider fix, int fixIndex, Document doc, Diagnostic diag)
		{
			var actions = new List<CodeAction>();
			var fixContex = new CodeFixContext(doc, diag, (a, d) => actions.Add(a), CancellationToken.None);
			fix.RegisterCodeFixesAsync(fixContex).Wait();

			if (!actions.Any())
			{
				return null;
			}

			var codeAction = actions[fixIndex];

			var operations = codeAction.GetOperationsAsync(CancellationToken.None).Result;
			return operations;
		}
	}
}
