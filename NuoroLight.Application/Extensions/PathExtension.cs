﻿namespace NuoroLight.Application.Extensions;
public static class PathExtension
{
    #region SiteUrl
    public static string SiteUrl = "http://localhost:5272";
    #endregion

    #region UploadImage
    public static string UploadImage = "/Images/Upload/";
    public static string UploadImageServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Upload/");
    #endregion

    #region DefaultAvatar
    public static string DefaultAvatar = "/Images/defaults/Default.jpg";
    public static string DefaultAvatarThum = "/Images/defaults/DefaultThum.jpg";
    #endregion

    #region user avatar
    public static string UserAvatarOrigin = "/Images/User/origin/";
    public static string UserAvatarOriginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/User/origin/");

    public static string UserAvatarThumb = "/Images/User/Thumb/";
    public static string UserAvatarThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/User/Thumb/");
    #endregion
}

