using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace cuoiki.Models;

public partial class Product
{
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
    [Display(Name = "Tên sản phẩm")]
    public string? NamePro { get; set; }

    [Display(Name = "Mô tả")]
    public string? DecriptionPro { get; set; }

    [Required(ErrorMessage = "Loại sản phẩm không được để trống")]
    [Display(Name = "Loại sản phẩm")]
    public string? Category { get; set; }

    [Required(ErrorMessage = "Giá sản phẩm không được để trống")]
    [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
    [Display(Name = "Giá")]
    [DisplayFormat(DataFormatString = "{0:N0} VNĐ")]
    public decimal? Price { get; set; }

    [Display(Name = "Hình ảnh")]
    public string? ImagePro { get; set; }

    [Required(ErrorMessage = "Ngày sản xuất không được để trống")]
    [Display(Name = "Ngày sản xuất")]
    [DataType(DataType.Date)]
    public DateOnly ManufacturingDate { get; set; }
}
