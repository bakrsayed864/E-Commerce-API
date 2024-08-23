using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Own_Service.DTO;
using Own_Service.Models;
using Own_Service.Services;

namespace Own_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var list=_categoryRepository.getAll();
            if(list.Count==0)
                return BadRequest("no categories exist");
            return Ok(list);
        }
        
        [HttpGet("{id}",Name ="GetOneCategory")]
        public IActionResult GetOne(int id)
        {
            var category = _categoryRepository.getOne(id);
            if(category==null)
                return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public IActionResult Create(CategoryDTO categorydto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            int changes=_categoryRepository.Creat(categorydto);
            if (changes == 0)
                return BadRequest("can not save category");
            string link = Url.Link("GetOneCategory", new { id = categorydto.Id });
            return Created(link,categorydto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Category category, int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            int changes = _categoryRepository.Update(category, id);
            if (changes == 0)
                return NotFound();
            return Ok(category);
        }

        [HttpDelete]
        public IActionResult Delet(int id)
        {
            int changes=_categoryRepository.Delet(id);
            if (changes == 0)
                return NotFound("no category found with specific id");
            return NoContent(); //204
        }

        
    }
}
