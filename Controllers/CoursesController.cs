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
    public class CoursesController : ControllerBase
    {
        private readonly LmsApiContext _context;
        private readonly IMapper _mapper;


        public CoursesController(LmsApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        // CRUD - Read
        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourse(bool includeModules = true)
        {
            IEnumerable<CourseDto> courseDto;

            if (includeModules)
            {
                courseDto = await _mapper.ProjectTo<CourseUpdateDto>(_context.Course.Include(c => c.Modules)).ToListAsync();
            }
            else
            {
                courseDto = await _mapper.ProjectTo<CourseDto>(_context.Course).ToListAsync();
            }
            return Ok(courseDto);
        }


        // CRUD - read
        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourse(int id, bool includeModules = true)
    { 
            Course course;

        if (includeModules)
            {
            course = await _context.Course.Include(c => c.Modules).FirstOrDefaultAsync(c => c.Id == id);
         return course == null ? NotFound() : Ok(_mapper.Map<CourseUpdateDto>(course));
    }
    course = await _context.Course.FirstOrDefaultAsync(c => c.Id == id);
        return course == null ? NotFound() : Ok(_mapper.Map<CourseDto>(course));
}

//CRUD - Update
// PUT: api/Courses/5
// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
[HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, [FromBody] CourseDto courseDto)
        {
            var preCourse = _context.Course.Find(id);

            var course = (Course)_mapper.ProjectTo<Course>((IQueryable)courseDto);

            if (course.Id != preCourse.Id)
            {
                return BadRequest(400);
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
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
        // CRUD  create
        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CourseDto>> PostCourse(CourseDto courseDto)
        {
            var course = (Course)_mapper.ProjectTo<Course>((IQueryable)courseDto);

            _context.Course.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = course.Id }, course);
        }

        // CRUD - delete
        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound(404);
            }

            _context.Course.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

       
        //CRUD - Patch
        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPatch("{courseId}")]
        public async Task<ActionResult<CourseDto>> PatchCourse(int courseId, [FromBody] JsonPatchDocument<CourseDto> patchDoc)
        {
            if (patchDoc == null)

            {
                return BadRequest("patchDoc object is null");
            }


        Course course = await _context.Course.FindAsync(courseId);

        if (course == null)
        {
            return NotFound($"Course with id {courseId} not found!");
        }

        CourseDto courseDto = _mapper.Map<CourseDto>(course);

        patchDoc.ApplyTo(courseDto, ModelState);

        if (!TryValidateModel(courseDto))
        {
            return BadRequest("Model is not valid!");
        }

        _mapper.Map(courseDto, course);

        if (await _context.SaveChangesAsync() < 0)
        {
            return StatusCode(500, "Unable to save");
        }
        return Ok(_mapper.Map<CourseDto>(course));
    }

    private bool CourseExists(int id)
    {
        return _context.Course.Any(e => e.Id == id);
    }
   

}


