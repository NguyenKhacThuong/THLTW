﻿using System.Collections.Generic;
using System.Linq; 
using WebBanHang.Models;

namespace WebBanHang.Repository_Pattern
{
    
    public class MockCategoryRepository : ICategoryRepository
    {
        private List<Category> _categoryList;

        public MockCategoryRepository()
        {
            _categoryList = new List<Category>
            {
                new Category { Id = 1, Name = "Laptop" },
                new Category { Id = 2, Name = "Desktop" },
            };
        }

        
        public IEnumerable<Category> GetAllCategories()
        {
            return _categoryList;
        }
    }
}