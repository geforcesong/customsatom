
namespace ProTemplate.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // The MetadataTypeAttribute identifies UIGroupMetadata as the class
    // that carries additional metadata for the UIGroup class.
    [MetadataTypeAttribute(typeof(UIGroup.UIGroupMetadata))]
    public partial class UIGroup
    {

        // This class allows you to attach custom attributes to properties
        // of the UIGroup class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class UIGroupMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private UIGroupMetadata()
            {
            }

            public string Icon { get; set; }

            public int Id { get; set; }

            public bool IsActive { get; set; }

            public string Name { get; set; }

            public int SortOrder { get; set; }

            [Include]
            public EntityCollection<UIPage> UIPage { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies UIPageMetadata as the class
    // that carries additional metadata for the UIPage class.
    [MetadataTypeAttribute(typeof(UIPage.UIPageMetadata))]
    public partial class UIPage
    {

        // This class allows you to attach custom attributes to properties
        // of the UIPage class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class UIPageMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private UIPageMetadata()
            {
            }

            public string Icon { get; set; }

            public int Id { get; set; }

            public bool IsActive { get; set; }

            public string Name { get; set; }

            public string NavigationURL { get; set; }

            public EntityCollection<RoleAccess> RoleAccess { get; set; }

            public int SortOrder { get; set; }

            public UIGroup UIGroup { get; set; }

            public int UIGroupId { get; set; }
        }
    }
}
