﻿namespace OpenStackNetAnalyzers
{
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class RenderAsMarkdownAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "RenderAsMarkdown";
        internal const string Title = "Render documentation as Markdown (Refactoring)";
        internal const string MessageFormat = "Render documentation as Markdown";
        internal const string Category = "OpenStack.Documentation";
        internal const string Description = "Render documentation as Markdown (Refactoring)";

        private static DiagnosticDescriptor Descriptor =
            new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Hidden, isEnabledByDefault: true, description: Description);

        private static readonly ImmutableArray<DiagnosticDescriptor> _supportedDiagnostics =
            ImmutableArray.Create(Descriptor);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                return _supportedDiagnostics;
            }
        }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(HandleDocumentedNode, SyntaxKind.PropertyDeclaration);
        }

        private void HandleDocumentedNode(SyntaxNodeAnalysisContext context)
        {
            DocumentationCommentTriviaSyntax documentationComment = context.Node.GetDocumentationCommentTriviaSyntax();
            if (documentationComment == null)
                return;

            // only report the diagnostic for elements which have documentation comments
            context.ReportDiagnostic(Diagnostic.Create(Descriptor, documentationComment.GetLocation()));
        }
    }
}