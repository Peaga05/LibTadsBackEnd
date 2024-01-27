using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using LibTads.Authorization;
using LibTads.Autores.Dto;
using LibTads.Domain;
using LibTads.Generos.Dto;
using LibTads.Roles.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTads.Generos
{
    [AbpAuthorize(PermissionNames.Pages_Generos)]
    public class GeneroAppService : AsyncCrudAppService<Genero, GeneroDto, int, PagedGeneroResultRequestDto, CreateGeneroDto, UpdateGeneroDto>
    {
        public GeneroAppService(IRepository<Genero, int> repository) : base(repository)
        {
        }

        public override async Task<GeneroDto> CreateAsync(CreateGeneroDto generoDto)
        {
            CheckCreatePermission();
            var genero = ObjectMapper.Map<Genero>(generoDto);
            genero.CreationTime = DateTime.Now;
            await Repository.InsertAsync(genero);
            return MapToEntityDto(genero);
        }

        public override async Task<GeneroDto> UpdateAsync(UpdateGeneroDto generoDto)
        {
            CheckCreatePermission();
            var genero = ObjectMapper.Map<Genero>(generoDto);
            await Repository.UpdateAsync(genero);
            return await GetAsync(generoDto);
        }

        public async Task DeActivate(int idGenero)
        {
            CheckUpdatePermission();
            var genero = await Repository.FirstOrDefaultAsync(x => x.Id == idGenero);
            genero.IsDeleted = true;
            await Repository.UpdateAsync(genero);
        }

        public async Task<PagedResultDto<GeneroDto>> GetGenero(PagedRoleResultRequestDto input, int pageNumber, int pageSize)
        {
            if (input.Keyword == null) input.Keyword = "";
            var query = Repository.GetAllListAsync(e => e.IsDeleted == false && e.Descricao.Contains(input.Keyword));

            var totalCount = query.Result.Count;
            var items = query.Result.Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize).ToList();

            var itemDtos = ObjectMapper.Map<List<GeneroDto>>(items);

            return new PagedResultDto<GeneroDto>(totalCount, itemDtos);
        }

        public async Task<GeneroDto> GetAutorById(int id)
        {
            var genero = await Repository.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            return ObjectMapper.Map<GeneroDto>(genero);
        }
    }
}
