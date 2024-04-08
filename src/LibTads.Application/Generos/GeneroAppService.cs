using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
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
    public class GeneroAppService : LibTadsAppServiceBase
    {
        private readonly IRepository<Genero, int> Repository;
        public GeneroAppService(IRepository<Genero, int> _repository)
        {
            Repository = _repository;
        }

        public  async Task<GeneroDto> CreateAsync(CreateGeneroDto generoDto)
        {
            var havegenero = await Repository.FirstOrDefaultAsync(x => x.Descricao.Equals(generoDto.Descricao));
            if (havegenero != null)
            {
                if (havegenero.IsDeleted)
                {
                    havegenero.IsDeleted = false;
                    await Repository.UpdateAsync(havegenero);
                    return ObjectMapper.Map<GeneroDto>(havegenero);
                }
                throw new UserFriendlyException("Esse gênero já está cadastrado");
            }
            var genero = ObjectMapper.Map<Genero>(generoDto);
            genero.CreationTime = DateTime.Now;
            await Repository.InsertAsync(genero);
            return ObjectMapper.Map<GeneroDto>(genero);
        }

        public async Task<GeneroDto> UpdateAsync(UpdateGeneroDto generoDto)
        {
            var genero = ObjectMapper.Map<Genero>(generoDto);
            await Repository.UpdateAsync(genero);
            return ObjectMapper.Map<GeneroDto>(genero);
        }

        public async Task DeActivate(int idGenero)
        {
            var genero = await Repository.FirstOrDefaultAsync(x => x.Id == idGenero);
            genero.IsDeleted = true;
            await Repository.UpdateAsync(genero);
        }

        public async Task<PagedResultDto<GeneroDto>> GetGenero(PagedAutorResultRequestDto input, int pageNumber, int pageSize)
        {
            if (input.Keyword == null) input.Keyword = "";
            var query = Repository.GetAllListAsync(e => e.IsDeleted == false && e.Descricao.Contains(input.Keyword));

            var totalCount = query.Result.Count;
            var items = query.Result.Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize).ToList();

            var itemDtos = ObjectMapper.Map<List<GeneroDto>>(items);

            return new PagedResultDto<GeneroDto>(totalCount, itemDtos);
        }

        public async Task<GeneroDto> GetGeneroById(int id)
        {
            var genero = await Repository.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            return ObjectMapper.Map<GeneroDto>(genero);
        }

        public async Task<List<GeneroDto>> GetAllGenero()
        {
            var query = Repository.GetAllListAsync(e => e.IsDeleted == false);
            var itemDtos = ObjectMapper.Map<List<GeneroDto>>(query.Result);
            return itemDtos;
        }

    }
}
