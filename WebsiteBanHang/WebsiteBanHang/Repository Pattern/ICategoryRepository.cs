using Microsoft.AspNetCore.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Repository_Pattern
{
    public interface ICategoryRepository { 
        IEnumerable<Category> GetAllCategories();
  
    }
}
