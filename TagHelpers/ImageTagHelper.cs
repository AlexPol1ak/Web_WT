using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;

namespace Poliak_UI_WT.TagHelpers
{
    public class ImageTagHelper : TagHelper
    {
        LinkGenerator _linkGenerator;

        public ImageTagHelper(LinkGenerator linkGenerator)
        { 
            _linkGenerator = linkGenerator;
        }

        public string ImgController { get; set; }
        public string ImgAction { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("src", _linkGenerator.GetPathByAction(ImgAction, ImgController));

        }
            

    }
}

