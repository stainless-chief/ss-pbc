﻿using Abstractions.IRepository;
using Abstractions.Model;
using BusinessService.Logic.Supervision;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers.Private
{
    [ApiController]
    [Route("api/v1/")]
    public class PrivateCategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public PrivateCategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpPost("category")]
        public async Task<IActionResult> Save([FromBody] Category category)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _categoryRepository.SaveCategory(category);
            });

            return new JsonResult(result);
        }

        [HttpDelete("category")]
        public async Task<IActionResult> Delete([FromBody] Category category)
        {
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return _categoryRepository.DeleteCategory(category);
            });

            return new JsonResult(result);
        }
    }
}