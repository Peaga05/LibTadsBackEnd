﻿using Abp.Application.Services;
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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using QRCoder;

namespace LibTads.Livros
{
    [AbpAuthorize(PermissionNames.Pages_Livros)]
    public class LivroAppService : AsyncCrudAppService<Livro, LivroDto, int, PagedLivroResultRequestDto, CreateLivroDto, UpdateLivroDto>
    {
        private readonly IRepository<Autor, int> _repositoryAutor;
        private readonly IRepository<Genero, int> _repositoryGenero;
        public LivroAppService(
            IRepository<Livro, int> repository,
            IRepository<Autor, int> repositoryAutor,
            IRepository<Genero, int> repositoryGenero
        ) : base(repository)
        {
            _repositoryAutor = repositoryAutor;
            _repositoryGenero = repositoryGenero;
        }

        public override async Task<LivroDto> CreateAsync(CreateLivroDto livroDto)
        {
            CheckCreatePermission();
            var haveLivro = await Repository.FirstOrDefaultAsync(x => x.Isbn.Equals(livroDto.Isbn));
            if (haveLivro != null)
            {
                if (haveLivro.IsDeleted)
                {
                    haveLivro.IsDeleted = false;
                    await Repository.UpdateAsync(haveLivro);
                    return MapToEntityDto(haveLivro);
                }
                throw new UserFriendlyException("Esse livro já está cadastrado");
            }
            var livro = ObjectMapper.Map<Livro>(livroDto);
            livro.CreationTime = DateTime.Now;
            await Repository.InsertAsync(livro);
            var livroMap =  MapToEntityDto(livro);
            if (livroDto.GerarQrCode)
            {
                livro.QrCode = gerarQrCode(livroMap.Id);
                livroMap.QrCode = livro.QrCode;
                await Repository.UpdateAsync(livro);
            }
            return livroMap;
        }

        private string gerarQrCode(int id)
        {
            var url = "http://localhost:4200/app/emprestimos/create-emprestimo/" + id;
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.H);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(5);

                using (MemoryStream stream = new MemoryStream())
                {
                    qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    return Convert.ToBase64String(stream.ToArray());
                }
            }
        }

        public string CadastrarQrCode(int idLivro)
        {
            var livro = Repository.FirstOrDefault(x => x.Id.Equals(idLivro));
            livro.QrCode = gerarQrCode(idLivro);
            return livro.QrCode;
        }

        public override async Task<LivroDto> UpdateAsync(UpdateLivroDto livroDto)
        {
            CheckCreatePermission();
            var haveIsbn = Repository.FirstOrDefault(x => x.Isbn.Equals(livroDto.Isbn) && x.Id != livroDto.Id);
            if(haveIsbn != null)
            {
                throw new UserFriendlyException("Esse isbn esta vinculado a outro livro!");
            }
            var livro = ObjectMapper.Map<Livro>(livroDto);
            await Repository.UpdateAsync(livro);
            return await GetAsync(livroDto);
        }

        public async Task DeActivate(int idLivro)
        {
            CheckUpdatePermission();
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
    }
}
