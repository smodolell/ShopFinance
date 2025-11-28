using System.ComponentModel;

namespace ShopFinance.Domain.Enums;

public enum UploadType : byte
{
    [Description(@"Products")] Product,
    [Description(@"ProfilePictures")] ProfilePicture,
    [Description(@"Documents")] Document,
    [Description(@"Images")] Image,
}