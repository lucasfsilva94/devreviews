using System;
using System.Collections.Generic;

namespace DevReviews.API.Models
{
  public class ProductDetailsViewModel
  {
    public ProductDetailsViewModel(int id, string title, string description, decimal price, DateTime registeredAt, List<ProductReviewViewModel> reviews)
    {
      Id = id;
      Title = title;
      Description = description;
      Price = price;
      RegisteredAt = registeredAt;
      Reviews = reviews;
    }

    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public DateTime RegisteredAt { get; private set; }

    public List<ProductReviewViewModel> Reviews { get; private set; }
  }
}