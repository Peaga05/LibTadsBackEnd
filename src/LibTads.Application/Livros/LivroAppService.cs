using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using LibTads.Authorization;
using LibTads.Autores.Dto;
using LibTads.Domain;
using LibTads.Livros.Dto;
using LibTads.Roles.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibTads.Livros
{
    [AbpAuthorize(PermissionNames.Pages_Livros)]
    public class LivroAppService : LibTadsAppServiceBase
    {
        private readonly IRepository<Livro, int> Repository;
        private readonly IRepository<Autor, int> _repositoryAutor;
        private readonly IRepository<Genero, int> _repositoryGenero;
        public LivroAppService(
            IRepository<Livro, int> repository,
            IRepository<Autor, int> repositoryAutor,
            IRepository<Genero, int> repositoryGenero
        )
        {
            _repositoryAutor = repositoryAutor;
            _repositoryGenero = repositoryGenero;
            Repository = repository;
        }

        public async Task<LivroDto> CreateAsync(CreateLivroDto livroDto)
        {
            var haveLivro = await Repository.FirstOrDefaultAsync(x => x.Isbn.Equals(livroDto.Isbn));
            if (haveLivro != null)
            {
                if (haveLivro.IsDeleted)
                {
                    haveLivro.IsDeleted = false;
                    await Repository.UpdateAsync(haveLivro);
                    return ObjectMapper.Map<LivroDto>(haveLivro);
                }
                throw new UserFriendlyException("Esse livro já está cadastrado");
            }
            var livro = ObjectMapper.Map<Livro>(livroDto);
            livro.CreationTime = DateTime.Now;
            livro.QuantidadeDisponivel = livro.Quantidade;
            await Repository.InsertAsync(livro);
            return ObjectMapper.Map<LivroDto>(livro);
        }

        public async Task<LivroDto> UpdateAsync(UpdateLivroDto livroDto)
        {
            var quantidade = 0;
            var haveIsbn = Repository.FirstOrDefault(x => x.Isbn.Equals(livroDto.Isbn) && x.Id != livroDto.Id);
            if (haveIsbn != null)
            {
                throw new UserFriendlyException("Esse isbn esta vinculado a outro livro!");
            }
            quantidade = AlterarQuantidadeDisponivel(livroDto, quantidade);
            var livro = ObjectMapper.Map<Livro>(livroDto);
            livro.QuantidadeDisponivel += quantidade;
            await Repository.UpdateAsync(livro);
            return ObjectMapper.Map<LivroDto>(livro);
        }

        public async Task DeActivate(int idLivro)
        {
            var livro = await Repository.FirstOrDefaultAsync(x => x.Id == idLivro);
            livro.IsDeleted = true;
            await Repository.UpdateAsync(livro);
        }

        public async Task<PagedResultDto<LivroDto>> GetLivros(PagedLivroResultRequestDto input, int pageNumber, int pageSize)
        {
            if (input.Keyword == null) input.Keyword = "";
            var query = Repository.GetAllListAsync(e => e.IsDeleted == false && (e.Titulo.Contains(input.Keyword) || e.Autor.Nome.Contains(input.Keyword) || e.Genero.Descricao.Contains(input.Keyword)));

            var totalCount = query.Result.Count;
            var items = query.Result.Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize).ToList();

            var itemDtos = ObjectMapper.Map<List<LivroDto>>(items);

            foreach (var item in itemDtos)
            {
                var autor = await _repositoryAutor.FirstOrDefaultAsync(x => x.Id == item.AutorId);
                var genero = await _repositoryGenero.FirstOrDefaultAsync(x => x.Id == item.GeneroId);
                item.DescricaoGenero = genero.Descricao;
                item.NomeAutor = autor.Nome;
            }

            return new PagedResultDto<LivroDto>(totalCount, itemDtos);
        }

        public async Task<LivroDto> GetLivroById(int id)
        {
            var livro = await Repository.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            return ObjectMapper.Map<LivroDto>(livro);
        }

        private int AlterarQuantidadeDisponivel(UpdateLivroDto livroDto, int quantidade)
        {
            var livroEntity = Repository.FirstOrDefault(x => x.Id.Equals(livroDto.Id));
            if (livroEntity == null)
            {
                throw new UserFriendlyException("Livro não encontrado!");
            }
            if (livroDto.Quantidade < livroDto.QuantidadeDisponivel)
            {
                throw new UserFriendlyException("A qauntidade de livros não pode ser menor que a quantiodade de livros disponíveis!");
            }
            if (livroEntity.Quantidade > livroDto.Quantidade)
            {
                quantidade = livroDto.Quantidade - livroEntity.Quantidade;
            }
            else if (livroEntity.Quantidade < livroDto.Quantidade)
            {
                quantidade = livroEntity.Quantidade - livroDto.Quantidade;
            }

            return quantidade;
        }
    }
}
