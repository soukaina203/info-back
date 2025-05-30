using DTO;
namespace Interfaces
{
public interface IService<TDto>
{
	Task<IEnumerable<TDto>> GetAll();
	Task<TDto> GetById(int id);
	Task<object> Post(TDto dto);
	Task<object> Put(int id, TDto dto);
	Task<ResponseDTO> Delete(int id);
}

}