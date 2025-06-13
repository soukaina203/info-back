using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interfaces;
using DTO;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[ApiController]
[Route("api/[controller]/[action]")]
public class SuperController<TDto, TService> : ControllerBase
	where TService : class, IService<TDto>
{
	protected readonly TService _service;

	public SuperController(TService service)
	{
		_service = service;
	}

	[HttpGet]
	public virtual async Task<ActionResult<IEnumerable<TDto>>> GetAll()
	{
		var result = await _service.GetAll();
		return Ok(result);
	}

	[HttpGet("{id}")]
	public virtual async Task<ActionResult<TDto>> GetById(int id)
	{
		var result = await _service.GetById(id);
		return Ok(result);
	}

	[HttpPost]
	public virtual async Task<ActionResult<TDto>> Post([FromBody] TDto dto)
	{
		var result = await _service.Post(dto);
		return Ok(result);
	}

	[HttpPut("{id}")]
	public virtual async Task<ActionResult<TDto>> Put(int id, [FromBody] TDto dto)
	{
		var result = await _service.Put(id, dto);
		return Ok(result);
	}

	[HttpDelete("{id}")]
	public virtual async Task<ResponseDTO> Delete(int id)
	{
		var success = await _service.Delete(id);
		return success ;
	}
}
