﻿namespace BaseNKatmanProject.Application.DTOs.Product;
public class ProductCreateDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
}