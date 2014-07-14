using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.CodeRush.Core;
using DevExpress.CodeRush.PlugInCore;
using DevExpress.CodeRush.StructuralParser;

namespace CR_CorrectReturnType
{
    public partial class PlugIn1 : StandardPlugIn
    {
        // DXCore-generated code...
        #region InitializePlugIn
        public override void InitializePlugIn()
        {
            base.InitializePlugIn();
            registerCorrectReturnType();
        }
        #endregion
        #region FinalizePlugIn
        public override void FinalizePlugIn()
        {
            base.FinalizePlugIn();
        }
        #endregion
        public void registerCorrectReturnType()
        {
            DevExpress.CodeRush.Core.CodeProvider CorrectReturnType = new DevExpress.CodeRush.Core.CodeProvider(components);
            ((System.ComponentModel.ISupportInitialize)(CorrectReturnType)).BeginInit();
            CorrectReturnType.ProviderName = "CorrectReturnType"; // Should be Unique
            CorrectReturnType.DisplayName = "Correct Return Type";
            CorrectReturnType.CheckAvailability += CorrectReturnType_CheckAvailability;
            CorrectReturnType.PreparePreview += CorrectReturnType_PreparePreview;
            CorrectReturnType.Apply += CorrectReturnType_Apply;
            ((System.ComponentModel.ISupportInitialize)(CorrectReturnType)).EndInit();
        }

        private void CorrectReturnType_CheckAvailability(Object sender, CheckContentAvailabilityEventArgs ea)
        {
            if (ea.Element.ElementType != LanguageElementType.Return)
                return;
            Method Parent = ea.Element.GetParentMethod();
            if (Parent == null)
                return;
            if (Parent.MemberType == "void")
                return;

            var Return = (Return)ea.Element;

            var ExpressionType = Return.Expression.Resolve(ParserServices.SourceTreeResolver);

            if (ExpressionType.Name == Parent.MemberType)
                return;
            ea.Available = true;  
        }
        private void CorrectReturnType_PreparePreview(object sender, PrepareContentPreviewEventArgs ea)
        {
            Method method = ea.Element.GetParentMethod();
            SourceRange OldTypeRange = GetMethodReturnTypeRange(method);
            ea.AddStrikethrough(OldTypeRange);

            var Return = (Return)ea.Element;
            string Code = GetExpressionTypeCode(Return.Expression);

            ea.AddCodePreview(OldTypeRange.Start, Code);
        }
        private void CorrectReturnType_Apply(Object sender, ApplyContentEventArgs ea)
        {
            Method method = ea.Element.GetParentMethod();
            SourceRange TypeRange = GetMethodReturnTypeRange(method);
            var Return = (Return)ea.Element;

            string Code = GetExpressionTypeCode(Return.Expression);
            ea.TextDocument.SetText(TypeRange, Code);

        }

        private static string GetExpressionTypeCode(Expression Expression)
        {
            var resolvedElement = Expression.Resolve(ParserServices.SourceTreeResolver);
            ITypeReferenceExpression typeRef = resolvedElement as ITypeReferenceExpression;
            var simpleName = CodeRush.Language.GetSimpleTypeName(resolvedElement.FullName);

            var Code = CodeRush.CodeMod.GenerateCode(SourceModelUtils.CreateTypeReferenceExpression(resolvedElement));
            
            //var Code = CodeRush.CodeMod.GenerateCode(new TypeReferenceExpression(simpleName), true);
            return CodeRush.Language.GetSimpleTypeName(Code);
        }
        private static SourceRange GetMethodReturnTypeRange(Method method)
        {
            var methodTypeReference = method.MemberTypeReference;
            SourceRange TypeRange = methodTypeReference.Range;
            return TypeRange;
        }
    }
}