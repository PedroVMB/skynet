using System;
using System.Formats.Asn1;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IGenericRepository<Product> _context) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand,
        string? type, string? sort)
    {
        var spec = new ProductSpecification(brand, type, sort);

        var products = await _context.ListAsync(spec);

        return Ok(products);
    }

    [HttpGet("{id:int}")] // api.products/2
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _context.GetByIdAsync(id);

        if (product == null) return NotFound();

        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        _context.Add(product);

        if (await _context.SaveAllAsync())
            return CreatedAtAction("GeTProduct", new { id = product.Id}, product);

        return BadRequest("Problem creating product");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (product.Id != id || !ProductExists(id))
            return BadRequest("Cannot update this product");

        _context.Update(product);

        if (await _context.SaveAllAsync())
            return NoContent();

        return BadRequest("Problem updating the product");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {

        var product = await _context.GetByIdAsync(id);

        if (product == null) return NotFound();

        _context.Remove(product);

         if (await _context.SaveAllAsync())
            return NoContent();

        return BadRequest("Problem deleting the product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var spec = new BrandListSpecification();

        return Ok(await _context.ListAsync(spec));
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var spec = new TypeListSpecification();

        return Ok(await _context.ListAsync(spec));
    }

    private bool ProductExists(int id)
    {
        return _context.Exists(id);
    }

}
