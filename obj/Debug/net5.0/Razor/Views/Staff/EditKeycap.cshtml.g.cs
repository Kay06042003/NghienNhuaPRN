#pragma checksum "D:\ProjectPRN\Nghien_Nhua\Views\Staff\EditKeycap.cshtml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "d7a514991129ae4579d413c833f4c882beeafc725966f4af9807b79ef65502f1"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Staff_EditKeycap), @"mvc.1.0.view", @"/Views/Staff/EditKeycap.cshtml")]
namespace AspNetCore
{
    #line hidden
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading.Tasks;
    using global::Microsoft.AspNetCore.Mvc;
    using global::Microsoft.AspNetCore.Mvc.Rendering;
    using global::Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\ProjectPRN\Nghien_Nhua\Views\_ViewImports.cshtml"
using Nghien_Nhua;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\ProjectPRN\Nghien_Nhua\Views\_ViewImports.cshtml"
using Nghien_Nhua.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"Sha256", @"d7a514991129ae4579d413c833f4c882beeafc725966f4af9807b79ef65502f1", @"/Views/Staff/EditKeycap.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"Sha256", @"82e0436fdd10ae7141b44e3979f6ca4fcc3015b977b6d488224fe828f25a5022", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Staff_EditKeycap : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Keycap>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "D:\ProjectPRN\Nghien_Nhua\Views\Staff\EditKeycap.cshtml"
  
    ViewData["Title"] = "EditKeycap";
    Layout = "~/Views/Shared/_LayoutDefault.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<div class=""col-md-9 row my-2 d-flex justify-content-between"">
    <div class=""col-auto"">
        <label for=""exampleFormControlInput1"" class=""form-label"">Chất liệu:</label>
    </div>
    <div class=""col-md-10"">
        <input type=""text"" class=""form-control"" id=""keycap_material"" name=""material_key_cap""");
            BeginWriteAttribute("value", " value=\"", 428, "\"", 453, 1);
#nullable restore
#line 12 "D:\ProjectPRN\Nghien_Nhua\Views\Staff\EditKeycap.cshtml"
WriteAttributeValue("", 436, Model.KcMaterial, 436, 17, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@">
        <div class=""form-message""></div>
    </div>
</div>
<div class=""col-md-9 row my-2 d-flex justify-content-between"">
    <div class=""col-auto"">
        <label for=""exampleFormControlInput1"" class=""form-label"">Layout:</label>
    </div>
    <div class=""col-md-10"">
        <input type=""text"" class=""form-control"" id=""keycap_layout"" name=""layout_keycap""");
            BeginWriteAttribute("value", " value=\"", 821, "\"", 844, 1);
#nullable restore
#line 21 "D:\ProjectPRN\Nghien_Nhua\Views\Staff\EditKeycap.cshtml"
WriteAttributeValue("", 829, Model.KcLayout, 829, 15, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@">
        <div class=""form-message""></div>
    </div>
</div>
<div class=""col-md-9 row my-2 d-flex justify-content-between"">
    <div class=""col-auto"">
        <label for=""exampleFormControlInput1"" class=""form-label"">Độ dày:</label>
    </div>
    <div class=""col-md-10"">
        <input type=""text"" class=""form-control"" id=""keycap_thickness"" name=""thickness_keycap""");
            BeginWriteAttribute("value", "\r\n            value=\"", 1218, "\"", 1257, 1);
#nullable restore
#line 31 "D:\ProjectPRN\Nghien_Nhua\Views\Staff\EditKeycap.cshtml"
WriteAttributeValue("", 1239, Model.KcThickness, 1239, 18, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@">
        <div class=""form-message""></div>
    </div>
</div>
<div class=""col-md-9 row my-2 d-flex justify-content-between"">
    <div class=""col-auto"">
        <label for=""exampleFormControlInput1"" class=""form-label"">Độ bền:</label>
    </div>
    <div class=""col-md-10"">
        <input type=""text"" class=""form-control"" id=""keycap_durability"" name=""durability_keycap""");
            BeginWriteAttribute("value", "\r\n            value=\"", 1633, "\"", 1674, 1);
#nullable restore
#line 41 "D:\ProjectPRN\Nghien_Nhua\Views\Staff\EditKeycap.cshtml"
WriteAttributeValue("", 1654, Model.KcReliability, 1654, 20, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n        <div class=\"form-message\"></div>\r\n    </div>\r\n</div>");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Keycap> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
