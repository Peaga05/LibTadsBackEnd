using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using LibTads.Authorization;
using LibTads.Authorization.Roles;
using LibTads.Authorization.Users;
using LibTads.Autores.Dto;
using LibTads.Domain;
using LibTads.Roles.Dto;
using LibTads.Users.Dto;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTads.Autores
{
    [AbpAuthorize(PermissionNames.Pages_Autores)]
    public class AutorAppService : AsyncCrudAppService<Autor, AutorDto, int, PagedAutoresRequestDto, CreateAutorDto, UpdateAutorDto>
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

        public async Task<List<AutorDto>> GetAutores()
        {
            var autores = await Repository.GetAllListAsync(x=> !x.IsDeleted);
            return ObjectMapper.Map<List<AutorDto>>(autores);
        }

        public async Task<AutorDto> GetAutorById(int id)
        {
            var autor = await Repository.FirstOrDefaultAsync(x => x.Id == id);
            return ObjectMapper.Map<AutorDto>(autor);
        }
    }
}
