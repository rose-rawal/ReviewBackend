﻿using Microsoft.EntityFrameworkCore;
using ReviewApp.Data;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;
        public CategoryRepository(DataContext context)
        {
            _context= context;
        }

        public bool CategoryExists(int id)
        {
            var exists=_context.Categories.Any(c=>c.Id== id);
            return exists;
        }

        public ICollection<Category> GetCategories()
        {
            ICollection<Category> category=_context.Categories.OrderBy(c=>c.Id).ToList();
            return category;
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Where(c => c.Id == id).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonByCategory(int categoryId)
        {
            return _context.PokemonCategories.Where(c=>c.CategoryId== categoryId).Select(c=>c.Pokemon).ToList();
        }
    }
}