using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using LibTads.Authorization;
using LibTads.Autores.Dto;
using LibTads.Domain;
using LibTads.Generos.Dto;
using LibTads.Roles.Dto;
using LibTads.Users;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;

namespace LibTads.Autores
{
    [AbpAuthorize(PermissionNames.Pages_Autores)]
    public class AutorAppService : LibTadsAppServiceBase
    {
        private readonly IRepository<Autor, int> _repository;
        public AutorAppService(IRepository<Autor, int> repository)
        {
            _repository = repository;
        }

        public async Task<AutorDto> CreateAsync(CreateAutorDto autorDto)
        {
            var haveAutor = await _repository.FirstOrDefaultAsync(x => x.Nome.Equals(autorDto.Nome));
            if (haveAutor != null)
            {
                if (haveAutor.IsDeleted)
                {
                    haveAutor.IsDeleted = false;
                    await _repository.UpdateAsync(haveAutor);
                    return ObjectMapper.Map<AutorDto>(haveAutor);
                }
                throw new UserFriendlyException("Esse autor já está cadastrado");
            }
            var autor = ObjectMapper.Map<Autor>(autorDto);
            autor.CreationTime = DateTime.Now;
            await _repository.InsertAsync(autor);
            return ObjectMapper.Map<AutorDto>(autor);
        }

        public async Task<AutorDto> UpdateAsync(UpdateAutorDto autorDto)
        {
            var autor = ObjectMapper.Map<Autor>(autorDto);
            await _repository.UpdateAsync(autor);
            return ObjectMapper.Map<AutorDto>(autor);
        }

        public async Task DeActivate(int idAutor)
        {
            var autor = await _repository.FirstOrDefaultAsync(x => x.Id == idAutor);
            autor.IsDeleted = true;
            await _repository.UpdateAsync(autor);
        }

        public async Task<PagedResultDto<AutorDto>> GetAutores(PagedAutorResultRequestDto input, int pageNumber, int pageSize)
        {
            if (input.Keyword == null) input.Keyword = "";
            var query = _repository.GetAllListAsync(e => e.IsDeleted == false && e.Nome.Contains(input.Keyword));

            var totalCount = query.Result.Count;
            var items = query.Result.Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize).ToList();

            var itemDtos = ObjectMapper.Map<List<AutorDto>>(items);

            return new PagedResultDto<AutorDto>(totalCount, itemDtos);
        }

        public async Task<AutorDto> GetAutorById(int id)
        {
            var autor = await _repository.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            return ObjectMapper.Map<AutorDto>(autor);
        }

        public IQueryable<Autor> CreateFilteredQuery(PagedRoleResultRequestDto input)
        {
            return _repository.GetAllIncluding()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Nome.Contains(input.Keyword));
        }

        public async Task<List<AutorDto>> GetAllAutor()
        {
            var query = _repository.GetAllListAsync(e => e.IsDeleted == false);
            var itemDtos = ObjectMapper.Map<List<AutorDto>>(query.Result);
            return itemDtos;

        }
    }
}
