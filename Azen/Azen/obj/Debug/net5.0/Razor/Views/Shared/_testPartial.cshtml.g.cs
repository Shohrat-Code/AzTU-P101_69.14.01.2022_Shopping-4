#pragma checksum "D:\Code Academy\Teaching\AzTU-Code\AzTU-P101\Lessons\69.14.01.2022-Shopping-4\Codes\Azen\Azen\Views\Shared\_testPartial.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "841170499b4ee28448ef034302db87fd8a79d7cd"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__testPartial), @"mvc.1.0.view", @"/Views/Shared/_testPartial.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\Code Academy\Teaching\AzTU-Code\AzTU-P101\Lessons\69.14.01.2022-Shopping-4\Codes\Azen\Azen\Views\_ViewImports.cshtml"
using Azen;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Code Academy\Teaching\AzTU-Code\AzTU-P101\Lessons\69.14.01.2022-Shopping-4\Codes\Azen\Azen\Views\_ViewImports.cshtml"
using Azen.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\Code Academy\Teaching\AzTU-Code\AzTU-P101\Lessons\69.14.01.2022-Shopping-4\Codes\Azen\Azen\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\Code Academy\Teaching\AzTU-Code\AzTU-P101\Lessons\69.14.01.2022-Shopping-4\Codes\Azen\Azen\Views\_ViewImports.cshtml"
using Azen.ViewModels;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\Code Academy\Teaching\AzTU-Code\AzTU-P101\Lessons\69.14.01.2022-Shopping-4\Codes\Azen\Azen\Views\_ViewImports.cshtml"
using Azen.Data;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"841170499b4ee28448ef034302db87fd8a79d7cd", @"/Views/Shared/_testPartial.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6f4a3c15cf04e67c9b92d21ca0c09195a659c1d3", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__testPartial : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<string>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<ul>\r\n    <li>1</li>\r\n    <li>2</li>\r\n    <li>3</li>\r\n    <li>4</li>\r\n    <li>");
#nullable restore
#line 8 "D:\Code Academy\Teaching\AzTU-Code\AzTU-P101\Lessons\69.14.01.2022-Shopping-4\Codes\Azen\Azen\Views\Shared\_testPartial.cshtml"
   Write(Model);

#line default
#line hidden
#nullable disable
            WriteLiteral(" </li>\r\n</ul>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<string> Html { get; private set; }
    }
}
#pragma warning restore 1591