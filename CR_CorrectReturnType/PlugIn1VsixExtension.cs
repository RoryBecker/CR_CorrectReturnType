using System.ComponentModel.Composition;
using DevExpress.CodeRush.Common;

namespace CR_CorrectReturnType
{
    [Export(typeof(IVsixPluginExtension))]
    public class CR_CorrectReturnTypeExtension : IVsixPluginExtension { }
}