#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lms.Data.Data;
using Lms.Core.Entities;
using AutoMapper;
using Lms.Core.Dto;
using Microsoft.AspNetCore.JsonPatch;


namespace Lms.Api.Controllers;

   [Route("api/[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly LmsApiContext _context;
        private readonly IMapper _mapper;

        public ModulesController(LmsApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // CRUD - read
        // GET: api/Modules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModuleDto>>> GetModule()
        {
            var moduleDto = await _mapper.ProjectTo<ModuleDto>(_context.Module).ToListAsync();
            return Ok(moduleDto);
        }

        //CRUD - Read
        // GET: api/Modules/5
        [HttpGet("query")]
    public async Task<ActionResult<IEnumerable<ModuleDto>>> GetModule(int? id, string title)
    {
        IEnumerable<ModuleDto> modules;
        if (id != null)
        {
            modules = new List<ModuleDto>();

            var module = await _context.Module.FindAsync(id);

            if (module == null)
            {
                return NotFound($"Module with id {id} not found.");
            }

            ModuleDto moduleDto = _mapper.Map<ModuleDto>(module);

            //modules = new List<ModuleDto>((IEnumerable<ModuleDto>)_mapper.Map<ModuleDto>(module));
            ((List<ModuleDto>)modules).Add(moduleDto);

            return Ok(modules);
        }
        modules = await _mapper.ProjectTo<ModuleDto>(_context.Module).Where(m => m.Title == title).ToListAsync();
        return Ok(modules);
    }
    /*[HttpGet("{title}")]
    public async Task<ActionResult<IEnumerable<ModuleDto>>> GetModule(string title)
    {
        var moduleDto = await _mapper.ProjectTo<ModuleDto>(_context.Module).Where(m => m.Title == title).ToListAsync();
        return Ok(moduleDto);
    }*/
    //CRUD - update
    // PUT: api/Modules/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
        public async Task<IActionResult> PutModule(int id, ModuleDto moduleDto)
        {
            var preModule = _context.Course.Find(id);

            var module = (Module)_mapper.ProjectTo<Module>((IQueryable)moduleDto);

            if (module.Id != preModule.Id)
            {
                return BadRequest(400);
            }

            _context.Entry(module).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModuleExists(id))
                {
                    return NotFound(404);
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //CRUD - create
        // POST: api/Modules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ModuleDto>> PostModule(ModuleDto moduleDto)
        {
            var module = (Module)_mapper.ProjectTo<Module>((IQueryable)moduleDto);


            _context.Module.Add(module);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = module.Id }, module);
        }

        //CRUD - delete
        // DELETE: api/Modules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModule(int id)
        {
            var @module = await _context.Module.FindAsync(id);
            if (@module == null)
            {
                return NotFound(404);
            }

            _context.Module.Remove(@module);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    [HttpPatch("{moduleId}")]
    public async Task<ActionResult<ModuleDto>> PatchModule(int moduleId, JsonPatchDocument<ModuleDto> patchDoc)
    {
        if (patchDoc == null)
        {
            return BadRequest("PatchDocument is null!"); 
        }

        Module module = await _context.Module.FindAsync(moduleId);

        if (module == null)
        {
            return NotFound($"Module with id {moduleId} is not found!");
        }

        ModuleDto moduleDto = _mapper.Map<ModuleDto>(module);

        patchDoc.ApplyTo(moduleDto, ModelState);

        if (!TryValidateModel(moduleDto))
        {
            return BadRequest("Model is not valid!");
        }

        _mapper.Map(moduleDto, module);

        if (await _context.SaveChangesAsync() < 0)
        {
            return StatusCode(500, "Failed to save");
        }
        return Ok(_mapper.Map<ModuleDto>(module));
    }





    private bool ModuleExists(int id)
        {
            return _context.Module.Any(e => e.Id == id);
        }
    }
