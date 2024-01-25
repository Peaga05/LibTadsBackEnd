using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using LibTads.Authorization;
using LibTads.Autores.Dto;
using LibTads.Domain;
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
    public class AutorAppService : AsyncCrudAppService<Autor, AutorDto, int, PagedAutorResultRequestDto, CreateAutorDto, UpdateAutorDto>
    {
        public AutorAppService(IRepository<Autor, int> repository) : base(repository)
        {
        }

        public override async Task<AutorDto> CreateAsync(CreateAutorDto autorDto)
        {
            CheckCreatePermission();
            var autor = ObjectMapper.Map<Autor>(autorDto);
            autor.CreationTime = DateTime.Now;
            await Repository.InsertAsync(autor);
            return MapToEntityDto(autor);
        }

        public override async Task<AutorDto> UpdateAsync(UpdateAutorDto autorDto)
        {
            CheckUpdatePermission();
            var autor = ObjectMapper.Map<Autor>(autorDto);
            await Repository.UpdateAsync(autor);
            return await GetAsync(autorDto);
        }

        public async Task DeActivate(int idAutor)
        {
            CheckUpdatePermission();
            var autor = await Repository.FirstOrDefaultAsync(x => x.Id == idAutor);
            autor.IsDeleted = true;
            await Repository.UpdateAsync(autor);
        }

        public async Task<PagedResultDto<AutorDto>> GetAutores(PagedRoleResultRequestDto input, int pageNumber, int pageSize)
        {
            if (input.Keyword == null) input.Keyword = "";
            var query = Repository.GetAllListAsync(e => e.IsDeleted == false && e.Nome.Contains(input.Keyword));

            var totalCount = query.Result.Count;
            var items = query.Result.Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize).ToList();

            var itemDtos = ObjectMapper.Map<List<AutorDto>>(items); 

            return new PagedResultDto<AutorDto>(totalCount, itemDtos);
        }

        public async Task<AutorDto> GetAutorById(int id)
        {
            var autor = await Repository.FirstOrDefaultAsync(x => x.Id == id);
            return ObjectMapper.Map<AutorDto>(autor);
        }

        public IQueryable<Autor> CreateFilteredQuery(PagedRoleResultRequestDto input)
        {
            return Repository.GetAllIncluding()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Nome.Contains(input.Keyword));
        }
    }
}
