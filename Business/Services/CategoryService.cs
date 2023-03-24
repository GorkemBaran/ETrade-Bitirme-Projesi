using AppCore.Business.Services.Bases;
using AppCore.Results;
using AppCore.Results.Bases;
using Business.Models;
using DataAccess.Entities;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public interface ICategoryService : IService<CategoryModel>
    {
        Task<List<CategoryModel>> GetListAsync();
    }

    public class CategoryService : ICategoryService
    {
        private readonly CategoryRepoBase _categoryRepo;

        public CategoryService(CategoryRepoBase categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public Result Add(CategoryModel model)
        {
            if (Query().Any(c => c.Name.ToUpper() == model.Name.ToUpper().Trim()))
                return new ErrorResult("Category with same name exists!");
            var entity = new Category()
            {
                Name = model.Name.Trim(),
                Description = model.Description?.Trim()
            };
            _categoryRepo.Add(entity);
            return new SuccessResult("Category created successfully.");
        }

        public Result Delete(int id)
        {
            var category = Query().SingleOrDefault(c => c.Id == id);
            if (category.ProductCountDisplay > 0)
                return new ErrorResult("Category cannot be deleted because it has products!");
            _categoryRepo.Delete(id);
            return new SuccessResult("Category deleted successfully.");
        }

        public void Dispose()
        {
            _categoryRepo.Dispose();
        }

        public IQueryable<CategoryModel> Query()
        {
            return _categoryRepo.Query(c => c.Products).OrderBy(c => c.Name).Select(c => new CategoryModel()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Guid = c.Guid,
                ProductCountDisplay = c.Products.Count

                // CategoryNameDescriptionDisplay = c.Name + " (" + c.Description + ")"
            });
        }

        public Result Update(CategoryModel model)
        {
            if (Query().Any(c => c.Name.ToUpper() == model.Name.ToUpper().Trim() && c.Id != model.Id))
                return new ErrorResult("Category with same name exists!");
            var entity = new Category()
            {
                Name = model.Name.Trim(),
                Description = model.Description?.Trim(),
                Id = model.Id
            };
            _categoryRepo.Update(entity);
            return new SuccessResult("Category updated successfully.");
        }

        public async Task<List<CategoryModel>> GetListAsync()
        {
            //Task<List<CategoryModel>> task = Query().ToListAsync();
            //List<CategoryModel> categories = task.Result;

            List<CategoryModel> categories = await Query().ToListAsync();
            return categories;
        }
    }
}
