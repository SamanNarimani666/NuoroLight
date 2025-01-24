﻿using System.ComponentModel.DataAnnotations;
namespace NuoroLight.Domain.ViewModels.User;

public class EditUserForAdminViewModel
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    [Display(Name = "نام ")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string? FirstName { get; set; }
    [Display(Name = "نام خانوادگی")]

    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string? LastName { get; set; }
    public string? Avatar { get; set; }
    [Display(Name = "کلمه عبور")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    [MinLength(8, ErrorMessage = "{0} نمی تواند كمتر از {1} کاراکتر باشد .")]
    public string? PassWord { get; set; }

    [Display(Name = "تکرار کلمه عبور")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    [MinLength(8, ErrorMessage = "{0} نمی تواند كمتر از {1} کاراکتر باشد .")]
    [Compare("PassWord", ErrorMessage = "کلمه های عبور مغایرت دارند")]
    public string? ConferimPassWord { get; set; }

    [Display(Name = "فعال/غیر فعال ")]
    public bool IsActive { get; set; }

    [Display(Name = "بلاک ")]
    public bool IsBlock { get; set; }
}
public enum EditUserResult
{
    Success,
    NotFound,
    Error
}


